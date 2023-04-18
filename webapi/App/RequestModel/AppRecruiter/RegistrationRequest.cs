using System;
using System.ComponentModel.DataAnnotations;
namespace webapi.App.RequestModel.AppRecruiter
{
    public class RegistrationRequest
    {
        public string RegisterID;
        public string Type;
        public string Role;
        public string GeneralCoordinator;
        public string Coordinator;

        public string Firstname;
        public string Middlename;
        public string Lastname;
        public string Fullname;

        public string BirthDate;
        public string Gender;
        public string BloodType;
        public string Nationality;
        public string Citizenship;
        public string MaritalStatus;

        public string MobileNumber;
        public string EmailAddress;
        public string Address;
        public string Occupation;
        public string Skills;

        public string Region;
        public string Province;
        public string Municipality;
        public string Barangay;

        //
        public string Img;
        public string ImageUrl;
    }
    public class Sitio
    {
        public string LOC_REG;
        public string LOC_REG_NM;
        public string LOC_PROV;
        public string LOC_PROV_NM;
        public string LOC_MUN;
        public string LOC_MUN_NM;
        public string ID;
        public string LOC_BRGY_NM;
        public string SIT_ID;
        public string SIT_NM;
    }
    public class Barangay
    {
        public string Code;
        public string Name;
        public string ID;
    }
    public class Group
    {
        public string PL_ID;
        public string PGRP_ID;
        public string USR_ID;
    }
    public class EducAttainment
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Userid;
        public int SEQ_NO;
        public string EducLevel;
        public string School;
        public string SchoolAddress;
        public string SchoolYear;
        public string Course;
    }
    public class Organization 
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Userid;
        public int SEQ_NO;
        public string OrganizationName;
        public string OrganizationID;
        public string OrganizationAbbr;
        public string Estabalished;
        public string Search;
        public string NextFilter;
    }

    public class Employment_History
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Userid;
        public int SEQ_NO;
        public string Company;
        public string CompanyAddress;
        public string RenderedFrom;
        public string RenderedTo;
    }
    public class Government_Valid_ID
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Userid;
        public string GovValID;
        public string GovernmentID;
        public string GovValIDNo;
        public string GovValIDURL;
        public string Img;
        public string Attachments;
        public string Search;
        public string NextFilter;
        public string num_row;
        public int SEQ_NO;
    }
    public class Events
    {
        public string PL_ID;
        public string PGRP_ID;
        public string Title;
        public string Description;
        public string EventDate;
        public string EventTime;
        public string Location;
        public string EventID;
        public string ProccessID;
    }
    public class STLMembership
    {
        public string PL_ID;
        public string PGRP_ID;
        public string GroupRef;
        public string SiteLeader;
        public string Userid;
        public string ACT_ID;

        public string Firstname;
        public string Lastname;
        public string Middlename;
        public string Nickname;

        public string BirthDate;
        public string Gender;
        public string BloodType;
        public string Nationality;
        public string Citizenship;
        public string MaritalStatus;

        public string MobileNumber;
        public string EmailAddress;
        public string PrecentNumber;
        public string ClusterNumber;
        public string HomeAddress;
        public string PresentAddress;
        public string Occupation;
        public string Skills;

        public string Region;
        public string Province;
        public string Municipality;
        public string Barangay;
        public string Sitio;
        public string SitioName;
        public string LocationSite;
        public string isLeader;
        public string isMember;


        public string AccountType;
        public string Username;
        public string Userpassword;
        public string SubType;
        public string Type;


        public string Img;
        public string ImageUrl;
    }

    public class BrgyClearance
    {
        public string ClearanceID;
        public string ClearanceNo;
        public string ControlNo;
        public string TypeofClearance;
        public string TypeofClearanceNM;
        public string PurposeID;
        public string Purpose;
        public string ORNumber;
        public string AmountPaid;
        public string DocStamp;
        public int EnableCommunityTax;
        public string CTCNo;
        public string CTCIssuedAt;
        public string CTCIssuedOn;
        public string DocumentID;
        public string UserID;
        public string Requestor;
        public string RequestorNM;

        public string IssuedDate;
        public string ExpiryDate;
        public int MosValidity;
        public int Status;
        public string AppointmentDate;
        public string ApplicationDate;
        public string num_row;
    }
    public class BrgyBusinessClearance
    {
        public string AmountPaid;
        public string ApplicationDate;
        public string AppointmentDate;
        public string Birthdate;
        public string BusinessAddress;
        public string BusinessClearanceID;
        public string BusinessID;
        public string BusinessNM;
        public string CTCIssuedAt;
        public string CTCIssuedOn;
        public string CTCNo;
        public string Cancelled;
        public string CancelledBy;
        public string CancelledDate;
        public string CertifiedBy;
        public string ControlNo;
        public string DateIssued;
        public string DateOperate;
        public string DateRelease;
        public string DisplayName;
        public string DocStamp;
        public string EnableCommunityTax;
        public string ExpiryDate;
        public string IssuedDate;
        public string MobileNo;
        public string MosValidity;
        public string ORNumber;
        public string OwnerAddress;
        public string OwnerID;
        public string OwnerNM;
        public string PGRP_ID;
        public string PL_ID;
        public string ProfilePicture;
        public string Release;
        public string ReleasedBy;
        public string StatusRequest;
        public string TotalAmount;
        public string VerifiedBy;
        public string Status;
    }
    public class LegalDocument_Transaction
    {
        public string LegalDocumentID;
        public string ControlNo;
        public string TemplateTypeID;
        public string TemplateTypeNM;
        public string TemplateID;
        public string TemplateNM;
        public string Requestor;
        public string RequestorNM;
        public string ApplicationDate;
        public string Search;
        public string DateFrom;
        public string DateTo;
        public string Status;
        public string StatusRequestName;
        public string num_row;
    }

}