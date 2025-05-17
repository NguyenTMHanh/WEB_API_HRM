namespace WEB_API_HRM.DTO
{
    public class UpdateRoleDto
    {
        public string Name { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
        public string ConcurrencyStamp { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<UpdateRoleModuleActionDto> RoleModuleActions { get; set; } = new List<UpdateRoleModuleActionDto>();
    }

    public class UpdateRoleModuleActionDto
    {
        public string ModuleId { get; set; } = null!;
        public string ActionId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
    }


    public class CreateRoleDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
        public string ConcurrencyStamp { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<CreateRoleModuleActionDto> RoleModuleActions { get; set; } = new List<CreateRoleModuleActionDto>();
    }

    public class CreateRoleModuleActionDto
    {
        public string ModuleId { get; set; } = null!;
        public string ActionId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
    }
}