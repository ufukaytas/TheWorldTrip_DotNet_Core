
using System.Diagnostics;

namespace TheWorldTrip.Services
{
    public class DebugMailService : IMailService
    {
        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Message: To: {to} From: {from} Subject: {subject} Body: {body}");
        }
    }
}
