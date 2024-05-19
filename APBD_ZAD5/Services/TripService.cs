using APBD_ZAD5.Database;
using APBD_ZAD5.Dto;
using APBD_ZAD5.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_ZAD5.Services
{
    public class TripService
    {
        private readonly TripContext _tripContext;

        public TripService(TripContext tripContext)
        {
            _tripContext = tripContext;
        }

        public async Task<IEnumerable<GetTripDto>> GetAllTrips()
        {
            return await _tripContext.Trips
                .Include(trips => trips.IdCountries)
                .Include(trips => trips.ClientTrips)
                .OrderByDescending(trip => trip.DateFrom)
                .Select(trip =>
                    new GetTripDto()
                    {
                        Name = trip.Name,
                        Description = trip.Description,
                        MaxPeople = trip.MaxPeople,
                        DateFrom = trip.DateFrom,
                        DateTo = trip.DateTo,
                        Countries = trip.IdCountries.Select(country => country.Name),
                        Clients = trip.ClientTrips.Select(client => new GetTripDto.Client()
                        {
                            FirstName = client.IdClientNavigation.FirstName,
                            LastName = client.IdClientNavigation.LastName,
                        })
                    })
                .ToListAsync();
        }

        public async Task AddUserToTrip(int tripId, AddUserToTripDto addToTripData)
        {
            var trip = await _tripContext.Trips.FirstOrDefaultAsync(trip => trip.IdTrip == tripId);
            if (trip is null)
                throw new ValidationException("This trip don't exist");

            var client = await _tripContext.Clients
                .FirstOrDefaultAsync(client => client.Pesel == addToTripData.Pesel);

            if (client is null)
            {
                var newClient = new Client()
                {
                    FirstName = addToTripData.FirstName,
                    LastName = addToTripData.LastName,
                    Pesel = addToTripData.Pesel,
                    Email = addToTripData.Email
                };

                _tripContext.Clients.Add(newClient);
                await _tripContext.SaveChangesAsync();

                client = await _tripContext.Clients
                    .FirstOrDefaultAsync(client => client.Pesel == addToTripData.Pesel);
            }

            var clientTrip = _tripContext.ClientTrips
                            .Include(c => c.IdClientNavigation)
                            .FirstOrDefault(c => c.IdTrip == addToTripData.IdTrip && c.IdClientNavigation.Pesel == addToTripData.Pesel);

            if (clientTrip is null)
                throw new ValidationException("This user is assigned to this trip");

            var newClientTrip = new ClientTrip()
            {
                IdClient = client.IdClient,
                IdTrip = addToTripData.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = addToTripData.PaymentDate,
            };

            _tripContext.ClientTrips.Add(clientTrip);
            await _tripContext.SaveChangesAsync();
        }
    }
}
