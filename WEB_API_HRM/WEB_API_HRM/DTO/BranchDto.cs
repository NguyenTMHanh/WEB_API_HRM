namespace WEB_API_HRM.DTO
{
    public class BranchDto
    {
        public string Id { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Status { get; set; } = null!;
        public ICollection<string> DepartmentName { get; set; } = null!;
    }
}
