using CEBiblioteca;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CESHP.VIEWMODEL
{
	class NovoSHPDialogVM : BaseVM
	{
		#region Parametros

		private int _numero_hidrantes;
		public int numero_hidrantes { get { return _numero_hidrantes; } set { _numero_hidrantes = value; OnPropertyChanged(); } }

		private float _altura_piso_a_piso;
		public float altura_piso_a_piso { get { return _altura_piso_a_piso; } set { _altura_piso_a_piso = value; OnPropertyChanged(); } }

		public bool salvo { get; set; }

		private ICommand _SalvaCommand;
		private ICommand _CancelaCommand;

		private Window window;

		#endregion

		#region NovoSHP
		public void NovoSHP()
		{
			Debug.WriteLine("NovoSHPDialogVM, NovoSHP");
			numero_hidrantes = 7;
			altura_piso_a_piso = (float)2.88;
			salvo = false;

			window = new Window
			{
				Title = "Novo Calculo de SHP",
				Content = new VIEW.NovoSHPDialog(),
				DataContext = this,
				Owner = Application.Current.MainWindow,
				ResizeMode = ResizeMode.NoResize,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				ShowInTaskbar = false,
				SizeToContent = SizeToContent.WidthAndHeight,
				WindowStyle = WindowStyle.ToolWindow
			};
			if ((bool)window.ShowDialog())
			{
				salvo = true;
			}
		}
		public bool canNovoSHP()
		{
			return true;
		}
		#endregion

		#region SalvaCommand
		public ICommand SalvaCommand
		{
			get
			{
				if (_SalvaCommand == null)
				{
					_SalvaCommand = new RelayCommand(p => Salva(), p => canSalva());
				}
				return _SalvaCommand;
			}
		}
		public void Salva()
		{
			Debug.WriteLine("NovoSHPDialogVM, Salva");
			window.DialogResult = true;
			window.Close();
		}
		public bool canSalva()
		{
			if (
				numero_hidrantes > 0 || altura_piso_a_piso > 0
			)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region CancelaCommand
		public ICommand CancelaCommand
		{
			get
			{
				if (_CancelaCommand == null)
				{
					_CancelaCommand = new RelayCommand(p => Cancela(), p => canCancela());
				}
				return _CancelaCommand;
			}
		}
		public void Cancela()
		{
			Debug.WriteLine("NovoSHPDialogVM, Cancela");
			window.DialogResult = false;
			window.Close();
		}
		public bool canCancela()
		{
			return true;
		}
		#endregion
	}
}
