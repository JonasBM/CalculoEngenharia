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

		public InicioVM()
		{
			Debug.WriteLine("InicioVM, InicioVM");
		}
		public bool Start(string __fileName)
		{
			Debug.WriteLine("InicioVM, Start");
			if (!string.IsNullOrEmpty(__fileName))
			{
				FileInfo fileInfo = new FileInfo(__fileName);
				if (!fileInfo.Exists)
				{
					__fileName = null;
					Message.Alerta.ArquivoInexistente();
				}
				else
				{
					//ExtensionType extension = Extension.GetExtensionType(__fileName, true);



					/*
					switch (extension)
					{
						case ExtensionType.SHP:
							NovoSHP(__fileName);
							return true;
						case ExtensionType.IGC:
							NovoICG(__fileName);
							return true;
						case ExtensionType.CIE:
							NovoCIE(__fileName);
							return true;
						default:
							__fileName = null;
							break;
					}
					*/
				}
			}
			titulo = "Prevenção - Cálculo Engenharia";
			Application.Current.MainWindow.DataContext = this;
			conteudo = new VIEW.Inicio();
			conteudo.DataContext = this;
			return false;
		}

		#region NovoSHPCommand
		public ICommand NovoSHPCommand
		{
			get
			{
				if (_NovoSHPCommand == null)
				{
					_NovoSHPCommand = new RelayCommand(p => NovoSHP(), p => canNovoSHP());
				}
				return _NovoSHPCommand;
			}
		}
		private void NovoSHP(string __fileName = null)
		{
			Debug.WriteLine("InicioVM, NovoSHP");



			


			
		}
		private bool canNovoSHP()
		{
			return true;
		}
		#endregion

		#region NovoICGCommand
		public ICommand NovoICGCommand
		{
			get
			{
				if (_NovoICGCommand == null)
				{
					_NovoICGCommand = new RelayCommand(p => NovoICG(), p => canNovoICG());
				}
				return _NovoICGCommand;
			}
		}
		private void NovoICG(string __fileName = null)
		{
			Debug.WriteLine("InicioVM, NovoICG");
			Console.WriteLine("NovoICG");
			MessageBox.Show("NovoICG");
			MessageBox.Show(__fileName);
		}
		private bool canNovoICG()
		{
			return true;
		}
		#endregion

		#region NovoCIECommand
		public ICommand NovoCIECommand
		{
			get
			{
				if (_NovoCIECommand == null)
				{
					_NovoCIECommand = new RelayCommand(p => NovoCIE(), p => canNovoCIE());
				}
				return _NovoCIECommand;
			}
		}
		private void NovoCIE(string __fileName = null)
		{
			Debug.WriteLine("InicioVM, NovoCIE");
			Console.WriteLine("NovoCIE");
			MessageBox.Show("NovoCIE");
			MessageBox.Show(__fileName);
		}
		private bool canNovoCIE()
		{
			return true;
		}
		#endregion

	}
}
