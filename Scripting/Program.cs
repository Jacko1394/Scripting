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

			var t = ApiService.SoapAuthenticateUser("sysadmin", "sysadmin");
			t.Wait();


			Console.Write("Hello world.\n");

		}

		public static void ChangeValue(Animal animal) {
			animal.count = 1;
			animal.onoff = false;
			animal.test = "nothing";
		}
	}
}
