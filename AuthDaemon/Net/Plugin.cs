using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace AuthDaemon.Net
{
    public class Plugin
    {
        public AuthSession Session { get; internal set; }

        public virtual string Name { get { return string.Empty; } }
        public virtual string ConfigBlock { get { return "GAuthServer"; } }
        public virtual bool Enabled { get; set; }

        protected virtual string GetVar(string name)
        {
            var block = Session.Config[ConfigBlock];
            if (block == null) return string.Empty;
            return block[name].Safe();
        }
        public virtual Logger Log
        {
            get
            {
                return LogManager.GetLogger(Name);
            }
        }

        public virtual void Initialize()
        {
        }
    }
}
