using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Titan.SchemaModel;

namespace Titan.PdmSchemaReader
{
    public interface IExtensionReader
    {
        void Read(XmlDocument doc, XmlNamespaceManager ns, Entity model);
    }
}
