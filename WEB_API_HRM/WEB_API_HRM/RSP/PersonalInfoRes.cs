namespace WEB_API_HRM.RSP
{
    public class PersonalInfoRes
    {
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string Ethnicity { get; set; }

        public string NumberIdentification { get; set; }

        public DateTime DateIssueIdentification { get; set; }

        public string PlaceIssueIdentification { get; set; }
        public string FrontIdentificationPath { get; set; }
        public string BackIdentificationPath { get; set; }

        public string ProvinceResidence { get; set; }

        public string DistrictResidence { get; set; }

        public string WardResidence { get; set; }

        public string HouseNumberResidence { get; set; }

        public string ProvinceContact { get; set; }

        public string DistrictContact { get; set; }

        public string WardContact { get; set; }

        public string HouseNumberContact { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BankNumber { get; set; }
        public string NameBank { get; set; }
        public string BranchBank { get; set; }
    }
}
