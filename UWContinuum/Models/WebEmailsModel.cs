using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UWContinuum.Models
{
    [Table("WebEmails")]
    public class WebEmailsModel
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public bool HasOptedIn { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime SignUpDate { get; set; } = DateTime.Now;
    }

    //DTO Model to use on the web form
    public class WebEmailsDTOModel
    {
        
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "A maximum of 50 characters is allowed")]
        [Required(ErrorMessage = "Please enter your First Name")]
        [RegularExpression(@"^([a-zA-Z \-\']+)$", ErrorMessage = "Please enter alphabetical letters, hyphens or apostrophes for First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "A maximum of 50 characters is allowed")]
        [Required(ErrorMessage = "Please enter your Last Name")]
        [RegularExpression(@"^([a-zA-Z \-\']+)$", ErrorMessage = "Please enter alphabetical letters, hyphens or apostrophes for Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        [MaxLength(100, ErrorMessage = "A maximum of 100 characters is allowed")]
        [Required(ErrorMessage = "Please enter your Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Display(Name = "Re-enter your Email Address")]
        [MaxLength(100, ErrorMessage = "A maximum of 100 characters is allowed")]
        [Required(ErrorMessage = "Please re-enter your Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please verify the Email Address fields match")]
        [Compare("EmailAddress", ErrorMessage = "Please verify the Email Address fields match")]
        public string EmailAddressVerify { get; set; } = string.Empty;

        [Display(Name = "Check the box to opt in to receive program updates from UW Continuum College. You may unsubscribe at any time.")]
        [Required(ErrorMessage = "Please opt in to submit this form")]       
        public bool HasOptedIn { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,9}$", ErrorMessage = "Please enter a valid US or International Phone Number")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = $"Please enter between 10 and 20 characters for your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

    }

}
