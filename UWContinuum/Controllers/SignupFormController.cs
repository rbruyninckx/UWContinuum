using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;
using UWContinuum.Data;
using UWContinuum.Models;


namespace UWContinuum.Controllers
{
    public class SignupFormController : Controller
    {
        private readonly ILogger<SignupFormController> _logger;
        private readonly DatabaseContext _context;

        public SignupFormController(ILogger<SignupFormController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Default GET page
        public IActionResult Index()
        {
            SetFormMessage(null, null);
            return View();
        }

        //Default POST form 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("FirstName,LastName,EmailAddress,EmailAddressVerify,HasOptedIn,PhoneNumber")] WebEmailsDTOModel webform)
        {
            //if DTO model is not valid from form submit
            if (!ModelState.IsValid)
            {
                SetFormMessage("Please check the errors below and try submitting again.", "alert-danger");          
                return View();
            }
            //double check first and last names are not blank
            if (String.IsNullOrEmpty(webform.FirstName) || String.IsNullOrEmpty(webform.LastName)) 
            {
                SetFormMessage("Please enter your First Name and Last Name and try submitting again.", "alert-danger");
                return View();
            }

            //double check email address is not blank and that both email address fields match
            if (String.IsNullOrEmpty(webform.EmailAddress) || HttpUtility.HtmlEncode(webform.EmailAddress) != HttpUtility.HtmlEncode(webform.EmailAddressVerify))
            {
                SetFormMessage("Please make sure Email Addresses match and try submitting again.", "alert-danger");
                return View();
            }

            //double check that opt in checkbox has true value
            if (!webform.HasOptedIn)
            {
                SetFormMessage("Please check box to opt in and try submitting again.", "alert-danger");
                return View();
            }

            //if all looks good, then try to submit form to DB with encoding just in case
            try
            {
                //create new object with complete data to submit
                WebEmailsModel form = new()
                {
                    Id = 0,
                    FirstName = HttpUtility.HtmlEncode(webform.FirstName),
                    LastName = HttpUtility.HtmlEncode(webform.LastName),
                    EmailAddress = HttpUtility.HtmlEncode(webform.EmailAddress),
                    HasOptedIn = true,
                    PhoneNumber = HttpUtility.HtmlEncode(webform.PhoneNumber),                        
                    SignUpDate = DateTime.Now
                };
                
                //submit to db
                _context.WebEmails?.Add(form);
                await _context.SaveChangesAsync();

                //Display success message
                SetFormMessage($"Thank you for subscribing, {form.FirstName}!", "alert-success");
                    
                //Clear form data when displaying the page
                ModelState.Clear();
                
            }
            //in case there is an error/exception
            catch
            {
                throw;                
            }
            
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Assign form message and classname values to ViewData
        private void SetFormMessage(string? message, string? classname) 
        {
            ViewData["Message"] = message;
            ViewData["MessageClass"] = classname;
        }
    }
}