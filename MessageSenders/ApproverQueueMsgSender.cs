using System;
using LIC.Services.ITOperations.EventProcessing.Commands.Approve;
using LIC.Services.ITOperations.EventProcessing.Contract.v0;
using NServiceBus;

namespace MessageQueueTestHarness.MessageSenders
{
	public class ApproverQueueMsgSender
	{
		private readonly IBus _bus;

		public ApproverQueueMsgSender()
		{
			_bus = ServiceBusFactory.GetServiceBus();
		}

		public void SendToApproverQueue(Enums.EventType eventType)
		{
			if (eventType == Enums.EventType.HiEvent)
			{
				_bus.Send("eventprocessing.eventapprover.service.input",
					new ApproveEvent
					{
						EventId = Guid.Parse("2f18c499-dfb0-49bf-8e7a-6a6ce8c2b674"),
						EventType = "ITOperations.Treatment.HealthInterventionEvent",
						ValidationOutcome = ValidationResultFlag.Valid,
						EventSource = "MindaPro"
					});
			}
			if (eventType == Enums.EventType.PdEvent)
			{
				_bus.Send("eventprocessing.eventapprover.service.input",
					new ApproveEvent
					{
						EventId = Guid.Parse("1ec69094-80cf-4597-a841-78873f462d5a"),
						EventType = "ITOperations.Mating.PregnancyDiagnosisEvent",
						ValidationOutcome = ValidationResultFlag.Valid,
						EventSource = "MindaPro"
					});
			}
		}
	}
}