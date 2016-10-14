using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.SchemaModel
{ 
    public class TitanTypeDescriptionProvider : TypeDescriptionProvider
    {
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            TypeDescriptionProvider _baseProvider = TypeDescriptor.GetProvider(objectType);
            return new TitanCustomTypeDescriptor(  instance);
        }
    }
    public class TitanCustomTypeDescriptor : CustomTypeDescriptor
    {
        //private static HashSet<string> filter = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static HashSet<string> filter = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Properties", "CreationDate", "Id", "ObjectID", "ModificationDate", "Modifier", "Creator", "ExtendedAttributesText2" };

        //private Type objectType;
        private object instance;
        public TitanCustomTypeDescriptor(object instance)
        {
            this.instance = instance;
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            //HashSet<string> added = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            //PropertyDescriptorCollection origpds = TypeDescriptor.GetProperties(instance);
            //foreach (PropertyDescriptor origpd in origpds)
            //{
            //    if (!filter.Contains(origpd.Name))
            //    {
            //        pds.Add(origpd);
            //        added.Add(origpd.Name);
            //    }
            //}
 

            // Iterate the list of employees
            Entity entity=(Entity)instance ;
            foreach (KeyValuePair<string, object> kv in entity.Properties)
            {
                if (!filter.Contains(kv.Key))
                {
                    TitanPropertyDescriptor pd = new TitanPropertyDescriptor(entity, kv.Key);
                    pds.Add(pd);
                }
            }
            return pds;
        }
    }

    public class TitanPropertyDescriptor : PropertyDescriptor
    {
        public TitanPropertyDescriptor(Entity instance, string propertyName)
            : base(propertyName, null)
        {
            this.componentType = typeof(Entity);
            this.propertyName = propertyName;
            this.propertyType = GetValue(instance).GetType();
        }


        private Type componentType;
        private Type propertyType;
        private string propertyName;


        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return componentType; }
        }

        public override object GetValue(object component)
        {

            return ((Entity)component).Properties[propertyName];

        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return propertyType; }
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {
            ((Entity)component).Properties[propertyName] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

}
