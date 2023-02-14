using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBazaar.Domain.Enums
{
    public enum QuestionProgressType
    {
        sending = 1,
        answered = 5,
        completed = 10,
        noasnwered = 15,
        deleted = 20,
        edited = 25
    }
}
