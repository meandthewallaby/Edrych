using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Resources;

namespace SQLiteBrowser.Helpers
{
    public class TabControlExt : TabControl
    {
        public delegate void OnHeaderCloseEventHandler(object sender, CloseEventArgs e);
        public event OnHeaderCloseEventHandler OnClose;

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            RectangleF tabTextArea = RectangleF.Empty;
            for(int nIndex = 0 ; nIndex < this.TabCount ; nIndex++)
            {
                tabTextArea = (RectangleF)this.GetTabRect(nIndex);
                if( nIndex != this.SelectedIndex )
                {
                    /*if not active draw ,inactive close button*/
                    using(Bitmap bmp = Icons.inactiveClose)
                    {
                        e.Graphics.DrawImage(bmp, tabTextArea.X+tabTextArea.Width -16, 5, 13, 13);
                    }
                }
                else
                {
                    /*if active draw ,active close button*/
                    using(Bitmap bmp = Icons.activeClose)
                    {
                        e.Graphics.DrawImage(bmp,
                            tabTextArea.X+tabTextArea.Width -16, 5, 13, 13);
                    }
                }
                string str = this.TabPages[nIndex].Text;
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center; 
                using(SolidBrush brush = new SolidBrush(this.TabPages[nIndex].ForeColor))
                {
                    /*Draw the tab header text*/
                    e.Graphics.DrawString(str, this.Font, brush, tabTextArea,stringFormat);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point clickedPoint = new Point(e.X, e.Y);
            RectangleF tabRect = RectangleF.Empty;

            for (int i = 0; i < this.TabCount; i++)
            {
                tabRect = (RectangleF)this.GetTabRect(i);
                if (tabRect.Contains((PointF)clickedPoint))
                {
                    if (OnClose != null)
                    {
                        OnClose(this, new CloseEventArgs(this.SelectedIndex));
                    }
                }
            }
        }
    }

    public class CloseEventArgs:EventArgs
    {
        private int nTabIndex = -1;
        public CloseEventArgs(int nTabIndex)
        {
            this.nTabIndex = nTabIndex;
        }
        
        public int TabIndex 
        {
            get
            {
                return this.nTabIndex;
            }
            set
            {
                this.nTabIndex = value;
            }
        }
    }
}
