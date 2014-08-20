using System;
using NServiceBus;
using ARC = LIC.Services.AnimalRecordsCompliance.Commands.Internal;
using SOM = LIC.Services.SalesOrderManagement.Commands.Internal;
using Mating = LIC.Services.Mating.Commands;

namespace MessageQueueTestHarness.MessageSenders
{
	public class ValidatorQueueMsgSender
	{
		private readonly IBus _bus;

		public ValidatorQueueMsgSender()
		{
			_bus = ServiceBusFactory.GetServiceBus();
		}

		public void SendToValidatorQueue(Enums.EventType eventType, Enums.ServiceBoundary serviceBoundary)
		{
			if (eventType == Enums.EventType.HeatEvent)
			{
				if (serviceBoundary == Enums.ServiceBoundary.Arc)
				{
					_bus.Send("animalrecordscompliance.eventvalidator.service.input",
						new ARC.ValidateAnimalEvents
						{
							AnimalKey = 20159657,
							Date = DateTime.Now,
							EventId = new Guid("6aad2090-7e64-4668-b0b7-c06523058b4a"),
							EventType = "ITOperations.Mating.HeatEvent",
							EventSource = "MindaPro"
						});
				}
				if (serviceBoundary == Enums.ServiceBoundary.Som)
				{
					_bus.Send("salesordermanagement.eventvalidator.service.input",
						new SOM.ValidateAnimalEvents
						{
							AnimalKey = 20159657,
							Date = DateTime.Now,
							EventId = new Guid("6aad2090-7e64-4668-b0b7-c06523058b4a"),
							EventType = "ITOperations.Mating.HeatEvent",
							EventSource = "MindaPro"
						});
				}
			}
			if (eventType == Enums.EventType.PdEvent)
			{
				_bus.Send("mating.eventvalidator.service.input",
					new Mating.ValidatePregnancyDiagnosisEvent
					{
						AnimalKey = 20159657,
						EventId = new Guid("6aad2090-7e64-4668-b0b7-c06523058b4a"),
						Date = DateTime.Now
					});
			}
		}
	}
}