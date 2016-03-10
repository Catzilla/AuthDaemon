using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace AuthDaemon.Data
{
    public class MySqlQuery
    {
        public string Name { get; set; }
        public Dictionary<string, string> Tables { get; private set; }
        public Dictionary<string, string> Selects { get; private set; }
        public Dictionary<string, string> Columns { get; private set; }
        public MySqlQuery()
        {
            Tables = new Dictionary<string, string>();
            Selects = new Dictionary<string, string>();
            Columns = new Dictionary<string, string>();
        }
        public string Select(string name)
        {
            var query = string.Format("SELECT {0} FROM {1} {2}", string.Join(",", Columns.Select(x => x.Value + " as " + x.Key)), string.Join(",", Tables.Values), Selects[name]);
            return query;
        }
        public static IEnumerable<MySqlQuery> Load(string path)
        {
            foreach (var query in ReadQueriesFromXml(XmlReader.Create(File.OpenRead(path))))
            {
                yield return query;
            }
        }
        public static IEnumerable<MySqlQuery> ReadQueriesFromXml(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.IsStartElement() && xml.Name == "query")
                {
                    yield return ReadFromXml(xml);
                }
            }
        }
        public static MySqlQuery ReadFromXml(XmlReader reader)
        {
            var query = new MySqlQuery();
            do
            {
                var name = reader.Name;
                switch (name)
                {
                    case "query":
                        query.Name = reader.GetAttribute("name");
                        continue;
                    case "table":
                        query.Tables[reader.GetAttribute("alias")] = reader.GetAttribute("name");
                        continue;
                    case "column":
                        {
                            var column = reader.GetAttribute("column");
                            var index = column.IndexOf('.');
                            if (index != -1)
                            {
                                var table = column.Substring(0, index);
                                if (query.Tables.ContainsKey(table))
                                {
                                    column = query.Tables[table] + column.Substring(index);
                                }
                            }
                            query.Columns[reader.GetAttribute("name")] = column;
                        }
                        continue;
                    case "select":
                        {
                            var select = query.Name + "." + reader.GetAttribute("name");
                            var condition = reader.GetAttribute("condition");
                            foreach (var table in query.Tables)
                            {
                                condition = condition.Replace(table.Key + ".", table.Value + ".");
                            }

                            var args = condition.Split('?');
                            var sb = new StringBuilder();
                            var id = 0;
                            foreach (var arg in args)
                            {
                                if (arg != args.First())
                                {
                                    sb.Append("@argument" + (id++));
                                }
                                sb.Append(arg);
                            }
                            query.Selects[select] = sb.ToString();
                        }
                        continue;
                }
            } while (reader.Read() && reader.IsStartElement());

            return query;
        }
    }
}
