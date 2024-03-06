using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Mvc;

namespace DFW_CW_40452913.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetitionManager: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PetitionManager(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpPost("create")]
        public IActionResult CreatePetition([FromBody] Petition petition)
        {
            _context.Petitions.Add(petition);
            _context.SaveChanges();
            return Ok(petition);
        }

        [HttpGet("getAll")]
        public IActionResult GetAllPetitions()
        {
            var petitions = _context.Petitions.ToList();
            return Ok(petitions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetition(int id)
        {
            var petition = await _context.Petitions.FindAsync(id);
            if (petition == null)
            {
                return NotFound();
            }

            _context.Petitions.Remove(petition);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
