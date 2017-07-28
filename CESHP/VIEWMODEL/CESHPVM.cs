using CEBiblioteca;
using CESHP.MODEL;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CESHP.VIEWMODEL
{
	public class CESHPVM : BaseVM, IDropTarget
	{

		#region Commands
		private ICommand _NovoArquivoCommand;
		private ICommand _AbrirArquivoCommand;
		private ICommand _SalvarArquivoCommand;
		private ICommand _SalvarComoCommand;
		private ICommand _DuplicarCommand;
		private ICommand _CalcularCommand;
		private ICommand _ImprimirCommand;

		private ICommand _AdicionarTrechoCommand;
		private ICommand _AdicionarPontoCommand;
		private ICommand _EditarPecasCommand;
		private ICommand _AplicarCommand;
		private ICommand _MudarCommand;
		private ICommand _ResetCommand;
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

		public CESHPVM(shp __shp)
		{
			Debug.WriteLine("CESHPVM, CESHPVM(shp)");
			shp = __shp;
			_old_shp = __shp;
		}
		public CESHPVM()
		{
			Debug.WriteLine("CESHPVM, CESHPVM");
			conteudo = new VIEW.CESHP();
			conteudo.DataContext = this;
			shp = shp.novo();
			if (shp != null)
			{

			}
		}

		public static void compare()
		{
			Debug.WriteLine("CESHPVM, compare");

		}

		#region NovoArquivoCommand
		public ICommand NovoArquivoCommand
		{
			get
			{
				if (_NovoArquivoCommand == null)
				{
					_NovoArquivoCommand = new RelayCommand(p => NovoArquivo(), p => canNovoArquivo());
				}
				return _NovoArquivoCommand;
			}
		}
		public void NovoArquivo()
		{
			Debug.WriteLine("CESHPVM, NovoArquivo");
			Console.WriteLine("NovoArquivo");
		}
		public bool canNovoArquivo()
		{
			return true;
		}
		#endregion

		#region AbrirArquivoCommand
		public ICommand AbrirArquivoCommand
		{
			get
			{
				if (_AbrirArquivoCommand == null)
				{
					_AbrirArquivoCommand = new RelayCommand(p => AbrirArquivo(), p => canAbrirArquivo());
				}
				return _AbrirArquivoCommand;
			}
		}
		public void AbrirArquivo()
		{
			Debug.WriteLine("CESHPVM, AbrirArquivo");
			Console.WriteLine("AbrirArquivo");
		}
		public bool canAbrirArquivo()
		{
			return true;
		}
		#endregion

		#region SalvarArquivoCommand
		public ICommand SalvarArquivoCommand
		{
			get
			{
				if (_SalvarArquivoCommand == null)
				{
					_SalvarArquivoCommand = new RelayCommand(p => SalvarArquivo(), p => canSalvarArquivo());
				}
				return _SalvarArquivoCommand;
			}
		}
		public void SalvarArquivo()
		{
			Debug.WriteLine("CESHPVM, SalvarArquivo");
			Console.WriteLine("SalvarArquivo");
		}
		public bool canSalvarArquivo()
		{
			return true;
		}
		#endregion

		#region SalvarComoCommand
		public ICommand SalvarComoCommand
		{
			get
			{
				if (_SalvarComoCommand == null)
				{
					_SalvarComoCommand = new RelayCommand(p => SalvarComo(), p => canSalvarComo());
				}
				return _SalvarComoCommand;
			}
		}
		public void SalvarComo()
		{
			Debug.WriteLine("CESHPVM, SalvarComo");
			Console.WriteLine("SalvarArquivo");
		}
		public bool canSalvarComo()
		{
			return true;
		}
		#endregion

		#region DuplicarCommand
		public ICommand DuplicarCommand
		{
			get
			{
				if (_DuplicarCommand == null)
				{
					_DuplicarCommand = new RelayCommand(p => Duplicar(), p => canDuplicar());
				}
				return _DuplicarCommand;
			}
		}
		public void Duplicar()
		{
			Debug.WriteLine("CESHPVM, Duplicar");
			Console.WriteLine("Duplicar");
		}
		public bool canDuplicar()
		{
			return true;
		}
		#endregion

		#region CalcularCommand
		public ICommand CalcularCommand
		{
			get
			{
				if (_CalcularCommand == null)
				{
					_CalcularCommand = new RelayCommand(p => Calcular(), p => canCalcular());
				}
				return _CalcularCommand;
			}
		}
		public void Calcular()
		{
			Debug.WriteLine("CESHPVM, Calcular");
			Console.WriteLine("Calcular");
		}
		public bool canCalcular()
		{
			return true;
		}
		#endregion

		#region ImprimirCommand
		public ICommand ImprimirCommand
		{
			get
			{
				if (_ImprimirCommand == null)
				{
					_ImprimirCommand = new RelayCommand(p => Imprimir(), p => canImprimir());
				}
				return _ImprimirCommand;
			}
		}
		public void Imprimir()
		{
			Debug.WriteLine("CESHPVM, Imprimir");
			Console.WriteLine("Imprimir");
		}
		public bool canImprimir()
		{
			return true;
		}
		#endregion


		#region AdicionarTrechoCommand
		public ICommand AdicionarTrechoCommand
		{
			get
			{
				if (_AdicionarTrechoCommand == null)
				{
					_AdicionarTrechoCommand = new RelayCommand(p => AdicionarTrecho(), p => canAdicionarTrecho());
				}
				return _AdicionarTrechoCommand;
			}
		}
		public void AdicionarTrecho()
		{
			Debug.WriteLine("CESHPVM, AdicionarTrecho");
			Console.WriteLine("AdicionarTrecho");
		}
		public bool canAdicionarTrecho()
		{
			return true;
		}
		#endregion

		#region AdicionarPontoCommand
		public ICommand AdicionarPontoCommand
		{
			get
			{
				if (_AdicionarPontoCommand == null)
				{
					_AdicionarPontoCommand = new RelayCommand(p => AdicionarPonto(), p => canAdicionarPonto());
				}
				return _AdicionarPontoCommand;
			}
		}
		public void AdicionarPonto()
		{
			Debug.WriteLine("CESHPVM, AdicionarPonto");
			Console.WriteLine("AdicionarPonto");
		}
		public bool canAdicionarPonto()
		{
			return true;
		}
		#endregion

		#region EditarPecasCommand
		public ICommand EditarPecasCommand
		{
			get
			{
				if (_EditarPecasCommand == null)
				{
					_EditarPecasCommand = new RelayCommand(p => EditarPecas((trecho)p), p => canEditarPecas((trecho)p));
				}
				return _EditarPecasCommand;
			}
		}
		public void EditarPecas(trecho __trecho)
		{
			Debug.WriteLine("CESHPVM, EditarPecas");
			EditarPecasDialogVM EditarPecasDialogVM = new EditarPecasDialogVM();
			EditarPecasDialogVM.EditarPecas(__trecho);
			if (EditarPecasDialogVM.salvo)
			{
				Console.WriteLine("AddPecaDialogVM.salvoTRUE");
				__trecho.pecas_indexes = new ObservableCollection<int>();
				for (int i = 0; i < EditarPecasDialogVM.pecas.Count; i++)
				{
					Console.WriteLine(EditarPecasDialogVM.pecas[i].index);
					__trecho.pecas_indexes.Add(EditarPecasDialogVM.pecas[i].index);
				}
			}
			else
			{
				Console.WriteLine("AddPecaDialogVM.salvoFALSE");
			}
		}
		public bool canEditarPecas(trecho __trecho)
		{
			if (__trecho is trecho)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region AplicarCommand
		public ICommand AplicarCommand
		{
			get
			{
				if (_AplicarCommand == null)
				{
					_AplicarCommand = new RelayCommand(p => Aplicar(), p => canAplicar());
				}
				return _AplicarCommand;
			}
		}
		public void Aplicar()
		{
			Debug.WriteLine("CESHPVM, Aplicar");
			Console.WriteLine("Aplicar");
		}
		public bool canAplicar()
		{
			return true;
		}
		#endregion

		#region MudarCommand
		public ICommand MudarCommand
		{
			get
			{
				if (_MudarCommand == null)
				{
					_MudarCommand = new RelayCommand(p => Mudar(), p => canMudar());
				}
				return _MudarCommand;
			}
		}
		public void Mudar()
		{
			Debug.WriteLine("CESHPVM, Mudar");
			Console.WriteLine("Mudar");
		}
		public bool canMudar()
		{
			return true;
		}
		#endregion

		#region ResetCommand
		public ICommand ResetCommand
		{
			get
			{
				if (_ResetCommand == null)
				{
					_ResetCommand = new RelayCommand(p => Reset(), p => canReset());
				}
				return _ResetCommand;
			}
		}
		public void Reset()
		{
			Debug.WriteLine("CESHPVM, Reset");
			Console.WriteLine("Reset");
		}
		public bool canReset()
		{
			return true;
		}
		#endregion

		#region DragNDrop
		public void DragOver(IDropInfo dropInfo)
		{
			//Debug.WriteLine("CESHPVM, DragOver");
			if (dropInfo.Data != null && dropInfo.TargetItem != null && dropInfo.Data is trecho && dropInfo.TargetItem is trecho)
			{
				dropInfo.DropTargetAdorner = null;
				dropInfo.Effects = DragDropEffects.Move;

				trecho sourceItem = dropInfo.Data as trecho;
				trecho targetItem = dropInfo.TargetItem as trecho;
				int oldIndex = shp.trechos.IndexOf(sourceItem);
				int newIndex = shp.trechos.IndexOf(targetItem);
				//Debug.WriteLine("oldIndex, newIndex: {0} to {1}", oldIndex, newIndex);
				if (sourceItem.fim != null)
				{
					if (sourceItem.fim.tipo == tipos_de_ponto.Hidrante && newIndex == 0) { newIndex = 1; }
					if (oldIndex != newIndex)
					{
						shp.trechos.Move(oldIndex, newIndex);
						shp.trechosSelected = sourceItem;
					}
				}

			}
		}
		public void Drop(IDropInfo dropInfo)
		{
			Debug.WriteLine("CESHPVM, Drop");
			shp.IsEnabled = false;
			shp.OrganizaLetras();
			shp.IsEnabled = true;
		}
		#endregion
	}
}
