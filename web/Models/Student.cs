using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuetStudentPortal.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Roll Number is required (e.g. 21SW015)")]
        [StringLength(10)]
        [Display(Name = "Roll Number")]
        [RegularExpression(@"^[0-9]{2}[A-Za-z]{2,3}[0-9]{2,3}$", ErrorMessage = "Format must be like 21SW015 or 22CS040")]
        public string RollNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a department")]
        [StringLength(60)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Batch year is required (e.g. 2021)")]
        [Range(2015, 2030, ErrorMessage = "Enter valid batch year")]
        public int Batch { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "CGPA is required")]
        [Range(0.00, 4.00, ErrorMessage = "CGPA must be between 0.00 and 4.00")]
        [Column(TypeName = "decimal(3, 2)")]
        public decimal Cgpa { get; set; }

        [Display(Name = "Enrolled Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    }
}