using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using TheWorldTrip.Models;
using TheWorldTrip.Services;
using TheWorldTrip.ViewModels;

namespace TheWorldTrip.Controllers.Web
{
    public class AppController  :Controller
    {
        IMailService _mailService;
        IConfigurationRoot _config;
        TheWorldTripContext _context;
        public AppController(IMailService  mailService, IConfigurationRoot  config, TheWorldTripContext context)
        {
            _mailService = mailService;
            _config = config;
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Trips.ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            _mailService.SendMail(_config["MailSettings:ToAddress"], model.EMail, "Contact From", model.Message);

            return View();
        }
    }
}
