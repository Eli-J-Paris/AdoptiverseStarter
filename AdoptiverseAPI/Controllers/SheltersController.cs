using AdoptiverseAPI.DataAccess;
using AdoptiverseAPI.Models;
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

        [HttpGet("{id}")]
        public ActionResult ReturnShelter(int id)
        {
            var shelter = _context.Shelters.Find(id);

            return new JsonResult(shelter);

        }

        [HttpPost]
        public ActionResult CreateShelter(Shelter shelter)
        {
            _context.Shelters.Add(shelter);
            _context.SaveChanges();

            return new JsonResult(shelter);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateShelter(int id, Shelter shelter)
        {
            shelter.Id = id;
            _context.Shelters.Update(shelter);
            _context.SaveChanges();
            Response.StatusCode = 204;
            return new JsonResult(shelter);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteShelter(int id)
        {
            var shelters = _context.Shelters;
            var shelter = shelters.Find(id);
            _context.Shelters.Remove(shelter);
            _context.SaveChanges();

            return new JsonResult(shelters);
        }
    }
}