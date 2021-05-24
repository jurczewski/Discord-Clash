using DiscordClash.Application.Queries;
using DiscordClash.Application.UseCases.API;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordClash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChoicesController : ControllerBase
    {
        /// <summary>
        /// Get all users' choices (event, user, label).
        /// </summary>
        /// <param name="apiUseCase"></param>
        /// <returns>Users' choices.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChoiceDto>>> GetAll([FromServices] GetAllChoicesUseCase apiUseCase)
        {
            var choices = await apiUseCase.Execute();
            return Ok(choices);
        }
    }
}
