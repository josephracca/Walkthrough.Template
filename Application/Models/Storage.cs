using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public struct Storage
    {
        public bool Succeeded { get; set; }
        public string FileName { get; set; }
        public string Hash { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}
