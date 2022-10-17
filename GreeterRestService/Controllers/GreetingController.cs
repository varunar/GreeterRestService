using GreeterRestService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreeterRestService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GreetingController : ControllerBase
	{
		[HttpPost]
		public async Task<GreetResponse> PostGreet(GreetRequest greetRequest)
		{
			var greetResponse = new GreetResponse();
			greetResponse.Message = $"Hello {greetRequest.Name}";
			//return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
			return greetResponse;
		}
	}
}
