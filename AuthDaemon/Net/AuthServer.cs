using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;

namespace AuthDaemon.Net
{
    public delegate void SessionHandler<T>(object sender, T session);
    public class AuthServer<T> where T : AuthSession, new()
    {
        protected object startLock = new object();

        static Logger Log = LogManager.GetLogger("authd");

        public event SessionHandler<T> SessionPreAccepted = (a, b) => { };
        public event SessionHandler<T> SessionAccepted = (a, b) => { };
        public event SessionHandler<T> SessionStopped = (a, b) => { };

        public Socket BaseSocket { get; protected set; }
        public bool Started { get; protected set; }
        public IPEndPoint LocalEndPoint { get; protected set; }

        public object State { get; set; }
        
        public virtual void Start(IPEndPoint endPoint)
        {
            lock (startLock)
            {
                if (Started)
                {
                    return;
                }
                LocalEndPoint = endPoint;
                BaseSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                BaseSocket.Bind(endPoint);
                BaseSocket.Listen(100);

                Started = true;
                BeginAccept(BaseSocket);

                Log.Info("Server started on port {0}", endPoint.Port);
            }
        }
        public virtual void Stop()
        {
            lock(startLock)
            {
                if (!Started)
                {
                    return;
                }
                DisposeSocket(BaseSocket);
                Started = false;

                Log.Info("Server stoped");
            }
        }

        protected virtual void BeginAccept(Socket skt)
        {
            if (!Started || skt != BaseSocket)
            {
                DisposeSocket(skt);
                return;
            }
            try
            {
                skt.BeginAccept(EndAccept, skt);
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
            }
        }
        protected virtual void EndAccept(IAsyncResult e)
        {
            var skt = e.AsyncState as Socket;
            Socket client = null;
            try
            {
                client = skt.EndAccept(e);
                Log.Debug("New connection");
            }
            catch (Exception ex)
            {
                DisposeSocket(client);
                DisposeSocket(skt);
                Log.Debug(ex);
                return;
            }
            if (!Started || skt != BaseSocket)
            {
                DisposeSocket(client);
                DisposeSocket(skt);
                Log.Debug("EndAccept: (!Started || skt != BaseSocket)");
                return;
            }
            BeginAccept(skt);
            ProcessConnection(client);
        }
        protected static void DisposeSocket(Socket skt)
        {
            Log.Debug("[server] Dispose socket");
            try
            {
                skt.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            try
            {
                skt.Close();
            }
            catch
            {

            }
        }
        protected virtual void ProcessConnection(Socket client)
        {
            T session = new T();
            session.ClientState = new SocketStateObject(session, client, 1024);
            ProcessSession(session);
        }
        private int sessionId = 0;
        protected virtual void ProcessSession(T session)
        {
            session.Stopped += OnSessionStopped;

            session.Id = ++sessionId;
            session.PacketsRegistry = new PacketsRegistry();
            session.Plugins = new PluginManager(session);
            session.Handler = new PacketsHandler();
            session.State = State;

            SessionPreAccepted(this, session);
            session.Initialize();
            SessionAccepted(this, session);

            session.ClientState.BeginReceive();
        }
        protected virtual void OnSessionStopped(object sender, EventArgs e)
        {
            T session = (T)sender;
            SessionStopped(this, session);
        }
    }
}
