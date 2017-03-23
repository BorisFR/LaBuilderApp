using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace CreateiBeacon
{

	public delegate void DataInfo (string lastCommand, string data);

	public class Communication
	{
		public event DataInfo ReceivedData;

		string port = null;
		string bauds = null;
		string parity = null;
		string data = null;
		SerialPort sp = null;

		bool mustSendDataToWakeUp = true;

		public Communication ()
		{
		}

		public string [] AvailablePorts ()
		{
			return SerialPort.GetPortNames ();
		}

		public void WakeUp ()
		{
			if (!mustSendDataToWakeUp) return;
			if (!IsConnect) return;
			sp.Write ("AZERTYUIOP 1234567890 AZERTYUIOP 1234567890 AZERTYUIOP 1234567890 AZERTYUIOP 1234567890 AZERTYUIOP ");
			Thread.Sleep (50);
			while (sp.BytesToRead > 0) {
				received += Convert.ToChar (sp.ReadChar ());
			}
			if (received.Length > 0) {
				mustSendDataToWakeUp = false;
			}
		}

		public bool IsConnect {
			get {
				if (sp == null) return false;
				return sp.IsOpen;
			}
		}

		public void Connect ()
		{
			if (sp != null) return;
			try {
				sp = new SerialPort (port, Convert.ToInt32 (bauds), Parity.None, Convert.ToInt16 (data));
				sp.Encoding = Encoding.ASCII;
				sp.Handshake = Handshake.None;
				//sp.NewLine = string.Empty;
				sp.StopBits = StopBits.One;
				//sp.DataReceived += Sp_DataReceived;
				sp.ErrorReceived += Sp_ErrorReceived;
				sp.Open ();
			} catch (Exception err) {
			}
		}

		public void Disconnect ()
		{
			if (sp == null) return;
			//sp.DataReceived -= Sp_DataReceived;
			sp.ErrorReceived -= Sp_ErrorReceived;
			if (!sp.IsOpen) { sp = null; return; }
			sp.Close ();
			sp = null;
		}

		void Sp_ErrorReceived (object sender, SerialErrorReceivedEventArgs e)
		{
			ReceivedData ("Sp_ErrorReceived", e.ToString ());
		}

		public void SetInfo (string port, string bauds, string parity, string data)
		{
			this.port = port;
			this.bauds = bauds;
			this.parity = parity;
			this.data = data;
		}

		string lastCommand = string.Empty;
		public void SendCommand (string command)
		{
			if (!IsConnect) return;
			received = string.Empty;
			lastCommand = command;
			sp.Write (command);
			Thread.Sleep (150);
			while (sp.BytesToRead > 0) {
				received += Convert.ToChar (sp.ReadChar ());
			}
			if (received.Length > 0) {
				mustSendDataToWakeUp = false;
				Thread.Sleep (50);
				ReceivedData (lastCommand, received);
			} else mustSendDataToWakeUp = true;
		}

		public void ReadData ()
		{
			if (!IsConnect) return;
			//sp.read
		}

		string received = string.Empty;

		/*
		void Sp_DataReceived (object sender, SerialDataReceivedEventArgs e)
		{
			while (sp.BytesToRead > 0) {
				received += sp.ReadChar ();
			}
			if (ReceivedData != null)
				ReceivedData (lastCommand, received);
		}
		*/
	}
}