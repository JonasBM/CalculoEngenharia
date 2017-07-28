using CEBiblioteca;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		#endregion

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

		public InicioVM()
		{
			Debug.WriteLine("InicioVM, InicioVM");
			conteudo = new VIEW.Inicio();
			conteudo.DataContext = this;
		}

		public void NovoSHP()
		{
			Debug.WriteLine("InicioVM, NovoSHP");
			Application.Current.MainWindow.DataContext = new CESHP.VIEWMODEL.CESHPVM();
		}
		public bool canNovoSHP()
		{
			return true;
		}
		#endregion



	}
}
