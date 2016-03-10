using System;
using System.Collections.Generic;
using System.Reflection;
using AuthDaemon.IO;
using AuthDaemon.IO.Serialization;
using System.Globalization;
using System.Linq;

namespace AuthDaemon.Net
{
    public class PacketsRegistry
    {
        public static IEnumerable<char> GetPacketFlags(string packetId)
        {
            var index = 0;
            while(index < packetId.Length && !char.IsDigit(packetId[index]))
            {
                yield return packetId[index];
                index++;
            }
        }
        public static uint ParsePacketId(string packetId)
        {
            var index = 0;
            while(index < packetId.Length && !char.IsDigit(packetId[index]))
            {
                index++;
            }
            if (index != 0)
                packetId = packetId.Substring(index);
            
            if (packetId.StartsWith("0x"))
            {
                return uint.Parse(packetId.Substring(2), NumberStyles.HexNumber);
            }
            else
            {
                return uint.Parse(packetId);
            }
        }

        public DynamicSerializer Serializer { get; private set; }
        public Dictionary<string, FieldBase> PacketsDeclaration { get; private set; }
        public PacketsRegistry()
        {
            Serializer = new DynamicSerializer();
            PacketsDeclaration = new Dictionary<string, FieldBase>();
        }

        public static string GetKey(char flag, uint packetId)
        {
            return flag + packetId.ToString();
        }

        public void Register(string packetId, FieldBase declaration)
        {
            var flags = PacketsRegistry.GetPacketFlags(packetId);
            var id = PacketsRegistry.ParsePacketId(packetId);

            var count = 0;
            foreach (var flag in flags)
            {
                count++;
                PacketsDeclaration[GetKey(flag, id)] = declaration;
            }

            if (count == 0)
            {
                PacketsDeclaration[GetKey('i', id)] = declaration;
                PacketsDeclaration[GetKey('o', id)] = declaration;                
            }
        }

        public bool Serialize(uint packetId, DataStream ds, object value, char flag = 'o')
        {
            var key = GetKey(flag, packetId);

            FieldBase field;
            if (!PacketsDeclaration.TryGetValue(key, out field))
            {
                return false;
            }

            field.Serialize(Serializer, ds, value);
            return true;
        }
        public object Deserialize(uint packetId, DataStream ds, char flag = 'i')
        {
            var key = GetKey(flag, packetId);

            FieldBase field;
            if (!PacketsDeclaration.TryGetValue(key, out field))
            {
                return null;
            }

            return field.Deserialize(Serializer, ds);
        }
    }
}
