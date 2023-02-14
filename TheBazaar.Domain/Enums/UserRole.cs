using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum UserRole : byte
    {
        WarehouseWorker = 1,
        DeliveryToWarehouse = 5,
        Seller = 10,
        Client = 15,
        DeliveryFromWarehouse = 20,
        DeliveryFromMagazineToClient = 25,
    }
}
