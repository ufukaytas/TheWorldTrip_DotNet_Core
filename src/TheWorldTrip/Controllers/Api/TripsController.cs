using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheWorldTrip.Models;
using TheWorldTrip.ViewModels;

namespace TheWorldTrip.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        ITripRepository _repository;
        ILogger<TripsController> _logger;
        public TripsController(ITripRepository  repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = _repository.GetAllTrips();

                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(list));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get All Trips: {ex}");
                return BadRequest(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var item = Mapper.Map<Trip>(trip);

                _repository.AddTrip(item);
                if(await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{item.Name}", Mapper.Map<TripViewModel>(item));
                }
                else
                {
                    return BadRequest("Failed to save changes to the database");
                } 
            }

            return BadRequest(ModelState);
        }
    }
}
