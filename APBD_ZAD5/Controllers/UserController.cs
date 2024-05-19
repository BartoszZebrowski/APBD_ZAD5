using APBD_ZAD5.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_ZAD5.Controllers
{
    [Route("/api")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ClientService _clientService;

        public UserController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("/clients/{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            await _clientService.DeleteClient(idClient);
            return Ok();
        }
    }
}
