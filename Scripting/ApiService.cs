// ApiService.cs
// MAGIQ Documents
// Created by Jack Della on 26/04/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace MagiqApp.Services {

	public static class ApiService {

		//
		private static HttpResponseMessage response;
		private static readonly HttpClient client = new HttpClient();

		//
		#region URL
		private const string BeginningSoapXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"><soap12:Body>";
		private const string EndingSoapXml = "</soap12:Body></soap12:Envelope>";

		//
		public static string _url = "http://192.168.50.133/srv.asmx";
		public const string documentPath = "\\Corporate\\Invoices Test\\image004.png";

		#endregion

		//
		static ApiService() {
			client.Timeout = TimeSpan.FromSeconds(120); //default timeout
		}

		#region Soap
		//Attempts to authenticate a user, taking in username and password:
		//Returns an empty user if authentication failed, otherwise a user with an authentication ticket and id number:
		public static async Task<MagiqUser> SoapAuthenticateUser(string username, string password) {
			//New user item to return:
			var user = new MagiqUser();
			var doc = new XmlDocument();

			//SOAP Body Request:
			doc.LoadXml(
				BeginningSoapXml +
				"<AuthenticateUser xmlns=\"http://tempuri.org/\"><UID>" +
				username + "</UID><PWD>" +
				password + "</PWD></AuthenticateUser>" +
				EndingSoapXml
			);

			//Does the call:
			var content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/soap+xml");
			var theUrl = new Uri(_url);
			var result = await PostCallAsync(theUrl, content);

			if (result.Item1 == Status.Success) {
				//Converts web response to XML Node:
				doc.LoadXml(result.Item2);
				var dict = CleanXml(doc.DocumentElement);

				if (dict["success"] == "true") {
					//Puts response values into user item:
					user.Id = dict["userid"];
					user.Ticket = dict["ticket"];
				}
			}
			return user;
		}

		//NO HANDLER, uploads straight basse64string using SOAP-POST:
		public static async Task<bool> SoapUploadDocument(string ticket, string path, string base64file) {

			string xmlParams =
				"<uploadparams>" +
				"<item name=\"\" value=\"\"/>" +
				//"<item name=\"checkout\" value=\"false\"/>" + //jd don't need
				"</uploadparams>";

			//Escapes for sub-XML:
			xmlParams = xmlParams.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

			var doc = new XmlDocument();

			var name = documentPath + path;

			//SOAP Body Request:
			doc.LoadXml(
				BeginningSoapXml +
				"<UploadDocument4 xmlns=\"http://tempuri.org/\">" +
				"<AuthenticationTicket>" + ticket + "</AuthenticationTicket>" +
				"<Path>" + name + "</Path>" +
				"<FileContent>" + base64file + "</FileContent>" +
				"<xmlParameters>" + xmlParams + "</xmlParameters>" +
				"</UploadDocument4>" +
				EndingSoapXml
			);

			//Does the call:
			var content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/soap+xml");
			var theUrl = new Uri(_url);
			var result = await PostCallAsync(theUrl, content);

			if (result.Item1 == Status.Success) {
				//Converts web response to XML Node:
				doc.LoadXml(result.Item2);
				XmlNode fileContent = doc.DocumentElement;
				var dict = CleanXml(fileContent);
				return bool.Parse(dict["success"]);
			}
			return false;
		}

		//Downloads document at path from MAGIQ Documents, returns base64string:
		public static async Task<string> SoapDownloadDocument(string ticket, string path) {

			var doc = new XmlDocument();
			client.Timeout = TimeSpan.FromSeconds(120);
			//SOAP Body Request:
			doc.LoadXml(
				BeginningSoapXml +
				"<DownloadDocument xmlns = \"http://tempuri.org/\">" +
				"<AuthenticationTicket>" + ticket + "</AuthenticationTicket>" +
				"<Path>" + path + "</Path>" +
				"</DownloadDocument>" +
				EndingSoapXml
			);

			//Does the call:
			var content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/soap+xml");
			var theUrl = new Uri(_url);
			var result = await PostCallAsync(theUrl, content);

			if (result.Item1 == Status.Success) {
				//Converts web response to XML Node:
				doc.LoadXml(result.Item2);
				return doc.DocumentElement.InnerText;
			}
			return null;
		}



		public static async Task<string> SoapSetFav(string ticket, string path) {

			var doc = new XmlDocument();
			client.Timeout = TimeSpan.FromSeconds(120);
			//SOAP Body Request:
			doc.LoadXml(
				BeginningSoapXml +
				"<AddToFavorites xmlns = \"http://tempuri.org/\">" +
				"<authenticationTicket>" + ticket + "</authenticationTicket>" +
				"<itemPath>" + path + "</itemPath>" +
				"</AddToFavorites>" +
				EndingSoapXml
			);

			//Does the call:
			var content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/soap+xml");
			var theUrl = new Uri(_url);
			var result = await PostCallAsync(theUrl, content);

			if (result.Item1 == Status.Success) {
				//Converts web response to XML Node:
				doc.LoadXml(result.Item2);
				return doc.DocumentElement.InnerText;
			}
			return "Favourites failed";

		}




		public static async Task<string> SoapCompleteTask(string ticket, int taskId, string comments) {

			var doc = new XmlDocument();
			client.Timeout = TimeSpan.FromSeconds(120);
			//SOAP Body Request:
			doc.LoadXml(
				BeginningSoapXml +
				"<CompleteTask xmlns = \"http://tempuri.org/\">" +
				"<AuthenticationTicket>" + ticket + "</AuthenticationTicket>" +
				"<taskId>" + taskId + "</taskId>" +
				"<comments>" + comments + "</comments>" +
				"</CompleteTask>" +
				EndingSoapXml
			);

			//Does the call:
			var content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/soap+xml");
			var theUrl = new Uri(_url);
			var result = await PostCallAsync(theUrl, content);

			if (result.Item1 == Status.Success) {
				//Converts web response to XML Node:
				doc.LoadXml(result.Item2);
				return doc.DocumentElement.InnerText;
			}
			return "Task completion failed";

		}



		#endregion

		#region HttpRequests
		//HEAD - HTTP Request:
		public static async Task<Status> HeadCallAsync(Uri uri) {

			try { //Does HTTP request:
				response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return Status.Timeout;
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				return Status.Success;
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return Status.Failure;
		}

		//GET - HTTP Request:
		public static async Task<Tuple<Status, Stream>> GetCallAsync(Uri uri) {

			try { //Does HTTP request:
				response = await client.GetAsync(uri);
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return new Tuple<Status, Stream>(Status.Timeout, null);
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				//return await response.Content.ReadAsStringAsync();
				return new Tuple<Status, Stream>(Status.Success, await response.Content.ReadAsStreamAsync());
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return new Tuple<Status, Stream>(Status.Failure, null);
		}

		//GET STRING - HTTP Request:
		public static async Task<Tuple<Status, string>> GetStringCallAsync(Uri uri) {

			try { //Does HTTP request:
				response = await client.GetAsync(uri);
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return new Tuple<Status, string>(Status.Timeout, null);
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				//return await response.Content.ReadAsStringAsync();
				return new Tuple<Status, string>(Status.Success, await response.Content.ReadAsStringAsync());
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return new Tuple<Status, string>(Status.Failure, null);
		}

		//POST - HTTP Request:
		public static async Task<Tuple<Status, string>> PostCallAsync(Uri uri, StringContent content) {

			try { //Does HTTP request:
				response = await client.PostAsync(uri, content);
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return new Tuple<Status, string>(Status.Timeout, ex.Message);
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				//return await response.Content.ReadAsStringAsync();
				return new Tuple<Status, string>(Status.Success, await response.Content.ReadAsStringAsync());
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return new Tuple<Status, string>(Status.Failure, response.StatusCode.ToString());
		}
		#endregion

		#region HelperFuncs

		//Converts XML node attributes into neat key-value pairs:
		private static Dictionary<string, string> CleanXml(XmlNode node) {
			//Some API responses have XML sub-nodes, this drills down on the top one:
			while (node.HasChildNodes) {
				node = node.FirstChild; //POSSIBLY THROWING AWAY RELEVANT INFO
			}

			var dict = new Dictionary<string, string>();

			foreach (XmlAttribute att in node.Attributes) {
				Debug.WriteLine(att.Name + ": " + att.Value); //print each key-value pair
				dict.Add(att.Name, att.Value);
			}

			return dict;
		}

		#endregion

		//Model for Auth API Response:
		private class AuthLoginResponse {
			public string token { get; set; }
		}
		public class CustListResponse {
			public string Name { get; set; }
			public string Description { get; set; }
			public string Url { get; set; }
		}
		public enum Status {
			Success, Timeout, Failure
		}
		public struct MagiqUser {

			//Ticket and ID strings:
			public string Ticket;
			public string Id;
		}
	}
}
