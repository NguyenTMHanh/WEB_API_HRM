using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.DTO
{
    public class CreateContractEmployeeDto
    {
        public string EmployeeCode { get; set; }
        public string NameEmployee { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CodeContract { get; set; }
        public string TypeContract { get; set; }
        public DateTime StartContract { get; set; }
        public DateTime EndContract { get; set; }
        public string StatusContract { get; set;}
        public double HourlySalary { get; set; }
        public double HourWorkStandard { get; set; }
        public string NamePosition { get; set; }
        public double CoefficientSalary { get; set; }
        public double DayWorkStandard { get; set; }
        public double BasicSalary { get; set; }
        public List<AllowanceRes> Allowances { get; set; }
    }
}
