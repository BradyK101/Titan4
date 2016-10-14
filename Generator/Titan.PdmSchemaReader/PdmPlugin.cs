using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Titan.SchemaModel;

namespace Titan.PdmSchemaReader
{
    public class PdmPlugin : IPlugin
    {
        public Context Context { get; set; }

        public void Init()
        {
            ToolStripMenuItem openPdmMenu = new ToolStripMenuItem();
            //openPdmMenu.Name = "openToolStripMenuItem";
            openPdmMenu.Text = "打开pdm(&O)...";
            openPdmMenu.Click += new System.EventHandler(this.openFileMenuItem_Click);
            Context.FileMenu.DropDownItems.Add(openPdmMenu);

            latestPdmMenu = new ToolStripMenuItem();
            latestPdmMenu.Text = "最近的pdm文件";
            Context.FileMenu.DropDownItems.Add(latestPdmMenu);


            List<string> historys = loadHistory();
            int i = 1;
            foreach (string history in historys)
            {
                ToolStripMenuItem historyMenu = new ToolStripMenuItem();
                historyMenu.Text = i + " " + history;
                historyMenu.Tag = history;
                historyMenu.Click += new System.EventHandler(this.openFile);
                latestPdmMenu.DropDownItems.Add(historyMenu);
                i++;
                if (i > MAX_HISTORY_COUNT) break;
            }
        }
        public void OnModelTreeClick()
        {

        }

        const string PDM_HISTORY = "PdmHistory.xml";
        const int MAX_HISTORY_COUNT = 4;

        private ToolStripMenuItem latestPdmMenu;
        private List<string> loadHistory()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PDM_HISTORY);
            if (!File.Exists(fileName)) return new List<string>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            //反序列化，并将反序列化结果值赋给变量i
            FileStream file = System.IO.File.OpenRead(fileName);
            List<string> list = (List<string>)serializer.Deserialize(file);
            file.Close();
            return list;
        }
        private void saveHistory()
        {

            //to history
            int i = 0;
            int foundIndex = -1;
            foreach (ToolStripMenuItem menu in latestPdmMenu.DropDownItems)
            {
                if (menu.Tag.ToString().Equals(latestFileName, StringComparison.OrdinalIgnoreCase))
                {
                    foundIndex = i;
                    break;
                }
                i++;
            }
            if (foundIndex == -1)
            {
                if (latestPdmMenu.DropDownItems.Count >= MAX_HISTORY_COUNT)
                {
                    //太多了移走一个
                    latestPdmMenu.DropDownItems[latestPdmMenu.DropDownItems.Count - 1].Click -= this.openFile;
                    latestPdmMenu.DropDownItems.RemoveAt(latestPdmMenu.DropDownItems.Count - 1);
                }
                ToolStripMenuItem historyMenu = new ToolStripMenuItem();
                historyMenu.Text = (latestPdmMenu.DropDownItems.Count + 1) + " " + latestFileName;
                historyMenu.Tag = latestFileName;
                historyMenu.Click += new System.EventHandler(this.openFile);
                latestPdmMenu.DropDownItems.Add(historyMenu);
                foundIndex = latestPdmMenu.DropDownItems.Count - 1;
            }
            if (foundIndex > 0)
            {
                //说明找到了，需要移动到第一个
                for (int k = foundIndex; k > 0; k--)
                {
                    string fileName = latestPdmMenu.DropDownItems[k - 1].Tag.ToString();
                    latestPdmMenu.DropDownItems[k].Text = (k + 1) + " " + fileName;
                    latestPdmMenu.DropDownItems[k].Tag = fileName;
                }
                latestPdmMenu.DropDownItems[0].Text = "1 " + latestFileName;
                latestPdmMenu.DropDownItems[0].Tag = latestFileName;

                //save

                List<string> list = new List<string>();
                foreach (ToolStripMenuItem menu in latestPdmMenu.DropDownItems)
                {
                    list.Add(menu.Tag.ToString());
                }

                string fileNamex = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PDM_HISTORY);
                if (File.Exists(fileNamex)) File.Delete(fileNamex);
                XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                //反序列化，并将反序列化结果值赋给变量i
                FileStream file = System.IO.File.OpenWrite(fileNamex);
                serializer.Serialize(file, list);
                file.Close();
            } 


        }

        //正确打开后加入历史，否则不加入历史
        private string latestFileName = null;
        private void openFile(object sender, EventArgs e)
        {
            string fileName = ((ToolStripMenuItem)sender).Tag.ToString();
            PdmReader reader = new PdmReader();
            Entity model = reader.Read(fileName);

            Context.ModelToTree(model);
            latestFileName = fileName;
            saveHistory();
        }
        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "pdm文件|*.pdm";
            f.FileName = latestFileName;
            if (f.ShowDialog() != DialogResult.OK) return;
            latestFileName = f.FileName;

            PdmReader reader = new PdmReader();
            Entity model = reader.Read(latestFileName);
            //Entity model = reader.Read(@"D:\TitanDocument\Titan.pdm");

            Context.ModelToTree(model);


            saveHistory();
        }
    }
}
