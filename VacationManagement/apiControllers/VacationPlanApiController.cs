using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationManagement.Data;

namespace VacationManagement.apiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationPlanApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VacationPlanApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> FindEmployee(string name)
        {
            try
            {
                var result = _context.Employees.Where(a => a.Name.Contains(name)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
