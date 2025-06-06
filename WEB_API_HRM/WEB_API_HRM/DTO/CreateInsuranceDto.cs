namespace WEB_API_HRM.DTO
{
    public class CreateInsuranceDto
    {
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CodeBHYT { get; set; }
        public double RateBHYTEmpt { get; set; }
        public double RateBHYTBussiness { get; set; }
        public string RegisterMedical { get; set; }
        public DateTime DateStartParticipateBHYT { get; set; }
        public bool HasBHXH { get; set; }
        public string CodeBHXH { get; set; }
        public double RateBHXHEmpt { get; set; }
        public double RateBHXHBussiness { get; set; }
        public DateTime DateStartParticipateBHXH { get; set; }
        public double RateBHTNEmpt { get; set; }
        public double RateBHTNBussiness { get; set; }
        public DateTime DateStartParticipateBHTN { get; set; }
        public string InsuranceStatus { get; set; }
        public DateTime DateEndParticipateInsurance { get; set; }
    }
}
