namespace Titan.Common.Forms
{
    partial class AutoSaveFileSelector
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ctlCSharpEntityOutputFile = new System.Windows.Forms.ComboBox();
            this.btnCSharpEntityBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctlCSharpEntityOutputFile
            // 
            this.ctlCSharpEntityOutputFile.FormattingEnabled = true;
            this.ctlCSharpEntityOutputFile.Location = new System.Drawing.Point(3, 3);
            this.ctlCSharpEntityOutputFile.Name = "ctlCSharpEntityOutputFile";
            this.ctlCSharpEntityOutputFile.Size = new System.Drawing.Size(413, 20);
            this.ctlCSharpEntityOutputFile.TabIndex = 38;
            // 
            // btnCSharpEntityBrowse
            // 
            this.btnCSharpEntityBrowse.Location = new System.Drawing.Point(422, 1);
            this.btnCSharpEntityBrowse.Name = "btnCSharpEntityBrowse";
            this.btnCSharpEntityBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCSharpEntityBrowse.TabIndex = 37;
            this.btnCSharpEntityBrowse.Text = "浏览";
            this.btnCSharpEntityBrowse.UseVisualStyleBackColor = true;
            this.btnCSharpEntityBrowse.Click += new System.EventHandler(this.btnCSharpEntityBrowse_Click);
            // 
            // AutoSaveFileSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlCSharpEntityOutputFile);
            this.Controls.Add(this.btnCSharpEntityBrowse);
            this.Name = "AutoSaveFileSelector";
            this.Size = new System.Drawing.Size(503, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ctlCSharpEntityOutputFile;
        private System.Windows.Forms.Button btnCSharpEntityBrowse;
    }
}
