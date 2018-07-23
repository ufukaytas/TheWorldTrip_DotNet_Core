using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TheWorldTrip.Models
{
    public class User : IdentityUser
    {
        public string FirstTrip { get; set; }
    }
}