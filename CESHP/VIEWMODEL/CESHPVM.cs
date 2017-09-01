using CEBiblioteca;
using CESHP.MODEL;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
		private ICommand _AdicionarHidranteCommand;

		private ICommand _EditarPecasCommand;
		private ICommand _RemoverTrechoCommand;

		private ICommand _AplicarCommand;
		private ICommand _MudarCommand;

		private ICommand _ResetCommand;

		private ICommand _OrganizarCommand;
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


		public shp shp { get { return _shp; } set { _shp = value; OnPropertyChanged(); Refresh(); } }

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

		
		private CESHPVM()
		{
			Debug.WriteLine("CESHPVM, CESHPVM");
		}
		public static CESHPVM TryCreate(string __fileName = null, bool novaJanela = false)
		{
			Debug.WriteLine("CESHPVM, TryCreate");
			CESHPVM CESHPVM = new CESHPVM();
			if (CESHPVM.AbrirArquivoFileName(__fileName, novaJanela))
			{
				CESHPVM.conteudo = new VIEW.CESHP();
				CESHPVM.conteudo.DataContext = CESHPVM;
				if (novaJanela)
				{
					return null;
				}
				return CESHPVM;
			}
			return null;
		}

		public void Refresh()
		{
			if (shp != null)
			{
				shp.Refresh();
				UpdateTitulo();
			}
			if (conteudo != null)
			{
				conteudo.UpdateLayout();
			}
		}
		public void UpdateTitulo()
		{
			Debug.WriteLine("CESHPVM, UpdateTitulo");
			if (shp != null)
			{
				string nome;
				if (shp.arquivo != null)
				{
					nome = Path.GetFileNameWithoutExtension(shp.arquivo.FullName);
					if (shp.arquivo.IsReadOnly) { nome += "(ReadOnly)"; }
				}
				else
				{
					nome = "Calculo de SHP";
				}
				titulo = nome;
			}

		}

		#region NovoArquivoCommand E AbrirArquivoCommand
		public ICommand NovoArquivoCommand
		{
			get
			{
				if (_NovoArquivoCommand == null)
				{
					_NovoArquivoCommand = new RelayCommand(p => AbrirArquivo(true), p => canAbrirArquivo());
				}
				return _NovoArquivoCommand;
			}
		}
		public ICommand AbrirArquivoCommand
		{
			get
			{
				if (_AbrirArquivoCommand == null)
				{
					_AbrirArquivoCommand = new RelayCommand(p => AbrirArquivo(false), p => canAbrirArquivo());
				}
				return _AbrirArquivoCommand;
			}
		}
		private void AbrirArquivo(bool novo = true)
		{
			Debug.WriteLine("CESHPVM, AbrirArquivo");
			bool novaJanela = true;
			if (MessageBox.Show("Abrir em Nova Janela?", "Nova Janela", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				novaJanela = true;
			}
			else
			{
				novaJanela = false;
			}
			if (novo)
			{
				AbrirArquivoFileName(null, novaJanela);
			}
			else
			{
				OpenFileDialog openFileDialog1 = new OpenFileDialog();
				openFileDialog1.Filter = "tshp files (*.tshp)|*.tshp|All files (*.*)|*.*";
				openFileDialog1.FilterIndex = 1;
				openFileDialog1.RestoreDirectory = true;
				if ((bool)openFileDialog1.ShowDialog())
				{
					if (openFileDialog1.FileName != "")
					{
						AbrirArquivoFileName(openFileDialog1.FileName, novaJanela);
					}
				}
			}

		}
		private bool canAbrirArquivo()
		{
			return true;
		}
		private bool AbrirArquivoFileName(string fileName, bool novaJanela = true)
		{
			shp _shp = null;
			if (fileName == null || fileName == "")
			{
				if (!novaJanela)
				{
					_shp = shp.Novo();
				}
			}
			else
			{
				FileInfo fileInfo = new FileInfo(fileName);
				if (!fileInfo.Exists) { MessageBox.Show("Arquivo inexistente!"); }

				string extension = Path.GetExtension(fileName);
				if (extension == ".tshp")
				{
					Stream stream = null;
					try
					{
						IFormatter formatter = new BinaryFormatter();
						stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
						_shp = (shp)formatter.Deserialize(stream);
						_shp.arquivo = fileInfo;
					}
					catch (Exception e)
					{
						MessageBox.Show("Arquivo invalido!");
						Debug.WriteLine("Arquivo invalido: {0}", e.Message);
					}
					finally
					{
						if (stream != null) { stream.Close(); }
					}
				}
				else if (extension == ".tgas")
				{
					
				}
				else if (extension == ".tcaf")
				{
					throw new NotImplementedException();
				}
				else
				{
					MessageBox.Show("Extensão invalida!");
				}
			}
			if (novaJanela)
			{
				//abrir novo arquivo em nova janela
				//path = System.Reflection.Assembly.GetExecutingAssembly().Location;
				//path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
				//path = Application.ExecutablePath;
				//var info = new ProcessStartInfo(Application.ExecutablePath);
				//path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
				string fullName = "novotshp.tshp";
				if (_shp != null)
				{
					if (_shp.arquivo != null)
					{
						fullName = _shp.arquivo.FullName;
					}
				}
				try
				{
					string path = Process.GetCurrentProcess().MainModule.FileName;
					ProcessStartInfo info = new ProcessStartInfo(path, fullName);
					Process.Start(info);
				}
				catch (Exception e)
				{
					MessageBox.Show("Arquivo invalido!");
					Debug.WriteLine("Arquivo invalido: {0}", e.Message);
				}
				finally { }
				
			}
			else
			{
				if (_shp != null)
				{
					shp = _shp;
				}
			}
			if (_shp != null)
			{
				return true;
			}
			return false;
		}
		#endregion
		//fazer
		#region SalvarArquivoCommand E SalvarComoCommand
		public ICommand SalvarArquivoCommand
		{
			get
			{
				if (_SalvarArquivoCommand == null)
				{
					_SalvarArquivoCommand = new RelayCommand(p => SalvarArquivo(false), p => canSalvarArquivo());
				}
				return _SalvarArquivoCommand;
			}
		}
		public ICommand SalvarComoCommand
		{
			get
			{
				if (_SalvarComoCommand == null)
				{
					_SalvarComoCommand = new RelayCommand(p => SalvarArquivo(true), p => canSalvarArquivo());
				}
				return _SalvarComoCommand;
			}
		}
		private void SalvarArquivo(bool novo = true)
		{
			Debug.WriteLine("CESHPVM, SalvarArquivo");
			if (shp == null) { MessageBox.Show("Nenhum arquivo aberto!", "Erro"); return; }
			if (shp.arquivo == null)
			{
				novo = true;
			}
			else
			{
				if (shp.arquivo.IsReadOnly)
				{
					novo = true;
				}
			}
			string nomeArquivo = "";
			if (novo)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.Filter = "tshp files (*.tshp)|*.tshp|All files (*.*)|*.*";
				saveFileDialog1.FilterIndex = 1;
				saveFileDialog1.RestoreDirectory = true;
				if ((bool)saveFileDialog1.ShowDialog())
				{
					nomeArquivo = saveFileDialog1.FileName;
				}
			}
			else
			{
				nomeArquivo = shp.arquivo.FullName;
				if (nomeArquivo == "") { MessageBox.Show("Problemas com o nome do arquivo," + Environment.NewLine + "por favor tente \"Salvar Como...\"!", "Erro"); return; }
			}
			if (nomeArquivo == "") { return; }
			SalvarArquivoFileName(nomeArquivo);
		}
		private bool canSalvarArquivo()
		{
			return true;
		}
		private void SalvarArquivoFileName(string nomeArquivo)
		{
			Debug.WriteLine("CESHPVM, SalvarArquivoFileName");
			if (nomeArquivo == "") { return; }
			if (Path.GetExtension(nomeArquivo) != ".tshp") { nomeArquivo += ".tshp"; }
			Stream stream = null;
			try
			{
				IFormatter formatter = new BinaryFormatter();
				stream = new FileStream(nomeArquivo, FileMode.Create, FileAccess.Write, FileShare.None);
				formatter.Serialize(stream, shp);
				FileInfo novoArquivo = new FileInfo(nomeArquivo);
				if (novoArquivo == null) { MessageBox.Show("Problemas na hora de salvar!", "Erro"); return; }
				shp.arquivo = novoArquivo;
				MessageBox.Show("Arquivo \"" + shp.arquivo.Name + "\" Salvo com sucesso!", "Arquivo Salvo");
				UpdateTitulo();
			}
			catch (Exception e)
			{
				MessageBox.Show("Arquivo invalido!", "Erro");
				Debug.WriteLine("Arquivo invalido: {0}", e.Message);
			}
			finally
			{
				if (stream != null) { stream.Close(); }
			}
		}

		#endregion
		//OK

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
		private void Duplicar()
		{
			Debug.WriteLine("CESHPVM, Duplicar");
			Console.WriteLine("Duplicar");
		}
		private bool canDuplicar()
		{
			return true;
		}
		#endregion
		//fazer

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
		private void Calcular()
		{
			Debug.WriteLine("CESHPVM, Calcular");
			Console.WriteLine("Calcular");
		}
		private bool canCalcular()
		{
			return true;
		}
		#endregion
		//fazer
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
		private void Imprimir()
		{
			Debug.WriteLine("CESHPVM, Imprimir");
			Console.WriteLine("Imprimir");
		}
		private bool canImprimir()
		{
			return true;
		}
		#endregion
		//fazer



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
		private void AdicionarTrecho()
		{
			Debug.WriteLine("CESHPVM, AdicionarTrecho");

			trecho novoTrecho = new trecho(null, null, 0, 0, shp);
			novoTrecho.inicio = novoTrecho.PontoAnterior();
			novoTrecho.fim = novoTrecho.PontoPosterior();
			return;
			//ADIADO
			/*
			AdicionarTrechoDialogVM AdicionarTrechoDialogVM = new AdicionarTrechoDialogVM();
			trecho trecho = AdicionarTrechoDialogVM.AdicionarTrecho(shp);
			if (AdicionarTrechoDialogVM.salvo)
			{
				Console.WriteLine("AdicionarTrechoDialogVM.salvoTRUE");
				shp.trechos.Add(trecho);
			}
			else
			{
				Console.WriteLine("AdicionarTrechoDialogVM.salvoFALSE");
			}
			*/
		}
		private bool canAdicionarTrecho()
		{
			return true;
		}
		#endregion
		//OK (funcionalidade simples)
		#region AdicionarPontoCommand E AdicionarHidranteCommand
		public ICommand AdicionarPontoCommand
		{
			get
			{
				if (_AdicionarPontoCommand == null)
				{
					_AdicionarPontoCommand = new RelayCommand(p => AdicionarPontoOrHidrante(0, false), p => canAdicionarPontoOrHidrante());
				}
				return _AdicionarPontoCommand;
			}
		}
		public ICommand AdicionarHidranteCommand
		{
			get
			{
				if (_AdicionarHidranteCommand == null)
				{
					_AdicionarHidranteCommand = new RelayCommand(p => AdicionarPontoOrHidrante(1, false), p => canAdicionarPontoOrHidrante());
				}
				return _AdicionarHidranteCommand;
			}
		}
		private void AdicionarPontoOrHidrante(int __indexPontoOrHidranteSelected = 0, bool __pontoOrHidranteEnable = true)
		{
			Debug.WriteLine("CESHPVM, AdicionarPontoOrHidrante");

			AdicionarPontoDialogVM AdicionarPontoDialogVM = new AdicionarPontoDialogVM();
			AdicionarPontoDialogVM.AdicionarPonto(shp, __indexPontoOrHidranteSelected, __pontoOrHidranteEnable);
			if (AdicionarPontoDialogVM.salvo)
			{
				Console.WriteLine("AdicionarPontoDialogVM.salvoTRUE");
				tipos_de_ponto tipo = tipos_de_ponto.Ponto;
				if (AdicionarPontoDialogVM.pontoOrHidranteSelected == "Ponto")
				{
					tipo = tipos_de_ponto.Ponto;
				}
				else if (AdicionarPontoDialogVM.pontoOrHidranteSelected == "Hidrante")
				{
					tipo = tipos_de_ponto.Hidrante;
				}
				else
				{
					Debug.WriteLine("CESHPVM, AdicionarPonto, PROBLEMA");
					return;
				}
				for (int i = 0; i < AdicionarPontoDialogVM.adicionais; i++)
				{
					ponto.Novo(shp, tipo);
				}
			}
			else
			{
				Console.WriteLine("AdicionarPontoDialogVM.salvoFALSE");
			}
		}
		private bool canAdicionarPontoOrHidrante()
		{
			return true;
		}
		#endregion
		//OK

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
		private void EditarPecas(trecho __trecho)
		{
			Debug.WriteLine("CESHPVM, EditarPecas");
			EditarPecasDialogVM EditarPecasDialogVM = new EditarPecasDialogVM();
			EditarPecasDialogVM.EditarPecas(__trecho);
			if (EditarPecasDialogVM.salvo)
			{
				Console.WriteLine("AddPecaDialogVM.salvoTRUE");
				__trecho.pecasIndexes = new ObservableCollection<int>();
				for (int i = 0; i < EditarPecasDialogVM.pecas.Count; i++)
				{
					Console.WriteLine(EditarPecasDialogVM.pecas[i].index);
					__trecho.pecasIndexes.Add(EditarPecasDialogVM.pecas[i].index);
				}
			}
			else
			{
				Console.WriteLine("AddPecaDialogVM.salvoFALSE");
			}
		}
		private bool canEditarPecas(trecho __trecho)
		{
			if (__trecho is trecho)
			{
				return true;
			}
			return false;
		}
		#endregion
		//OK
		#region RemoverTrechoCommand
		public ICommand RemoverTrechoCommand
		{
			get
			{
				if (_RemoverTrechoCommand == null)
				{
					_RemoverTrechoCommand = new RelayCommand(p => RemoverTrecho((trecho)p), p => canRemoverTrecho((trecho)p));
				}
				return _RemoverTrechoCommand;
			}
		}
		private void RemoverTrecho(trecho __trecho)
		{
			Debug.WriteLine("CESHPVM, RemoverTrecho");
			shp.trechos.Remove(__trecho);
		}
		private bool canRemoverTrecho(trecho __trecho)
		{
			if (__trecho is trecho) { return true; }
			return false;
		}
		#endregion
		//OK

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
		private void Aplicar()
		{
			Debug.WriteLine("CESHPVM, Aplicar");
			Console.WriteLine("Aplicar");
		}
		private bool canAplicar()
		{
			return true;
		}
		#endregion
		//adiado
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
		private void Mudar()
		{
			Debug.WriteLine("CESHPVM, Mudar");
			Console.WriteLine("Mudar");
		}
		private bool canMudar()
		{
			return true;
		}
		#endregion
		//adiado

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
		private void Reset()
		{
			Debug.WriteLine("CESHPVM, Reset");
			Console.WriteLine("Reset");
		}
		private bool canReset()
		{
			return true;
		}
		#endregion
		//fazer

		#region OrganizarCommand
		public ICommand OrganizarCommand
		{
			get
			{
				if (_OrganizarCommand == null)
				{
					_OrganizarCommand = new RelayCommand(p => Organizar(), p => canOrganizar());
				}
				return _OrganizarCommand;
			}
		}
		private void Organizar()
		{
			Debug.WriteLine("CESHPVM, Organizar");
			Console.WriteLine("Organizar");
		}
		private bool canOrganizar()
		{
			return true;
		}
		#endregion
		//fazer

		#region DragNDrop

		public void DragOver(IDropInfo dropInfo)
		{
			//Debug.WriteLine("CESHPVM, DragOver");
			if (dropInfo.Data != null && dropInfo.TargetItem != null && dropInfo.Data is trecho && dropInfo.TargetItem is trecho)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				dropInfo.Effects = DragDropEffects.Move;

			}
		}
		public void Drop(IDropInfo dropInfo)
		{
			Debug.WriteLine("CESHPVM, Drop");
			shp.IsEnabled = false;
			if (dropInfo.Data != null && dropInfo.TargetItem != null && dropInfo.Data is trecho && dropInfo.TargetItem is trecho)
			{
				dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				dropInfo.Effects = DragDropEffects.Move;
				trecho sourceItem = dropInfo.Data as trecho;
				trecho targetItem = dropInfo.TargetItem as trecho;
				int oldIndex = shp.trechos.IndexOf(sourceItem);
				int newIndex = shp.trechos.IndexOf(targetItem);
				if (sourceItem.isHidrante && newIndex < 1) { newIndex = 1; }
				shp.MoveTrechos(oldIndex, newIndex);

			}
			shp.IsEnabled = true;
		}
		#endregion
		//OK (funcionalidade simples)
	}
}
