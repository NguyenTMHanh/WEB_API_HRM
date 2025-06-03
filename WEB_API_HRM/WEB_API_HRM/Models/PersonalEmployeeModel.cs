using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEB_API_HRM.Models;

public class PersonalEmployeeModel
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string NameEmployee { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    public string Nationality { get; set; }
    public string Ethnicity { get; set; }
    [Required]
    public string NumberIdentification { get; set; }
    public DateTime DateIssueIdentification { get; set; }
    public string PlaceIssueIdentification { get; set; }
    [Required]
    public string FrontIdentificationPath { get; set; }
    [Required]
    public string BackIdentificationPath { get; set; }
    public string ProvinceResidence { get; set; }
    public string DistrictResidence { get; set; }
    public string WardResidence { get; set; }
    public string HouseNumberResidence { get; set; }
    public string ProvinceContact { get; set; }
    public string DistrictContact { get; set; }
    public string WardContact { get; set; }
    public string HouseNumberContact { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(10)]
    [MaxLength(10)]
    public string PhoneNumber { get; set; }
    [Required]
    public string BankNumber { get; set; }
    [Required]
    public string NameBank { get; set; }
    [Required]
    public string BranchBank { get; set; }
    public string EmployeeCode { get; set; }
    [ForeignKey("EmployeeCode")]
    public EmployeeModel Employee { get; set; }
}