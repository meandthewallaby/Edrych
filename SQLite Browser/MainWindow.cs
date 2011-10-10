using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

            App.IsCopyEnabled = false;
            App.IsPasteEnabled = false;
        }

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

        private void CopyEnabled_Changed(object sender, EventArgs e)
        {
            this.cutToolStripMenuItem.Enabled = App.IsCopyEnabled;
            this.copyToolStripMenuItem.Enabled = App.IsCopyEnabled;
        }

        private void PasteEnabled_Changed(object sender, EventArgs e)
        {
            this.pasteToolStripMenuItem.Enabled = App.IsPasteEnabled;
        }
    }
}
