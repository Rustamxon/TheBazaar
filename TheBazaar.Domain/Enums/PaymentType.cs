using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum PaymentType : byte
    {
        Click = 5,
        Payme = 10,
        Apellsin = 15,
        UzCard = 20,
        Humo = 25,
        Naqd = 30,
        MasterCard = 35,
        VisaCard = 40,
    }
}
