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
	class AdicionarTrechoDialogVM : BaseVM
	{
		#region PARAMETROS DIALOG
		private ICommand _SalvaCommand;
		private ICommand _CancelaCommand;
		public bool salvo { get; set; }
		private Window window;
		#endregion

		#region PARAMETROS
		public Visibility show_requinte
		{
			get
			{
				if (shp != null)
				{
					if (shp.jato == jato.Solido)
					{
						return Visibility.Visible;
					}
					else
					{
						return Visibility.Collapsed;
					}
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}
		public Visibility show_k
		{
			get
			{
				if (shp != null)
				{
					if (shp.jato == jato.Regulavel)
					{
						return Visibility.Visible;
					}
					else
					{
						return Visibility.Collapsed;
					}
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}
		

		private shp _old_shp;

		private shp _shp;
		public shp shp { get { return _shp; } set { _shp = value; OnPropertyChanged(); } }

		public ObservableCollection<material> materiais
		{
			get
			{
				return data.materiais;
			}
			set
			{
				data.materiais = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<mangueira> mangueiras
		{
			get
			{
				return data.mangueiras;
			}
			set
			{
				data.mangueiras = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<jato> jatos
		{
			get
			{
				return data.jatos;
			}
			set
			{
				data.jatos = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region AdicionarTrecho
		public trecho AdicionarTrecho(shp __shp)
		{
			DebugAlert();
			shp = new shp();
			//shp.IsEnabled = false;
			shp.pontos = new ObservableCollection<ponto>();
			for (int i = 0; i < __shp.pontos.Count; i++)
			{
				if (__shp.pontos[i].hasTrechoTermina)
				{
					shp.pontos.Add(__shp.pontos[i]);
				}
			}
			//shp.pontos = __shp.pontos;
			new trecho(__shp.trechos.LastOrDefault().PontoAnterior(), shp.pontosPontosOnly.LastOrDefault(), 0, 0, shp);
			salvo = false;
			window = new Window
			{
				Title = "Novo Trecho",
				Content = new VIEW.AdicionarTrechoDialog(),
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
			return shp.trechos.FirstOrDefault();
		}
		public bool canEditarPecas()
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
