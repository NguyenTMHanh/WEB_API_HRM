namespace WEB_API_HRM.RSP
{
    public class AllTaxRes
    {
        public string AvatarPath { get; set; }
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public bool HasTax { get; set; }
        public string CodeTax { get; set; }
        public int CountDependent { get; set; }
    }
}
