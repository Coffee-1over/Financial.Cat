using Financial.Cat.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Domain.Models.Business.AuthModels.Otp
{
    public class GenerateAndSendOtpResult
    {
		public GenerateAndSendOtpResult(OtpEntity otp, string otpCode)
		{
			Otp = otp;
			OtpCode = otpCode;
		}
		public OtpEntity Otp { get; set; }
        public string OtpCode { get; set; }
    }
}
