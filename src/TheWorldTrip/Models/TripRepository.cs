using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorldTrip.Models
{
    public class TripRepository : ITripRepository
    {
        private TheWorldTripContext _context;
        private ILogger<TripRepository> _logger;


        public TripRepository(TheWorldTripContext context, ILogger<TripRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, Stop newStop, string userName)
        {
            var trip = GetUserTripByName(tripName, userName);

            if (trip != null)
            {
                trip.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips from the Database");

            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByUserName(string name)
        {
            _logger.LogInformation("Getting All Trips by Username from the Database");

            return _context.Trips.Include(t => t.Stops).Where(q => q.UserName == name).ToList();

        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
              .Include(t => t.Stops)
              .Where(t => t.Name == tripName)
              .FirstOrDefault();
        }

        public Trip GetUserTripByName(string tripName, string userName)
        {
            return _context.Trips.Include(t => t.Stops).SingleOrDefault(q => q.Name == tripName && q.UserName == userName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
