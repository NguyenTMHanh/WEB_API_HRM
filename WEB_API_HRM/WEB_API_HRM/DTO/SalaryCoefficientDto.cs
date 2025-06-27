namespace WEB_API_HRM.DTO
{
    public class SalaryCoefficientDto
    {
        public string Id { get; set; } = null!;
        public double SalaryCoefficient { get; set; }
        public string PositionName { get; set; } = null!; 
    }
}
