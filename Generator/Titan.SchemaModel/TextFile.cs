using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.SchemaModel
{
    public class TextFile
    {
        public static void Write(string fileName, string text)
        {
            Write(fileName, text, Encoding.Default);
        }
        public static void Write(string fileName, string text, Encoding encoding)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }


            System.IO.StreamWriter sr = new System.IO.StreamWriter(fileName, true, encoding);
            sr.WriteLine(text);
            sr.Close();
        }
    }
}
