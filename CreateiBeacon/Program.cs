using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace CreateiBeacon
{

	public enum PositionCursor
	{
		Login,
		Password,
		ButtonLogin,
		ButtonRead,
		ButtonWrite,
		Port,
		Bauds,
		Parity,
		Data
	}

	class MainClass
	{
		static Settings settings = new Settings ();
		static Communication comm = new Communication ();
		static bool mustSendDataToWakeUp = false;

		static List<string> availablePorts = null;
		static Dictionary<int, List<string>> options = null;
		static Dictionary<int, int> optionsSelected = null;

		static Dictionary<int, int> columns = null;

		static bool showCursor = false;
		static PositionCursor positionCursor = PositionCursor.Login;

		static string login = string.Empty;
		static string password = string.Empty;

		public static void SetColorsStandard ()
		{
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
		}

		public static void SetColorsStandardSelected ()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void SetColorsInput ()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void SetColorsChoice ()
		{
			Console.BackgroundColor = ConsoleColor.Blue;
			Console.ForegroundColor = ConsoleColor.Yellow;
		}

		public static void SetColorsChoiceSelected ()
		{
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.ForegroundColor = ConsoleColor.Blue;
		}

		public static void SetColorsStatus ()
		{
			Console.BackgroundColor = ConsoleColor.Gray;
			Console.ForegroundColor = ConsoleColor.White;
		}

		static int posUserY = 4;
		static int posBtConnectX = 42; static int posBtConnectY = posUserY + 2;
		static int posHardwareY = 11;
		static int posBtReadX = 2; static int posBtReadY = posHardwareY + 2;
		static int posBtWriteX = 11; static int posBtWriteY = posHardwareY + 2;

		public static void Main (string [] args)
		{

			Console.Title = "Create an iBeacon";
			int width = Console.BufferWidth;
			int height = Console.BufferHeight;
			Console.SetWindowSize (width, height);
			SetColorsStandard ();
			Console.Clear ();

			availablePorts = new List<string> ();
			Communication c = new Communication ();
			foreach (string s in c.AvailablePorts ()) {
				if (s.Contains (".")) {
					//Console.WriteLine (s);
					availablePorts.Add (s);
				}
			}
			PopulateOptions ();
			columns = new Dictionary<int, int> ();
			columns.Add (0, 1);
			int br = MaxLength (1);
			int pa = MaxLength (2);
			//int br = MaxLength (3);
			columns.Add (1, width - (4 + 2 + 2) - 2 - (pa + 2 + 2) - 0 - (br + 2 + 2));
			columns.Add (2, width - (4 + 2 + 2) - 2 - (pa + 2 + 2));
			columns.Add (3, width - (4 + 2 + 2)); // .DATA.
			DrawScreen ();

			comm.ReceivedData += Comm_ReceivedData;

			System.Threading.Timer timer = new System.Threading.Timer (DoTimer, null, TimeSpan.FromMilliseconds (400), TimeSpan.FromMilliseconds (400));
			DoPositionCursor ();
			ConsoleKeyInfo key = Console.ReadKey (true);
			while (key.Key != ConsoleKey.Escape) {
				if (key.Key == ConsoleKey.Enter && positionCursor == PositionCursor.ButtonLogin) {
					key = new ConsoleKeyInfo ();
				}
				if (key.Key == ConsoleKey.Enter && positionCursor == PositionCursor.ButtonRead) {
					key = new ConsoleKeyInfo ();
					if (!comm.IsConnect) {
						comm.SetInfo (options [0] [optionsSelected [0]],
							options [1] [optionsSelected [1]],
							"",
							options [3] [optionsSelected [3]]);
						comm.Connect ();
						mustSendDataToWakeUp = true;
					}
					SendCommand ("AT+VERR?");
				}
				if (key.Key == ConsoleKey.Enter && positionCursor == PositionCursor.ButtonWrite) {
					key = new ConsoleKeyInfo ();
				}
				switch (key.Key) {
				case ConsoleKey.Enter:
				case ConsoleKey.Tab:
					if (positionCursor == PositionCursor.Login)
						positionCursor = PositionCursor.Password;
					else if (positionCursor == PositionCursor.Password)
						positionCursor = PositionCursor.ButtonLogin;
					else if (positionCursor == PositionCursor.ButtonLogin)
						positionCursor = PositionCursor.ButtonRead;
					else if (positionCursor == PositionCursor.ButtonRead)
						positionCursor = PositionCursor.ButtonWrite;
					else if (positionCursor == PositionCursor.ButtonWrite)
						positionCursor = PositionCursor.Port;
					else if (positionCursor == PositionCursor.Port)
						positionCursor = PositionCursor.Bauds;
					else if (positionCursor == PositionCursor.Bauds)
						positionCursor = PositionCursor.Parity;
					else if (positionCursor == PositionCursor.Parity)
						positionCursor = PositionCursor.Data;
					else if (positionCursor == PositionCursor.Data)
						positionCursor = PositionCursor.Login;
					DoPositionCursor ();
					break;
				case ConsoleKey.DownArrow:
					switch (positionCursor) {
					case PositionCursor.Port:
						MoveListDown (0);
						break;
					case PositionCursor.Bauds:
						MoveListDown (1);
						break;
					case PositionCursor.Parity:
						MoveListDown (2);
						break;
					case PositionCursor.Data:
						MoveListDown (3);
						break;
					}
					break;
				case ConsoleKey.UpArrow:
					switch (positionCursor) {
					case PositionCursor.Port:
						MoveListUp (0);
						break;
					case PositionCursor.Bauds:
						MoveListUp (1);
						break;
					case PositionCursor.Parity:
						MoveListUp (2);
						break;
					case PositionCursor.Data:
						MoveListUp (3);
						break;
					}
					break;
				case ConsoleKey.Backspace:
					switch (positionCursor) {
					case PositionCursor.Login:
						if (login.Length > 0) {
							if (login.Length == 1)
								login = string.Empty;
							else
								login = login.Substring (0, login.Length - 1);
						}
						ShowInput (login, 10, posUserY + 1, 30);
						break;
					case PositionCursor.Password:
						if (password.Length > 0) {
							if (password.Length == 1)
								password = string.Empty;
							else
								password = password.Substring (0, password.Length - 1);
						}
						ShowPassword ();
						break;
					}
					break;
				default:
					switch (positionCursor) {
					case PositionCursor.Login:
						login += key.KeyChar;
						ShowInput (login, 10, posUserY + 1, 30);
						break;
					case PositionCursor.Password:
						password += key.KeyChar;
						ShowPassword ();
						break;
					}
					break;
				}
				key = Console.ReadKey (true);
			}

			comm.Disconnect ();
			comm.ReceivedData -= Comm_ReceivedData;
			comm = null;
			Console.SetCursorPosition (0, height - 2);
		}

		static void SendCommand (string command)
		{
			comm.Connect ();
			if (comm.IsConnect) {
				ShowStatus (command);
				comm.SendCommand (command);
			} else {
				ShowStatus ("Error connecting to comm port");
			}
		}

		static void Comm_ReceivedData (string lastCommand, string data)
		{
			ShowStatus ($"Command: {lastCommand} - {data}");
			switch (lastCommand) {
			case "AT+VERR?":
				version = "iBeacon: " + data;
				ShowiBeacon ();
				SendCommand ("AT+IBE0?");
				break;
			case "AT+IBE0?":
				uuid = data.Replace ("OK+Get:0x", "") + "-";
				ShowiBeacon ();
				SendCommand ("AT+IBE1?");
				break;
			case "AT+IBE1?":
				string temp = data.Replace ("OK+Get:0x", "");
				uuid += temp.Substring (0, 4) + "-" + temp.Substring (4) + "-";
				ShowiBeacon ();
				SendCommand ("AT+IBE2?");
				break;
			case "AT+IBE2?":
				string temp2 = data.Replace ("OK+Get:0x", "");
				uuid += temp2.Substring (0, 4) + "-" + temp2.Substring (4);
				ShowiBeacon ();
				SendCommand ("AT+IBE3?");
				break;
			case "AT+IBE3?":
				uuid += data.Replace ("OK+Get:0x", "");
				ShowiBeacon ();
				SendCommand ("AT+MARJ?");
				break;
			case "AT+MARJ?":
				major = data.Replace ("OK+Get:", "");
				ShowiBeacon ();
				SendCommand ("AT+MINO?");
				break;
			case "AT+MINO?":
				minor = data.Replace ("OK+Get:", "");
				ShowiBeacon ();
				SendCommand ("AT+NAME?");
				break;
			case "AT+NAME?":
				region = data.Replace ("OK+NAME:", "");
				ShowiBeacon ();
				break;
			}
		}

		static void ShowPassword ()
		{
			string hide = string.Empty;
			for (int i = 0; i < password.Length; i++)
				hide += "*";
			ShowInput (hide, 10, posUserY + 3, 30);
		}

		static void MoveListDown (int index)
		{
			optionsSelected [index]++;
			if (optionsSelected [index] >= options [index].Count)
				optionsSelected [index] = 0;
			ShowList (index, columns [index], 1);
			DoSaveSelected (index);
		}

		static void MoveListUp (int index)
		{
			optionsSelected [index]--;
			if (optionsSelected [index] < 0)
				optionsSelected [index] = options [index].Count - 1;
			ShowList (index, columns [index], 1);
			DoSaveSelected (index);
		}

		static void DoSaveSelected (int index)
		{
			string value = options [index] [optionsSelected [index]];
			switch (index) {
			case 0:
				settings.SetValue ("Port", value);
				break;
			case 1:
				settings.SetValue ("Bauds", value);
				break;
			case 2:
				settings.SetValue ("Parity", value);
				break;
			case 3:
				settings.SetValue ("Data", value);
				break;
			}
		}

		static void DoPositionCursor ()
		{
			switch (positionCursor) {
			case PositionCursor.Login:
				SetColorsChoice ();
				ShowList (3, columns [3], 1);
				showCursor = true;
				Console.CursorVisible = true;
				Console.SetCursorPosition (10, posUserY + 1);
				break;
			case PositionCursor.Password:
				Console.SetCursorPosition (10, posUserY + 3);
				break;
			case PositionCursor.ButtonLogin:
				showCursor = false;
				Console.CursorVisible = false;
				ShowButtonSelected ("Connect", posBtConnectX, posBtConnectY);
				break;
			case PositionCursor.ButtonRead:
				ShowButton ("Connect", posBtConnectX, posBtConnectY);
				ShowButtonSelected ("Read", posBtReadX, posBtReadY);
				break;
			case PositionCursor.ButtonWrite:
				ShowButton ("Read", posBtReadX, posBtReadY);
				ShowButtonSelected ("Write", posBtWriteX, posBtWriteY);
				break;
			case PositionCursor.Port:
				ShowButton ("Write", posBtWriteX, posBtWriteY);
				SetColorsChoiceSelected ();
				ShowList (0, columns [0], 1);
				break;
			case PositionCursor.Bauds:
				SetColorsChoice ();
				ShowList (0, columns [0], 1);
				SetColorsChoiceSelected ();
				ShowList (1, columns [1], 1);
				break;
			case PositionCursor.Parity:
				SetColorsChoice ();
				ShowList (1, columns [1], 1);
				SetColorsChoiceSelected ();
				ShowList (2, columns [2], 1);
				break;
			case PositionCursor.Data:
				SetColorsChoice ();
				ShowList (2, columns [2], 1);
				SetColorsChoiceSelected ();
				ShowList (3, columns [3], 1);
				break;
			default:
				showCursor = true;
				break;
			}
		}

		static void DoTimer (object state)
		{
			if (showCursor)
				Console.CursorVisible = !Console.CursorVisible;
			comm.WakeUp ();
		}


		private static int MaxLength (int index)
		{
			int maxLength = 0;
			foreach (string s in options [index]) {
				if (s.Length > maxLength)
					maxLength = s.Length;
			}
			return maxLength;
		}

		private static void PopulateOptions ()
		{
			options = new Dictionary<int, List<string>> ();
			optionsSelected = new Dictionary<int, int> ();
			options.Add (0, availablePorts);
			List<string> baudrate = new List<string> ();
			baudrate.Add ("9600");
			baudrate.Add ("19200");
			baudrate.Add ("38400");
			baudrate.Add ("57600");
			baudrate.Add ("74880");
			baudrate.Add ("115200");
			baudrate.Add ("230400");
			baudrate.Add ("250000");
			options.Add (1, baudrate);
			List<string> parity = new List<string> ();
			parity.Add (Parity.None.ToString ());
			parity.Add (Parity.Space.ToString ());
			parity.Add (Parity.Mark.ToString ());
			parity.Add (Parity.Even.ToString ());
			parity.Add (Parity.Odd.ToString ());
			options.Add (2, parity);
			List<string> data = new List<string> ();
			data.Add ("8");
			options.Add (3, data);
			SetDefautSelected (0, settings.ValueOf ("Port"));
			SetDefautSelected (1, settings.ValueOf ("Bauds", "9600"));
			SetDefautSelected (2, settings.ValueOf ("Parity", "none"));
			SetDefautSelected (3, settings.ValueOf ("Data", "8"));
			//optionsSelected.Add (0, 0);
			//optionsSelected.Add (1, 5);
			//optionsSelected.Add (2, 0);
			//optionsSelected.Add (3, 0);

		}

		private static void SetDefautSelected (int index, string value)
		{
			if (value.Length == 0) {
				optionsSelected.Add (index, 0);
				return;
			}
			if (!options [index].Contains (value)) {
				optionsSelected.Add (index, 0);
				return;
			}
			int i = 0;
			foreach (string s in options [index]) {
				if (s.Equals (value)) {
					optionsSelected.Add (index, i);
					return;
				}
				i++;
			}
		}

		private static void WriteTextOnSize (string text, int length, int x, int y)
		{
			Console.SetCursorPosition (x, y);
			Console.Write (text);
			int l = text.Length;
			while (l < length) {
				Console.Write (" ");
				l++;
			}
		}

		private static void WriteText (string text, int x, int y)
		{
			Console.SetCursorPosition (x, y);
			Console.Write (text);
		}

		private static void DrawLine (int x, int y, int width)
		{
			Console.SetCursorPosition (x, y);
			for (int i = x; i <= (x + width); i++)
				Console.Write ("─");
		}

		private static void DrawRectangleSimple (int x, int y, int width, int height)
		{
			Console.SetCursorPosition (x, y);
			Console.Write ("┌");
			for (int i = (x + 1); i <= (x + width); i++)
				Console.Write ("─");
			Console.Write ("┐");
			for (int j = (y + 1); j <= (y + height); j++) {
				Console.SetCursorPosition (x, j); Console.Write ("│");
				for (int i = (x + 1); i <= (x + width); i++)
					Console.Write (" ");
				//Console.SetCursorPosition (x + width + 1, j); 
				Console.Write ("│");
			}
			Console.SetCursorPosition (x, y + height + 1);
			Console.Write ("└");
			for (int i = (x + 1); i <= (x + width); i++)
				Console.Write ("─");
			Console.Write ("┘");
		}

		private static void DrawRectangleDouble (int x, int y, int width, int height)
		{
			Console.SetCursorPosition (x, y);
			Console.Write ("╔");
			for (int i = (x + 1); i <= (x + width); i++)
				Console.Write ("═");
			Console.Write ("╗");
			for (int j = (y + 1); j <= (y + height); j++) {
				Console.SetCursorPosition (x, j); Console.Write ("║");
				for (int i = (x + 1); i <= (x + width); i++)
					Console.Write (" ");
				//Console.SetCursorPosition (x + width + 1, j); 
				Console.Write ("║");
			}
			Console.SetCursorPosition (x, y + height + 1);
			Console.Write ("╚");
			for (int i = (x + 1); i <= (x + width); i++)
				Console.Write ("═");
			Console.Write ("╝");
		}

		private static int ShowBowSimple (string title, int x, int y, int width, int height)
		{
			SetColorsChoice ();
			int w = width;
			int decal = 0;
			if (title.Length + 2 > width) {
				w = title.Length + 2;
				decal = (w - width) / 2;
			}
			DrawRectangleSimple (x, y, w, height);
			//WriteText ($" {title} ", 1 + x + (int)((width + 0.5 - (title.Length + 2)) / 2), y);
			WriteText ($" {title} ", x + 1, y);
			return decal;
		}

		private static void ShowBowDouble (string title, int x, int y, int width, int height)
		{
			SetColorsStandard ();
			DrawRectangleDouble (x, y, width, height);
			WriteText ($" {title} ", 1 + x + (int)((width + 0.5 - (title.Length + 2)) / 2), y);
		}

		private static void ShowTitle (string title, int y)
		{
			SetColorsStandard ();
			Console.SetCursorPosition (0, y);
			for (int i = 0; i <= Console.BufferWidth; i++)
				Console.Write ("─");
			WriteText ($" {title} ", (int)((Console.BufferWidth + 0.5 - (title.Length + 2)) / 2), y);
		}

		private static void ShowInput (int x, int y, int width)
		{
			SetColorsInput ();
			Console.SetCursorPosition (x, y);
			for (int i = x; i <= (x + width); i++)
				Console.Write (" ");
			DrawLine (x, y + 1, width);
			Console.SetCursorPosition (x, y);
		}

		private static void ShowInput (string text, int x, int y, int width)
		{
			SetColorsInput ();
			Console.SetCursorPosition (x, y);
			Console.Write (text);
			for (int i = (x + text.Length); i <= (x + width); i++)
				Console.Write (" ");
			DrawLine (x, y + 1, width);
			Console.SetCursorPosition (x + text.Length, y);
		}

		private static void ShowButton (string title, int x, int y)
		{
			SetColorsStandard ();
			DrawRectangleSimple (x, y, title.Length + 2, 1);
			WriteText ($" {title} ", x + 1, y + 1);
		}

		private static void ShowButtonSelected (string title, int x, int y)
		{
			SetColorsStandard ();
			DrawRectangleSimple (x, y, title.Length + 2, 1);
			SetColorsStandardSelected ();
			WriteText ($" {title} ", x + 1, y + 1);
		}

		private static void ShowStatus (string text)
		{
			SetColorsStatus ();
			Console.SetCursorPosition (0, Console.BufferHeight - 1);
			Console.Write (" ");
			Console.Write (text);
			for (int i = (1 + text.Length); i < Console.BufferWidth; i++)
				Console.Write (" ");
		}

		private static void DrawScreen ()
		{
			ShowBowSimple ("Port", columns [0] - 1, 0, MaxLength (0), 1);
			ShowList (0, columns [0], 1);
			//WriteText ("Bauds", columns [1], 0);
			int decal = ShowBowSimple ("Bauds", columns [1] - 1, 0, MaxLength (1), 1);
			columns [1] += decal;
			ShowList (1, columns [1], 1);
			//WriteText ("Parity", columns [2], 0);
			decal = ShowBowSimple ("Parity", columns [2] - 1, 0, MaxLength (2), 1);
			columns [2] += decal;
			ShowList (2, columns [2], 1);
			//WriteText ("Data", columns [3], 0);
			decal = ShowBowSimple ("Data", columns [3] - 1, 0, MaxLength (3), 1);
			columns [3] += decal;
			ShowList (3, columns [3], 1);

			ShowTitle ("User", posUserY);
			SetColorsStandard ();
			Console.SetCursorPosition (0, posUserY + 1); Console.Write ("   Login:"); ShowInput (10, posUserY + 1, 30);
			SetColorsStandard ();
			Console.SetCursorPosition (0, posUserY + 3); Console.Write ("Password:"); ShowInput (10, posUserY + 3, 30);

			ShowButton ("Connect", posBtConnectX, posBtConnectY);

			ShowTitle ("Hardware", posHardwareY);
			ShowBowDouble ("Commands", posBtReadX - 2, posBtReadY - 1, 20, 3);
			ShowButton ("Read", posBtReadX, posBtReadY);
			ShowButton ("Write", posBtWriteX, posBtWriteY);

			ShowStatus ("Ready");
			ShowiBeacon ();
		}

		static string uuid = "????????-????-????-????-????????????";
		static string region = "????????";
		static string minor = "0x????";
		static string major = "0x????";
		static string version = "iBeacon";

		private static void ShowiBeacon ()
		{
			SetColorsStandard ();
			ShowBowDouble (version, Console.BufferWidth - 45, posBtReadY - 1, 43, 3);
			WriteText ("UUID:  " + uuid, Console.BufferWidth - 44, posBtReadY);
			WriteText ("Major: " + major, Console.BufferWidth - 44, posBtReadY + 1);
			WriteText ("/ Minor: " + minor, Console.BufferWidth - 44 + 14, posBtReadY + 1);
			WriteText ("Region name: " + region, Console.BufferWidth - 44, posBtReadY + 2);
		}

		private static void ShowList (int index, int x, int y)
		{
			int maxLength = MaxLength (index);
			WriteTextOnSize (options [index] [optionsSelected [index]], maxLength, x, y);
		}
	}
}

