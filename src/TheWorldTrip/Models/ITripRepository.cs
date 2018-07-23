using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorldTrip.Models
{
    public interface ITripRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUserName(string name);
        Trip GetTripByName(string tripName);

        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop newStop, string userName);

        Task<bool> SaveChangesAsync();
        Trip GetUserTripByName(string tripName, string name);
    }
}