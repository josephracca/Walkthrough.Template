using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Email
    {
        public string? To { get; set; }
        public string? Cc { get; set; }
        public List<EmailAddress>? Ccs { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Attachment { get; set; }
        public List<string>? Attachments { get; set; }
    }
}
