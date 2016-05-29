using System;
using System.Json;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace ufbAcessivel
{
	public class RestService 
	{
		public RestService ()
		{
		}

		public async Task<JsonValue> FetchWeatherAsync (string url)
		{
			try{
				var uri = new Uri (string.Format ("http://ufbacessivel.azurewebsites.net/api/values/10482", string.Empty));
				// Create an HTTP web request using the URL:
				//var response = await client.GetAsync (uri);
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (uri);
				request.ContentType = "application/json";
				request.Method = "GET";


				// Send the request to the server and wait for the response:
				using (WebResponse response = await request.GetResponseAsync ())
				{
					// Get a stream representation of the HTTP web response:
					using (Stream stream = response.GetResponseStream ())
					{
						// Use this stream to build a JSON document object:
						JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
						Console.Out.WriteLine("Response: {0}", jsonDoc.ToString ());

						// Return the JSON document:
						return jsonDoc;
					}
				}
			}catch(Exception e){
				throw(e);
			}

		}

	}
}

