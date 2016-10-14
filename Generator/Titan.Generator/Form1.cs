using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Titan.CSharpGenerator;
using Titan.PdmSchemaReader;
using Titan.SchemaModel;

namespace Titan.Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } 

        #region private
       
         
        #endregion

        private List<IPlugin> plugins = null;
        private Context context = null;

        private void Form1_Load(object sender, EventArgs e)
        {

            plugins = new List<IPlugin>();

            //自动扫描插件
            try
            {
                string[] dllFileNames = Directory.GetFiles(Application.StartupPath, "*.dll");
                foreach (string dllFileName in dllFileNames)
                {
                    Assembly assembly = Assembly.LoadFile(dllFileName);
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (typeof(IPlugin).IsAssignableFrom(type) && typeof(IPlugin)!=type)
                        {
                            IPlugin plugInInstance = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugInInstance);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            context=new Context();
            context.FileMenu=this.MenuFile;
            context.GenerateMenu=this.MenuGenerate;
            context.ModelTree = this.treeView1;
            context.Plugins = plugins;

            foreach (IPlugin plugin in plugins)
            {
                plugin.Context = context;
                plugin.Init();
            }

            //List<IGenerator> generators = LoadGenerators();
            //foreach (IGenerator generator in generators)
            //{
            //    ToolStripMenuItem menu = new ToolStripMenuItem(); 
            //    GeneratorBehaviorAttribute attr = generator.GetType().GetCustomAttribute<GeneratorBehaviorAttribute>();
            //    menu.Text = attr.Name;
            //    menu.Tag = generator;
            //    menu.Enabled = false;
            //    menu.Click += GeneratorMenuItem_Click;
            //    this.MenuGenerate.DropDownItems.Add(menu);
            //}
        }
        //private void GeneratorMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (this.treeView1.Nodes.Count <= 0)
        //    {
        //        MessageBox.Show("请先打开模型文件");
        //        return;
        //    }
        //    ToolStripMenuItem menu = (ToolStripMenuItem)sender;
        //    IGenerator generator = (IGenerator)menu.Tag;
        //    generator.Generate(((Entity)(this.treeView1.Nodes[0].Tag)), null);
        //} 

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propertyGrid1.SelectedObject = e.Node.Tag;
            foreach (IPlugin plugin in plugins)
            {
                plugin.OnModelTreeClick();
            }
        } 
    }
}
