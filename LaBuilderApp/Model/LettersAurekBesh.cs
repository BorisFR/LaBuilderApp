using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LaBuilderApp
{
	public class LettersAurekBesh : CModel<LettersAurekBesh>
	{

		private string letter; public string Letter { get { return letter; } set { letter = value; RaisePropertyChanged (); } }
		private string name; public string Name { get { return name; } set { name = value; RaisePropertyChanged (); } }
		private string detail; public string Detail { get { return detail; } set { detail = value; RaisePropertyChanged (); } }

		public static void PopulateData ()
		{
			All.Clear ();
			All.Add (new LettersAurekBesh { Letter = "a", Name = "Aurek", Detail = "(a)" });
			All.Add (new LettersAurekBesh { Letter = "b", Name = "Besh", Detail = "(b)" });
			All.Add (new LettersAurekBesh { Letter = "c", Name = "Cresh", Detail = "(c)" });
			All.Add (new LettersAurekBesh { Letter = "d", Name = "Dorn", Detail = "(d)" });
			All.Add (new LettersAurekBesh { Letter = "e", Name = "Esk", Detail = "(e)" });
			All.Add (new LettersAurekBesh { Letter = "f", Name = "Forn", Detail = "(f)" });
			All.Add (new LettersAurekBesh { Letter = "g", Name = "Grek", Detail = "(g)" });
			All.Add (new LettersAurekBesh { Letter = "h", Name = "Herf", Detail = "(h)" });
			All.Add (new LettersAurekBesh { Letter = "i", Name = "Isk", Detail = "(i)" });
			All.Add (new LettersAurekBesh { Letter = "j", Name = "Jenth", Detail = "(j)" });
			All.Add (new LettersAurekBesh { Letter = "k", Name = "Krill", Detail = "(k)" });
			All.Add (new LettersAurekBesh { Letter = "l", Name = "Leth", Detail = "(l)" });
			All.Add (new LettersAurekBesh { Letter = "m", Name = "Mern", Detail = "(m)" });
			All.Add (new LettersAurekBesh { Letter = "n", Name = "Nern", Detail = "(n)" });
			All.Add (new LettersAurekBesh { Letter = "o", Name = "Osk", Detail = "(o)" });
			All.Add (new LettersAurekBesh { Letter = "p", Name = "Peth", Detail = "(p)" });
			All.Add (new LettersAurekBesh { Letter = "q", Name = "Qek", Detail = "(q)" });
			All.Add (new LettersAurekBesh { Letter = "r", Name = "Resh", Detail = "(r)" });
			All.Add (new LettersAurekBesh { Letter = "s", Name = "Senth", Detail = "(s)" });
			All.Add (new LettersAurekBesh { Letter = "t", Name = "Trill", Detail = "(t)" });
			All.Add (new LettersAurekBesh { Letter = "u", Name = "Usk", Detail = "(u)" });
			All.Add (new LettersAurekBesh { Letter = "v", Name = "Vev", Detail = "(v)" });
			All.Add (new LettersAurekBesh { Letter = "w", Name = "Wesk", Detail = "(w)" });
			All.Add (new LettersAurekBesh { Letter = "x", Name = "Xesh", Detail = "(x)" });
			All.Add (new LettersAurekBesh { Letter = "y", Name = "Yirt", Detail = "(y)" });
			All.Add (new LettersAurekBesh { Letter = "z", Name = "Zerek", Detail = "(z)" });
			All.Add (new LettersAurekBesh { Letter = "ç", Name = "Cherek", Detail = "(ç)" });
			All.Add (new LettersAurekBesh { Letter = "æ", Name = "Enth", Detail = "(æ)" });
			All.Add (new LettersAurekBesh { Letter = "œ", Name = "Onith", Detail = "(œ)" });
			//All.Add (new LettersAurekBesh { Letter = "kh", Name = "Krenth", Detail = "(kh)" });
			All.Add (new LettersAurekBesh { Letter = "ñ", Name = "Nen", Detail = "(ñ)" });
			All.Add (new LettersAurekBesh { Letter = "ø", Name = "Orenth", Detail = "(ø)" });
			//All.Add (new LettersAurekBesh { Letter = "sh", Name = "Shen", Detail = "(sh)" });
			//All.Add (new LettersAurekBesh { Letter = "∂", Name = "Tesh", Detail = "(∂)" });
			All.Add (new LettersAurekBesh { Letter = ",", Name = "," });
			All.Add (new LettersAurekBesh { Letter = ".", Name = "." });
			All.Add (new LettersAurekBesh { Letter = "?", Name = "?" });
			All.Add (new LettersAurekBesh { Letter = "!", Name = "!" });
			All.Add (new LettersAurekBesh { Letter = ":", Name = ":" });
			All.Add (new LettersAurekBesh { Letter = ";", Name = ";" });
			All.Add (new LettersAurekBesh { Letter = "-", Name = "-" });
			All.Add (new LettersAurekBesh { Letter = "\"", Name = "\"" });
			//All.Add (new LettersAurekBesh { Letter = "“", Name = "“" });
			All.Add (new LettersAurekBesh { Letter = "'", Name = "'" });
			All.Add (new LettersAurekBesh { Letter = "‘", Name = "‘" });
			All.Add (new LettersAurekBesh { Letter = "(", Name = "(" });
			All.Add (new LettersAurekBesh { Letter = ")", Name = ")" });
			All.Add (new LettersAurekBesh { Letter = "/", Name = "/" });
			All.Add (new LettersAurekBesh { Letter = "$", Name = "Crédit" });
			All.Add (new LettersAurekBesh { Letter = "0", Name = "0" });
			All.Add (new LettersAurekBesh { Letter = "1", Name = "1" });
			All.Add (new LettersAurekBesh { Letter = "2", Name = "2" });
			All.Add (new LettersAurekBesh { Letter = "3", Name = "3" });
			All.Add (new LettersAurekBesh { Letter = "4", Name = "4" });
			All.Add (new LettersAurekBesh { Letter = "5", Name = "5" });
			All.Add (new LettersAurekBesh { Letter = "6", Name = "6" });
			All.Add (new LettersAurekBesh { Letter = "7", Name = "7" });
			All.Add (new LettersAurekBesh { Letter = "8", Name = "8" });
			All.Add (new LettersAurekBesh { Letter = "9", Name = "9" });

		}

		static LettersAurekBesh ()
		{
			PopulateData ();
			//LettersAurekBesh [] letters = new LettersAurekBesh [26];
			//All.CopyTo (letters, 0);
			//Data = letters;
		}

		//public static IEnumerable<LettersAurekBesh> Data { get; set; }
	}
}