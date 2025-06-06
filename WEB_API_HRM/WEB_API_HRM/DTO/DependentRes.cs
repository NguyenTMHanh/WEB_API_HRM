using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.DTO
{
    public class DependentRes
    {
        public string RegisterDependentStatus { get; set; }
        public string TaxCode { get; set; }
        public string NameDependent { get; set; }
        public DateTime DayOfBirthDependent { get; set; }
        public string Relationship { get; set; }
        public string EvidencePath { get; set; }
    }
}
