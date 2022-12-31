using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using UWContinuum.Data;
using UWContinuum.Models;


namespace UWContinuum.Controllers
{
    public class SignupFormWebServiceController : Controller
    {
        private readonly ILogger<SignupFormWebServiceController> _logger;
        private readonly DatabaseContext _context;

        private static readonly HttpClient client = new();

        public SignupFormWebServiceController(ILogger<SignupFormWebServiceController> logger, DatabaseContext context)
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

            //if all looks good, then try to submit form to web service with encoding just in case
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
                //_context.WebEmails?.Add(form);
                //await _context.SaveChangesAsync();

                //send to web service
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                //url to simple API call that I created using Azure Functions to demonstrate how sending form data to REST API would work, along with sample class to read the result
                var url = "https://icy-sand-00ba9841e.2.azurestaticapps.net/api/hello";
                
                var formData = new StringContent(JsonSerializer.Serialize(form), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, formData);
                if (response.IsSuccessStatusCode)
                {
                    //get json result using sample result class below
                    var formResult = await response.Content.ReadFromJsonAsync<FormResult>();
                    string? result = formResult?.Text;

                    //Result should be something like: "Hey, {FirstName}. Happy New Year! Thanks for using this API!"
                    SetFormMessage(result, "alert-success");

                    //Clear form data when displaying the page
                    ModelState.Clear();
                }
                else
                {
                    //if response status code is not 200, then return this message
                    SetFormMessage("Sorry, web service did not work. Please try again.", "alert-danger");
                }
                

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

    //Json response from API call returns only a "text" property
    public class FormResult
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}