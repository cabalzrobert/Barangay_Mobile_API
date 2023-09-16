using System;
using System.ComponentModel.DataAnnotations;
namespace webapi.App.RequestModel.AppRecruiter
{
    public class SignInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
        public bool Terminal { get; set; }

        //[Required]
        public string ApkVersion { get; set; }

        //[Required]
        public string CoordinateLocation { get; set; }
        //[Required]
        public string AddressLocation { get; set; }
    }
    public class Donation
    {
        public string PL_ID;
        public string PGRP_ID;
        public double Amount;
        public string Purpose;
        public string DateReceived;
        public string iUSR_ID;
        public string DonoID;
        public string OTP;
        public string MobileNo;
        public string REF_GRP_ID;
        public string REF_LDR_ID;
    }
    public class STLSignInRequest
    {
        public string Username;
        public string Password;
        public string Password1;
        public string plid;
        public string groupid;
        public string psncd;
        public string ApkVersion;
        public string DeviceID;
        public string DeviceName;
    }
    public class RequiredChangePassword
    {
        public string PLID;
        public string PGRPID;
        public string Username;
        public string OldPassword;
        public string Password;
        public string ConfirmPassword;
    }
}
