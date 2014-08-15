using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using LIC.Library.Validation.v1;
using LIC.MINDAWeb.Calendar.Commands;
using LIC.Services.ITOperations.EventProcessing.Events.ValiationResultGeneric.v0;
using NServiceBus;
using AC = LIC.Services.AnimalCharacteristic.Commands;
using AGM = LIC.Services.AnimalGroupMembership.Calendar.Commands.v0;
using AGMWebAPI = LIC.Services.ITOperations.AnimalGroupMembership.WebApi.v0;
using AI = LIC.Services.AnimalIdentity.Calendar.Commands;
using AIWebAPI = LIC.Services.ITOperations.AnimalIdentity.WebApi.v0;
using ARC = LIC.Services.AnimalRecordsCompliance.Commands;
using Mating = LIC.Services.Mating.Commands;
using MatingWebAPI = LIC.Services.ITOperations.Mating.WebApi.v0;
using SOM = LIC.Services.SalesOrderManagement.Commands;

namespace MessageQueueTestHarness.MessageSenders
{
	public class ReceiverQueueMsgSender
	{
		private readonly IBus _bus;

		public ReceiverQueueMsgSender()
		{
			_bus = ServiceBusFactory.GetServiceBus();
		}

		public void SendToReceiverQueue(Enums.EventType eventType, Enums.ServiceBoundary serviceBoundary)
		{
			if (eventType == Enums.EventType.HeatEvent || eventType == Enums.EventType.PdEvent)
			{
				switch (serviceBoundary)
				{
					case Enums.ServiceBoundary.Calendar:
						SendToCalenderQueue();
						break;
					case Enums.ServiceBoundary.Agm:
						SendToAgmQueue();
						break;
					case Enums.ServiceBoundary.Ai:
						SendToAiQueue();
						break;
					case Enums.ServiceBoundary.Arc:
						SendToArcQueue();
						break;
					case Enums.ServiceBoundary.Som:
						SendToSomQueue();
						break;
					case Enums.ServiceBoundary.AnimalCharacteristics:
						SendToAcQueue();
						break;
					case Enums.ServiceBoundary.Mating:
						SendToMatingQueue();
						break;
				}
			}
			if (eventType == Enums.EventType.EnrolEvent)
			{
				_bus.Send("treatment.service.input",
					new Mating.ValidatePregnancyDiagnosisEvent
					{
						AnimalKey = 20159657,
						EventId = new Guid("6aad2090-7e64-4668-b0b7-c06523058b4a"),
						Date = DateTime.Now
					});
			}
		}

		private void SendToMatingQueue()
		{
			_bus.Send("mating.service.input@localhost", new Mating.SubmitHeatEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 123456
			});
		}

