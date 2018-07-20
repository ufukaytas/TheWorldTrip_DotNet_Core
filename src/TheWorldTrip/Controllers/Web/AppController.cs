using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        IWorldTripRepository _worldTripRepository;
        ILogger<AppController> _logger;
        public AppController(
                IWorldTripRepository worldTripRepository, 
                IMailService  mailService, 
                IConfigurationRoot  config, 
                TheWorldTripContext context,
                ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _worldTripRepository = worldTripRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var data = _worldTripRepository.GetAllTrips();
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed" + ex.Message);
                return Redirect("/error");
            }
            

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
