using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class GenreController:ControllerBase
    {
        private readonly IReferenceDataService referenceDataService;
        public GenreController(IReferenceDataService referenceDataService)
        {
            this.referenceDataService = referenceDataService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var results = await referenceDataService.GetGenres();
            return Ok(results);
        }
    }
}
