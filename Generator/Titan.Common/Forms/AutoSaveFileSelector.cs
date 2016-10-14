using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Titan.Common.Forms
{
    public partial class AutoSaveFileSelector : UserControl
    {
        private string _Extension = ".cs";
        public string Extension
        {
            get { return _Extension; }
            set { _Extension = value; }
        }

        public override string Text
        {
            get
            {
                return this.ctlCSharpEntityOutputFile.Text; 
            } 
            set
            {
                this.ctlCSharpEntityOutputFile.Text  = value;
            }
        } 

        public AutoSaveFileSelector()
        {
            InitializeComponent();
        }

        private void btnCSharpEntityBrowse_Click(object sender, EventArgs e)
        {

            SaveFileDialog f = new SaveFileDialog();
            f.FileName = this.ctlCSharpEntityOutputFile.Text;
            f.Filter = string.Format("{0}文件(*{0})|*{0}",Extension);
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.ctlCSharpEntityOutputFile.Text = f.FileName;
            }
        }
 

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Config.ConfigToComboBoxItems(this, this.ctlCSharpEntityOutputFile);


            Form f = FindForm();
            f.FormClosing += FormClosing;
        }
        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.ComboBoxItemsToConfig(this, this.ctlCSharpEntityOutputFile);
        }
    }
}
