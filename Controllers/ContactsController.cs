using System;
using Microsoft.AspNetCore.Mvc;
using EFTraining.Data;
using EFTraining.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EFTraining.Controllers
{

    [Route("api/[controller]")]
    public class ContactsController : BaseApiController
    {
        private DigiBookDbContext context;
        public ContactsController(DigiBookDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
        ) : base(context, roleManager, userManager, configuration)
        {
            this.context = context;
        }

        [HttpGet("")]
        public IActionResult GetContacts()
        {
            var contacts = context.Contacts
                .Select(
                    c=> new ContactDTO{
                        FullName = $"{c.FirstName} {c.MiddleName.Substring(0,1)}. {c.LastName}",
                        MobilePhone = c.MobilePhone

                    }
                )
                .ToList();

            return  Json(contacts);
        }

        [HttpGet("{id}/appointments` ")]
        public IActionResult GetContactAppointments(Guid id)
        {
            var contacts = context.Contacts
                .Where( c => c.ContactId == id)
                .Include(c=> c.Appointments)
                .ToList();
                
            return  Json(contacts);
        }
    }
}