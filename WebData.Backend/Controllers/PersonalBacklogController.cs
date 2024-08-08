using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalBacklogController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<PersonalBacklogController> _logger;

        public PersonalBacklogController(ILogger<PersonalBacklogController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetPersonalBacklog")]
        public async Task<List<Aufgabe>> GetAsync()
        {

            return await _context.PersonenAufgaben.ToListAsync(); 
        }
    }
}
