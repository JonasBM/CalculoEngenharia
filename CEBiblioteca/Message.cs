using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CEBiblioteca
{
	public class Message
	{
		public class Alerta {
			public static void ArquivoInexistente() { MessageBox.Show("Arquivo inexistente!", "Extensão invalida", MessageBoxButton.OK, MessageBoxImage.Warning); }
			public static void ExtensaoInvalida() { MessageBox.Show("Por favor, verifique o nome do arquivo!", "Extensão invalida", MessageBoxButton.OK, MessageBoxImage.Warning); }
			public static void ArquivoInvalido(Exception e)
			{
				MessageBox.Show("Arquivo invalido!");
				Debug.WriteLine("Arquivo invalido: {0}", e.Message);
			}
			public static void FaltaCESHPDLL() { MessageBox.Show("CESHP.dll Não encontrada, ou corrompida!", "Falha no carregamento da DLL", MessageBoxButton.OK, MessageBoxImage.Stop); }

			public static void FaltaCEIGCDLL() { MessageBox.Show("CEIGC.dll Não encontrada, ou corrompida!", "Falha no carregamento da DLL", MessageBoxButton.OK, MessageBoxImage.Stop); }

			public static void FaltaCECIEDLL() { MessageBox.Show("CECIE.dll Não encontrada, ou corrompida!", "Falha no carregamento da DLL", MessageBoxButton.OK, MessageBoxImage.Stop); }


			//public static void FaltaCECIEDLL() { MessageBox.Show("CECIE.dll Não encontrada," + Environment.NewLine + "ou corrompida!", "Falha no carregamento da DLL"); }

		}

		public class Prompt
		{
			public static bool YesOrNo(string titulo, string mensagem)
			{
				if (MessageBox.Show(mensagem, titulo, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					return true;
				}
				return false;
			}
			public static bool NovaJanela()
			{
				return YesOrNo("Nova Janela", "Abrir em Nova Janela?");
			}
		}
	}
}
