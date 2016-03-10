using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.Net
{
    public class PacketHandlerContainer : IComparable<PacketHandlerContainer>
    {
        public PacketEventHandler Handler { get; private set; }
        public int Priority { get; private set; }
        public int Id { get; private set; }

        public PacketHandlerContainer(PacketEventHandler handler, int priority, int id)
        {
            Handler = handler;
            Priority = priority;
            Id = id;
        }

        public int CompareTo(PacketHandlerContainer other)
        {
            var res = Priority.CompareTo(other.Priority);
            if (res == 0) res = Id.CompareTo(other.Id);

            return res;
        }
    }
}
