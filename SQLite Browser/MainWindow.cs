using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteBrowser.Helpers;
using SQLiteBrowser.Views;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            App.CopyEnabledChanged += this.CopyEnabled_Changed;
            App.PasteEnabledChanged += this.PasteEnabled_Changed;
            App.UndoEnabledChanged += this.UndoEnabled_Changed;
            App.RedoEnabledChanged += this.RedoEnabled_Changed;

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
            App.IsUndoEnabled = false;
            App.IsRedoEnabled = false;
        }

        #region Menu Item Handling - File Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryView qp = new QueryView();
            qp.Dock = DockStyle.Fill;
            TabPage tp = new TabPage();
            tp.Name = "New query";
            tp.Text = "New query";
            tp.Controls.Add(qp);
            this.tabControl1.TabPages.Add(tp);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Menu Item Handling - Edit Menu

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            tbActiveQuery.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            tbActiveQuery.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            tbActiveQuery.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            tbActiveQuery.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Text);
            tbActiveQuery.Paste(myFormat);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox tbActiveQuery = FindFocusedControl() as RichTextBox;
            tbActiveQuery.SelectAll();
        }

        #endregion

        private void CopyEnabled_Changed(object sender, EventArgs e)
        {
            this.cutToolStripMenuItem.Enabled = App.IsCopyEnabled;
            this.copyToolStripMenuItem.Enabled = App.IsCopyEnabled;
        }

        private void PasteEnabled_Changed(object sender, EventArgs e)
        {
            this.pasteToolStripMenuItem.Enabled = App.IsPasteEnabled;
        }

        private void UndoEnabled_Changed(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = App.IsUndoEnabled;
        }

        private void RedoEnabled_Changed(object sender, EventArgs e)
        {
            this.redoToolStripMenuItem.Enabled = App.IsRedoEnabled;
        }

        private void TabClosing(object sender, CloseEventArgs e)
        {
            //Need some saving and all that here
            this.tabControl1.TabPages.RemoveAt(e.TabIndex);
        }

        private Control FindFocusedControl()
        {
            Control control = this;
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }
            return control;
        }
    }
}
