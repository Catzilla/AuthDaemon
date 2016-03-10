using System;
using System.Net.Sockets;
using AuthDaemon.Net.Security;
using AuthDaemon.IO;
using NLog;

namespace AuthDaemon.Net
{
    /// <summary>Доп. состояние сокета</summary>
    public class SocketStateObject
    {
        static Logger Log = LogManager.GetLogger("authd");

        /// <summary>
        /// Базовая сессия
        /// </summary>
        public AuthSession Session { get; private set; }
        /// <summary>
        /// Источник.
        /// </summary>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Буффер источника.
        /// </summary>
        public byte[] FromBuffer { get; private set; }
        public DataStream FromStream { get; private set; }
        
        public Boolean Connected { get; private set; }

        public Encryptor Encryptor { get; set; }
        
        private readonly int bufferSize;

        /// <summary>
        /// Инициализация объекта состояния.
        /// </summary>
        /// <param name="connection">Источник</param>
        /// <param name="bufferSize">Размер буфера</param>
        public SocketStateObject(AuthSession session, Socket connection, int bufferSize)
        {
            Session = session;

            Connection = connection;
            Connected = true;

            this.bufferSize = bufferSize;
            FromBuffer = new byte[bufferSize];
            FromStream = new DataStream();

            Encryptor = Encryptor.Default;
        }

        public void ResetFromBuffer()
        {
            FromBuffer = new byte[bufferSize];
        }

        public void BeginReceive()
        {
            
            try
            {
                Connection.BeginReceive(FromBuffer, 0, FromBuffer.Length, SocketFlags.None, OnReceive, null);
            }
            catch (Exception ex)
            {
                Log.ConditionalDebug(ex);
                Stop();
            }
        }
        private void OnReceive(IAsyncResult e)
        {
            try
            {
                var length = Connection.EndReceive(e);
                if (length <= 0 || !Connection.Connected)
                {
                    Log.ConditionalDebug("OnReceive stop. Length: {0}, connected: {1}", length, Connection.Connected);
                    Stop();
                    return;
                }
                var data = Encryptor.Decrypt(FromBuffer, 0, length);
                FromStream.PushBack(data);
            }
            catch (Exception ex)
            {
                Log.ConditionalDebug(ex);
                Stop();
                return;
            }
            try
            {
                Session.ProcessStream(this, FromStream);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            BeginReceive();
        }

        public void Send(byte[] buffer)
        {
            Send(buffer, 0, buffer.Length);
        }
        public void Send(byte[] buffer, int offset, int length)
        {
            lock (Encryptor)
            {
                try
                {
                    var data = Encryptor.Encrypt(buffer, offset, length);
                    BeginSend(data);
                }
                catch(Exception ex)
                {
                    Stop();
                    Log.Fatal(ex);
                    return;
                }
            }
        }
        private void BeginSend(byte[] buffer)
        {
            BeginSend(buffer, 0, buffer.Length);
        }
        private void BeginSend(byte[] buffer, int pos, int length)
        {
            try
            {
                lock(Connection)
                {
                    if (!Connection.Connected)
                    {
                        return;
                    }

                    var bytesToSend = new byte[length];
                    Buffer.BlockCopy(buffer, pos, bytesToSend, 0, length);

                    Connection.BeginSend(bytesToSend, 0, length, SocketFlags.None, null, null);
                    //To.Send(bytesToSend, 0, length, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                Log.ConditionalDebug(ex);
                Stop();
                return;
            }
        }

        protected void DisposeSocket(Socket skt)
        {
            Log.Debug("[client] Dispose socket session id: " + Session.Id);
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
        public void Stop()
        {
            lock (Connection)
            {
                if (!Connected)
                {
                    return;
                }
                Connected = false;
            }
            DisposeSocket(Connection);
            Session.OnDisconnected(this);
        }
    }
}