using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Titan.SchemaModel
{
    public interface IPlugin
    {
        Context Context { get; set; }
        void Init( );
        void OnModelTreeClick();
    }
}
