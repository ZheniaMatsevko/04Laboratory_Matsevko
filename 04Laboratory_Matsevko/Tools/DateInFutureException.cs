using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04Laboratory_Matsevko.Tools
{
    class DateInFutureException : Exception
    {
        public DateInFutureException() { }
        public DateInFutureException(DateTime date) : base(String.Format("Person isn't born while date is in future: {0}", date.ToShortDateString()))
        {

        }
    }
}
