namespace WEB_API_HRM.RSP
{
    public class AllInsuranceRes
    {
        public string AvatarPath { get; set; }
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string CodeBHYT { get; set; }
        public double BusinessRateBHYT { get; set; }
        public double EmptRateBHYT { get; set; }
        public bool HasBHXH { get; set; }
        public string CodeBHXH { get; set; }
        public double BussinessRateBHXH { get; set; }
        public double EmptRateBHXH { get; set; }
        public double BusinessRateBHTN { get; set; }
        public double EmptRateBHTN { get; set; }
        public string StatusInsurance { get; set; }
        public DateTime EndInsurance { get; set; }
    }
}
