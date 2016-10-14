using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titan.Model;
using Titan.SchemaModel;

namespace Titan.CSharpGenerator
{
    public class CSharpPlugin:IPlugin
    {
        public Context Context { get; set; }

        public void Init()
        {
            ToolStripMenuItem createCSharp = new ToolStripMenuItem();
            //openPdmMenu.Name = "openToolStripMenuItem";
            createCSharp.Text = "生成C#代码...";
            createCSharp.Click += new System.EventHandler(this.createCSharp_Click);
            Context.GenerateMenu.DropDownItems.Add(createCSharp);

        }

        public void OnModelTreeClick()
        {
            //throw new NotImplementedException();
        }

        private void createCSharp_Click(object sender, EventArgs e)
        {
            //try
            //{
                EntityClassConfigForm f = new EntityClassConfigForm();
                Entity rootModel = Context.GetModel();
                if (rootModel == null)
                {
                    MessageBox.Show("请先打开pdm");
                    return;
                }
                f.Project = Project.LoadProject(rootModel);
                f.ShowDialog();
                f = null;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
    }
}
