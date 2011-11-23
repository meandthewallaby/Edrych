using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Edrych.Helpers;
using Edrych.ViewModels;
using Edrych.Properties;

namespace Edrych.Views
{
    /// <summary>UI class of the query/results tab</summary>
    partial class QueryView : TabPage
    {
        #region Private/Global Variables

        private QueryViewModel _queryViewModel;
        private int _numLines = 0;
        private BackgroundWorker _bgWorker = new BackgroundWorker();

        #endregion

        #region Constructor(s)

        /// <summary>Initializes the tab</summary>
        /// <param name="Browser">BrowserViewModel to use</param>
        public QueryView(ref ServerBrowserViewModel Browser)
        {
            InitializeComponent();
            _queryViewModel = new QueryViewModel(ref Browser);
            this.dgResults.DataSource = _queryViewModel.DataBinding;
            this.tbMessages.DataBindings.Add("Text", _queryViewModel, "Messages");
            this.Name = Guid.NewGuid().ToString();

            _queryViewModel.BeginQuery += this.BeginQuery;
            _queryViewModel.EndQuery += this.EndQuery;

            _bgWorker.WorkerReportsProgress = true;
            _bgWorker.WorkerSupportsCancellation = true;
            _bgWorker.DoWork += this.TimeQuery;
            _bgWorker.ProgressChanged += this.UpdateTimer;

            //Change status bar
            UpdateConnectionInfo();
        }

        #endregion

        #region public Properties

        /// <summary>Returns the name of the tab</summary>
        public string TabName
        {
            get { return _queryViewModel.SafeFileName; }
        }

        /// <summary>Returns whether or not the tab is saved</summary>
        public bool IsSaved
        {
            get { return _queryViewModel.IsSaved; }
        }

        #endregion

        #region public Methods

        /// <summary>Creates the query</summary>
        /// <param name="OpenQuery">Whether or not to open an existing query</param>
        public void CreateQueryView(bool OpenQuery)
        {
            TabControlExt tc = this.Parent as TabControlExt;
            if (tc != null)
            {
                tc.Closing += this.TabClosing;
            }
            this.tbQuery.Text = _queryViewModel.InitQuery(OpenQuery);
            this.ResetTabName();
            this.tbQuery.Focus();
        }

        /// <summary>Sets the active database of the query</summary>
        /// <param name="DatabaseName">Name of the database</param>
        public void SetDatabase(string DatabaseName)
        {
            _queryViewModel.SetDatabase(DatabaseName);
        }

        /// <summary>Runs the selected query</summary>
        public void RunQuery()
        {
            string query = (string.IsNullOrEmpty(tbQuery.SelectedText.Trim()) ? tbQuery.Text : tbQuery.SelectedText).Trim();
            _queryViewModel.RunQuery(query);
        }

        /// <summary>Cancels the running query</summary>
        public void CancelQuery()
        {
            _queryViewModel.CancelQuery();
        }

        /// <summary>Indents the selected lines</summary>
        public void IndentLines(bool ButtonPress)
        {
            int firstLineChar = this.tbQuery.GetFirstCharIndexOfCurrentLine();
            int firstLine = this.tbQuery.GetLineFromCharIndex(firstLineChar);
            if (ButtonPress && string.IsNullOrEmpty(this.tbQuery.SelectedText))
            {
                this.tbQuery.Select(firstLineChar, this.tbQuery.Lines[firstLine].Length);
            }

            if (!string.IsNullOrEmpty(tbQuery.SelectedText))
            {
                int numLines = this.tbQuery.SelectedText.Where(c => c == '\n').Count(); //0-based number of lines (e.g.-1 line would return a value of 0)
                int lastLine = firstLine + numLines;
                int selectLength = this.tbQuery.SelectionLength + (this.tbQuery.SelectionStart - firstLineChar);
                this.tbQuery.SuspendLayout();
                StringBuilder replacement = new StringBuilder();

                for (int i = firstLine; i <= lastLine; i++)
                {
                    replacement.AppendLine("\t" + this.tbQuery.Lines[i]);
                }

                replacement.Remove(replacement.Length - Environment.NewLine.Length, Environment.NewLine.Length);

                string replacer = replacement.ToString();
                this.tbQuery.Text = this.tbQuery.Text.Remove(firstLineChar, selectLength).Insert(firstLineChar, replacer);
                this.tbQuery.Select(firstLineChar, replacer.Length);

                this.tbQuery.ResumeLayout();
            }
        }

