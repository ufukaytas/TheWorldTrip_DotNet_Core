using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Services;
using TheWorldTrip.Models;
using TheWorldTrip.ViewModels;

namespace TheWorldTrip.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ITripRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(ITripRepository repository, ILogger<StopsController> logger, GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(q => q.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Get Trip Stops: {ex}");
                return BadRequest($"Failed to Get Trip Stops: {ex}");
            }
        }

        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel stopVM)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var stop = Mapper.Map<Stop>(stopVM);
                    
                    var result = await _coordsService.GetCoordsAsync(stop.Name);
                    if(!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }

                    stop.Latitude = result.Latitude;
                    stop.Longitude = result.Longitude;

                    _repository.AddStop(tripName, stop, User.Identity.Name);

                    if(await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/stips/{tripName}/stops/{stop.Name}", Mapper.Map<StopViewModel>(stop));
                    } 
                }

                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save stop: {ex}");
                return BadRequest("Failed to save stop");
            }
        }



    }
}
