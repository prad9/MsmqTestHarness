using System.Net;
using MessageQueueTestHarness.MessageSenders;
using EPSValidationResultGenericV0 = LIC.Services.ITOperations.EventProcessing.Events.ValiationResultGeneric.v0;

namespace MessageQueueTestHarness
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const Enums.EventType eventType = Enums.EventType.PdEvent;
			const Enums.ServiceBoundary sb = Enums.ServiceBoundary.Mating;

			//ITOpsUtils.SendToItOps();
			//new ReceiverQueueMsgSender().SendToReceiverQueue(eventType, sb);
			//new ValidatorQueueMsgSender().SendToValidatorQueue(eventType, sb);
			//new AggQueueMsgSender().SendToAggregatorQueue(eventType);
			//new ApproverQueueMsgSender().SendToApproverQueue(eventType);
			new WriterQueueMsgSender().SendToWriterQueue(eventType);
		}

		private static WebProxy GetProxy()
		{
			var proxy = new WebProxy("dnzakpx2.datacom.co.nz");
			//proxy.Credentials = new NetworkCredential(@"DATACOM\davidy", "Auckland47");
			proxy.UseDefaultCredentials = true;

			return proxy;
		}
	}

	public static class StringExtensions
	{
		public static string GoToTestServer(this string input)
		{
			const string newServer = "LIC-SISVCTest.dsldev.local";
			return input.Replace("davidyhpw8", newServer).Replace("localhost", newServer);
		}
	}
}