using CEBiblioteca;
using CESHP.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace CESHP.VIEWMODEL
{
	class AdicionarPontoDialogVM : BaseVM
	{
		#region PARAMETROS DIALOG
		private ICommand _SalvaCommand;
		private ICommand _CancelaCommand;
		public bool salvo { get; set; }
		private Window window;
		#endregion

		#region VISIBILIDADE
		public Visibility showPontoOrHidrante
		{
			get
			{
				if (pontoOrHidranteEnable)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}
		#endregion

		#region PARAMETROS

		private shp _old_shp;

		private shp _shp;
		public shp shp { get { return _shp; } set { _shp = value; OnPropertyChanged(); } }

		private bool _pontoOrHidranteEnable;
		public bool pontoOrHidranteEnable { get { return _pontoOrHidranteEnable; } set { _pontoOrHidranteEnable = value; OnPropertyChanged(); OnPropertyChanged("showPontoOrHidrante"); } }

		private ObservableCollection<string> _pontoOrHidrante;
		public ObservableCollection<string> pontoOrHidrante { get { return _pontoOrHidrante; } set { _pontoOrHidrante = value; OnPropertyChanged(); } }

		private string _pontoOrHidranteSelected;
		public string pontoOrHidranteSelected { get { return _pontoOrHidranteSelected; } set { _pontoOrHidranteSelected = value; OnPropertyChanged(); explicativoUpdate(); } }


		private int _adicionais;
		public int adicionais { get { return _adicionais; } set { _adicionais = value; OnPropertyChanged(); explicativoUpdate(); } }

		private string _explicativo;
		public string explicativo { get { return _explicativo; } set { _explicativo = value; OnPropertyChanged(); } }
		#endregion

		#region AdicionarPonto
		public void AdicionarPonto(shp __shp, int __indexPontoOrHidranteSelected = 0, bool __pontoOrHidranteEnable = true)
		{

			DebugAlert();
			shp = __shp;
			pontoOrHidrante = new ObservableCollection<string>() { "Ponto", "Hidrante" };
			pontoOrHidranteSelected = pontoOrHidrante[__indexPontoOrHidranteSelected];
			pontoOrHidranteEnable = __pontoOrHidranteEnable;
			salvo = false;
			window = new Window
			{
				Title = "Adicionar Pontos/Hidrantes",
				Content = new VIEW.AdicionarPontoDialog(),
				DataContext = this,
				Owner = Application.Current.MainWindow,
				ResizeMode = ResizeMode.NoResize,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				ShowInTaskbar = false,
				SizeToContent = SizeToContent.WidthAndHeight,
				WindowStyle = WindowStyle.ToolWindow
			};
			explicativoUpdate();
			if ((bool)window.ShowDialog())
			{
				salvo = true;
			}
		}
		public bool canEditarPecas()
		{
			return true;
		}
		#endregion

		public void explicativoUpdate()
		{
			DebugAlert();
			if (pontoOrHidranteSelected == "Ponto")
			{
				int indexLetra = shp.pontosPontosOnly.Count + adicionais - 3;
				string Letra = data.alfabeto.GetLetra(indexLetra);
				explicativo = "Adicionar até Ponto \"" + Letra + "\"";
				if (window != null) { window.Title = "Adicionar Pontos"; }

			}
			else if (pontoOrHidranteSelected == "Hidrante")
			{
				int numeroHidrante = shp.pontosHidrantesOnly.Count + adicionais;
				explicativo = "Adicionar até Hidrante \"H" + numeroHidrante + "\"";
				if (window != null) { window.Title = "Adicionar Hidrantes"; }
			}
			else
			{
				DebugAlertMessage("AdicionarPontoDialogVM, explicativoUpdate, PROBLEMA");
			}
		}

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
			return true;
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
