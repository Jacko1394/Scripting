// Animal.cs
// MAGIQ Documents
// Created by Jack Della on 24/05/2018
// Copyright © 2018 MAGIQ Software Ltd. All rights reserved.
//
using System;
using Newtonsoft.Json;

namespace Scripting {
	
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
