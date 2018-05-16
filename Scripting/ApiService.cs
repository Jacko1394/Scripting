// ApiService.cs
// MAGIQ Documents
// Created by Jack Della on 26/04/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Net;

namespace Scripting {

	public static class ApiService {

		//public static Uri Url = new Uri("https://i.imgur.com/xmTuSaj.png"); //test download
		public static Uri Url = new Uri("https://cdn-images-1.medium.com/max/1600/1*dMI4bncj_l4dYjZostWS4g.png");

		//Authenticates using email, password API Call to MAGIQ Cloud Auth:
		public async static Task<string> MakeCallAsync() {

			//var json = JsonConvert.SerializeObject(new {
			//	Email = email,
			//	Password = password
			//});
			//var json = "requestBodyInHere";
			//var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await GetCallAsync(Url);

			//if (response.Item1 == Status.Success) {
			//	var token = JsonConvert.DeserializeObject<AuthLoginResponse>(response.Item2).token;
			//	return new Tuple<Status, string>(Status.Success, token);
			//}

			Console.WriteLine("RESPONSE Length: " + response.Length);

			var path = "output.png";

			Stream stream = await DownloadAsync(Url);
			Console.WriteLine("stream: " + stream.Length);
			using (Stream file = File.Create(path)) {
				stream.CopyTo(file);
			}

			//return response;
			return null;
		}

		//POST - HTTP Request:
		public static async Task<string> PostCallAsync(Uri uri, StringContent content) {

			//HTTP Setup:
			var client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(8) //timeout after 8 seconds
			};

			HttpResponseMessage response;

			try { //Does HTTP request:
				response = await client.PostAsync(uri, content);
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return ex.Message;
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				//return await response.Content.ReadAsStringAsync();
				return await response.Content.ReadAsStringAsync();
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return response.StatusCode.ToString();
		}

		//GET - HTTP Request:
		public static async Task<string> GetCallAsync(Uri uri) {

			//HTTP Setup:
			var client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(8) //timeout after 8 seconds
			};

			HttpResponseMessage response;

			try { //Does HTTP request:
				response = await client.GetAsync(uri);
			} catch (Exception ex) {
				Debug.WriteLine("API Request error: " + ex.Message);
				return ex.Message;
			}

			//If successful, return content as string:
			if (response.IsSuccessStatusCode) {
				return await response.Content.ReadAsStringAsync();
				//return await response.Content.ReadAsStreamAsync();
			}
			//If not successful:
			Debug.WriteLine("API Response error: " + response.StatusCode.ToString());
			return response.StatusCode.ToString();
		}

		//STREAM:
		public static async Task<Stream> DownloadAsync(Uri uri) {

			//HTTP Setup:
			var client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(8) //timeout after 8 seconds
			};

			HttpResponseMessage response;

			response = await client.GetAsync(uri);

			return await response.Content.ReadAsStreamAsync();

		}

		//HEAD - HTTP Request: returns TRUE for http success 200 codes:
		public static async Task<Status> HeadCallAsync(Uri uri) {

			//HTTP Setup:
			var client = new HttpClient {
				Timeout = TimeSpan.FromSeconds(8) //timeout after 8 seconds
			};

			HttpResponseMessage response;

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

		//Enum for HTTP Request statuses:
		public enum Status { Success, Malformed, NoInternet, Timeout, Failure }

		//Gets photo, converts to upload thru API:
		//Function converts stream input, to base64string output:
		public static string StreamToString(Stream stream) {
			var ms = new MemoryStream();
			stream.CopyTo(ms);
			return Convert.ToBase64String(ms.ToArray());
		}

		//Download to image stream:
		//Function converts base64string input, to stream output:
		public static Stream StringToStream(string base64String) {
			var bytes = Convert.FromBase64String(base64String);
			return new MemoryStream(bytes);
		}

	}
}
