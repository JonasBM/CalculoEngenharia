using CEBiblioteca;
using CESHP.MODEL;
using GongSolutions.Wpf.DragDrop;
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
	class EditarPecasDialogVM : BaseVM, IDropTarget
	{

		#region PARAMETROS DIALOG
		private ICommand _SalvaCommand;
		private ICommand _CancelaCommand;
		public bool salvo { get; set; }
		private Window window;
		#endregion

		#region Parametros

		private ICommand _AdicionarPecaCommand;
		private ICommand _RemoverPecaCommand;

		private ObservableCollection<peca> _pecas;
		public ObservableCollection<peca> pecas
		{
			get
			{
				if (_pecas == null)
				{
					_pecas = new ObservableCollection<peca>();
				}
				return _pecas;
			}
			set
			{
				_pecas = value;
				OnPropertyChanged();
			}
		}

		private int _pecasSelectedIndex;
		public int pecasSelectedIndex
		{
			get
			{
				return _pecasSelectedIndex;
			}
			set
			{
				_pecasSelectedIndex = value;
				OnPropertyChanged();
			}
		}


		private trecho _oldTrecho;
		public trecho oldTrecho { get { return _oldTrecho; } set { _oldTrecho = value; OnPropertyChanged(); } }

		private trecho _trecho;
		public trecho trecho { get { return _trecho; } set { _trecho = value; OnPropertyChanged(); } }

		
		#endregion

		#region EditarPecas
		public void EditarPecas(trecho __trecho)
		{
			trecho = __trecho;
			pecas = new ObservableCollection<peca>();
			for (int i = 0; i < trecho.pecasIndexes.Count; i++)
			{
				pecas.Add(new peca(trecho.material.pecas[trecho.pecasIndexes[i]].index, trecho.material.pecas[trecho.pecasIndexes[i]].nome));
			}
			Debug.WriteLine("EditarPecasDialogVM, EditarPecas");
			salvo = false;
			window = new Window
			{
				Title = "Peças do Trecho: " + trecho.inicio.nome + " - " + trecho.fim.nome,
				Content = new VIEW.EditarPecasDialog(),
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
		public bool canEditarPecas()
		{
			return true;
		}
		#endregion

		#region AdicionarPecaCommand
		public ICommand AdicionarPecaCommand
		{
			get
			{
				if (_AdicionarPecaCommand == null)
				{
					_AdicionarPecaCommand = new RelayCommand(p => AdicionarPeca((peca)p), p => canAdicionarPeca((peca)p));
				}
				return _AdicionarPecaCommand;
			}
		}
		public void AdicionarPeca(peca __peca)
		{
			Debug.WriteLine("EditarPecasDialogVM, AdicionarPeca");
			pecas.Add(new peca(__peca.index, __peca.nome));
		}
		public bool canAdicionarPeca(peca __peca)
		{
			if (__peca is peca)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region RemoverPecaCommand
		public ICommand RemoverPecaCommand
		{
			get
			{
				if (_RemoverPecaCommand == null)
				{
					_RemoverPecaCommand = new RelayCommand(p => RemoverPeca((peca)p), p => canRemoverPeca((peca)p));
				}
				return _RemoverPecaCommand;
			}
		}
		public void RemoverPeca(peca __peca)
		{
			Debug.WriteLine("EditarPecasDialogVM, RemoverPeca");
			pecas.Remove(__peca);
		}
		public bool canRemoverPeca(peca __peca)
		{
			if (__peca is peca)
			{
				return true;
			}
			return false;
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
			Debug.WriteLine("EditarPecasDialogVM, Salva");
			window.DialogResult = true;
			window.Close();
		}
		public bool canSalva()
		{
			if (true)
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
			Debug.WriteLine("EditarPecasDialogVM, Cancela");
			window.DialogResult = false;
			window.Close();
		}
		public bool canCancela()
		{
			return true;
		}
		#endregion

		#region DragNDrop
		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			//Debug.WriteLine("EditarPecasDialogVM, DragOver");
			if (dropInfo.Data != null && dropInfo.TargetItem != null && dropInfo.Data is peca && dropInfo.TargetItem is peca)
			{

				dropInfo.DropTargetAdorner = null;
				dropInfo.Effects = DragDropEffects.Move;

				peca sourceItem = dropInfo.Data as peca;
				peca targetItem = dropInfo.TargetItem as peca;
				int oldIndex = pecas.IndexOf(sourceItem);
				int newIndex = pecas.IndexOf(targetItem);
				//Debug.WriteLine("oldIndex, newIndex: {0} to {1}", oldIndex, newIndex);
				if (oldIndex != newIndex)
				{
					pecas.Move(oldIndex, newIndex);
					pecasSelectedIndex = newIndex;
				}
			}
		}
		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			Debug.WriteLine("EditarPecasDialogVM, Drop");
		}
		#endregion
	}
}
