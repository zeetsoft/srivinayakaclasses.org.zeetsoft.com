namespace StairwayDesigns.Services;

public interface INotifyUser
{
    Task<bool> SendEmail(string argSentEmailTo, string argMailSubject, string argMailContent);
}