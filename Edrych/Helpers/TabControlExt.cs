using System;
using System.Drawing;
using System.Windows.Forms;
using Edrych.Properties;

namespace Edrych.Helpers
{
    /// <summary>Extended TabControl class</summary>
    public class TabControlExt : TabControl
    {
        private const int CLOSE_ICON_PADDING = 4;
        private const int CLOSE_ICON_SIZE = 16;
        private const int MIN_TAB_WIDTH = 100;
        private const int MAX_TAB_WIDTH = 300;
        private const int TAB_HEIGHT = 24;

        /// <summary>Delegate for the HeaderClose event</summary>
        public delegate void OnHeaderCloseEventHandler(object sender, CloseEventArgs e);
        /// <summary>Closing event</summary>
        public event OnHeaderCloseEventHandler Closing;

        /// <summary>Override for the drawing of the control to include a Close Tab button</summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            RectangleF tabTextArea = RectangleF.Empty;
            for(int nIndex = 0 ; nIndex < this.TabCount ; nIndex++)
            {
                tabTextArea = (RectangleF)this.GetTabRect(nIndex);
                
                using(SolidBrush brush = new SolidBrush(this.TabPages[nIndex].BackColor))
                {
                    //Clear the tab
                    e.Graphics.FillRectangle(brush, tabTextArea);
                }

                Bitmap bmp = nIndex == this.SelectedIndex ? Resources.activeClose : Resources.inactiveClose;
                e.Graphics.DrawImage(bmp, tabTextArea.X + tabTextArea.Width - (CLOSE_ICON_PADDING + CLOSE_ICON_SIZE), tabTextArea.Y + CLOSE_ICON_PADDING, CLOSE_ICON_SIZE, CLOSE_ICON_SIZE);
                bmp.Dispose();

                string str = this.TabPages[nIndex].Text;
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                using(SolidBrush brush = new SolidBrush(this.TabPages[nIndex].ForeColor))
                {
                    //Draw the tab header text
                    e.Graphics.DrawString(str, this.Font, brush, tabTextArea,stringFormat);
                }
            }
        }

        /// <summary>Override for the OnMouseDown event to detect clicking on the Close Tab event</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point clickedPoint = new Point(e.X, e.Y);
            RectangleF tabRect = RectangleF.Empty;

            for (int i = 0; i < this.TabCount; i++)
            {
                tabRect = (RectangleF)this.GetTabRect(i);

                RectangleF closeIcon = new RectangleF(tabRect.X + tabRect.Width - 20, tabRect.Y + 4, 16, 16);

                if (closeIcon.Contains((PointF)clickedPoint))
                {
                    if (Closing != null)
                    {
                        Closing(this, new CloseEventArgs(this.SelectedIndex));
                    }
                }
            }
        }

        /// <summary>Resizes the tabs based on the min and max width, and the number of tabs currently open</summary>
        public void ResizeTabs()
        {
            if (this.TabCount > 0)
            {
                int itemWidth = Math.Min(Math.Max((this.Size.Width - 50) / this.TabCount, MIN_TAB_WIDTH), MAX_TAB_WIDTH);
                this.ItemSize = new Size(itemWidth, TAB_HEIGHT);
            }
        }
    }

    /// <summary>Event Arguments from the Closing event</summary>
    public class CloseEventArgs:EventArgs
    {
        private int nTabIndex = -1;

        /// <summary>Constructor to set the index of the tab being closed</summary>
        /// <param name="nTabIndex">Index of the tab being closed</param>
        public CloseEventArgs(int nTabIndex)
        {
            this.nTabIndex = nTabIndex;
        }
        
        /// <summary>Returns the index of the tab closing</summary>
        public int TabIndex { get { return this.nTabIndex; } }
    }
}
