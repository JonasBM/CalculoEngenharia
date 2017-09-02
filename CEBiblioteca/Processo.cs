using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CEBiblioteca
{
	public class Processo
	{
		public static bool Novo(string __fullName)
		{

			try
			{
				string path = Process.GetCurrentProcess().MainModule.FileName;
				ProcessStartInfo info = new ProcessStartInfo(path, __fullName);
				Process.Start(info);
			}
			catch (Exception e)
			{
				Message.Alerta.ArquivoInvalido(e);
				return false;
			}
			finally { }
			return true;
		}


		public static object Abrir(string __fullName = null)
		{
			string pathDLL = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			string type = null;
			string staticMethod = "TryCreate";
			ExtensionType extension = ExtensionType.None;

			if (!string.IsNullOrEmpty(__fullName))
			{
				ExtensionType novoArquivoExtensionType = Extension.IsNovoArquivoFileName(__fullName);
				if (novoArquivoExtensionType != ExtensionType.None)
				{
					__fullName = null;
					extension = novoArquivoExtensionType;
				}
				else
				{
					FileInfo fileInfo = new FileInfo(__fullName);
					if (fileInfo != null)
					{
						if (fileInfo.Exists)
						{
							extension = Extension.GetExtensionType(__fullName, true);
						}
						else
						{
							__fullName = null;
						}
					}
				}
				switch (extension)
				{
					case ExtensionType.SHP:
						pathDLL += "\\CESHP.dll";
						type = "CESHP.VIEWMODEL.CESHPVM";
						break;
					case ExtensionType.IGC:
						pathDLL += "\\CEIGC.dll";
						type = "CEIGC.VIEWMODEL.CEIGCVM";
						break;
					case ExtensionType.CIE:
						pathDLL += "\\CECIE.dll";
						type = "CECIE.VIEWMODEL.CECIEVM";
						break;
					default:
						pathDLL = null;
						type = null;
						__fullName = null;
						break;
				}
			}
			object returnObject = null;
			try
			{
				if (pathDLL != null && type != null)
				{
					Assembly myAssembly = Assembly.LoadFile(pathDLL);
					Type myType = myAssembly.GetType(type);
					returnObject = myType.InvokeMember(staticMethod, BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.Public, null, null, new object[] { __fullName, false });
					if (returnObject != null)
					{
						if (returnObject.GetType() == myType)
						{
							return returnObject;
						}
					}
				}
			}
			catch (Exception e)
			{
				Message.Alerta.FaltaCESHPDLL();
				Debug.WriteLine(e.Message);
			}
			finally { }
			return null;
		}
	}
}
