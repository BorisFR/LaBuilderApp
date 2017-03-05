using System;
namespace LaBuilderApp
{
	public class ReturnAuthenticate
	{
		private string state; public string State { get { return state; } set { state = value; } }
		private string error; public string Error { get { return error; } set { error = value; } }
	}
}