using System;
using System.IO.Ports;

namespace CreateiBeacon
{
	public class Communication
	{
		public Communication ()
		{
		}

		public string [] AvailablePorts ()
		{
			return SerialPort.GetPortNames ();
		}

		public void Test ()
		{

		}

	}
}