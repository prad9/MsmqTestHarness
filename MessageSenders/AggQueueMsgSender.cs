using System;
using LIC.Services.ITOperations.EventProcessing.Events.ValiationResultGeneric.v0;
using NServiceBus;

namespace MessageQueueTestHarness.MessageSenders
{
	public class AggQueueMsgSender
	{
		private readonly IBus _bus;

		public AggQueueMsgSender()
		{
			_bus = ServiceBusFactory.GetServiceBus();
		}

		public void SendToAggregatorQueue(Enums.EventType eventType)
		{
			if (eventType == Enums.EventType.HeatEvent)
			{
				_bus.Send("eventprocessing.validationaggregator.service.input",
					new ValidationResult
					{
						EventID = Guid.Parse("12f9040f-50c3-4532-a1f5-7a876bd3aa8e"),
						EventServiceBoundary = "ITOperations.Mating",
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = "ITOperations.Mating.HeatEvent",
						EventSource = "CalvingMobileApp"
					});
			}
			if (eventType == Enums.EventType.PdEvent)
			{
				_bus.Send("eventprocessing.validationaggregator.service.input",
					new ValidationResult
					{
						EventID = Guid.Parse("A3A52979-313F-41E3-8628-1F0D7D9AD3F7"),
						EventServiceBoundary = "ITOperations.Mating",
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = "ITOperations.Mating.PregnancyDiagnosisEvent",
						EventSource = "MindaPro"
					});
			}
			if (eventType == Enums.EventType.HiEvent)
			{
				_bus.Send("eventprocessing.validationaggregator.service.input",
					new ValidationResult
					{
						EventID = Guid.Parse("1ec69094-80cf-4597-a841-78873f462d5a"),
						EventServiceBoundary = "Treatment",
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = "ITOperations.Treatment.HealthInterventionEvent",
						EventSource = "MindaPro"
					});
			}
		}
	}
}