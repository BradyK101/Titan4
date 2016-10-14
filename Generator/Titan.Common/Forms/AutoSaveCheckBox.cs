using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan.Common.Forms
{
    public class AutoSaveCheckBox:CheckBox
    { 

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Config.ConfigToCheckBox(this, this);


            Form f = FindForm();
            f.FormClosing += FormClosing;
        }
        private void FormClosing(object sender, FormClosingEventArgs e)
        { 
            Config.CheckBoxToConfig(this, this);
        }
    }
}
