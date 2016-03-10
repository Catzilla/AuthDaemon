using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace AuthDaemon.IO.Serialization
{
    public class DynamicStructure : DynamicObject
    {
        private object Value;
        private static DynamicStructure Default = new DynamicStructure();

        public DynamicStructure()
        {

        }
        public DynamicStructure(object value)
        {
            Value = value;
        }

        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public Dictionary<string, object> Dictionary
        {
            get
            {
                return dictionary;
            }
        }
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public bool Contains(string name)
        {
            return dictionary.ContainsKey(name.ToLower());
        }
        private static object emptyObject = 0;
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            var ok = dictionary.TryGetValue(name, out result);
            if (!ok)
            {
                if (binder.ReturnType.IsValueType)
                {
                    result = Activator.CreateInstance(binder.ReturnType);
                }
                else
                {
                    result = Default;
                }
            }
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name.ToLower()] = value;
            return true;
        }
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.ReturnType.IsValueType)
            {
                result = Activator.CreateInstance(binder.ReturnType);
            }
            else
            {
                result = null;
            }
            return true;
        }
    }
}
