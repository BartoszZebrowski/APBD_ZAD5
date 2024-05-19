using APBD_ZAD5.Database;
using APBD_ZAD5.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_ZAD5.Services
{
    public class ClientService
    {
        private readonly TripContext _tripContext;

        public ClientService(TripContext tripContext)
        {
            _tripContext = tripContext;
        }

        public async Task DeleteClient(int clientId)
        {
            var client = await _tripContext.Clients
                .Include(client => client.ClientTrips)
                .FirstOrDefaultAsync(client => client.IdClient == clientId);

            if (client is null)
                throw new ValidationException("This client don't exist");

            if (client.ClientTrips.Count >= 0)
                throw new ValidationException("This client has trips");

            _tripContext.Clients.Remove(client);
            await _tripContext.SaveChangesAsync();
        }
    }
}
