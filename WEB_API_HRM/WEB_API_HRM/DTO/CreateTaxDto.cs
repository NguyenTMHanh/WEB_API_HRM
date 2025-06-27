using WEB_API_HRM.Models;

namespace WEB_API_HRM.DTO
{
    public class CreateTaxDto
    {
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasTaxCode { get; set; }
        public string TaxCode { get; set; }
        public List<DependentRes> Dependents { get; set; }
    }
}
