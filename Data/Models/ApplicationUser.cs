using System;
using Microsoft.AspNetCore.Identity;

namespace EFTraining.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}