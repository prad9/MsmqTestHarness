using System;
using LIC.Library.Messaging.Markers;

namespace LIC.Services.ITOperations.EventProcessing.Commands.Write
{
	public class InsertEvent : ICommand, IMessage
	{
		public Guid EventId { get; set; }

		public string EventType { get; set; }
	}
}