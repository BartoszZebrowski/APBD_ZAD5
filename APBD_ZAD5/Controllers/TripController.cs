using APBD_ZAD5.Database;
using APBD_ZAD5.Dto;
using APBD_ZAD5.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_ZAD5.Controllers
{
    [Route("/api")]
    [ApiController]
    public class TripController : Controller
    {
        private readonly TripService _tripService;

        public TripController(TripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("/trips")]
        public async Task<ActionResult<IEnumerable<GetTripDto>>> GetTrips()
        {
            var trips = await _tripService.GetAllTrips();
            return Ok(trips);
        }

        [HttpPost("/trips/{idTrip}/clients")]
        public async Task<IActionResult> GetTrips(int idTrip, [FromBody]AddUserToTripDto addToTripData)
        {
            await _tripService.AddUserToTrip(idTrip, addToTripData);
            return Ok();
        }
    }
}