        /// <summary>Outdents the selected lines</summary>
        public void OutdentLines(bool ButtonPress)
        {
            int firstLineChar = this.tbQuery.GetFirstCharIndexOfCurrentLine();
            int firstLine = this.tbQuery.GetLineFromCharIndex(firstLineChar);
            if (ButtonPress && string.IsNullOrEmpty(this.tbQuery.SelectedText))
            {
                this.tbQuery.Select(firstLineChar, this.tbQuery.Lines[firstLine].Length);
            }

            if (!string.IsNullOrEmpty(tbQuery.SelectedText))
            {
                //Flesh this part out
                int[] tabs = this.tbQuery.SelectionTabs;

                int numLines = this.tbQuery.SelectedText.Where(c => c == '\n').Count(); //0-based number of lines (e.g.-1 line would return a value of 0)
                int lastLine = firstLine + numLines;
                int selectLength = this.tbQuery.SelectionLength + (this.tbQuery.SelectionStart - firstLineChar);
                this.tbQuery.SuspendLayout();
                StringBuilder replacement = new StringBuilder();

                for (int i = firstLine; i <= lastLine; i++)
                {
                    int line = this.tbQuery.GetFirstCharIndexFromLine(i);
                    if (this.tbQuery.Text[line] == '\t')
                    {
                        replacement.AppendLine(this.tbQuery.Lines[i].Substring(1));
                    }
                    else if (char.IsWhiteSpace(this.tbQuery.Text[line]))
                    {
                        var afterTabs = this.tbQuery.Lines[i].SkipWhile(c => char.IsWhiteSpace(c));
                        Point targetPoint = this.tbQuery.GetPositionFromCharIndex(line + (this.tbQuery.Lines[i].Length - afterTabs.Count()));
                        int target = targetPoint.X;

                        int pos = (
                            from n in tabs
                            where target >= n
                            orderby Math.Abs(n - target)
                            select n
                            ).FirstOrDefault();
                        if (target > pos && afterTabs.Count() > this.tbQuery.Lines[i].Length)
                        {
                            int removeStart = this.tbQuery.GetCharIndexFromPosition(new Point(pos, targetPoint.Y));
                            int removeEnd = this.tbQuery.GetCharIndexFromPosition(targetPoint);
                            replacement.AppendLine(this.tbQuery.Lines[i].Remove(removeStart, removeEnd - removeStart));
                        }
                    }
                    else
                    {
                        replacement.AppendLine(this.tbQuery.Lines[i]);
                    }
                }

                replacement.Remove(replacement.Length - Environment.NewLine.Length, Environment.NewLine.Length);

                string replacer = replacement.ToString();
                this.tbQuery.Text = this.tbQuery.Text.Remove(firstLineChar, selectLength).Insert(firstLineChar, replacer);
                this.tbQuery.Select(firstLineChar, replacer.Length);

                this.tbQuery.ResumeLayout();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Redraws the line numbers on the query</summary>
        private void UpdateLineNumbers()
        {
            int d = this.tbQuery.GetPositionFromCharIndex(0).Y %
                              (this.tbQuery.Font.Height + 1);
            this.tbLines.Location = new Point(0, d);

            int firstCharIndex = this.tbQuery.GetCharIndexFromPosition(new Point(0, 0));
            int lineNumber = this.tbQuery.GetLineFromCharIndex(firstCharIndex);
            int newLines = this.tbQuery.Lines.Count();
            List<string> lines = new List<string>();
            for (int i = lineNumber + 1; i <= newLines; i++)
            {
                lines.Add(i.ToString());
            }
            this.tbLines.Lines = lines.ToArray();
        }

        /// <summary>Updates the tab's name</summary>
        private void ResetTabName()
        {
            this.Text = this.TabName;
            if (_queryViewModel.IsSaved == false)
            {
                this.Text = "*" + this.Text;
            }
        }

        /// <summary>Adds corresponding tab indents to the current line</summary>
        private void AddTabIndents()
        {
            int currLineChar = this.tbQuery.GetFirstCharIndexOfCurrentLine();
            int currLine = this.tbQuery.GetLineFromCharIndex(currLineChar);
            if (currLine > 0 && this.tbQuery.Lines.Count() > 0 && this.tbQuery.Lines[currLine].Length == 0)
            {
                string beforeLine = this.tbQuery.Lines[currLine-1];
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
                    this.tbQuery.Text = this.tbQuery.Text.Insert(currLineChar, "\t");
                }

                this.tbQuery.Select(currLineChar + counter, 0);
            }
        }

        /// <summary>Updates the status bar on the bottom of the query to the current connection info</summary>
        private void UpdateConnectionInfo()
        {
            if (_queryViewModel != null && _queryViewModel.Data != null && _queryViewModel.Databases != null)
            {
                App.OnConnectionChanged(this, new ConnectionChangedEventArgs(this._queryViewModel.Databases, this._queryViewModel.Data.SelectedDatabase));
                App.IsDatabaseDropDownEnabled = true;
                App.IsQueryDisconnectEnabled = true;

                this.connectionLabel.Image = Resources.connect;
                this.connectionLabel.Text = "        Connected to " + _queryViewModel.Data.DataSource;
            }
            else
            {
                App.IsQueryDisconnectEnabled = false;
                App.IsDatabaseDropDownEnabled = false;
                this.connectionLabel.Image = Resources.disconnect;
                this.connectionLabel.Text = "        Disconnected";
            }
        }

        #endregion

        #region Private Methods - Event Handlers

        /// <summary>Handles when the query text is changed</summary>
        private void QueryView_QueryChanged(object sender, EventArgs e)
        {
            int newLines = this.tbQuery.Lines.Count();

            if (_numLines != newLines)
            {
                UpdateLineNumbers();
                AddTabIndents();
            }

            _numLines = newLines;

            App.IsUndoEnabled = this.tbQuery.CanUndo;
            App.IsRedoEnabled = this.tbQuery.CanRedo;

            if (_queryViewModel.IsSaved && this.tbQuery.CanUndo)
            {
                _queryViewModel.IsSaved = false;
                ResetTabName();
            }
        }

        private void QueryView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\t' && !string.IsNullOrEmpty(this.tbQuery.SelectedText))
                e.Handled = true;
        }

