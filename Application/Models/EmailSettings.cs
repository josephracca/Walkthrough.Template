using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ContactEmail { get; set; }
        public bool IsTest { get; set; }
        public string TestEmail { get; set; }
    }
}
