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

		private int _numeroHidrantes;
		public int numeroHidrantes { get { return _numeroHidrantes; } set { _numeroHidrantes = value; OnPropertyChanged(); } }

		private float _alturaPisoAPiso;
		public float alturaPisoAPiso { get { return _alturaPisoAPiso; } set { _alturaPisoAPiso = value; OnPropertyChanged(); } }

		public bool salvo { get; set; }

		private ICommand _SalvaCommand;
		private ICommand _CancelaCommand;

		private Window window;

		#endregion

		#region NovoSHP
		public void NovoSHP()
		{
			DebugAlert();
			numeroHidrantes = 7;
			alturaPisoAPiso = (float)2.88;
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
			DebugAlert();
			window.DialogResult = true;
			window.Close();
		}
		public bool canSalva()
		{
			if (
				numeroHidrantes > 0 || alturaPisoAPiso > 0
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
			DebugAlert();
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
