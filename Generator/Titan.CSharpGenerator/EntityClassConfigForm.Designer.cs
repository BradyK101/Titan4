namespace Titan.CSharpGenerator
{
    partial class EntityClassConfigForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.btnCSharpEntityGenerate = new System.Windows.Forms.Button();
            this.lblCSharpEntityNameSpace = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ctlCSharpEntityNameSpace = new Titan.Common.Forms.AutoSaveComboBox();
            this.ctlCSharpEntityUseValidate = new Titan.Common.Forms.AutoSaveCheckBox();
            this.ctlCSharpEntityUseAttribute = new Titan.Common.Forms.AutoSaveCheckBox();
            this.ctlCSharpEntityWcfEnabled = new Titan.Common.Forms.AutoSaveCheckBox();
            this.ctlDbType = new Titan.Common.Forms.AutoSaveComboBox();
            this.ctlCSharpEntityOutputFile = new Titan.Common.Forms.AutoSaveFileSelector();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 40;
            this.label3.Text = "数据库类型";
            // 
            // btnCSharpEntityGenerate
            // 
            this.btnCSharpEntityGenerate.Location = new System.Drawing.Point(262, 276);
            this.btnCSharpEntityGenerate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCSharpEntityGenerate.Name = "btnCSharpEntityGenerate";
            this.btnCSharpEntityGenerate.Size = new System.Drawing.Size(202, 58);
            this.btnCSharpEntityGenerate.TabIndex = 38;
            this.btnCSharpEntityGenerate.Text = "生成";
            this.btnCSharpEntityGenerate.UseVisualStyleBackColor = true;
            this.btnCSharpEntityGenerate.Click += new System.EventHandler(this.btnCSharpEntityGenerate_Click);
            // 
            // lblCSharpEntityNameSpace
            // 
            this.lblCSharpEntityNameSpace.AutoSize = true;
            this.lblCSharpEntityNameSpace.Location = new System.Drawing.Point(24, 108);
            this.lblCSharpEntityNameSpace.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCSharpEntityNameSpace.Name = "lblCSharpEntityNameSpace";
            this.lblCSharpEntityNameSpace.Size = new System.Drawing.Size(106, 24);
            this.lblCSharpEntityNameSpace.TabIndex = 35;
            this.lblCSharpEntityNameSpace.Text = "命名空间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 24);
            this.label2.TabIndex = 33;
            this.label2.Text = "C#代码文件创建到";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(520, 276);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 58);
            this.button1.TabIndex = 45;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ctlCSharpEntityNameSpace
            // 
            this.ctlCSharpEntityNameSpace.FormattingEnabled = true;
            this.ctlCSharpEntityNameSpace.Location = new System.Drawing.Point(262, 92);
            this.ctlCSharpEntityNameSpace.Margin = new System.Windows.Forms.Padding(6);
            this.ctlCSharpEntityNameSpace.Name = "ctlCSharpEntityNameSpace";
            this.ctlCSharpEntityNameSpace.Size = new System.Drawing.Size(634, 32);
            this.ctlCSharpEntityNameSpace.TabIndex = 54;
            // 
            // ctlCSharpEntityUseValidate
            // 
            this.ctlCSharpEntityUseValidate.AutoSize = true;
            this.ctlCSharpEntityUseValidate.Location = new System.Drawing.Point(912, 144);
            this.ctlCSharpEntityUseValidate.Margin = new System.Windows.Forms.Padding(6);
            this.ctlCSharpEntityUseValidate.Name = "ctlCSharpEntityUseValidate";
            this.ctlCSharpEntityUseValidate.Size = new System.Drawing.Size(138, 28);
            this.ctlCSharpEntityUseValidate.TabIndex = 53;
            this.ctlCSharpEntityUseValidate.Text = "使用验证";
            this.ctlCSharpEntityUseValidate.UseVisualStyleBackColor = true;
            // 
            // ctlCSharpEntityUseAttribute
            // 
            this.ctlCSharpEntityUseAttribute.AutoSize = true;
            this.ctlCSharpEntityUseAttribute.Location = new System.Drawing.Point(1070, 100);
            this.ctlCSharpEntityUseAttribute.Margin = new System.Windows.Forms.Padding(6);
            this.ctlCSharpEntityUseAttribute.Name = "ctlCSharpEntityUseAttribute";
            this.ctlCSharpEntityUseAttribute.Size = new System.Drawing.Size(198, 28);
            this.ctlCSharpEntityUseAttribute.TabIndex = 52;
            this.ctlCSharpEntityUseAttribute.Text = "使用Attribute";
            this.ctlCSharpEntityUseAttribute.UseVisualStyleBackColor = true;
            // 
            // ctlCSharpEntityWcfEnabled
            // 
            this.ctlCSharpEntityWcfEnabled.AutoSize = true;
            this.ctlCSharpEntityWcfEnabled.Location = new System.Drawing.Point(912, 100);
            this.ctlCSharpEntityWcfEnabled.Margin = new System.Windows.Forms.Padding(6);
            this.ctlCSharpEntityWcfEnabled.Name = "ctlCSharpEntityWcfEnabled";
            this.ctlCSharpEntityWcfEnabled.Size = new System.Drawing.Size(126, 28);
            this.ctlCSharpEntityWcfEnabled.TabIndex = 51;
            this.ctlCSharpEntityWcfEnabled.Text = "允许WCF";
            this.ctlCSharpEntityWcfEnabled.UseVisualStyleBackColor = true;
            // 
            // ctlDbType
            // 
            this.ctlDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ctlDbType.FormattingEnabled = true;
            this.ctlDbType.Items.AddRange(new object[] {
            "SqlServer",
            "Odp(Oracle Data Provider)",
            "MySql"});
            this.ctlDbType.Location = new System.Drawing.Point(260, 180);
            this.ctlDbType.Margin = new System.Windows.Forms.Padding(6);
            this.ctlDbType.Name = "ctlDbType";
            this.ctlDbType.Size = new System.Drawing.Size(300, 32);
            this.ctlDbType.TabIndex = 50;
            // 
            // ctlCSharpEntityOutputFile
            // 
            this.ctlCSharpEntityOutputFile.Extension = ".cs";
            this.ctlCSharpEntityOutputFile.Location = new System.Drawing.Point(260, 24);
            this.ctlCSharpEntityOutputFile.Margin = new System.Windows.Forms.Padding(12);
            this.ctlCSharpEntityOutputFile.Name = "ctlCSharpEntityOutputFile";
            this.ctlCSharpEntityOutputFile.Size = new System.Drawing.Size(1020, 56);
            this.ctlCSharpEntityOutputFile.TabIndex = 49;
            // 
            // EntityClassConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 394);
            this.Controls.Add(this.ctlCSharpEntityNameSpace);
            this.Controls.Add(this.ctlCSharpEntityUseValidate);
            this.Controls.Add(this.ctlCSharpEntityUseAttribute);
            this.Controls.Add(this.ctlCSharpEntityWcfEnabled);
            this.Controls.Add(this.ctlDbType);
            this.Controls.Add(this.ctlCSharpEntityOutputFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCSharpEntityGenerate);
            this.Controls.Add(this.lblCSharpEntityNameSpace);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "EntityClassConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EntityClassConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCSharpEntityGenerate;
        private System.Windows.Forms.Label lblCSharpEntityNameSpace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private Common.Forms.AutoSaveFileSelector ctlCSharpEntityOutputFile;
        private Common.Forms.AutoSaveComboBox ctlDbType;
        private Common.Forms.AutoSaveCheckBox ctlCSharpEntityWcfEnabled;
        private Common.Forms.AutoSaveCheckBox ctlCSharpEntityUseAttribute;
        private Common.Forms.AutoSaveCheckBox ctlCSharpEntityUseValidate;
        private Common.Forms.AutoSaveComboBox ctlCSharpEntityNameSpace;
    }
}