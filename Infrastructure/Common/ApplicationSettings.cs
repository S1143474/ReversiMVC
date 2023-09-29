using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;

namespace Infrastructure.Common
{
    public class ApplicationSettings
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
