// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using AuthDaemon.IO;
using AuthDaemon.Net.Plugins;
using NLog;
using IniParser.Model;
using System.Reflection;
using System.IO;
using AuthDaemon.Data;
using System.Dynamic;

namespace AuthDaemon.Net
{
    public class AuthSession
    {
        public int Id { get; internal set; }
        protected static readonly Logger Log = LogManager.GetLogger("session");

        public SocketStateObject ClientState { get; set; }
        public PacketsRegistry PacketsRegistry { get; set; }
        public PacketsHandler Handler { get; set; }
        public PacketsHandler SendHandler { get; set; }
        public PacketsHandler SendCompleteHandler { get; set; }
        public PluginManager Plugins { get; set; }

        public Database Database { get; set; }
        
        public Socket Client
        {
            get
            {
                return ClientState.Connection;
            }
        }

        public event EventHandler Stopped = (a, b) => { };

        public object State { get; set; }


        const string MainBlock = "GAuthServer";

        public IniData Config { get; set; }

        private void InitProtocol()
        {
            var protocolPath = Config[MainBlock]["protocol"];

            if (!File.Exists(protocolPath))
            {
                Log.Fatal("Protocol file not found: {0}", protocolPath);
            }

            try
            {
                var lines = File.ReadAllLines(protocolPath);
                ProtocolParser.Parse(PacketsRegistry, lines);

                Log.Info("Init protocol complete");

                foreach(var packet in PacketsRegistry.PacketsDeclaration)
                {
                    Log.Trace("Registered packet: {0}", packet.Key);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }
        }
        private void InitPlugins()
        {
            var pluginType = typeof(Plugin);

            var plugins = new HashSet<string>(Config[MainBlock]["plugins"].Safe().Split(new[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
            var assembliesDir = Config[MainBlock]["assembliesdir"].Safe();

            var assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetCallingAssembly());

            try
            {
                if (!string.IsNullOrWhiteSpace(assembliesDir))
                {
                    foreach (var file in Directory.GetFiles(assembliesDir))
                    {
                        try
                        {
                            var asm = Assembly.LoadFrom(file);
                            assemblies.Add(asm);
                        }
                        catch (Exception ex)
                        {
                            Log.Debug(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    try
                    {
                        if (type.IsSubclassOf(pluginType))
                        {
                            var nplugin = type.GetConstructor(Ext.EmptyTypes).Invoke(Ext.EmptyObjects);
                            var name = (string)type.GetProperty("Name").GetValue(nplugin);

                            if (plugins.Contains(name))
                            {
                                Plugins.Register(type);
                                Log.Info("Plugin {0} registered", name);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
            foreach (var plugin in Plugins.ToArray())
            {
                if (!plugins.Contains(plugin.Name))
                {
                    Log.Info("! Plugin {0} registered", plugin.Name);
                }
            }
        }
        public virtual void Initialize()
        {
            Config = (IniData)State;
            InitProtocol();
            InitPlugins();
        }
        public virtual void Stop()
        {
            ClientState.Stop();
        }
        public virtual void OnDisconnected(SocketStateObject state)
        {
            if (!Client.Connected)
            {
                Stopped(this, EventArgs.Empty);
            }
            else
            {
                Stop();
            }
        }
        public virtual void ProcessStream(SocketStateObject state, DataStream ds)
        {
            var goodPos = 0;
            while (true)
            {
                var id = 0U;
                var length = 0U;
                if (!ds.TryReadCompactUInt32(out id) || !ds.TryReadCompactUInt32(out length) || !ds.CanReadBytes((int)length))
                {
                    break;
                }
                var packetStream = new DataStream(ds.ReadBytes((int)length));
                packetStream.IsLittleEndian = false;
                goodPos = ds.Position;

                ProcessPacketStream(state, id, packetStream);
            }
            ds.Position = goodPos;
            ds.Flush();
        }
        public virtual void ProcessPacketStream(SocketStateObject state, uint packetId, DataStream packetStream)
        {
            Log.ConditionalDebug("Packet: {0} (0x{0:X2})", packetId);
            if (Handler.Contains(packetId))
            {
                object packet = PacketsRegistry.Deserialize(packetId, packetStream);
                if (packet != null)
                {
                    Handler.HandlePacket(packetId, packet);
                }
            }
        }
        public virtual void Send(uint packetId, int connectionId, object argument, object result)
        {
            connectionId = (int)(((uint)connectionId) & 0x7FFFFFFF);

            dynamic d = new DynamicStructure();
            d.Id = connectionId;
            d.Res = result;
            d.Arg = argument;
            Send(packetId, d);
        }
        public virtual void Send(uint packetId, object packet)
        {
            var cancel = SendHandler.HandlePacket(packetId, packet)?.Cancel ?? false;
            if (cancel) return;

            var ds = new DataStream { IsLittleEndian = false };
            var ok = PacketsRegistry.Serialize(packetId, ds, packet);

            if (ok)
            {
                Send(packetId, ds);
                SendCompleteHandler.HandlePacket(packetId, packet);
            }
            else
            {
                Log.Error("Try to send unknown packet {0}", packetId);
            }
        }
        private DataStream sendStream = new DataStream();
        public virtual void Send(uint packetId, DataStream packetStream)
        {
            lock(sendStream)
            {
                sendStream.Clear();
                sendStream.WriteCompactUInt32(packetId);
                sendStream.Write(packetStream);

                ClientState.Send(sendStream.Buffer, 0, sendStream.Count);
            }
        }
    }
}
