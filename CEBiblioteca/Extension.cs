using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public static string GetNovoArquivoFileName(ExtensionType __Extension, bool message = false)
		{
			switch (__Extension)
			{
				case ExtensionType.SHP:
					return "novo:?|novo" + GetExtension (__Extension) + GetExtension(__Extension);
				case ExtensionType.IGC:
					return "novo:?|novo" + GetExtension(__Extension) + GetExtension(__Extension);
				case ExtensionType.CIE:
					return "novo:?|novo" + GetExtension(__Extension) + GetExtension(__Extension);
				default:
					if (message) { Message.Alerta.ExtensaoInvalida(); }
					return null;
			}
		}

		public static ExtensionType IsNovoArquivoFileName(string __fileName, bool message = false)
		{

			if (__fileName == GetNovoArquivoFileName(ExtensionType.SHP, false))
			{
				return ExtensionType.SHP;
			}
			else if (__fileName == GetNovoArquivoFileName(ExtensionType.IGC, false))
			{
				return ExtensionType.SHP;
			}
			else if (__fileName == GetNovoArquivoFileName(ExtensionType.CIE, false))
			{
				return ExtensionType.SHP;
			}
			else
			{
				if (message) { Message.Alerta.ExtensaoInvalida(); }
				return ExtensionType.None;
			}
		}

		public static bool IsFileNameValid(string __fileName, bool message = false)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(__fileName);
			}
			catch (Exception e)
			{
				
				Debug.WriteLine(e.Message);
			}
			finally { }

			return false;
		}
	}
}
