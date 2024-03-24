namespace ShuffleLit.Interfaces
{
    public interface IGSMTPService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
