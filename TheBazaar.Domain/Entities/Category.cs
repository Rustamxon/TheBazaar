using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Domain.Commons;

namespace TheBazaar.Domain.Entities
{
    public class Category : Auditable
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
