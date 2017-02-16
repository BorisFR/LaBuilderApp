using System;
using System.Threading.Tasks;

namespace LaBuilderApp
{

	// see in each projet to se the real implementation

	public interface IFiles
	{
		Task<bool> IsExit (string name);
		Task<string> ReadFile (string name);
		Task<byte []> ReadFileBytes (string name);
		Task SaveFile (string name, string data);
		Task SaveFile (string name, byte [] data);
		Task DeleteFile (string name);
	}
}