using System;

namespace Reversid.WebSockets
{
	public interface IWebSocketConnection
	{
		event EventHandler<string> OnMessage;
		event EventHandler<string> OnError;

		void Send(string message);
	}
}
