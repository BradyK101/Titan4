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
using Titan.SchemaModel;

namespace Titan.CSharpGenerator
{
    public partial class EntityClassConfigForm : Form
    {

        public Project Project { get; set; }
         
        public EntityClassConfigForm()
        {
            InitializeComponent();
        }



        private void btnCSharpEntityGenerate_Click(object sender, EventArgs e)
        {

            EntityClassConfig config = new EntityClassConfig();
            config.NameSpace = this.ctlCSharpEntityNameSpace.Text;
            config.OutputFileName = this.ctlCSharpEntityOutputFile.Text;
            config.WcfEnabled = this.ctlCSharpEntityWcfEnabled.Checked;
            config.UseAttribute = this.ctlCSharpEntityUseAttribute.Checked;
            config.UseValidate = this.ctlCSharpEntityUseValidate.Checked;
            //config.SupportMySql = this.ctlSupport.GetItemChecked(0);
            //config.SupportSQLite = this.ctlSupport.GetItemChecked(1);
            //config.SupportOracle = this.ctlSupport.GetItemChecked(2);
            //config.SupportSqlServer = this.ctlSupport.GetItemChecked(3);

            if (this.ctlDbType.Text == "MySql")
            {
                EntityClassGenerator_MySql CSharpEntityGenerator_MySql = new EntityClassGenerator_MySql();
                CSharpEntityGenerator_MySql.Create(Project, config);
            }
            else if (this.ctlDbType.Text == "Odp(Oracle Data Provider)")
            {
                EntityClassGenerator_Oracle CSharpEntityGenerator_Oracle = new EntityClassGenerator_Oracle();
                CSharpEntityGenerator_Oracle.Create(Project, config);
            }
            else if (this.ctlDbType.Text == "SqlServer")
            {
                EntityClassGenerator_SqlServer CSharpEntityGenerator_SqlServer = new EntityClassGenerator_SqlServer();
                CSharpEntityGenerator_SqlServer.Create(Project, config);
            }
            MessageBox.Show("Entities生成完成");
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
