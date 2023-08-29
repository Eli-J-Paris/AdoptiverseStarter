using AdoptiverseAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace AdoptiverseAPI.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        private readonly AdoptiverseApiContext _context;

        public SheltersController(AdoptiverseApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetShelters()
        {
            var shelters = _context.Shelters;
            return new JsonResult(shelters);
        }
    }
}