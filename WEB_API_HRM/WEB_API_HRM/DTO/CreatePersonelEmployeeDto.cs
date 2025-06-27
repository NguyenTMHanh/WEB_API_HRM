namespace WEB_API_HRM.DTO
{
    public class CreatePersonelEmployeeDto
    {
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoinCompany { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        //public string JobtitleName { get; set; }
        public string RankName { get; set; }
        public string PositionName { get; set; }
        public string ManagerId { get; set; }
        public string JobTypeName { get; set; }
        public double BreakLunch { get; set; }
        public string AvatarPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
