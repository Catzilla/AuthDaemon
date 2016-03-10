// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon
{
    public static class Ext
    {
        public static readonly Type[] EmptyTypes = { };
        public static readonly string[] EmptyString = { };
        public static readonly byte[] EmptyBytes = { };
        public static readonly object[] EmptyObjects = { };

        public static string Safe(this string s)
        {
            return s == null ? string.Empty : s;
        }
        public static T Safe<T>(this object obj)
        {
            if (obj == null) return default(T);
            return (T)(dynamic)obj;
        }

        public static List<string> ParseArgs(string s)
        {
            var args = new List<string>();

            var pos = 0;
            while (pos < s.Length)
            {
                if (char.IsWhiteSpace(s[pos]))
                {
                    pos++;
                    continue;
                }
                if (pos + 1 < s.Length && s[pos] == s[pos + 1] && s[pos] == '/')
                {
                    args.Add(s.Substring(pos));
                    break;
                }
                if (s[pos] == '\"')
                {
                    var begin = pos;
                    var end = begin + 1;

                    while (s[end] != '\"')
                    {
                        if (s[end] == '\\')
                        {
                            end++;
                        }
                        end++;
                    }
                    var line = s.Substring(begin + 1, (end - begin - 1));
                    args.Add(line);
                    pos = end + 1;
                }
                else
                {
                    var end = pos;
                    while (end < s.Length && !char.IsWhiteSpace(s[end]))
                    {
                        end++;
                    }
                    var line = s.Substring(pos, end - pos);
                    args.Add(line);
                    pos = end;
                }
            }
            for (var i = 0; i < args.Count; i++)
            {
                args[i] = NDecode(args[i]);
            }
            return args;
        }
        public static string NDecode(string text, char cc = '\\')
        {
            var sb = new StringBuilder();
            var forceWrite = false;
            foreach (var ch in text)
            {
                if (forceWrite)
                {
                    forceWrite = false;
                    switch (ch)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case '*': sb.Append((char)58002); break;
                        case '0': sb.Append('\0'); break;
                        default: sb.Append(ch); break;
                    }
                    continue;
                }
                if (ch == cc)
                {
                    forceWrite = true;
                }
                else
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }
    }
}