// Name: HMSoft; Baud: 9600, N, 8, 1; Pin code: 000000; Peripheral Role;
// si pas de réponse, c'est que en mode sleep
// alors envoyer 200 caractères
// on reçoit : OK+WAKE
// AT		répond OK ou OK+LOST (si était connecté en bluetooth à quelque chose)
// AT+ADDR? OK+ADDR:544A162F39A4
// AT+ADVI?	OK+Get:5 Advertisinginterval, 5=545,25ms
// AT+ADTY?	OK+Get:3 AdvertisingType, 3=Only allow Advertising
// AT+BATT?	OK+Get:069 battery information, de 0 à 100
// AT+IBEA?	OK+Get:1 Module iBeacon switch: 0=Off, 1=On
// AT+IBE0?	OK+Get:0x74278BDA iBeacon UUID
// AT+IBE1?	OK+Get:0xB6444520
// AT+IBE2?	OK+Get:0x8F0C720E
// AT+IBE3?	OK+Get:0xAF059935
// AT+MARJ?	OK+Get:0x1234 (4660)
// AT+MINO?	OK+Get:0xFA01 (64001)
// AT+NAME?	OK+NAME:R2Builders (max length=12)
// AT+PWRM?	OK+Get:0 0=auto sleep, 1=don't auto sleep
// AT+RENEW	reset setting to factory
// AT+RESET	restart the module
// AT+SLEEP	Module into sleep mode
// AT+TEMP?	OK+Get:023.326 000.000~255.000
// AT+VERR?	HMSoft V539
// AT+VERS?	HMSoft V539

/* ajouter AT+IBE0xxxxx *4
1. AT+RENEW Restores factory defaults
2. AT+RESET Reboot HM-10
3. AT Wait for OK
4. AT+MARJ0x1234 Set iBeacon Major number to 0x1234 (hexadecimal)
5. AT+MINO0xFA01 Set iBeacon Minor number to 0xFA01 (hexadecimal)
6. AT+ADVI5 Set advertising interval to 5 (546.25 milliseconds)
7. AT+NAMEDOPEY Set HM-10 module name to DOPEY.Make this unique.
8. AT+ADTY3 Make non-connectable (save power)
9. AT+IBEA1 Enable iBeacon mode
10.AT+DELO2 iBeacon broadcast-only (save power)
11.AT+PWRM0 Enable auto-sleep.This reduces power from 8 to 0.18 mA
12.AT+RESET Reboot
*/