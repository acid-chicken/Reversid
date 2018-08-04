using Newtonsoft.Json.Linq;

namespace Reversid.Misskey
{
	public class StreamEvent
	{
		public string Type { get; set; }
		public JObject Body { get; set; }
	}
}
