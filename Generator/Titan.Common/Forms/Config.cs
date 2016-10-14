using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan.Common.Forms
{
    static class Config
    {


        #region config
        public static void ConfigToCheckBox(Control control, CheckBox checkBox)
        {
            string value = Config.Get(FormNameToConfigFileName(control.FindForm()), checkBox.Name);
            bool b = false;
            if (bool.TryParse(value, out b))
            {
                checkBox.Checked = b;
            }

        }
        public static void CheckBoxToConfig(Control control, CheckBox checkBox)
        {
            Config.Set(FormNameToConfigFileName(control.FindForm()), checkBox.Name, checkBox.Checked.ToString());
        }
        public static void ConfigToComboBoxItems(Control control, ComboBox comboBox)
        {
            char[] splitChars = { '\r', '\n' };
            string value = Config.Get(FormNameToConfigFileName(control.FindForm()), control.Name);
            if (!string.IsNullOrEmpty(value))
            {
                string[] ss = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                comboBox.Items.Clear();
                comboBox.BeginUpdate();
                foreach (string s in ss)
                {
                    comboBox.Items.Add(s.Trim());
                }
                comboBox.EndUpdate();
                if (comboBox.Items.Count > 0)
                {
                    comboBox.Text = comboBox.Items[0].ToString();
                }
            }
        }
        public static void ComboBoxItemsToConfig(Control control, ComboBox comboBox)
        {
            Dictionary<string, bool> h = new Dictionary<string, bool>();
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(comboBox.Text))
            {
                h.Add(comboBox.Text, false);
                list.Add(comboBox.Text);
            }
            foreach (string s in comboBox.Items)
            {
                if (!h.ContainsKey(s.Trim()))
                {
                    h.Add(s.Trim(), false);
                    list.Add(s.Trim());
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= list.Count - 1; i++)
            {
                sb.Append(list[i]);
                sb.Append("\r\n");
            }

            Config.Set(FormNameToConfigFileName(control.FindForm()), control.Name, sb.ToString());
        }

        public static void ConfigToCheckedListBox(string configFile, CheckedListBox checkedListBox)
        {
            char[] splitChars = { ',' };
            string value = Config.Get(configFile, checkedListBox.Name);
            if (!string.IsNullOrEmpty(value))
            {
                string[] ss = value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    int index = int.Parse(s);
                    checkedListBox.SetItemChecked(index, true);
                }
            }
        }
        public static void CheckedListBoxToConfig(string configFile, CheckedListBox checkedListBox)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in checkedListBox.CheckedIndices)
            {
                sb.Append(i);
                sb.Append(",");
            }
            Config.Set(configFile, checkedListBox.Name, sb.ToString());
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFile">指定配置文件</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string configFile, string key)
        {


            ExeConfigurationFileMap file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = configFile;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);
            AppSettingsSection appSection = configuration.AppSettings;

            if (appSection.Settings[key] == null)
            {
                return null;
            }
            else
            {
                return appSection.Settings[key].Value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return Get(DefaultConfigFile, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string configFile, string key, string value)
        {
            ExeConfigurationFileMap file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = configFile;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);

            AppSettingsSection appSection = configuration.AppSettings;

            //赋值并保存
            if (appSection.Settings[key] == null)
            {
                appSection.Settings.Add(key, value);
            }
            else
            {
                appSection.Settings[key].Value = value;
            }
            configuration.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            Set(DefaultConfigFile, key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultConfigFile
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public static bool IsWeb
        //{
        //    get
        //    {
        //        string configFile = ConfigFile;
        //        return configFile.EndsWith("web.config", StringComparison.OrdinalIgnoreCase);
        //    }
        //}


        public static string FormNameToConfigFileName(System.Windows.Forms.Form form)
        {
            return form.GetType().FullName + ".config";
        }
    }
}
 