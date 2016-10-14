using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titan.Model;
using Titan.SchemaModel;

namespace Titan.NodeJsGenerator
{
    public class NodeJsPlugin:IPlugin
    {
        public Context Context { get; set; }

        public void Init()
        {
            ToolStripMenuItem createCSharp = new ToolStripMenuItem();
            //openPdmMenu.Name = "openToolStripMenuItem";
            createCSharp.Text = "生成NodeJs sequelize Model...";
            createCSharp.Click += new System.EventHandler(this.createCSharp_Click);
            Context.GenerateMenu.DropDownItems.Add(createCSharp);

            ToolStripMenuItem createCSharp1 = new ToolStripMenuItem();
            createCSharp1.Text = "生成NodeJs 页面...";
            createCSharp1.Click += new System.EventHandler(this.createNodePage_Click);
            Context.GenerateMenu.DropDownItems.Add(createCSharp1);
        }

        public void OnModelTreeClick()
        {
            //throw new NotImplementedException();
        }

        private void createCSharp_Click(object sender, EventArgs e)
        {
            Entity rootModel = Context.GetModel();
            if (rootModel == null)
            {
                MessageBox.Show("请先打开pdm");
                return;
            }
            ModelForm f = new ModelForm();
            f.Project = Project.LoadProject(rootModel);
            f.ShowDialog();
            f = null;
        }

        private void createNodePage_Click(object sender, EventArgs e)
        {
            Entity rootModel = Context.GetModel();
            if (rootModel == null)
            {
                MessageBox.Show("请先打开pdm");
                return;
            }
            PageForm f = new PageForm();
            f.Project = Project.LoadProject(rootModel);
            f.ChangeNodes();
            f.ShowDialog();
            f = null;
        }
    }
}
