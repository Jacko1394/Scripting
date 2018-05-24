// ApiService.cs
// MAGIQ Documents
// Created by Jack Della on 26/04/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.IO;

namespace Scripting {

	public static class ApiService {

		//
		private static readonly HttpClient client = new HttpClient();
		private static HttpResponseMessage response;

		static ApiService() {
			client.Timeout = TimeSpan.FromSeconds(12); //default timeout
		}

		#region URL
		private static Uri BaseUrl {
			get { return new Uri(""); }
		}
		private static Uri AuthUrl {
			get { return new Uri(BaseUrl + "/api/v1/auth/login"); }
		}
		private static Uri NEWUrl { //template
			get { return new Uri(BaseUrl + "/NEWURL"); }
		}
		#endregion

		#region Authentication
		//Checks if given URL is reachable:
		public static async Task<Status> ValidateUrlAsync(Uri url) {
			return await HeadCallAsync(url);
		}
		//Checks with the server that the currently stored token is still valid:
		public static async Task<bool> CheckTokenValidAsync(string token) {
			//implement
			await Task.Delay(1000).ContinueWith(t => Debug.WriteLine("API: TOKEN no longer valid. (to implement API)"));
			return false;
		}
		//Authenticates using email, password API Call to MAGIQ Cloud Auth:
		public static async Task<Tuple<Status, string>> AuthenticateAsync(string email, string password) {

			var json = JsonConvert.SerializeObject(new {
				Email = email,
				Password = password
			});

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var result = await PostCallAsync(AuthUrl, content);

			if (result.Item1 == Status.Success) {
				var token = JsonConvert.DeserializeObject<AuthLoginResponse>(result.Item2).token;
				return new Tuple<Status, string>(Status.Success, token);
			}

			return new Tuple<Status, string>(result.Item1, result.Item2);
		}
		//Logouts current user from the server:
		public static async Task<Status> LogoutAsync() {
			await Task.Delay(1000).ContinueWith(t => Debug.WriteLine("API: Logged out. (to implement API)"));
			return Status.Failure;
		}
		#endregion

		#region Documents
		//Downloads a file from given url location:
		public static async Task<Tuple<Status, Stream>> DownloadFileAsync(Uri url) {
			//Increase timout time for possible larger file download times:
			client.Timeout = TimeSpan.FromMinutes(3);
			var file = await GetCallAsync(url);
			client.Timeout = TimeSpan.FromSeconds(12);
			return file;
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

		//Model for Auth API Response:
		private class AuthLoginResponse {
			public string token { get; set; }
		}

		//Enum for HTTP Request statuses:
		public enum Status { Success, Malformed, NoInternet, Timeout, Failure }

	}
}