		private void SendToAcQueue()
		{
			_bus.Send("animalcharacteristic.service.input@localhost", new AC.SubmitAnimalEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 123456,
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName
			});
		}

		private void SendToSomQueue()
		{
			_bus.Send("salesordermanagement.service.input@localhost", new SOM.SubmitAnimalEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 31542059,
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName,
				EventSource = "MindaPro"
			});
		}

		private void SendToArcQueue()
		{
			_bus.Send("animalrecordscompliance.service.input@localhost", new ARC.SubmitAnimalEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 31542059,
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName,
				EventSource = "MindaPro"
			});
		}

		private void SendToAiQueue()
		{
			_bus.Send("animalidentity.service.input@localhost", new AI.SubmitAnimalEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 123456,
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName
			});
		}

		private void SendToAgmQueue()
		{
			_bus.Send("animalgroupmembership.service.input@localhost", new AGM.SubmitAnimalEvent
			{
				EventId = Guid.NewGuid(),
				Date = DateTime.Now,
				AnimalKey = 123456,
				AnimalGroupBpNumber = "",
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName
			});
		}

		private void SendToCalenderQueue()
		{
			_bus.Send("calendar.service.input@localhost", new SubmitEventToHoldingPen
			{
				EventId = Guid.NewGuid(),
				EventDate = DateTime.Now,
				EventType = MatingWebAPI.ResourceLocator.HeatEvent.ResourceName,
				AnimalGroupBpNumber = "",
				Source = "MindaPro",
				SourceId = ""
			});
		}

		private static void TestSbSingle()
		{
			var bus = ServiceBusFactory.GetServiceBus();
			var eventId = new Guid("cf0a200c-e6d3-472e-8c2c-286b2032ba6e");

			const string EventType = "ITOperations.Mating.HeatEvent";

			var validationResult = new EventValidator("Animal", "animalIdentity").GetValidationResult(eventId);
			const string ValidatorQueueName = "EventProcessing.ValidationAggregator.service.input";
			using (var ts = new TransactionScope())
			{
				bus.Send(ValidatorQueueName,
					new ValidationResult
					{
						EventID = eventId,
						EventServiceBoundary = AIWebAPI.ResourceLocator.ServiceName,
						EventSource = AnimalIdentityServiceEventSources.MindaPro,
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = EventType
					});

				bus.Send(ValidatorQueueName,
					new ValidationResult
					{
						EventID = eventId,
						EventServiceBoundary = AIWebAPI.ResourceLocator.ServiceName,
						EventSource = AnimalIdentityServiceEventSources.MindaPro,
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = EventType
					});
				bus.Send(ValidatorQueueName,
					new ValidationResult
					{
						EventID = eventId,
						EventServiceBoundary = "ITOperations.AnimalCharacteristic",
						EventSource = AnimalIdentityServiceEventSources.MindaPro,
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = EventType
					});

				bus.Send(ValidatorQueueName,
					new ValidationResult
					{
						EventID = eventId,
						EventServiceBoundary = AIWebAPI.ResourceLocator.ServiceName,
						EventSource = AnimalIdentityServiceEventSources.MindaPro,
						ValidationOutcome = ValidationResultFlag.Valid,
						EventType = EventType
					});

				ts.Complete();
			}
		}

		private static void TestSbParallel()
		{
			var bus = ServiceBusFactory.GetServiceBus();
			var addDays = new List<int>();

			Debug.Print(DateTime.Now.ToString());

			for (var i = 119; i <= 119; i++)
			{
				addDays.Add(i);
			}


			Parallel.ForEach(addDays, addDay =>
			{
				Debug.Print(Guid.NewGuid().ToString());

				bus.Send(ConfigurationManager.AppSettings["CalendarServiceQueue"].GoToTestServer(), new SubmitEventToHoldingPen
				{
					EventId = Guid.NewGuid(),
					EventDate = DateTime.Now,
					EventType = AGMWebAPI.ResourceLocator.ServiceName,
					AnimalGroupBpNumber = "",
					Source = "MindaPro",
					SourceId = ""
				});

				bus.Send(ConfigurationManager.AppSettings["AnimalGroupMembershipServiceQueue"].GoToTestServer(), new AGM.SubmitAnimalEvent
				{
					EventId = Guid.NewGuid(),
					Date = DateTime.Now,
					AnimalKey = 123456,
					AnimalGroupBpNumber = "",
					EventType = AGMWebAPI.ResourceLocator.ServiceName
				});

				bus.Send(ConfigurationManager.AppSettings["AnimalIdentityServiceQueue"].GoToTestServer(), new AI.SubmitAnimalEvent
				{
					EventId = Guid.NewGuid(),
					Date = DateTime.Now,
					AnimalKey = 123456,
					EventType = AIWebAPI.ResourceLocator.ServiceName
				});

				bus.Send(ConfigurationManager.AppSettings["AnimalCharacteristicServiceQueue"].GoToTestServer(), new AC.SubmitAnimalEvent
				{
					EventId = Guid.NewGuid(),
					Date = DateTime.Now,
					AnimalKey = 123456,
					EventType = AIWebAPI.ResourceLocator.ServiceName
				});

				bus.Send(ConfigurationManager.AppSettings["MatingServiceQueue"].GoToTestServer(), new Mating.SubmitHeatEvent
				{
					EventId = Guid.NewGuid(),
					Date = DateTime.Now,
					AnimalKey = 123456
				});
			}
				);
		}
	}

	public class Heat
	{
		public Guid EventId { get; set; }
		public DateTime Date { get; set; }
		public int AnimalKey { get; set; }
	}

	public static class AnimalIdentityServiceEventSources
	{
		public const string MindaPro = "MindaPro";
	}
}