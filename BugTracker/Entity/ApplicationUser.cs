using Microsoft.AspNetCore.Identity;

namespace BugTracker.Entity
{
        public class ApplicationUser : IdentityUser
        {
            public Person Person { get; set; }
            public int PersonId { get; set; }
        }
}
