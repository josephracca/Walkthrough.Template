using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class CustomException : ApplicationException
    {
        public CustomException(int statusCode, string message) : base(message)
        {
            base.Data.Add("StatusCode", statusCode);
        }
    }
}
