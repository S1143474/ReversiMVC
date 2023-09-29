using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Responses
{
    public class GoogleCaptchaResponse
    {
        public bool Success { get; set; }

        public double Score { get; set; }

    }
}
