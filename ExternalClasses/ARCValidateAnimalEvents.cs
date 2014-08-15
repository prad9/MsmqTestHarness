using System;
using LIC.Library.Messaging.Markers;

namespace LIC.Services.AnimalRecordsCompliance.Commands
{
	public class ValidateAnimalEvents : ICommand
	{
		public int AnimalKey { get; set; }
		public Guid EventId { get; set; }
		public string EventType { get; set; }
		public DateTime Date { get; set; }
		public string EventSource { get; set; }
	}
}