using System;

namespace webapi.App.RequestModel.Feature
{
    public class ProfileRequest
    {

    }

    public class Resident
    {
        public string Search;
    }

    public class Family
    {
        public string UserId;
        public string FamilyId;
        public string Relationship;
        public Int16 IsRemoved;
        //public string MemberRelationship;
    }
}
