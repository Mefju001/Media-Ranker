using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("Api/Liked")]
    public class LikedMovieGameTvSeries : ControllerBase
    {
        private readonly ILikedMediaServices likedMediaServices;

        public LikedMovieGameTvSeries(ILikedMediaServices likedMediaServices)
        {
            this.likedMediaServices = likedMediaServices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await likedMediaServices.GetAllAsync();
            return Ok(movies);
        }
    }
}
