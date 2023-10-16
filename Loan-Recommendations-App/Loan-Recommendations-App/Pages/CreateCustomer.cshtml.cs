using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Loan_Recommendations_App.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loan_Recommendations_App.Pages
{
    public class CreateCustomer : PageModel
    {
        ApplicationDBContext _context;
        public CreateCustomer(ApplicationDBContext context)
        {
            _context = context;
        }

        #region Model Properties
        public int CustomerId { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = nameof(IdNumber), ResourceType = typeof(Labels))]
        [PageRemote(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PleaseEnsureThatYourIdNumberContainsThirteenDigits),
                    AdditionalFields = $"__RequestVerificationToken,{nameof(IsSouthAfricanCitizen)}",
                    HttpMethod = "post",
                    PageHandler = "ValidateIdNumber"
        )]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string IdNumber { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(Name), ResourceType = typeof(Labels))]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string Name { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Labels))]
        [PageRemote(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PleaseEnsureThatYourPhoneNumberContainsTenDigits),
                    AdditionalFields = $"__RequestVerificationToken,{nameof(IsSouthAfricanCitizen)}",
                    HttpMethod = "post",
                    PageHandler = "ValidatePhoneNumber"
        )]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(AlternatePhoneNumber), ResourceType = typeof(Labels))]
        [PageRemote(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PleaseEnsureThatYourPhoneNumberContainsTenDigits),
                    AdditionalFields = $"__RequestVerificationToken,{nameof(IsSouthAfricanCitizen)}",
                    HttpMethod = "post",
                    PageHandler = "ValidateAlternatePhoneNumber"
        )]
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string AlternatePhoneNumber { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [EmailAddress]
        [Display(Name = nameof(Email), ResourceType = typeof(Labels))]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string Email { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(Address), ResourceType = typeof(Labels))]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string Address { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(DateOfBirth), ResourceType = typeof(Labels))]
        [PageRemote(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IncorrectAge),
                    AdditionalFields = "__RequestVerificationToken",
                    HttpMethod = "post",
                    PageHandler = "CheckDateOfBirth"
        )]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [BindProperty]
        [Display(Name = nameof(IsSouthAfricanCitizen), ResourceType = typeof(Labels))]
        public bool IsSouthAfricanCitizen { get; set; }

        [BindProperty]
        [Display(Name = nameof(Gender), ResourceType = typeof(Labels))]
        public int Gender { get; set; }

        [BindProperty]
        public List<SelectListItem> GenderList { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = nameof(Income), ResourceType = typeof(Labels))]
        [Range(0, 1000000 , ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.NotGreaterThanAndLessThan))]
        public decimal Income { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = nameof(JobTitle), ResourceType = typeof(Labels))]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [Display(Name = nameof(Employer), ResourceType = typeof(Labels))]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.MaxStringLength))]
        public string Employer { get; set; } = string.Empty;
        #endregion

        [DataContract]
        public enum GenderEnum
        {
            [Description("Male")]
            [EnumMember]
            Male = 0,

            [Description("Female")]
            [EnumMember]
            Female = 1,

            [Description("Other")]
            [EnumMember]
            Other = 2,
        }

        public void OnGet()
        {
          GenderList = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().Select(x => new SelectListItem { Text = Enum.GetName(x), Value = ((int)x).ToString() }).OrderBy(x => x.Text).ToList();
        }

        public IActionResult OnPost()
        {
            Customer newN = new Customer()
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                AlternatePhoneNumber = AlternatePhoneNumber,
                Email = Email,
                Address = Address,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                IsSouthAfricanCitizen = IsSouthAfricanCitizen,
                IdNumber = IdNumber,
                Income = Income,
                Employer = Employer,
                JobTitle = JobTitle,
            };
     
           var newCust = _context.Customer.Add(newN);
           _context.SaveChanges();

           return RedirectToPage("/LoanOffers", new { CustomerId = newCust.Entity.CustomerId });
        }

        #region Validation
        public JsonResult OnPostCheckDateOfBirth(DateTime DateOfBirth)
        {
            var IsAdult = DateOfBirth.AddYears(18).Date <= DateTime.Now.Date;
            return new JsonResult(IsAdult);
        }
        
        public JsonResult OnPostValidateIdNumber(string IdNumber, bool IsSouthAfricanCitizen)
        {
            if (IsSouthAfricanCitizen)
            {
                if(IdNumber.Length != 13)
                   return new JsonResult(false);
            }
            return new JsonResult(true);
        }

        public JsonResult OnPostValidatePhoneNumber(string PhoneNumber, bool IsSouthAfricanCitizen)
        {
            if (IsSouthAfricanCitizen)
            {
                if (PhoneNumber.Length != 10)
                    return new JsonResult(false);
            }
            return new JsonResult(true);
        }

        public JsonResult OnPostValidateAlternatePhoneNumber(string AlternatePhoneNumber, bool IsSouthAfricanCitizen)
        {
            if (IsSouthAfricanCitizen)
            {
                if(AlternatePhoneNumber != null && AlternatePhoneNumber.Length != 10)
                {
                    return new JsonResult(false);
                }
            }
            return new JsonResult(true);
        }
        #endregion Validation
    }
}