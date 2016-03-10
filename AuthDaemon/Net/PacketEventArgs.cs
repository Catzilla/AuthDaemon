// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthDaemon.Net
{
    public class PacketEventArgs : EventArgs
    {
        public uint PacketId { get; set; }
        public dynamic Packet { get; set; }

        public PacketEventArgs(uint packetId, dynamic gamePacket)
        {
            PacketId = packetId;
            Packet = gamePacket;
        }

        public override string ToString()
        {
            return PacketId.ToString();
        }
    }
}
