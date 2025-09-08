using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="User")]
    [ApiController]
    [Route("Api/Liked")]
    public class LikedMovieGameTvSeries : ControllerBase
    {

    }
}
