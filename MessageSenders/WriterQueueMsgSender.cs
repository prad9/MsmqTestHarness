using System;
using LIC.Services.ITOperations.EventProcessing.Commands.Write;
using NServiceBus;

namespace MessageQueueTestHarness.MessageSenders
{
	public class WriterQueueMsgSender
	{
		private readonly IBus _bus;

		public WriterQueueMsgSender()
		{
			_bus = ServiceBusFactory.GetServiceBus();
		}

		//"EventProcessing.ValidationAggregator.service.input@davidyhpw8"
		public void SendToWriterQueue(Enums.EventType eventType)
		{
			if (eventType == Enums.EventType.PdEvent)
			{
				_bus.Send("eventprocessing.eventwrite.service.input",
					new InsertEvent
					{
						EventId = Guid.Parse("A3A52979-313F-41E3-8628-1F0D7D9AD3F7"),
						EventType = "ITOperations.Mating.PregnancyDiagnosisEvent"
					});
			}
			if (eventType == Enums.EventType.HiEvent)
			{
				_bus.Send("eventprocessing.eventwrite.service.input",
					new InsertEvent
					{
						EventId = Guid.Parse("5DA4E958-04DF-44BA-BD80-45BDF03B52D5"),
						EventType = "ITOperations.Treatment.HealthInterventionEvent"
					});
			}
		}
	}
}