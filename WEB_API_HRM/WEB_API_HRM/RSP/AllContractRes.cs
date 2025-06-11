namespace WEB_API_HRM.RSP
{
    public class AllContractRes
    {
        public string AvatarPath { get; set; }
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string ContractCode { get; set; }
        public string TypeContract { get; set; }
        public string StatusContract { get; set; }
        public double HourlySalary { get; set; }
        public double HourWorkStandard { get; set; }
        public double CoefficientSalary { get; set; }
        public DateTime StartContract { get; set; }
        public DateTime EndContract { get; set; }
    }
}
