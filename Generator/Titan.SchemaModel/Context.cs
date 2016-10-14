using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan.SchemaModel
{
    public class Context
    {
        public List<IPlugin> Plugins { get; set; }
        public ToolStripMenuItem FileMenu { get; set; }
        public ToolStripMenuItem GenerateMenu { get; set; }
        public TreeView ModelTree { get; set; } 

        public Entity GetModel()
        {
            if (ModelTree.Nodes.Count <= 0) return null;
            return (Entity)(ModelTree.Nodes[0].Tag);
        }
        public void ModelToTree(Entity entity )
        {
            ModelTree.BeginUpdate();
            ModelTree.Nodes.Clear();
            ModelToTree(entity, ModelTree.Nodes);
            ModelTree.EndUpdate();
            foreach (IPlugin plugin in Plugins)
            {
                plugin.OnModelTreeClick();
            }
        }
        private void ModelToTree(Entity entity, TreeNodeCollection nodes)
        {
            Type entityType = typeof(Entity);
            Type stringType = typeof(string);
             
            string entityName = entity.ToString(); 
            TreeNode fnode = new TreeNode(entityName);
            fnode.Tag = entity;
            nodes.Add(fnode);
            TreeNodeCollection newNodes = fnode.Nodes;
             
            foreach (KeyValuePair<string,object> kv in entity.Properties)
            {
                string propertyName = kv.Key;
                object propertyValue = kv.Value;
                Type propertyType = propertyValue.GetType();
                if (entityType.IsAssignableFrom(propertyType))
                {
                    if (kv.Value != null)
                    {
                        string nodeText = propertyName;
                        TreeNode node = new TreeNode(nodeText);
                        node.Tag = propertyValue;
                        newNodes.Add(node);
                    }
                }
                else if (propertyType.IsGenericType)
                {
                    Type[] types = propertyType.GetGenericArguments();
                    if (types.Length == 2 && stringType == types[0] && entityType.IsAssignableFrom(types[1]))
                    {
                        string nodeText = propertyName; 
                        TreeNode node = new TreeNode(nodeText);
                        node.Tag = propertyValue;

                        IDictionary subEntitys = (IDictionary)propertyValue;
                        foreach (object subEntity in subEntitys.Values)
                        {
                            ModelToTree((Entity)subEntity, node.Nodes);
                        }
                        newNodes.Add(node);
                    }
                    else if (types.Length == 1 && entityType.IsAssignableFrom(types[0]))
                    {
                        string nodeText = propertyName; 
                        TreeNode node = new TreeNode(nodeText);
                        node.Tag = propertyValue;

                        IList subEntitys = (IList)propertyValue;
                        foreach (object subEntity in subEntitys)
                        {
                            ModelToTree((Entity)subEntity, node.Nodes);
                        }
                        newNodes.Add(node);
                    }
                }  
            }
 
        }

    }
}
