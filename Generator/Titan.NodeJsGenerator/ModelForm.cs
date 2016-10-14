using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titan.Model;

namespace Titan.NodeJsGenerator
{
    public partial class ModelForm : Form
    {
        public Project Project { get; set; }

        public ModelForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ORM_ModelGenerator_MySql ORM_ModelGenerator_MySql = new ORM_ModelGenerator_MySql();
            ORM_ModelGenerator_MySql.Create(Project, this.ctlCSharpEntityOutputFile.Text);
            MessageBox.Show("Model生成完成");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
