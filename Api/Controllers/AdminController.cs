using Application.Features.AdminPanel.GetAll;
using Application.Features.User.GetUserExcludeAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController:ControllerBase
    {
        private readonly IMediator mediator;
        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNumbers()
        {
            var results = await mediator.Send(new GetAllQuery());
            return Ok(results);
        }
        [Authorize]
        [HttpGet("Users")]
        public async Task<IActionResult> getUsers()
        {
            var results = await mediator.Send(new GetUserExcludeAdminQuery());
            return Ok(results);
        }
    }
}
