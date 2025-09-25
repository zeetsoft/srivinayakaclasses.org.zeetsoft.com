using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StairwayDesigns.Services;
using System.Reflection;
using System.IO;

namespace StairwayDesigns.Pages;

public class Contact_UsModel : PageModel
{
    private readonly INotifyUser _notifyUser;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Contact_UsModel> _logger;

    public Contact_UsModel(INotifyUser notifyUser, IWebHostEnvironment hostEnvironment, IConfiguration configuration, ILogger<Contact_UsModel> logger)
    {
        _notifyUser = notifyUser;
        _hostEnvironment = hostEnvironment;
        _configuration = configuration;
        _logger = logger;
    }

    public class ContactForm
    {
        [Required, Display(Name = "First Name"), StringLength(80)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name"), StringLength(80)]
        public string? LastName { get; set; }

        [Required, EmailAddress, Display(Name = "Email"), StringLength(160)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone"), StringLength(40)]
        public string? Phone { get; set; }

        [Required, Display(Name = "Message"), StringLength(2000)]
        public string Message { get; set; } = string.Empty;
    }

    [BindProperty]
    public ContactForm Form { get; set; } = new();

    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Send email notification
            bool emailSent = await SendEmailNotification();

            if (emailSent)
            {
                SuccessMessage = "Thank you for contacting us. We will get back to you soon.";
                
                // Clear the form fields after successful submission
                Form = new ContactForm();
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "There was an issue sending your message. Please try again later.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing contact form submission from {Email}", Form.Email);
            ModelState.AddModelError(string.Empty, "An error occurred while sending your message. Please try again later.");
        }

        return Page();
    }

    private async Task<bool> SendEmailNotification()
    {
        try
        {
            string subject = "Contact Form Submission - Stairway Designs";
            string emailTemplateFile = Path.Combine(_hostEnvironment.WebRootPath, "Website", "eMailTemplate", "stairwaydesign", "Contact.html");
            string productWebsite = _configuration.GetValue<string>("InternalLink:ZSFT_APPSTOR") ?? "https://stairwaydesigns.in";
            
            if (!System.IO.File.Exists(emailTemplateFile))
            {
                _logger.LogWarning("Email template file not found at: {FilePath}", emailTemplateFile);
                return false;
            }

            string emailHTMLContent = await System.IO.File.ReadAllTextAsync(emailTemplateFile);
            
            var emailSetup = _configuration.GetSection("EMailSetup");
            string mainImage = emailSetup["MainImage"] ?? string.Empty;
            string mainImageLink = emailSetup["MainImageLink"] ?? productWebsite;
            string mainImageALT = emailSetup["MainImageALT"] ?? "Company Logo";
            string signatureName = emailSetup["SignatureName"] ?? "Team";
            string signatureCompanyName = emailSetup["SignatureCompanyName"] ?? "Stairway Design";
            string signatureCompanyLink = emailSetup["SignatureCompanyLink"] ?? productWebsite;
            string currentYear = DateTime.UtcNow.Year.ToString();

            // Replace placeholders with actual values
            var fullName = $"{Form.FirstName} {Form.LastName}".Trim();
            emailHTMLContent = emailHTMLContent.Replace("#UserName#", fullName)
                                               .Replace("#Email#", Form.Email)
                                               .Replace("#Phone#", Form.Phone ?? "Not provided")
                                               .Replace("#ProductKey#", "Contact Inquiry")
                                               .Replace("#ProductName#", "Website Contact Form")
                                               .Replace("#Message#", Form.Message)
                                               .Replace("#RedirectURL#", productWebsite)
                                               .Replace("#MainImage#", mainImage)
                                               .Replace("#MainImageLink#", mainImageLink)
                                               .Replace("#MainImageALT#", mainImageALT)
                                               .Replace("#SignatureName#", signatureName)
                                               .Replace("#SignatureCompanyName#", signatureCompanyName)
                                               .Replace("#SignatureCompanyLink#", signatureCompanyLink)
                                               .Replace("#Year#", currentYear);
            
            // Clean up any remaining double slashes in URLs
            emailHTMLContent = ReplaceDoubleSlash(emailHTMLContent);

            // Send email to the configured recipient
            string toEmail = emailSetup["SenderEmail"] ?? "info@stairwaydesigns.in";
            
            // Use the new service method that returns bool
            bool result = await _notifyUser.SendEmail(toEmail, subject, emailHTMLContent);

            if (result)
            {
                _logger.LogInformation("Contact form email sent successfully for submission from {Email}", Form.Email);
            }
            else
            {
                _logger.LogWarning("Failed to send contact form email for submission from {Email}", Form.Email);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending contact form email notification");
            return false;
        }
    }

    private static string ReplaceDoubleSlash(string content)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        // Replace double slashes in URLs, but preserve protocol slashes (http://, https://)
        return content
            .Replace("http:////", "http://")
            .Replace("https:////", "https://")
            .Replace("http:///", "http://")
            .Replace("https:///", "https://")
            .Replace("//", "/")
            .Replace("http:/", "http://")
            .Replace("https:/", "https://");
    }
}
