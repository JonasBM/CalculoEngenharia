using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CEBiblioteca
{
	public enum ExtensionType { None, SHP, IGC, CIE };

	public class Extension
	{
		public static string GetExtension(ExtensionType __Extension, bool message = false)
		{
			switch (__Extension)
			{
				case ExtensionType.SHP:
					return ".tshp";
				case ExtensionType.IGC:
					return ".tigc";
				case ExtensionType.CIE:
					return ".tcie";
				default:
					if (message) { Message.Alerta.ExtensaoInvalida(); }
					return null;
			}
		}
		public static ExtensionType GetExtensionType(string __fileName, bool message = false)
		{
			string extension = Path.GetExtension(__fileName).ToLower();
			switch (extension)
			{
				case ".tshp":
					return ExtensionType.SHP;
				case ".tigc":
					return ExtensionType.IGC;
				case ".tcie":
					return ExtensionType.CIE;
				default:
					if (message) { Message.Alerta.ExtensaoInvalida(); }
					return ExtensionType.None;
			}
		}


	}
}
