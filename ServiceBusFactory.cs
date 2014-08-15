using NServiceBus;
using NServiceBus.Installation.Environments;

namespace MessageQueueTestHarness
{
	public class ServiceBusFactory
	{
		public static IBus GetServiceBus()
		{
			return Configure.With()
				.EnablePerformanceCounters()
				.DefiningCommandsAs(t => typeof (ICommand).IsAssignableFrom(t) || typeof (LIC.Library.Messaging.Markers.ICommand).IsAssignableFrom(t))
				.DefiningEventsAs(t => typeof (IEvent).IsAssignableFrom(t))
				.DefiningMessagesAs(t => typeof (IMessage).IsAssignableFrom(t))
				.DefaultBuilder()
				.XmlSerializer()
				.MsmqTransport()
				.IsTransactional(false)
				.PurgeOnStartup(false)
				.UnicastBus()
				.LoadMessageHandlers()
				.ImpersonateSender(false)
				.CreateBus()
				.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
		}
	}
}