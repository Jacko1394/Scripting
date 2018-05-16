// Program.cs
// MAGIQ Documents
// Created by Jack Della on 22/03/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Scripting {

	public class MainClass {

		public static void Main() {
			Console.Write("Hello world.\n");

			//var x = new Animal(69, "lit", false);
			//var y = new Animal(420, "fam", true);
			//var z = new Animal(1394, "blazeit", true);
			//var list = new List<Animal>();
			//list.Add(x);
			//list.Add(y);
			//list.Add(z);
			//var json = JsonConvert.SerializeObject(list, Formatting.Indented);
			//Console.WriteLine(json);
			//var animal = new Animal(420, "somethings", true);
			//Console.WriteLine("Before: " + animal);
			//ChangeValue(animal);
			//Console.WriteLine("After: " + animal);

			//STRING READ AND WRITE:::::
			//Console.WriteLine("InfoRouterSoap API:");
			//var lines = File.ReadLines("/Users/jd/Projects/Scripting/Scripting/text.swift");
			//string textFile = "";
			//int count = 0;
			//foreach(var l in lines) {
			//	if(l.Contains("http://tempuri.org")) {
			//		++count;
			//		textFile += l + "\n";
			//	}
			//}
			//Console.WriteLine("Count: " + count);
			//textFile += "Count: " + count + "\n";
			//System.IO.File.WriteAllText("/Users/jd/Projects/Scripting/Scripting/text2.swift", textFile);

			var t = ApiService.MakeCallAsync();
			t.Wait();


		}

		public static void ChangeValue(Animal animal) {
			animal.count = 1;
			animal.onoff = false;
			animal.test = "nothing";
		}
	}

	public class Animal {

		public int count;
		public string test;
		public bool onoff;

		public Animal(int i, string s, bool b) {
			count = i;
			test = s;
			onoff = b;
		}

		public override string ToString() {
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
