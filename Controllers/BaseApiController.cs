using System;
using EFTraining.Data;
using EFTraining.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EFTraining.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region Constructor
        public BaseApiController(
            DigiBookDbContext context, 
            RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration
            ) 
        {
            // Instantiate the required classes through DI DbContext = context;
            RoleManager = roleManager;
            UserManager = userManager;
            Configuration = configuration;
            // Instantiate a single JsonSerializerSettings object
            // that can be reused multiple times.
            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        } 
        #endregion

        #region Shared Properties
        protected DigiBookDbContext DbContext { get; private set; } 
        protected RoleManager<ApplicationRole> RoleManager { get; private set; }
        protected UserManager<ApplicationUser> UserManager { get; private set; }
        protected IConfiguration Configuration { get; private set; } 
        protected JsonSerializerSettings JsonSettings { get; private set; }
        #endregion
    } 
}