// Program.cs
// MAGIQ Documents
// Created by Jack Della on 22/03/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Threading.Tasks;
using MagiqApp.Services;

namespace Scripting {

	public class MainClass {

		public static void Main() {

			var t = ApiService.SoapAuthenticateUser("sysadmin", "info2929");
			t.Wait();
			var t2 = ApiService.SoapCompleteTask(t.Result.Ticket, 1013, "wowzers");
			t2.Wait();
			Console.WriteLine(t2.Result);
		}

		public static void ChangeValue(Animal animal) {
			animal.count = 1;
			animal.onoff = false;
			animal.test = "nothing";
		}
	}
}
