using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDaemon.IO.Serialization
{
    public abstract class FieldBase
    {
        public virtual void Serialize(DynamicSerializer serializer, DataStream ds, object value)
        {
            throw new NotImplementedException();
        }
        public virtual object Deserialize(DynamicSerializer serializer, DataStream ds)
        {
            throw new NotImplementedException();
        }
    }
}
