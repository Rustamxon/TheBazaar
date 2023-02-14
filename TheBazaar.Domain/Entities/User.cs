using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBazaar.Domain.Commons;
using TheBazaar.Domain.Enums;

namespace TheBazaar.Domain.Entities
{
    public class User : Auditable
    {
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole role { get; set; }
        public List<string> LastFiveSearch { get; set; }
    }
}
