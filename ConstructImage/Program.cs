using System;

namespace ConstructImage
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			Console.WriteLine ("Hello World!");

			GenerateBackground x = new GenerateBackground ();
			x.DoIt (512, 512);
		}
	}
}