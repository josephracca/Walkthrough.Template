using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class CaptchaVerificationResponse
    {
        public bool Success { get; set; }
        public decimal Score { get; set; }
        public string Action { get; set; }
        public DateTime Challenge_ts { get; set; }
        public string Hostname { get; set; }
    }
}
