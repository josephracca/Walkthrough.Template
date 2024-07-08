using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class HashSaltDto
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
