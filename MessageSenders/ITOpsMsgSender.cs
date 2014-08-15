using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace MessageQueueTestHarness.MessageSenders
{
	public class ItOpsMsgSender
	{
		public static void SendToItOps()
		{
			var token = GetStsToken();
			var newGuid = Guid.NewGuid();
			const bool isHeat = false;
			const bool isHealth = true;

			Debug.Print(newGuid.ToString());
			var dt = DateTime.Parse("2014-06-24");
			var sepdt = DateTime.Parse("2014-06-25");
			var lastdosedt = DateTime.Parse("2014-06-26");
			var rettovatdt = DateTime.Parse("2014-06-27");

			var xmlString = new StringBuilder();
			HttpWebRequest httpWebRequest;

			if (isHeat)
			{
				xmlString.Append("<HeatEvent");
				xmlString.Append(" xmlns=\"http://lic.co.nz/mating/heat/v0\">");
				xmlString.Append("<EventId>" + newGuid + "</EventId>");
				xmlString.Append("<Date>" + dt.ToString("yyyy-MM-dd") + "</Date>");
				xmlString.Append("<AnimalKey>31541957</AnimalKey>");
				xmlString.Append("</HeatEvent>");
				httpWebRequest = (HttpWebRequest) WebRequest.Create("http://itops-dev.api.minda.livestock.org.nz/Services/ITOperations.Mating/v0/event/heat/" + newGuid);
			}
			if (isHealth)
			{
				xmlString.Append("<?xml version=\"1.0\"?>");
				xmlString.Append("<HealthInterventionEvent");
				xmlString.Append(
					" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://lic.co.nz/treatment/treatmentevents/v0\">");
				xmlString.Append("<EventId>" + newGuid + "</EventId>");
				//xmlString.Append("<EventId>5c1dcd3e-f555-46e2-953d-50c1056818fc</EventId>");
				xmlString.Append("<AnimalKey>31541957</AnimalKey>");
				xmlString.Append("<Date>" + dt.ToString("yyyy-MM-dd") + "</Date>");
				//xmlString.Append("<Source></Source>"); // what is this???
				xmlString.Append("<ConditionCode>OC</ConditionCode>");
				xmlString.Append("<AffectedBodyPart>");
				xmlString.Append("<LF>true</LF>");
				xmlString.Append("<RF>false</RF>");
				xmlString.Append("<LR>false</LR>");
				xmlString.Append("<RR>false</RR>");
				xmlString.Append("</AffectedBodyPart>");
				xmlString.Append("<ProductCode>0067</ProductCode>");
				xmlString.Append("<DoseCount>1</DoseCount>");
				xmlString.Append("<DoseAmount>2</DoseAmount>");
				xmlString.Append("<DoseUnitOfMeasure>CAP</DoseUnitOfMeasure>");
				xmlString.Append("<MeatWithholdingDays>15</MeatWithholdingDays>");
				xmlString.Append("<MilkWithholdingHours>10</MilkWithholdingHours>");
				xmlString.Append("<SeparationDate xsi:nil='true' />");
				xmlString.Append("<Veterinarian>Bob Higgans</Veterinarian>");
				xmlString.Append("<RecordType>AH1</RecordType>");
				xmlString.Append("<BatchNumberExpDate>B=1234D=25/07/2020</BatchNumberExpDate>");
				xmlString.Append("<LastDoseAdminDate xsi:nil='true' />");
				xmlString.Append("<LastDoseAdminTimeOfDay>AM</LastDoseAdminTimeOfDay>");
				xmlString.Append("<MilkReturnToVatDate xsi:nil='true' />");
				xmlString.Append("<MilkReturnToVatTimeOfDay>PM</MilkReturnToVatTimeOfDay>");

				xmlString.Append("</HealthInterventionEvent>");
				httpWebRequest =
					(HttpWebRequest) WebRequest.Create("http://itops-dev.api.minda.livestock.org.nz/services/itoperations.Treatment/v0/event/treatment/HealthIntervention/" + newGuid);
			}
			else
			{
				xmlString.Append("<PregnancyDiagnosisEvent");
				xmlString.Append(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

				xmlString.Append(" xmlns=\"http://lic.co.nz/mating/pregnancydiagnosis/v0\">");

				//xmlString.Append(
				//     "<ns0:HeatEvent xmlns:ns0=\"http://lic.co.nz/mating/heat/v0\" xmlns:wsdl=\"http://microsoft.com/wsdl/types/\">");

				xmlString.Append("<EventId>" + newGuid + "</EventId>");
				//xmlString.Append("<EventId>217BD7A4-4AC5-49D6-A211-00DF7ABC7686</EventId>");
				xmlString.Append("<Date>" + dt.ToString("yyyy-MM-dd") + "</Date>");
				xmlString.Append("<AnimalKey>30712492</AnimalKey>");


				xmlString.Append("<Diagnosis>Pregnant</Diagnosis>");
				xmlString.Append("<Duration><Value>20</Value><Unit>Days</Unit></Duration>");

				/*
				xmlString.Append("<Diagnosis>Empty</Diagnosis>");
				xmlString.Append("<Duration xsi:nil=\"true\" />");
				*/

				xmlString.Append("<AssessorPtPtCode>MVCK</AssessorPtPtCode>");
				//xmlString.Append("<AssessorPtPtCode xsi:nil=\"true\" />");

				xmlString.Append("<NumberOfCalves>Two</NumberOfCalves>");
				//xmlString.Append("<NumberOfCalves xsi:nil=\"true\" />");

				xmlString.Append("</PregnancyDiagnosisEvent>");

				httpWebRequest = (HttpWebRequest) WebRequest.Create("http://itops-dev.api.minda.livestock.org.nz/Services/ITOperations.Mating/v0/event/pregnancydiagnosis/" + newGuid);
			}

			//httpWebRequest.Host = "itops-dev.api.minda.livestock.org.nz";
			httpWebRequest.Method = "PUT";
			httpWebRequest.ContentType = "application/xml";
			httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
			httpWebRequest.KeepAlive = true;
			httpWebRequest.AllowAutoRedirect = false;
			//httpWebRequest.Proxy = GetProxy();

			var byteArray = Encoding.UTF8.GetBytes(xmlString.ToString());
			httpWebRequest.ContentLength = byteArray.Length;
			var dataStream = httpWebRequest.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();
			dataStream.Dispose();

			var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
			var responseStream = httpWebResponse.GetResponseStream();
			var streamReader = new StreamReader(responseStream);
			var response = streamReader.ReadToEnd();

			Console.Write(response);
			Console.ReadLine();
		}

		private static string GetStsToken()
		{
			var ptptCode = "";
			const string pin = "37138";
			ptptCode = "CPHN"; //Animal Key 17236453, 26262373

			var headerContent = "username=" + ptptCode + "&password=" + pin + "&grant_type=password&client_id=MindaPro&client_secret=_6UZFqfeOeiKy8Ign1lSuNv$CC)26H";
			// var headerContent = "username=thirdparty1/5013191&password=Beta1234&grant_type=password&client_id=Connector&client_secret=ala2Y4-6F1hc*,i6@M67H2.3BR70:A";
			const string tokenUrl = "http://dev.sts.mindaweb.livestock.org.nz/oauth/token"; //"http://lic-sisvctest.dsldev.local:1339/oauth/token";

			var request = (HttpWebRequest) WebRequest.Create(tokenUrl);
			// Set the Method property of the request to POST.
			//request.Host = "itops-dev.api.minda.livestock.org.nz"; //_mindaWebHost;
			request.Method = "POST";
			// Set cookie container to maintain cookies
			request.AllowAutoRedirect = false;
			//request.Proxy = GetProxy();
			var content = headerContent;

			if (request.Method == "POST")
			{
				// Convert POST data to a byte array.
				var byteArray = Encoding.UTF8.GetBytes(content);
				// Set the ContentType property of the WebRequest.
				request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
				// Set the ContentLength property of the WebRequest.
				request.UserAgent = "DotNetOpenAuth/4.1.4.12333";
				request.ContentLength = byteArray.Length;
				// Get the request stream.
				var dataStream = request.GetRequestStream();
				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
				dataStream.Dispose();

				var resp = request.GetResponse();
				var respStream = resp.GetResponseStream();

				var a = new StreamReader(respStream);
				var s = a.ReadToEnd();
				var array = s.Split(',');
				var tokenString = new StringBuilder();
				tokenString.Append(array[0]);
				tokenString.Replace("{\"access_token\":\"", "");
				tokenString.Replace("\"", "");
				return tokenString.ToString();
			}
			return String.Empty;
		}

		public static string GetSecurityToken()
		{
			var request = (HttpWebRequest)WebRequest.Create("http://dev.sts.mindaweb.livestock.org.nz/OAuth/Token");
			request.Method = "POST";
			const string content = "username=Sarah&password=Beta1234&grant_type=password&client_id=MindaPro&client_secret=cU5uk_gu%3FegE9hAD6UVa%2Bap6metrep";

			if (request.Method == "POST")
			{
				var byteArray = Encoding.UTF8.GetBytes(content);
				request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
				request.UserAgent = "DotNetOpenAuth/4.1.4.12333";
				request.ContentLength = byteArray.Length;
				var dataStream = request.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
				dataStream.Dispose();

				var resp = request.GetResponse();

				var respStream = resp.GetResponseStream();
				var a = new StreamReader(respStream);
				var s = a.ReadToEnd();
				var array = s.Split(',');
				var tokenString = new StringBuilder();
				tokenString.Append(array[0]);
				tokenString.Replace("{\"access_token\":\"", "");
				tokenString.Replace("\"", "");
				return tokenString.ToString();
			}
			return null;
		}
	}
}