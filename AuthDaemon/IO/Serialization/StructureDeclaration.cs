using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.IO.Serialization
{
    // int x;
    // byte[] name;
    // byte[4] ip;
    public class StructureDeclaration : FieldBase
    {
        public List<KeyValuePair<string, string>> Fields { get; private set; }
        public StructureDeclaration()
        {
            Fields = new List<KeyValuePair<string, string>>();
        }

        public override object Deserialize(DynamicSerializer serializer, DataStream ds)
        {
            DynamicStructure dynvalue = new DynamicStructure();

            foreach(var field in Fields)
            {
                object vv = serializer.ReadField(field.Value, ds);
                dynvalue.Dictionary[field.Key.ToLower()] = vv;
            }
            return dynvalue;
        }
        public override void Serialize(DynamicSerializer serializer, DataStream ds, object value)
        {
            DynamicStructure dynvalue = (DynamicStructure)value;
            
            foreach(var field in Fields)
            {
                object vv;
                if (dynvalue != null)
                {
                    dynvalue.Dictionary.TryGetValue(field.Key.ToLower(), out vv);
                }
                else
                {
                    vv = null;
                }

                serializer.WriteField(field.Value, ds, vv);
            }
        }
    }
}
