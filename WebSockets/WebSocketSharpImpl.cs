using System;

namespace Reversid.WebSockets
{
	public class WebSocketSharpImpl : IWebSocket
	{
		private WebSocketSharp.WebSocket WS { get; set; }

		public WebSocketSharpImpl(string url)
		{
			WS = new WebSocketSharp.WebSocket(url);
		}

		public IWebSocketConnection Connect()
		{
			WS.Connect();
			return new WebSocketSharpConnectionImpl(WS);
		}
	}

	public class WebSocketSharpConnectionImpl : IWebSocketConnection
	{
		public WebSocketSharpConnectionImpl(WebSocketSharp.WebSocket ws)
		{
			ws.OnMessage += (s, e) => {
				OnMessage(this, e.Data);
			};
			ws.OnError += (s, e) => {
				OnError(this, e.Message);
			};

			WS = ws;
		}

		private WebSocketSharp.WebSocket WS { get; set; }

		public event EventHandler<string> OnMessage;
		public event EventHandler<string> OnError;

		public void Send(string message)
		{
			WS.Send(message);
		}
	}
}
