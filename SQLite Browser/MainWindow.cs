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
    }
}
