using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum PaymentType : byte
    {
        Click = 10,
        Payme = 20,
        Apellsin = 30,
        UzCard = 40,
        Humo = 50,
        Cash = 60,
        MasterCard = 70,
        VisaCard = 80
    }
}
