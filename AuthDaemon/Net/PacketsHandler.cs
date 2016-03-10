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
        protected Dictionary<uint, SortedSet<PacketHandlerContainer>> handlers;
        public PacketsHandler()
        {
            handlers = new Dictionary<uint, SortedSet<PacketHandlerContainer>>();
        }

        private int id = 0;
        public virtual void AddHandler(uint packetId, PacketEventHandler handler, int priority = 0)
        {
            SortedSet<PacketHandlerContainer> handlersList;
            if (!handlers.TryGetValue(packetId, out handlersList))
            {
                handlersList = new SortedSet<PacketHandlerContainer>();
                handlers.Add(packetId, handlersList);
            }
            var container = new PacketHandlerContainer(handler, priority, ++id);
            handlersList.Add(container);
        }
        public bool Contains(uint packetId)
        {
            return handlers.ContainsKey(packetId);
        }
        public PacketEventArgs HandlePacket(uint packetId, dynamic gamePacket)
        {
            SortedSet<PacketHandlerContainer> handlersList;
            if (handlers.TryGetValue(packetId, out handlersList))
            {
                var eventArgs = new PacketEventArgs(packetId, gamePacket);
                foreach (var handler in handlersList.Reverse())
                {
                    handler.Handler(this, eventArgs);
                    if (eventArgs.Cancel) break;
                }
                return eventArgs;
            }
            return null;
        }
    }
}
