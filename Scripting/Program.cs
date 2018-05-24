// Program.cs
// MAGIQ Documents
// Created by Jack Della on 22/03/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;

namespace Scripting {

	public class MainClass {

		public static void Main() {
			Console.Write("Hello world.\n");
		}

		public static void ChangeValue(Animal animal) {
			animal.count = 1;
			animal.onoff = false;
			animal.test = "nothing";
		}
	}
}
