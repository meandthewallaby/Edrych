namespace SQLiteBrowser.Views
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbQuery = new System.Windows.Forms.RichTextBox();
            this.tbLines = new System.Windows.Forms.RichTextBox();
            this.dgResults = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.dgResults);
            this.splitContainer1.Size = new System.Drawing.Size(578, 410);
            this.splitContainer1.SplitterDistance = 192;
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
            this.tbQuery.Size = new System.Drawing.Size(547, 190);
            this.tbQuery.TabIndex = 0;
            this.tbQuery.Text = "";
            this.tbQuery.VScroll += new System.EventHandler(this.QueryView_Scrolling);
            this.tbQuery.TextChanged += new System.EventHandler(this.QueryView_QueryChanged);
            this.tbQuery.Enter += new System.EventHandler(this.Query_Focus);
            this.tbQuery.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QueryView_KeyUp);
            this.tbQuery.Leave += new System.EventHandler(this.Query_UnFocus);
            // 
            // tbLines
            // 
            this.tbLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbLines.BackColor = System.Drawing.SystemColors.Control;
            this.tbLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLines.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLines.Location = new System.Drawing.Point(0, 0);
            this.tbLines.Name = "tbLines";
            this.tbLines.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.tbLines.Size = new System.Drawing.Size(32, 190);
            this.tbLines.TabIndex = 1;
            this.tbLines.Text = "";
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgResults.Location = new System.Drawing.Point(0, 0);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.Size = new System.Drawing.Size(578, 214);
            this.dgResults.TabIndex = 0;
            this.dgResults.Enter += new System.EventHandler(this.Query_Focus);
            this.dgResults.Leave += new System.EventHandler(this.Query_UnFocus);
            // 
            // QueryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "QueryView";
            this.Size = new System.Drawing.Size(578, 410);
            this.Load += new System.EventHandler(this.QueryView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.RichTextBox tbQuery;
        private System.Windows.Forms.RichTextBox tbLines;

    }
}
