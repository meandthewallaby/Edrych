using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Edrych.Helpers
{
    /// <summary>A rich textbox that does syntax higlighting!</summary>
    class SyntaxRichTextBox : RichTextBox
    {
        #region Private/Global Variables

        private Regex _highlightRegex;

        private int _numLines = 0;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int WM_SETREDRAW = 0x0b;

        #endregion

        #region Public Properties

        /// <summary>Color of normal text</summary>
        public Color NormalColor { get; set; }
        /// <summary>Color of a language keyword</summary>
        public Color KeywordColor { get; set; }
        /// <summary>Color of string text</summary>
        public Color StringColor { get; set; }
        /// <summary>Color of a language comment</summary>
        public Color CommentColor { get; set; }
        /// <summary>Comment pattern</summary>
        public string Comment { get; set; }
        /// <summary>Multiline comment delimiters</summary>
        public string[] MultilineComment { get; set; }
        /// <summary>List of keywords to highlight</summary>
        public List<string> Keywords { get; set; }

        #endregion

        #region Public Methods

        /// <summary>Creates the regex patterns and compiles the regular expressions</summary>
        public void InitializeSyntax()
        {
            string keywordPattern = SetKeywords();
            string pattern = @"(')|(" + this.Comment + ".*?$)|(" + this.MultilineComment[0] + @")|(" + this.MultilineComment[1] + @")|" + keywordPattern;
            _highlightRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        }

        /// <summary>Sends the interop message to hold painting</summary>
        public void BeginUpdate()
        {
            SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        }

        /// <summary>Sends the interop message to resume painting</summary>
        public void EndUpdate()
        {
            SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
        }

        #endregion

        #region Protected Methods

        /// <summary>Overrides the TextChanged event</summary>
        protected override void OnTextChanged(EventArgs e)
        {
            BeginUpdate();

            if (this.Lines.Count() != _numLines)
            {
                AddTabIndents();
            }

            _numLines = this.Lines.Count();

            int prevPos = this.SelectionStart;

            this.SelectAll();
            this.SelectionColor = this.NormalColor;
            this.Select(prevPos, 0);

            ParseSyntax(_highlightRegex);

            EndUpdate();
            this.Invalidate();

            this.Select(prevPos, 0);
            this.SelectionColor = this.NormalColor;
            base.OnTextChanged(e);
        }
        
        #endregion

        #region Private Methods

        /// <summary>Parses keywords and comments</summary>
        /// <param name="regKeywords">Regular expression object to use</param>
        private void ParseSyntax(Regex r)
        {
            bool inString = false;
            bool inComment = false;
            int commentsOpen = 0;
            int openStringPosition = -1;
            List<int> commentOpenPositions = new List<int>();
            List<int> commentClosedPositions = new List<int>();

            for (Match m = r.Match(this.Text); m.Success; m = m.NextMatch())
            {
                //Text delimeter
                if (m.Groups[1].Success && !inComment)
                {
                    if (openStringPosition >= 0)
                    {
                        //Need to add to the Index to make sure to highlight the ending '
                        HighlightText(openStringPosition, m.Index + 1 - openStringPosition, this.StringColor);
                        openStringPosition = -1;
                    }
                    else
                    {
                        openStringPosition = m.Index;
                    }
                    inString = !inString;
                }
                //Line comment delimeter
                else if (m.Groups[2].Success && !inString && (!inComment || m.Value.Contains("*/")))
                {
                    int start = !inComment ? m.Index : commentOpenPositions[commentOpenPositions.Count - 1];
                    int end = m.Index + m.Length;
                    HighlightText(start, end - start, this.CommentColor);
                }
                //Multiline comment open
                else if (m.Groups[3].Success && !inString)
                {
                    commentOpenPositions.Add(m.Index);
                    commentsOpen++;
                    inComment = true;
                }
                //Multiline comment closed
                else if (m.Groups[4].Success && !inString && commentsOpen > 0)
                {
                    int start = commentOpenPositions[commentOpenPositions.Count - 1];
                    int end = m.Index + 2;
                    HighlightText(start, end - start, this.CommentColor);
                    commentOpenPositions.Remove(start);
                    commentsOpen--;
                    inComment = commentsOpen > 0;
                }
                //Keyword
                else if (!inString && !inComment)
                {
                    HighlightText(m.Index, m.Length, this.KeywordColor);
                }
            }

            if (inString)
                HighlightText(openStringPosition, this.Text.Length - openStringPosition, this.StringColor);
            else if (inComment)
                HighlightText(commentOpenPositions[0], this.Text.Length - commentOpenPositions[0], this.CommentColor);
        }

        /// <summary>Highlights given swatch of a text a given color</summary>
        /// <param name="Start">Start index to start highlighting</param>
        /// <param name="Length">Length of the swath of text to highlight</param>
        /// <param name="HighlightColor">Color to highlight the text</param>
        private void HighlightText(int Start, int Length, Color HighlightColor)
        {
            int len = Length == -1 ? this.Text.Length - Start : Length;
            SelectionStart = Start;
            SelectionLength = len;
            SelectionColor = HighlightColor;
        }

        /// <summary>Creates a regex pattern from a list of keywords</summary>
        /// <returns>String representing the regular expression to use</returns>
        private string SetKeywords()
        {
            StringBuilder pattern = new StringBuilder();
            for (int i = 0; i < Keywords.Count; i++)
            {
                pattern.Append("(^+|\\b+)" + Keywords[i] + "(\\b+|$)");
                if (i < Keywords.Count - 1)
                    pattern.Append("|");
            }
            return pattern.ToString();
        }

        /// <summary>Adds corresponding tab indents to the current line</summary>
        private void AddTabIndents()
        {
            int currLineChar = this.GetFirstCharIndexOfCurrentLine();
            int currLine = this.GetLineFromCharIndex(currLineChar);
            if (currLine > 0 && this.Lines.Count() > 0 && this.Lines[currLine].Length == 0)
            {
                string beforeLine = this.Lines[currLine - 1];
                int counter = 0;
                foreach (char nextChar in beforeLine.ToCharArray())
                {
                    if (nextChar == '\t')
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < counter; i++)
                {
                    this.Select(currLineChar, 1);
                    this.SelectedText = '\t' + this.SelectedText;
                }

                this.Select(currLineChar + counter, 0);
            }
        }

        #endregion
    }
}
