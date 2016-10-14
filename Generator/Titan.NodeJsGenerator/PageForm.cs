using System;
using System.Collections;
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

namespace Titan.NodeJsGenerator
{
    public partial class PageForm : Form
    {
        public Project Project { get; set; }

        public Entity Entity { get; set; }
        public PageForm()
        {
            InitializeComponent();
        }

        public void ChangeNodes()
        {
            ModelToTree(this.treeView1.Nodes);
        }

        public void ModelToTree(TreeNodeCollection nodes)
        {
            string entityName = "Table";
            TreeNode fnode = new TreeNode(entityName);
            fnode.Tag = "Project";
            fnode.ExpandAll();
            nodes.Add(fnode);

            TreeNodeCollection newNodes = fnode.Nodes;

            foreach (Table table in Project.Tables)
            {
                string nodeText = table.TableName;
                TreeNode node = new TreeNode(nodeText);
                node.Tag = "Table";
                newNodes.Add(node);

                foreach (Column column in table.Columns) {
                    TreeNode columnNode = new TreeNode(column.ColumnName);
                    columnNode.Tag = "Column";
                    node.Nodes.Add(columnNode);
                }                
            }

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    //取消节点选中状态之后，取消所有父节点的选中状态
                    setChildNodeCheckedState(e.Node, true);

                }
                else
                {
                    //取消节点选中状态之后，取消所有父节点的选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }
        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;

            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
                foreach (TreeNode tn in nodes)
                {

                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = this.textBox1.Text;
            if (string.IsNullOrEmpty(path)) {
                MessageBox.Show("选择生成目录");
                return;
            }
            ORM_Page_MySql ORM_Page_MySql = new ORM_Page_MySql();
            foreach (TreeNode rtn in treeView1.Nodes)
            {
                foreach (TreeNode tn in rtn.Nodes)
                {
                    if (tn.Checked)
                    {
                        Table table = Project.Tables.Find(p => p.TableName.Equals(tn.Text));
                        ORM_Page_MySql.Create(table, this.textBox1.Text);
                    }
                }
            }
            MessageBox.Show("Model生成完成");
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Node.Text)) {
                return;
            }
            if (e.Node.Tag.ToString()== "Table")
            {
                Table table= Project.Tables.Find(p => p.TableName.Equals(e.Node.Text));
                ORM_Page_MySql ORM_Page_MySql = new ORM_Page_MySql();
                string code= ORM_Page_MySql.CreateController(table);
                string list = ORM_Page_MySql.CreateList(table);
                string edit = ORM_Page_MySql.CreateEdit(table);
                string detail = ORM_Page_MySql.CreateDetail(table);

                RichTextBox richTextBox1 = (RichTextBox)this.tabPage1.Controls[0];
                richTextBox1.Text = code;

                RichTextBox richTextBox2 = (RichTextBox)this.tabPage2.Controls[0];
                richTextBox2.Text = list;

                RichTextBox richTextBox3 = (RichTextBox)this.tabPage3.Controls[0];
                richTextBox3.Text = edit;

                RichTextBox richTextBox4 = (RichTextBox)this.tabPage4.Controls[0];
                richTextBox4.Text = detail;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
