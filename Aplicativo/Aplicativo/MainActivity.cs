using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//android.speech.RecognizerIntent
using System.IO;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;
using Android.Speech.Tts;
using ufbAcessivel;
using System.Collections.Generic;
using System.Json;
using ServiceStack;
using Android.Speech;


namespace ufbAcessivel
{
	[Activity (Label = "UFBAcessivel", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, TextToSpeech.IOnInitListener 
	{		
		ToggleButton tgConnect;
		TextView Result;
		VideoView videoView;

		private Java.Lang.String dataToSend;
		private BluetoothAdapter mBluetoothAdapter = null;
		private BluetoothSocket btSocket = null;
		private Stream outStream = null;
		private Stream inStream = null;
		//MAC Address Bluetooth
		private static string address = "00:14:01:21:12:48";
		//Id do bluetooth
		private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
		private const int VOICE_RECOGNITION_REQUEST_CODE = 1234;
		private String Fase = "Inicio";
		private int Estabelecimento = 0;
		private List<string> Voz = null;
		private Etiqueta Destino = new Etiqueta ();
		private List<Etiqueta> Lugares;
		private List<Etiqueta> Caminho;
		private bool trava = false;
		private bool conectado = false;
		private int val = 0;

		Bundle rBundle;

		TextToSpeech mTts;

		public void OnInit (OperationResult status)
		{
			if (status == OperationResult.Error)
				mTts.SetLanguage(Java.Util.Locale.Default);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			//Texto -> Voz
			if(requestCode == 100)
			{
				mTts = new TextToSpeech(this, this);
			}
			//Voz -> Texto
			if (requestCode == 150) {				
				if (Voz == null) {
					IList<string> voz = data.GetStringArrayListExtra (RecognizerIntent.ExtraResults);
					Voz = new List<string> (voz);
				}
			}
			base.OnActivityResult (requestCode, resultCode, data);
		}

		protected override void OnRestart(){	
			btSocket.Close ();
			Finish ();
			StartActivity (this.Intent );
			OnCreate (rBundle);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			rBundle = bundle;

			tgConnect = FindViewById<ToggleButton>(Resource.Id.toggleButton1);
			//Result = FindViewById<TextView>(Resource.Id.textView1);
			//videoView = FindViewById<VideoView> (Resource.Id.SampleVideoView);

			if (mTts == null){
				mTts = new TextToSpeech (this, this);
				Locale l = new Java.Util.Locale("pt-BR");
				mTts.SetLanguage(l);
			}

			tgConnect.CheckedChange += tgConnect_HandleCheckedChange;
			// status do bluetooth

			CheckBt();
		}

		private void CheckBt() {
			
			mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

			if (!mBluetoothAdapter.Enable()) {
				Toast.MakeText(this, "Bluetooth Desactivado",
					ToastLength.Short).Show();
			}
			//inacessível
			if (mBluetoothAdapter == null) {
				Toast.MakeText(this,
					"Bluetooth inacessível", ToastLength.Short)
					.Show();
			}
		}

		public void getSpeechInput(){			
			Intent intent = new Intent (RecognizerIntent.ActionRecognizeSpeech);
			intent.PutExtra (RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			intent.PutExtra (RecognizerIntent.ExtraLanguage, new Java.Util.Locale("pt-BR") );
			intent.PutExtra (RecognizerIntent.ExtraPrompt, "Capturando Voz");

			StartActivityForResult(intent,150);
		}

		//Click do botão
		void tgConnect_HandleCheckedChange (object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			if (e.IsChecked) {
				//entra se clicar no botão
				Connect();
			} else {
				//se desconectar
				if (btSocket.IsConnected) {
					try {
						btSocket.Close();
					} catch (System.Exception ex) {
						Console.WriteLine (ex.Message);
					}
				}
			}
		}
		//Evento de conexion al Bluetooth
		public void Connect() {
			//Inicia conexão com o arduino
			BluetoothDevice device = mBluetoothAdapter.GetRemoteDevice(address);
			System.Console.WriteLine("Conectando " + device);

			mBluetoothAdapter.CancelDiscovery();
			try {
				
				//socket de comunicação com o arduino
				btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
				//conectando socket
				btSocket.Connect();
				// Aviso sonoro do sistema
				var p = new Dictionary<string,string> ();
				mTts.Speak("Bem vindo ao UFBA Acessivel.", QueueMode.Add,  p);
				//Espera(3000);
				//inicio();
				conectado = true;
				System.Console.WriteLine("Conectado!!");
			} catch (System.Exception e) {				
				Console.WriteLine (e.Message);
				try {
					btSocket.Close();
				} catch (System.Exception) {
					System.Console.WriteLine("Erro de Conexão");
				}
			}

			//Inicio da espera de dados
			beginListenForData();
		}

		public void Espera(int tempo){
			try {
				System.Threading.Thread.Sleep(tempo);
			} catch (Exception ex) {
				System.Console.WriteLine("Erro ao esperar");
			}
		
		}

		public void inicio(){
			var p = new Dictionary<string,string> ();
			while (Fase == "Inicio") {
				if (!trava) {
					
					mTts.Speak ("No proximo Bipe pronuncie a sua localização.", QueueMode.Add, p);	
					Espera (5000);
					trava = true;
					getSpeechInput ();
				}
				if (Voz != null) {
					var client = new JsonServiceClient ("http://ufbacessivel.azurewebsites.net/api");
					Estabelecimento = client.Get<int> ("/values/estabelecimentoVoz/" + Voz [0]);

					if (Estabelecimento == 0) {
						trava = false;
						Voz = null;
						mTts.Speak ("Desculpe não entendi a sua localização.", QueueMode.Add, p);
						Espera (6000);
					} else {
						Fase = "Busca Voz";
						mTts.Speak ("Ok, vamos para o proximo passo.", QueueMode.Add, p);
						Espera (6000);
						Voz = null;
						trava = false;
					}
				}
			}		
		}

		public void destino(){
			var p = new Dictionary<string,string> ();
			while (Fase == "Busca Voz") {
				if (!trava) {
					mTts.Speak ("No proximo Bipe pronuncie o seu destino.", QueueMode.Add, p);	
					Espera (5000);
					trava = true;
					getSpeechInput ();
				}
				if (Voz != null) {
					var client = new JsonServiceClient ("http://ufbacessivel.azurewebsites.net/api");
					Lugares = client.Get<List<Etiqueta>> ("/values/voz?voz=" + Voz [0]);
					if (Lugares == null) {
						mTts.Speak ("Lugar não encontrado, Ao proximo toque tente novamente.", QueueMode.Add, p);
						Espera (5000);
						Voz = null;
						trava = false;
					} else {
						Fase = "Confirma";
						Espera (5000);
						Voz = null;
						trava = false;
					}									
				}
			}
		}

		public void confirmaDestino(){
			var p = new Dictionary<string,string> ();
			while (Fase == "Confirma") {
				if (Lugares != null) {
					if (Lugares.Count > 0) {
						var item = Lugares [0];
						if (item != null && Voz == null && !trava) {
							mTts.Speak ("Seu Destino é " + item.descricao + " ?", QueueMode.Add, p);
							Espera (6000);
							trava = true;
							getSpeechInput ();
						}
						if (Voz != null)
							if (Voz [0].ToUpper ().Trim () == "SIM") {
								Fase = "Navegacao";
								Destino = item;
							} else {
								Voz = null;
								Lugares.RemoveAt (0);
								trava = false;
							}
					}else {
						Fase = "Busca Voz";
						Voz = null;
					}
				} else {
					Fase = "Busca Voz";
					Voz = null;
				}
			}
		}

		public void beginListenForData(){
			
			try {
				inStream = btSocket.InputStream;
			} catch (System.IO.IOException ex) {
				Console.WriteLine (ex.Message);
			}

			Task.Factory.StartNew (() => {
				//OnRestart();
				if(conectado){
					inicio();
					//confirmaEstabelecimento();
					destino();
					confirmaDestino();

				}else{
					Connect();
				}
				//buffer de leitura
				byte[] buffer = new byte[1024];

				int bytes;
				while (Fase != "Chegada" && Fase != "Reinicio")  {
					try {
						//acessibilidade ();
						//ler o buffer e obtem a qtd de bytes
						bytes = inStream.Read(buffer, 0, buffer.Length);

						if((bytes>0 ) && (Fase == "Navegacao" || Fase == "Caminho"))
						{							
							RunOnUiThread(()=>{
								
								val = 0;
								//recebe dados da leitura
								//string valor = System.Text.Encoding.ASCII.GetString (buffer);

								string valor = "";
								int k = 0;
								//var valor = System.Text.Encoding.ASCII.GetString (buffer);
								try{
									while (System.Text.Encoding.ASCII.GetString (buffer)[k] != '\r'){
										if ((System.Text.Encoding.ASCII.GetString (buffer)[k] == '0') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '1') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '2') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '3') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '4') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '5') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '6') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '7') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '8') || (System.Text.Encoding.ASCII.GetString (buffer)[k] == '9'))
											valor = valor + System.Text.Encoding.ASCII.GetString (buffer)[k];
										
										k++;
									}									
								}catch(Exception ex){
									valor = null;
								}

								//Testa a leitura
								if (valor != null && valor != ""){
									try{
										//var separador = (valor.Split('\r'));
										val = Int32.Parse(valor);
										if (val < 0)
											val = val * (-1);
									}catch(Exception e){
										valor = null;
									}									
								}

								if (Fase == "Navegacao" && Caminho == null && valor != null && val != 0) {
									try {
										var p = new Dictionary<string,string> ();
										var client = new JsonServiceClient ("http://ufbacessivel.azurewebsites.net/api");
										Caminho = client.Get<List<Etiqueta>> ("/values/" + Estabelecimento + "/" + val + "/" + Destino.codigo);	
										if(Caminho != null){
											mTts.Speak ("Ok, Iniciando Navegação de " + Caminho[0].descricao +" ao " + Destino.descricao + "." , QueueMode.Add, p);	
											Fase = "Caminho";
										}else{
											mTts.Speak ("Desculpe, Falha na identificação do caminho... Reiniciando o Processo." , QueueMode.Add, p);	
											Voz = null;
											Fase = "Reinicio"; // tem que reiniciar o programa 
											OnRestart();
										}
									} catch (Exception ex) {
										var p = new Dictionary<string,string> ();
										mTts.Speak ("Desculpe, Falha na identificação do caminho... Reiniciando o Processo." , QueueMode.Add, p);	
										Voz = null;
										Fase = "Reinicio"; // tem que reiniciar o programa 
										OnRestart();
									}

									Espera(7000);

								}
								if (Fase == "Caminho" && Caminho != null && valor != null && val != 0) {
									var p = new Dictionary<string,string> ();
									//Caminho.
									var item = Caminho.Find(x => x.codigo == val.ToString());
									if (item != null){
										mTts.Speak ("Estamos em "+ item.descricao , QueueMode.Add, p);	
										Espera(3000);
										for (int i = 0; i < Caminho.Count; i++) {
											if (Caminho[i].codigo == item.codigo){
												if((i + 1) < Caminho.Count){
													mTts.Speak ("Proximo tag é "+ Caminho[i+1].descricao , QueueMode.Add, p);					
													Espera(3000);
													i = Caminho.Count;
												}else{
													mTts.Speak ("Chegamos ao Destino!!!", QueueMode.Add, p);
													Fase = "Chegada";

												}

											}
										}
									}else{
										mTts.Speak ("Esse caminho não faz parte da sua rota, volte para a sua rota.", QueueMode.Add, p);
										Espera(7000);
									}	
								}
								if (Fase == "Chegada")
									OnRestart(); // tentativa de parar o programa.
							});
						}
					} catch (Java.IO.IOException) {
						
						RunOnUiThread(()=>{
							Result.Text = string.Empty;                    
						});
						break;
					}
				}
			});
			if (Fase == "Reinicio"){
				Fase = "Inicio";
				beginListenForData ();
			}
		}

		private void writeData(Java.Lang.String data) {

			try {
				outStream = btSocket.OutputStream;
			} catch (System.Exception e) {
				System.Console.WriteLine("Error ao enviar"+e.Message);
			}

			Java.Lang.String message = data;

			byte[] msgBuffer = message.GetBytes();

			try {				
				outStream.Write(msgBuffer, 0, msgBuffer.Length);
			} catch (System.Exception e) {
				System.Console.WriteLine("Error ao enviar"+e.Message);
			}
		}


	}
}
