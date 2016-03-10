// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthDaemon.Net;
using System.IO;
using AuthDaemon.IO.Serialization;

namespace AuthDaemon
{
    public static class ProtocolParser
    {
        public static void Parse(PacketsRegistry packetsRegistry, string[] protocolLines)
        {
            var structures = new List<string>();

            var sb = new StringBuilder();
            foreach(var line in protocolLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    var newStruct = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(newStruct))
                    {
                        structures.Add(newStruct);
                    }

                    sb.Clear();
                }
                else
                {
                    sb.AppendLine(line);
                }
            }
            var end = sb.ToString();
            if (!string.IsNullOrWhiteSpace(end))
            {
                structures.Add(end);
            }

            foreach(var structure in structures)
            {
                ParseStructure(packetsRegistry, structure);
            }
        }
        public static void ParseStructure(PacketsRegistry packetsRegistry, string structure)
        {
            var reader = new StringReader(structure.Trim());

            var head = reader.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var type = head[0];
            var id = head[1];
            if (id.EndsWith(":")) id = id.Substring(0, id.Length - 1);

            var declaration = new StructureDeclaration();
            while(true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                var field = Ext.ParseArgs(line);

                var fieldType = field[0];
                var fieldName = field[1];

                if (fieldName.EndsWith(";")) fieldName = fieldName.Substring(0, fieldName.Length - 1);

                declaration.Fields.Add(new KeyValuePair<string, string>(fieldName, fieldType));
            }

            switch(type)
            {
                case "packet": packetsRegistry.Register(id, declaration); break;
                case "struct":
                case "type": packetsRegistry.Serializer.Register(id, declaration); break;
            }
        }
    }
}
