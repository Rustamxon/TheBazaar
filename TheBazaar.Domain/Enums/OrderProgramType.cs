using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum OrderProgramType : byte
    {
        Pending = 10,
        Processing = 20,
        Delivered = 30,
        Complete = 40,
        Cancelled = 50
    }
}
