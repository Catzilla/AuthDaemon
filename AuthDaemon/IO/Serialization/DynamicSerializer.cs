// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AuthDaemon.IO.Serialization
{
    public class DynamicSerializer
    {
        static Logger Log = LogManager.GetLogger("authd");

        public Dictionary<string, FieldBase> KnownFields { get; private set; }

        public DynamicSerializer()
        {
            KnownFields = new Dictionary<string, FieldBase>
            {
                { "byte", new PrimitiveField(x => x.ReadByte(), (ds, x) => ds.PushBack(x.Safe<byte>())) },
                { "sbyte", new PrimitiveField(x => x.ReadSByte(), (ds, x) => ds.Write(x.Safe<sbyte>())) },
                { "bool", new PrimitiveField(x => x.ReadBoolean(), (ds, x) => ds.Write(x.Safe<bool>())) },

                { "short", new PrimitiveField(x => x.ReadInt16(), (ds, x) => ds.Write(x.Safe<short>())) },
                { "int", new PrimitiveField(x => x.ReadInt32(), (ds, x) => ds.Write(x.Safe<int>())) },
                { "long", new PrimitiveField(x => x.ReadInt64(), (ds, x) => ds.Write(x.Safe<long>())) },

                { "ushort", new PrimitiveField(x => x.ReadUInt16(), (ds, x) => ds.Write(x.Safe<ushort>())) },
                { "uint", new PrimitiveField(x => x.ReadUInt32(), (ds, x) => ds.Write(x.Safe<uint>())) },
                { "ulong", new PrimitiveField(x => x.ReadUInt64(), (ds, x) => ds.Write(x.Safe<ulong>())) },

                { "cuint", new PrimitiveField(x => x.ReadCompactUInt32(), (ds, x) => ds.WriteCompactUInt32(x.Safe<uint>())) },

                { "float", new PrimitiveField(x => x.ReadSingle(), (ds, x) => ds.Write(x.Safe<float>())) },
                { "double", new PrimitiveField(x => x.ReadDouble(), (ds, x) => ds.Write(x.Safe<double>())) },
            };
        }

        public void Register(string name, FieldBase field)
        {
            KnownFields[name] = field;
        }

        private bool IsArray(string type)
        {
            return type.EndsWith("]");
        }
        private int GetArrayLength(string type)
        {
            if (type.EndsWith("[]"))
            {
                return -1; // read length from stream
            }

            var index1 = type.IndexOf('[');
            if (index1 == -1) return -1;

            var index2 = type.IndexOf(']', index1);
            if (index2 == -1) return -1;

            index1++;

            return int.Parse(type.Substring(index1, index2 - index1));
        }

        public object ReadField(string type, DataStream ds)
        {
            if (IsArray(type))
            {
                var length = GetArrayLength(type);
                var ntype = type.Substring(0, type.IndexOf('['));
                var field = KnownFields[ntype];

                if (length == -1)
                {
                    length = (int)ds.ReadCompactUInt32();
                }

                if (length <= 0) return null;

                var first = field.Deserialize(this, ds);
                var array = Array.CreateInstance(first.GetType(), length);
                array.SetValue(first, 0);

                for(var i = 1; i < length; i++)
                {
                    var value = field.Deserialize(this, ds);
                    array.SetValue(value, i);
                }
                return array;
            }
            else
            {
                var field = KnownFields[type];
                return field.Deserialize(this, ds);
            }
        }
        public void WriteField(string type, DataStream ds, object value)
        {
            if (IsArray(type))
            {
                var array = (Array)value;
                var length = GetArrayLength(type);
                var ntype = type.Substring(0, type.IndexOf('['));
                var field = KnownFields[ntype];

                if (length == -1) // dynamic length
                {
                    if (array == null) ds.WriteCompactUInt32(0);
                    else
                    {
                        ds.WriteCompactUInt32(array.Length);
                        length = array.Length;
                    }
                }
                for (var i = 0; i < length; i++)
                {
                    field.Serialize(this, ds, array.GetValue(i));
                }
            }
            else
            {
                var field = KnownFields[type];
                field.Serialize(this, ds, value);
            }
        }
    }
}
