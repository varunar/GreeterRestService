using System.Diagnostics.Tracing;

namespace GreeterRestService
{
	public class HardwareStatsEventListener : EventListener
	{
		private List<String> metricList = new List<String> { "cpu-usage", "working-set" };


		protected override void OnEventSourceCreated(EventSource eventSource)
		{
			Console.WriteLine($"{eventSource.Guid} | {eventSource.Name}");
			if (eventSource.Name.Equals("System.Runtime"))
				EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All, new Dictionary<string, string> { { "EventCounterIntervalSec", "1" } });
			base.OnEventSourceCreated(eventSource);
		}

		protected override void OnEventWritten(EventWrittenEventArgs eventData)
		{

			if (eventData.Payload == null || eventData.Payload.Count == 0)
				return;
			if (eventData.Payload[0] is IDictionary<string, object> eventPayload && eventPayload.TryGetValue("Name", out var nameData) && nameData is string name && metricList.Contains(name))
			{
				if (eventPayload.TryGetValue("Mean", out var value))
				{
					if (value is double dValue)
					{
					//Console.WriteLine($" {name} {dValue}");
						base.OnEventWritten(eventData);
					}
				}
			}
		}
	}
}
