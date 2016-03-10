// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthDaemon.IO;
using System.IO;

namespace AuthDaemon.Net
{
    public class PacketsHandler
    {
        protected Dictionary<uint, List<PacketEventHandler>> handlers;
        public PacketsHandler()
        {
            handlers = new Dictionary<uint, List<PacketEventHandler>>();
        }
        
        public virtual void AddHandler(uint packetId, PacketEventHandler handler)
        {
            List<PacketEventHandler> handlersList;
            if (!handlers.TryGetValue(packetId, out handlersList))
            {
                handlersList = new List<PacketEventHandler>();
                handlers.Add(packetId, handlersList);
            }
            handlersList.Add(handler);
        }
        public bool Contains(uint packetId)
        {
            return handlers.ContainsKey(packetId);
        }
        public PacketEventArgs HandlePacket(uint packetId, dynamic gamePacket)
        {
            List<PacketEventHandler> handlersList;
            if (handlers.TryGetValue(packetId, out handlersList))
            {
                var eventArgs = new PacketEventArgs(packetId, gamePacket);
                foreach (var handler in handlersList)
                {
                    handler(this, eventArgs);
                }
                return eventArgs;
            }
            return null;
        }
    }
}
