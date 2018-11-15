// Program.cs
// MAGIQ Documents
// Created by Jack Della on 22/03/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Threading.Tasks;
using MagiqApp.Services;
using System.Linq;
using MvvmHelpers;
using System.Diagnostics;

namespace Scripting {

	public class MainClass {

		public static void Main() {

			//var life = random number for average life expectancy

			for (var i = 0; i < life; i++) {

			}



			Init();
			var t = Task.Delay(20_000);
			t.Wait();
			Debug.WriteLine("Done.");

			/*
			AmazonGlacierClient client;
			client = new AmazonGlacierClient(Amazon.RegionEndpoint.USEast1);

			CreateVaultRequest request = new CreateVaultRequest() {
				AccountId = "-",
				VaultName = "*** Provide vault name ***"
			};

			CreateVaultResponse response = client.CreateVault(request);

			var client = new AmazonGlacierClient();
			var request = new ListVaultsRequest();
			var response = client.ListVaults(request);

			foreach (var vault in response.VaultList) {
				Console.WriteLine("Vault: {0}", vault.VaultName);
				Console.WriteLine("  Creation date: {0}", vault.CreationDate);
				Console.WriteLine("  Size in bytes: {0}", vault.SizeInBytes);
				Console.WriteLine("  Number of archives: {0}", vault.NumberOfArchives);

				try {
					var requestNotifications = new GetVaultNotificationsRequest {
						VaultName = vault.VaultName
					};
					var responseNotifications =
					  client.GetVaultNotifications(requestNotifications);

					Console.WriteLine("  Notifications:");
					Console.WriteLine("    Topic: {0}",
					  responseNotifications.VaultNotificationConfig.SNSTopic);

					var events = responseNotifications.VaultNotificationConfig.Events;

					if (events.Any()) {
						Console.WriteLine("    Events:");

						foreach (var e in events) {
							Console.WriteLine("{0}", e);
						}
					} else {
						Console.WriteLine("    No events set.");
					}

				} catch (ResourceNotFoundException) {
					Console.WriteLine("  No notifications set.");
				}

				var t = ApiService.SoapAuthenticateUser("sysadmin", "info2929");
				t.Wait();
				var t2 = ApiService.SoapCompleteTask(t.Result.Ticket, 1013, "wowzers");
				t2.Wait();
				Console.WriteLine(t2.Result);

			}*/
		}



		private static async void Init() {
			var temp = await Init2().WithTimeout(70000);
			Debug.WriteLine("1 done: ");
		}

		private static async Task<bool> Init2() {
			await Task.Delay(10_000);
			Debug.WriteLine("2 done");
			return false;
		}


		public static void ChangeValue(Animal animal) {
			animal.count = 1;
			animal.onoff = false;
			animal.test = "nothing";
		}
	}
}
