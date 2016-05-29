using Android.Speech.Tts;
using Xamarin;
using System.Collections.Generic;


namespace ufbAcessivel
{
	public class TextToSpeechImplementation : Java.Lang.Object, TextToSpeech.IOnInitListener
	{
		
		public TextToSpeechImplementation ()
		{	
		}

		public void OnInit (OperationResult status)
		{
			if (status.Equals (OperationResult.Success)) {
				var p = new Dictionary<string,string> ();
				//speaker.Speak (toSpeak, QueueMode.Flush, p);
			}
		}

	}
}

