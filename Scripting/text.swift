//OLD:
//
		private const string ticket = "sid-ttpd2azsmymxmfztbvyakk0m";
		private const string documentPath = "\\demo Business Classification Scheme\\Commercial Activities\\Audit\\New Empty Folder\\test.html";
		private const string _url = "https://cloud.infoxpert.com.au/srv.asmx";
		private const string BeginningSoapXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"><soap12:Body>";
		private const string EndingSoapXml = "</soap12:Body></soap12:Envelope>";

		//
		public async static Task<bool> CheckTicketValid(string ticket) {

			//SOAP Body Request:
			var doc = XDocument.Parse(
				BeginningSoapXml +
				"<isValidTicket xmlns=\"http://tempuri.org/\"><AuthenticationTicket>" +
				ticket + "</AuthenticationTicket></isValidTicket>" +
				EndingSoapXml
			);

			var req = PrepareRequest(_url);
			var reqStream = req.EndGetRequestStream(req.BeginGetRequestStream(null, req));

			doc.Save(reqStream);

			var responseStream = await req.GetResponseAsync();
			var result = new StreamReader(responseStream.GetResponseStream()).ReadToEnd();

			//Parse string to XML, get child elements, get last one, get attribute value for success:
			var response = XDocument.Parse(result).Descendants().LastOrDefault().Attribute("success").Value;

			Console.WriteLine(response);

			return false;
		}

		//Prepares the web request:
		private static HttpWebRequest PrepareRequest(string url) {

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.ContentType = "application/soap+xml; charset=utf-8";
			req.Accept = "text/xml";
			req.Method = "POST";

			return req;
		}

		//
		public async static Task<string> DownloadDocument(string ticket, string path) {

			//SOAP Body Request:
			var doc = XDocument.Parse(
				BeginningSoapXml +
				"<DownloadDocument xmlns = \"http://tempuri.org/\">" +
				"<AuthenticationTicket>" + ticket + "</AuthenticationTicket>" +
				"<Path>" + path + "</Path>" +
				"</DownloadDocument>" +
				EndingSoapXml
			);

			var req = PrepareRequest(_url);
			var reqStream = req.EndGetRequestStream(req.BeginGetRequestStream(null, req));

			doc.Save(reqStream);

			var responseStream = await req.GetResponseAsync();
			var result = new StreamReader(responseStream.GetResponseStream()).ReadToEnd();

			doc = XDocument.Parse(result);

			Console.Write(doc.Root.Value);

			return doc.Root.Value;
		}

	}
	var t = CheckTicketValid(ticket);
			//var t = DownloadDocument(ticket, documentPath);
			t.Wait();

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
