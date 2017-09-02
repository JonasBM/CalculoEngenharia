using CEBiblioteca;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculoEngenharia.VIEWMODEL
{
	class InicioVM : BaseVM
	{
		#region Commands
		private ICommand _NovoSHPCommand;
		private ICommand _NovoICGCommand;
		private ICommand _NovoCIECommand;
		#endregion

		public InicioVM() { DebugAlert(); }
		public bool Start(string __fullName)
		{
			DebugAlert();
			if (!string.IsNullOrEmpty(__fullName))
			{
				FileInfo fileInfo = new FileInfo(__fullName);
				if (!fileInfo.Exists)
				{
					__fullName = null;
					Message.Alerta.ArquivoInexistente();
				}
				else
				{
					if (canAbrir())
					{
						if (Abrir(__fullName))
						{
							return true;
						}
					}
				}
			}
			titulo = "Prevenção - Cálculo Engenharia";
			Application.Current.MainWindow.DataContext = this;
			conteudo = new VIEW.Inicio();
			conteudo.DataContext = this;
			return false;
		}
		#region NovoSHPCommand & NovoICGCommand & NovoCIECommand
		public ICommand NovoSHPCommand
		{
			get
			{
				if (_NovoSHPCommand == null)
				{
					_NovoSHPCommand = new RelayCommand(p => Abrir(Extension.GetNovoArquivoFileName(ExtensionType.SHP)), p => canAbrir());
				}
				return _NovoSHPCommand;
			}
		}
		public ICommand NovoICGCommand
		{
			get
			{
				if (_NovoICGCommand == null)
				{
					_NovoICGCommand = new RelayCommand(p => Abrir(Extension.GetNovoArquivoFileName(ExtensionType.SHP)), p => canAbrir());
				}
				return _NovoICGCommand;
			}
		}
		public ICommand NovoCIECommand
		{
			get
			{
				if (_NovoCIECommand == null)
				{
					_NovoCIECommand = new RelayCommand(p => Abrir(Extension.GetNovoArquivoFileName(ExtensionType.SHP)), p => canAbrir());
				}
				return _NovoCIECommand;
			}
		}
		private bool Abrir(string __fileName = null)
		{
			DebugAlert();
			object returnObject = Processo.Abrir(__fileName);
			if (returnObject != null)
			{
				Application.Current.MainWindow.DataContext = returnObject;
				return true;
			}
			return false;
		}
		private bool canAbrir()
		{
			return true;
		}
		#endregion
	}
}
