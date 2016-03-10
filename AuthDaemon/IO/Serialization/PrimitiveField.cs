// ======================================================================
// Author: freepvps
// Github: https://github.com/FreePVPs/AuthDaemon/tree/master/AuthDaemon
// ======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.IO.Serialization
{
    public delegate object PrimitiveReader(DataStream ds);
    public delegate void PrimitiveWriter(DataStream ds, object obj);
    public class PrimitiveField : FieldBase
    {
        public PrimitiveReader Reader { get; private set; }
        public PrimitiveWriter Writer { get; private set; }

        public PrimitiveField(PrimitiveReader reader, PrimitiveWriter writer)
        {
            Reader = reader;
            Writer = writer;
        }

        public override void Serialize(DynamicSerializer serializer, DataStream ds, object value)
        {
            Writer(ds, value);
        }
        public override object Deserialize(DynamicSerializer serializer, DataStream ds)
        {
            return Reader(ds);
        }
    }
}
