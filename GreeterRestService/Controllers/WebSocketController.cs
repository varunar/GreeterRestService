using System.Collections;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace GreeterRestService.Controllers;
// <snippet>
public class WebSocketController : ControllerBase
{
	private readonly ILogger<WebSocketController> _logger;
	private readonly IConfiguration _config;


	public WebSocketController(ILogger<WebSocketController> logger, IConfiguration config)
	{
		_logger = logger;
		_config = config;
	}
	[HttpGet("/ws")]
	public async Task Get()
	{
		if (HttpContext.WebSockets.IsWebSocketRequest)
		{
			using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
			await Echo(webSocket);
		}
		else
		{
			HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
		}
	}
	// </snippet>

	private async Task Echo(WebSocket webSocket)
	{
		var buffer = new byte[1024];
		var receiveResult = await webSocket.ReceiveAsync(
			new ArraySegment<byte>(buffer), CancellationToken.None);
		while (!receiveResult.CloseStatus.HasValue)
		{
			string result = System.Text.Encoding.UTF8.GetString(buffer);
			result = result.TrimEnd();
			result += " ";
			result += _config["server"];
			Console.WriteLine(result);
			byte[] resultBytes = Encoding.ASCII.GetBytes(result);
			await webSocket.SendAsync(
				new ArraySegment<byte>(resultBytes, 0, resultBytes.Length),
				receiveResult.MessageType,
				receiveResult.EndOfMessage,
				CancellationToken.None);

			receiveResult = await webSocket.ReceiveAsync(
				new ArraySegment<byte>(buffer), CancellationToken.None);
		}

		await webSocket.CloseAsync(
			receiveResult.CloseStatus.Value,
			receiveResult.CloseStatusDescription,
			CancellationToken.None);
	}
}