using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04Laboratory_Matsevko.Tools
{
    class InvalidEmailException : Exception
    {
        public InvalidEmailException() { }
        public InvalidEmailException(string email) : base(String.Format("Wrong email format: {0}\nMust be text@mydomain.com", email))
        {

        }
    }
}
