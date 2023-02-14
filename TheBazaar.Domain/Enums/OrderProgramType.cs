using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum OrderProgramType : byte
    {
        Pending = 1,
        Processing = 5,
        Shipped = 10,
        Delivered = 15,
        Cancelled = 20,
        Returned = 25,
        Complete = 30,
    }
}
