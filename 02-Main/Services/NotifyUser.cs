using System.Net;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using Polly.Registry;
using MailKit.Security;

namespace StairwayDesigns.Services;

public class NotifyUser : INotifyUser
{
    private readonly IConfiguration _config;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;
    private readonly ILogger<NotifyUser> _logger;

    public NotifyUser(IServiceProvider serviceProvider, ResiliencePipelineProvider<string> resiliencePipelineProvider, ILogger<NotifyUser> logger)
    {
        _config = serviceProvider.GetRequiredService<IConfiguration>();
        _pipelineProvider = resiliencePipelineProvider;
        _logger = logger;
    }

    public async Task<bool> SendEmail(string argSentEmailTo, string argMailSubject, string argMailContent)
    {
        try
        {
            // Replace template placeholders with configuration values
            argMailContent = argMailContent.Replace("#MainImageALT#", _config.GetSection("EMailSetup:MainImageALT").Value ?? "");
            argMailContent = argMailContent.Replace("#MainImageLink#", _config.GetSection("EMailSetup:MainImageLink").Value ?? "");
            argMailContent = argMailContent.Replace("#SignatureName#", _config.GetSection("EMailSetup:SignatureName").Value ?? "");
            argMailContent = argMailContent.Replace("#SignatureCompanyName#", _config.GetSection("EMailSetup:SignatureCompanyName").Value ?? "");
            argMailContent = argMailContent.Replace("#SignatureCompanyLink#", _config.GetSection("EMailSetup:SignatureCompanyLink").Value ?? "");

            var strToEmailList = argSentEmailTo.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var strCCEmailList = (_config.GetSection("EMailSetup:DefaultCCTo").Value ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (_config.GetSection("EMailSetup:ActiveMethod").Value == "MailKit")
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(
                    _config.GetSection("EMailSetup:SenderDisplayName").Value ?? "",
                    _config.GetSection("EMailSetup:SenderEmail").Value ?? ""));

                foreach (string strEmailTo in strToEmailList)
                {
                    if (!string.IsNullOrWhiteSpace(strEmailTo))
                    {
                        mailMessage.To.Add(new MailboxAddress("", strEmailTo.Trim()));
                    }
                }

                foreach (string strEmailCC in strCCEmailList)
                {
                    if (!string.IsNullOrWhiteSpace(strEmailCC))
                    {
                        mailMessage.Cc.Add(new MailboxAddress("", strEmailCC.Trim()));
                    }
                }

                mailMessage.Subject = argMailSubject;
                mailMessage.Body = new TextPart("html")
                {
                    Text = argMailContent
                };

                var pipeline = _pipelineProvider.GetPipeline("default");

                await pipeline.ExecuteAsync(async _ =>
                {
                    using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                    smtpClient.CheckCertificateRevocation = false;
                    
                    await smtpClient.ConnectAsync(
                        _config.GetSection("EMailSetup:SMTPServer").Value,
                        Convert.ToInt32(_config.GetSection("EMailSetup:SMTPPort").Value ?? "587"),
                        SecureSocketOptions.StartTls);
                    
                    await smtpClient.AuthenticateAsync(
                        _config.GetSection("EMailSetup:NetworkUserName").Value,
                        _config.GetSection("EMailSetup:NetworkPassword").Value);
                    
                    await smtpClient.SendAsync(mailMessage);
                    await smtpClient.DisconnectAsync(true);
                });

                _logger.LogInformation("Email sent successfully using MailKit to {Recipients}", argSentEmailTo);
                return true;
            }
            else
            {
                using (System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtpout.secureserver.net", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(
                        _config.GetSection("EMailSetup:NetworkUserName").Value,
                        _config.GetSection("EMailSetup:NetworkPassword").Value);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    using (MailMessage mail = new MailMessage())
                    {
                        // Handle local image if configured
                        var localImagePath = _config.GetSection("EMailSetup:MainImageLocal_Header").Value;
                        if (!string.IsNullOrEmpty(localImagePath) && File.Exists(localImagePath))
                        {
                            string base64Image = Convert.ToBase64String(File.ReadAllBytes(localImagePath));
                            argMailContent = argMailContent.Replace("#content_TopLogo#", base64Image);
                        }

                        mail.From = new MailAddress(
                            _config.GetSection("EMailSetup:SenderEmail").Value ?? "",
                            _config.GetSection("EMailSetup:SenderDisplayName").Value ?? "");

                        foreach (string strEmailTo in strToEmailList)
                        {
                            if (!string.IsNullOrWhiteSpace(strEmailTo))
                            {
                                mail.To.Add(new MailAddress(strEmailTo.Trim()));
                            }
                        }

                        foreach (string strEmailCC in strCCEmailList)
                        {
                            if (!string.IsNullOrWhiteSpace(strEmailCC))
                            {
                                mail.CC.Add(new MailAddress(strEmailCC.Trim()));
                            }
                        }

                        mail.Subject = argMailSubject;
                        mail.Body = argMailContent;
                        mail.IsBodyHtml = true;
                        await smtpClient.SendMailAsync(mail);
                    }
                }

                _logger.LogInformation("Email sent successfully using System.Net.Mail to {Recipients}", argSentEmailTo);
                return true;
            }
        }
        catch (SmtpFailedRecipientException ex)
        {
            _logger.LogError(ex, "Failed recipient: {FailedRecipient}", ex.FailedRecipient);
            return false;
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "SMTP Error - Status: {StatusCode}, Message: {Message}, Server Response: {ServerResponse}", 
                ex.StatusCode, ex.Message, ex.InnerException?.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending email to {Recipients}", argSentEmailTo);
            return false;
        }
    }

    public static async Task<string> SendSMS(
        string argRegMobileNo,
        string strMsg,
        string argSMSName,
        IConfiguration argConfig)
    {
        try
        {
            var objNotificationSection = argConfig.GetSection("NotificationAPI:SMS").Value;
            if (string.IsNullOrEmpty(objNotificationSection))
            {
                return "SMS API configuration not found";
            }

            string strAPI = objNotificationSection.Replace("#Message#", strMsg);
            strAPI = strAPI.Replace("#MobileNo#", argRegMobileNo);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(strAPI);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}