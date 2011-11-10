using Edrych.Helpers;

namespace Edrych.Views
{
    partial class QueryView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (_queryViewModel != null)
                    _queryViewModel.Dispose();
                if (_bgWorker != null)
                    _bgWorker.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryView));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbQuery = new System.Windows.Forms.RichTextBox();
            this.tbLines = new System.Windows.Forms.RichTextBox();
            this.tcResults = new System.Windows.Forms.TabControl();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.dgResults = new Edrych.Helpers.DataGridViewExt();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.tbMessages = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.queryTimer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tcResults.SuspendLayout();
            this.tpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.tpMessages.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbQuery);
            this.splitContainer1.Panel1.Controls.Add(this.tbLines);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcResults);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(862, 576);
            this.splitContainer1.SplitterDistance = 269;
            this.splitContainer1.TabIndex = 0;
            // 
            // tbQuery
            // 
            this.tbQuery.AcceptsTab = true;
            this.tbQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQuery.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbQuery.Location = new System.Drawing.Point(31, 0);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.ShortcutsEnabled = false;
            this.tbQuery.Size = new System.Drawing.Size(831, 267);
            this.tbQuery.TabIndex = 0;
            this.tbQuery.Text = "";
            this.tbQuery.VScroll += new System.EventHandler(this.QueryView_Scrolling);
            this.tbQuery.TextChanged += new System.EventHandler(this.QueryView_QueryChanged);
            this.tbQuery.Enter += new System.EventHandler(this.Query_Focus);
            this.tbQuery.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QueryView_KeyUp);
            this.tbQuery.Leave += new System.EventHandler(this.QueryOrResults_Leave);
            // 
            // tbLines
            // 
            this.tbLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbLines.BackColor = System.Drawing.SystemColors.Control;
            this.tbLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLines.Enabled = false;
            this.tbLines.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLines.Location = new System.Drawing.Point(0, 0);
            this.tbLines.Name = "tbLines";
            this.tbLines.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.tbLines.Size = new System.Drawing.Size(32, 267);
            this.tbLines.TabIndex = 1;
            this.tbLines.Text = "";
            // 
            // tcResults
            // 
            this.tcResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcResults.Controls.Add(this.tpResults);
            this.tcResults.Controls.Add(this.tpMessages);
            this.tcResults.Location = new System.Drawing.Point(0, 3);
            this.tcResults.Name = "tcResults";
            this.tcResults.SelectedIndex = 0;
            this.tcResults.Size = new System.Drawing.Size(862, 275);
            this.tcResults.TabIndex = 2;
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.dgResults);
            this.tpResults.Location = new System.Drawing.Point(4, 22);
            this.tpResults.Name = "tpResults";
            this.tpResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpResults.Size = new System.Drawing.Size(854, 249);
            this.tpResults.TabIndex = 0;
            this.tpResults.Text = "Results";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToOrderColumns = true;
            this.dgResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.NullValue = "(No column name)";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.NullValue = "NULL";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgResults.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgResults.Location = new System.Drawing.Point(3, 3);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgResults.RowTemplate.ReadOnly = true;
            this.dgResults.ShowEditingIcon = false;
            this.dgResults.Size = new System.Drawing.Size(848, 243);
            this.dgResults.TabIndex = 0;
            this.dgResults.Enter += new System.EventHandler(this.Query_Focus);
            this.dgResults.Leave += new System.EventHandler(this.QueryOrResults_Leave);
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.tbMessages);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessages.Size = new System.Drawing.Size(854, 249);
            this.tpMessages.TabIndex = 1;
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // tbMessages
            // 
            this.tbMessages.BackColor = System.Drawing.SystemColors.Window;
            this.tbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessages.Enabled = false;
            this.tbMessages.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMessages.Location = new System.Drawing.Point(3, 3);
            this.tbMessages.Multiline = true;
            this.tbMessages.Name = "tbMessages";
            this.tbMessages.ReadOnly = true;
            this.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMessages.ShortcutsEnabled = false;
            this.tbMessages.Size = new System.Drawing.Size(848, 243);
            this.tbMessages.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.connectionLabel);
            this.panel1.Controls.Add(this.queryTimer);
            this.panel1.Location = new System.Drawing.Point(0, 277);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(862, 26);
            this.panel1.TabIndex = 1;
            // 
            // connectionLabel
            // 
            this.connectionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionLabel.Image = ((System.Drawing.Image)(resources.GetObject("connectionLabel.Image")));
            this.connectionLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.connectionLabel.Location = new System.Drawing.Point(3, 4);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(792, 20);
            this.connectionLabel.TabIndex = 1;
            this.connectionLabel.Text = "      Disconnected";
            this.connectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // queryTimer
            // 
            this.queryTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.queryTimer.Location = new System.Drawing.Point(801, 4);
            this.queryTimer.Name = "queryTimer";
            this.queryTimer.Size = new System.Drawing.Size(58, 16);
            this.queryTimer.TabIndex = 0;
            this.queryTimer.Text = "0:00";
            this.queryTimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // QueryView
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "QueryView";
            this.Size = new System.Drawing.Size(862, 576);
            this.Enter += new System.EventHandler(this.QueryView_Focus);
            this.Leave += new System.EventHandler(this.QueryView_Leave);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tcResults.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.tpMessages.ResumeLayout(false);
            this.tpMessages.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataGridViewExt dgResults;
        private System.Windows.Forms.RichTextBox tbQuery;
        private System.Windows.Forms.RichTextBox tbLines;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label queryTimer;
        private System.Windows.Forms.TabControl tcResults;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.TabPage tpMessages;
        private System.Windows.Forms.TextBox tbMessages;
        private System.Windows.Forms.Label connectionLabel;

    }
}
