using System;
using System.Collections.Generic;

namespace webapi.App.RequestModel.Feature
{
    public class ReportAProblemRequest
    {
        public String TicketNo;
        public string TransactionNo;
        public String Sitio;
        public string SitioName;
        public string Latitude;
        public string Longitude;
        public String Subject;
        public String Body;
        public String CorrectiveAction;
        public List<String> Attachments;
        public String iAttachments;
        public String SenderAccount;
        public String SenderName;
        public String AddressLocation;
        public String PermanentAddress;
        public String DeviceID;
        public String DeviceName;
        public String Manufacturer;
        public String Serial;
        public String Brand;
        public String DeviceOS;
        public String DeviceVersion;
        public String IssuedDate;
        public String STAT;
    }
    public class ComplaintBlotter
    {
        public string CaseNo;
        public string ComplainantID;
        public string ComplainantName;
        public string Respondent;
        public string RespondentName;
        public string ComplaintType;
        public string IncidentPlace;
        public string IncidentDate;
        public string IncidentTime;
        public string Issue;
        public string Statement;
        public List<String> Attachments;
        public String iAttachments;
        public int num_row;
        public String From;
        public String To;
        public String Search;
        public String BaseFilter;
        public string PL_ID;
        public string PGRP_ID;
        public int isSummon;
        public int isComplain;
        public int isCancell;
        public int isRelease;
        public string Witness;

    }
    public class RequestDocument
    {
        public string ReqDocID;
        public string DoctypeID;
        public string BusinessName;
        public string BusinessAddress;
        public string Purpose;
        public string Type;
        public string BusinessOwnerAddress;
        public string BusinessOwnerName;
        public string RequestorNM;
        public string RequestorID;
        public List<String> Attachments;
        public String iAttachments;
        public string CTCNo;
        public string ORNO;
        public string Amount;
        public string STATUS;
        public string ApplicationDate;
        public string URLAttachment;
        public string URL_DocPath;
        public string CategoryID;
        public string OTRDocumentType;
        public string Category_Document;
        public string BizReport;
        public string IssuedDate;
        public string ControlNo;
        public string isFree;
    }
}