        /// <summary>Handles when the keyboard is pressed</summary>
        private void QueryView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None && e.KeyCode == Keys.F5)
            {
                this.RunQuery();
            }
            else if ((e.Modifiers & ModifierKeys) == Keys.Control && e.KeyCode == Keys.R)
            {
                this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            }
            else if ((e.Modifiers & ModifierKeys) == Keys.Control && e.KeyCode == Keys.U)
            {
                App.OnSwitchDatabases(this, new EventArgs());
            }
            else if (e.Modifiers == Keys.None && e.KeyCode == Keys.Tab)
            {
                IndentLines(false);
            }
            else if ((e.Modifiers & ModifierKeys) == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                OutdentLines(false);
            }
        }

        /// <summary>Handles when the query text is scrolled</summary>
        private void QueryView_Scrolling(object sender, EventArgs e)
        {
            UpdateLineNumbers();
        }

        /// <summary>Handles when the tab is focused</summary>
        private void QueryView_Focus(object sender, EventArgs e)
        {
            App.Save += this.QueryView_Save;
            App.SaveAs += this.QueryView_SaveAs;
            App.QueryConnect += this.QueryView_Connect;
            App.QueryDisconnect += this.QueryView_Disconnect;

            App.IsQueryMenuVisible = true;
            App.IsQueryConnectEnabled = true;

            App.OnActiveQueryChanged(this, new EventArgs());
            UpdateConnectionInfo();
            
            this.tbQuery.Focus();
        }

        /// <summary>Handles when the focus leaves the tab</summary>
        private void QueryView_Leave(object sender, EventArgs e)
        {
            App.Save -= this.QueryView_Save;
            App.SaveAs -= this.QueryView_SaveAs;
            App.QueryConnect -= this.QueryView_Connect;
            App.QueryDisconnect -= this.QueryView_Disconnect;

            App.IsQueryMenuVisible = false;
            App.IsQueryConnectEnabled = false;
            App.IsQueryDisconnectEnabled = false;
        }

        /// <summary>Handles when the query text is focused</summary>
        private void Query_Focus(object sender, EventArgs e)
        {
            App.IsCopyEnabled = true;
            App.IsCutEnabled = true;
            App.IsPasteEnabled = true;
            App.IsUndoEnabled = this.tbQuery.CanUndo;
            App.IsRedoEnabled = this.tbQuery.CanRedo;
            App.IsSelectAllEnabled = true;

            App.Cut += this.Query_Cut;
            App.Copy += this.Query_Copy;
            App.Paste += this.Query_Paste;
            App.Undo += this.Query_Undo;
            App.Redo += this.Query_Redo;
            App.SelectAll += this.Query_SelectAll;
        }

        /// <summary>Handles when the results are focused</summary>
        private void Results_Focus(object sender, EventArgs e)
        {
            App.IsCopyEnabled = true;
            App.IsSelectAllEnabled = true;

            App.Copy += this.Results_Copy;
            App.SelectAll += this.Results_SelectAll;
        }

        /// <summary>Handles when the query text or results are unfocused</summary>
        private void QueryOrResults_Leave(object sender, EventArgs e)
        {
            App.IsCutEnabled = false;
            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;

            App.Cut -= this.Query_Cut;
            App.Copy -= this.Query_Copy;
            App.Paste -= this.Query_Paste;
            App.Undo -= this.Query_Undo;
            App.Redo -= this.Query_Redo;
            App.SelectAll -= this.Query_SelectAll;

            App.Copy -= this.Results_Copy;
            App.SelectAll -= this.Results_SelectAll;
        }

        /// <summary>Handles when the query connect button is hit</summary>
        private void QueryView_Connect(object sender, EventArgs e)
        {
            _queryViewModel.Connect();
            UpdateConnectionInfo();
            App.IsQueryDisconnectEnabled = true;
        }

        /// <summary>Handles when the query disconnect button is hit</summary>
        private void QueryView_Disconnect(object sender, EventArgs e)
        {
            _queryViewModel.Disconnect();
            UpdateConnectionInfo();
            App.IsQueryDisconnectEnabled = false;
        }

        /// <summary>Handles when the save button is hit</summary>
        private void QueryView_Save(object sender, EventArgs e)
        {
            _queryViewModel.SaveQuery(this.tbQuery.Text, false);
            this.ResetTabName();
        }

        /// <summary>Handles when the save as button is hit</summary>
        private void QueryView_SaveAs(object sender, EventArgs e)
        {
            _queryViewModel.SaveQuery(this.tbQuery.Text, true);
            this.ResetTabName();
        }

        /// <summary>Handles when the cut button is hit</summary>
        private void Query_Cut(object sender, EventArgs e)
        {
            this.tbQuery.Cut();
        }

        /// <summary>Handles when the copy button is hit on the query text</summary>
        private void Query_Copy(object sender, EventArgs e)
        {
            this.tbQuery.Copy();
        }

        /// <summary>Handles when the paste button is hit</summary>
        private void Query_Paste(object sender, EventArgs e)
        {
            this.tbQuery.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
        }

        /// <summary>Handles when the undo button is hit</summary>
        private void Query_Undo(object sender, EventArgs e)
        {
            this.tbQuery.Undo();
        }

        /// <summary>Handles when the redo button is hit</summary>
        private void Query_Redo(object sender, EventArgs e)
        {
            this.tbQuery.Redo();
        }

        /// <summary>Handles when the select all button is hit on the query text</summary>
        private void Query_SelectAll(object sender, EventArgs e)
        {
            this.tbQuery.SelectAll();
        }

        /// <summary>Handles when the copy button is hit on the results</summary>
        private void Results_Copy(object sender, EventArgs e)
        {
            this.dgResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DataObject d = dgResults.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        /// <summary>Handles when the select all button is hit on the results</summary>
        private void Results_SelectAll(object sender, EventArgs e)
        {
            this.dgResults.SelectAll();
        }

        /// <summary>Handles when the tab is closing</summary>
        private void TabClosing(object sender, CloseEventArgs e)
        {
            TabControlExt tc = this.Parent as TabControlExt;
            if (tc != null && tc.TabPages[e.TabIndex].Name == this.Name)
            {
                DialogResult result = DialogResult.Yes;
                
                if (_queryViewModel.IsSaved == false)
                {
                    result = MessageBox.Show("This query is unsaved! Would you like to save all your hard work?", "Unsaved Query", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        _queryViewModel.SaveQuery(this.tbQuery.Text, false);
                    }
                }
                
                if (result != DialogResult.Cancel)
                {
                    tc.TabPages.RemoveAt(e.TabIndex);
                    tc.ResizeTabs();
                    if (tc.TabCount == 0)
                    {
                        App.IsQueryMenuVisible = false;
                        App.IsDatabaseDropDownEnabled = false;
                    }
                    this.Dispose(true);
                }
            }
        }

        /// <summary>Handles when a query begins execution</summary>
        private void BeginQuery(object sender, EventArgs e)
        {
            _bgWorker.RunWorkerAsync();
        }

        /// <summary>Handles when a query ends execution</summary>
        private void EndQuery(object sender, EndQueryEventArgs e)
        {
            if (_bgWorker.IsBusy)
            {
                _bgWorker.CancelAsync();
            }

            if (e.HasError || (this.dgResults.RowCount == 0 && this.dgResults.ColumnCount == 0))
            {
                this.tcResults.SelectTab("tpMessages");
            }
            else
            {
                this.tcResults.SelectTab("tpResults");
            }

            this.tbQuery.Focus();
        }
        
        #endregion

        #region Private Methods - Async

        /// <summary>Times the query execution by kicking off a parallel thread and waiting for the query to cancel it</summary>
        private void TimeQuery(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (!_bgWorker.CancellationPending)
            {
                TimeSpan ts = sw.Elapsed;
                _bgWorker.ReportProgress(0, String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
                Thread.Sleep(100);
            }
            sw.Stop();
        }

        /// <summary>Updates the UI timer with the results from TimeQuery</summary>
        private void UpdateTimer(object sender, ProgressChangedEventArgs e)
        {
            this.queryTimer.Text = e.UserState.ToString();
        }

        #endregion
    }
}
