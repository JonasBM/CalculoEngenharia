using CESHP.VIEW;
using CESHP.VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CESHP.MODEL
{
	[Serializable]
	public enum classe_de_risco { Leve, Medio, Elevado };

	[Serializable]
	public enum jato { Regulavel, Solido };

	[Serializable]
	public class shp : CEBiblioteca.baseModel //, IEquatable<shp>
	{
		#region ARQUIVO
		private FileInfo _arquivo;
		public FileInfo arquivo { get { return _arquivo; } set { _arquivo = value; OnPropertyChanged(); } }
		#endregion

		#region VISIBILIDADE
		public Visibility showRequinte
		{
			get
			{
				if (jato == jato.Solido)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}

			}
		}
		public Visibility showK
		{
			get
			{
				if (jato == jato.Regulavel)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}
		public int minWidthPonto
		{
			get
			{
				return pontos.Max(p => p.nome.Length) * 6 + 25;
			}
		}
		#endregion

		#region DADOS
		private string _obra;
		public string obra { get { return _obra; } set { _obra = value; OnPropertyChanged(); } }

		private string _observacao;
		public string observacao { get { return _observacao; } set { _observacao = value; OnPropertyChanged(); } }

		private jato _jato;
		public jato jato { get { return _jato; } set { _jato = value; OnPropertyChanged(); OnPropertyChanged("showRequinte"); OnPropertyChanged("showK"); } }

		private int _numeroHidrantes;
		public int numeroHidrantes { get { return _numeroHidrantes; } set { _numeroHidrantes = value; OnPropertyChanged(); } }

		private float _vazaoMinima;
		public float vazaoMinima { get { return _vazaoMinima; } set { _vazaoMinima = value; OnPropertyChanged(); } }

		private float _pressaoMinima;
		public float pressaoMinima { get { return _pressaoMinima; } set { _pressaoMinima = value; OnPropertyChanged(); } }

		private float _requinte;
		public float requinte { get { return _requinte; } set { _requinte = value; OnPropertyChanged(); } }

		private float _k;
		public float k { get { return _k; } set { _k = value; OnPropertyChanged(); } }

		private classe_de_risco _tipo;
		[Description("classe de risco (Leve, Medio, Elevado)")]
		public classe_de_risco tipo { get { return _tipo; } set { _tipo = value; OnPropertyChanged(); } }

		public static string letraHidrante = "H";

		private bool _IsEnabled = true;
		public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; OnPropertyChanged(); } }
		#endregion

		#region HIDRANTES A CALCULAR
		private hidrante _h01;
		public hidrante h01 { get { return _h01; } set { _h01 = value; OnPropertyChanged(); } }

		private hidrante _h02;
		public hidrante h02 { get { return _h02; } set { _h02 = value; OnPropertyChanged(); } }

		private hidrante _h03;
		public hidrante h03 { get { return _h03; } set { _h03 = value; OnPropertyChanged(); } }

		private hidrante _h04;
		public hidrante h04 { get { return _h04; } set { _h04 = value; OnPropertyChanged(); } }
		#endregion

		#region COLECOES
		private ObservableCollection<trecho> _trechos;
		[Description("trechos a calcular")]
		public ObservableCollection<trecho> trechos { get { return _trechos; } set { _trechos = value; OnPropertyChanged(); } }

		//public ObservableCollection<trecho> trechosPontosOnly { get { return _trechos.Where; } }

		private trecho _trechosSelected;
		public trecho trechosSelected { get { return _trechosSelected; } set { _trechosSelected = value; OnPropertyChanged(); } }

		private ObservableCollection<ponto> _pontos;

		[Description("pontos a calcular")]
		public ObservableCollection<ponto> pontos
		{
			get { return _pontos; }
			set
			{
				_pontos = value;
				OnPropertyChanged();
				OnPropertyChanged("pontosPontosOnly");
				OnPropertyChanged("pontosHidrantesOnly");
				OnPropertyChanged("pontosHidrantesEmUsoOnly");
			}
		}
		public List<ponto> pontosPontosOnly { get { return _pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante).ToList(); } }
		public List<ponto> pontosPontosOnlySemUso { get { return _pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante && p.trechoTermina == null).ToList(); } }
		public List<ponto> pontosHidrantesOnly { get { return _pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante).ToList(); } }
		public List<ponto> pontosHidrantesOnlyEmUso { get { return _pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && p.trechoTermina != null).ToList(); } }
		public List<ponto> pontosHidrantesOnlySemUso { get { return _pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && p.trechoTermina == null).ToList(); } }
		#endregion

		public shp()
		{
			DebugAlert();
			arquivo = null;
			tipo = classe_de_risco.Leve;
			pressaoMinima = 4;
			requinte = 13;
			k = 0;

			pontos = new ObservableCollection<ponto>();
			trechos = new ObservableCollection<trecho>();
		}
		public shp(int __numeroHidrantes, float __alturaPisoAPiso)
		{
			DebugAlert();
			IsEnabled = false;
			arquivo = null;
			tipo = classe_de_risco.Leve;
			vazaoMinima = 70;
			pressaoMinima = 4;
			requinte = 13;
			jato = jato.Solido;
			k = 0;
			numeroHidrantes = __numeroHidrantes;
			pontos = new ObservableCollection<ponto>();
			trechos = new ObservableCollection<trecho>();
			new nulo(this);
			ponto ponto1 = PrimeiroReservatorio();
			ponto ponto2 = PrimeiroPonto();
			ponto ponto3 = PrimeiroHidrante();
			trecho trecho1 = null;
			trecho trecho2 = null;
			for (int i = 0; i < __numeroHidrantes; i++)
			{
				trecho1 = new trecho(ponto1, ponto2, __alturaPisoAPiso, -1 * __alturaPisoAPiso, this);
				trecho2 = new trecho(ponto2, ponto3, (float)0.20, (float)0.00, this);
				ponto1 = ponto2;
				ponto2 = ponto2.Proximo();
				ponto3 = ponto3.Proximo();
			}
			IsEnabled = true;
		}
		public void Refresh() {
			DebugAlert();
			CollectionViewSource.GetDefaultView(trechos).Refresh();
		}
		public reservatorio PrimeiroReservatorio()
		{
			DebugAlert();
			reservatorio pontoReservatorio = pontos.Where(p => p.tipo == tipos_de_ponto.Reservatorio).FirstOrDefault() as reservatorio;
			if (pontoReservatorio == null) { pontoReservatorio = new reservatorio("Res.", this); }
			return pontoReservatorio;
		}
		public ponto PrimeiroPonto()
		{
			DebugAlert();
			ponto pontoPonto = pontos.Where(p => p.tipo == tipos_de_ponto.Ponto).FirstOrDefault();
			if (pontoPonto == null) { pontoPonto = new ponto(data.alfabeto.Primeira(), this); }
			return pontoPonto;
		}
		public hidrante PrimeiroHidrante()
		{
			DebugAlert();
			hidrante pontoHidrante = pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante).FirstOrDefault() as hidrante;
			if (pontoHidrante == null) { pontoHidrante = new hidrante(letraHidrante + "1", this); }
			return pontoHidrante;
		}
		public hidrante PrimeiroHidranteEmUso()
		{
			DebugAlert();
			return pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && p.trechoTermina != null).FirstOrDefault() as hidrante;
		}
		public static shp Novo()
		{
			DebugAlert();
			NovoSHPDialogVM NovoSHPDialog = new NovoSHPDialogVM();
			NovoSHPDialog.NovoSHP();
			if (NovoSHPDialog.salvo)
			{
				return new shp(NovoSHPDialog.numeroHidrantes, NovoSHPDialog.alturaPisoAPiso);
			}
			else
			{
				return null;
			}
		}
		public void MoveTrechos(int __oldIndex, int __newIndex)
		{
			DebugAlert();
			trechos.Move(__oldIndex, __newIndex);
			DebugAlertMessage("oldIndex, newIndex: "+ __oldIndex + " to "+ __newIndex);
			trechosSelected = trechos[__newIndex];
			for (int i = 0; i < trechos.Count(); i++)
			{
				trechos[i].index = trechos.IndexOf(trechos[i]);
			}
			//OrganizaLetras(trechos[__newIndex], __oldIndex);
		}

		#region ORGANIZACAO OBSOLETE
		[ObsoleteAttribute("Não funciona.", true)]
		public void OrganizaHidrantes(trecho __trechoModificado, int __oldIndexStarted)
		{
			DebugAlert();
			for (int i = 0; i < trechos.Count(); i++)
			{
				if (trechos[i].fim.trechoTermina != null)
				{
					Console.WriteLine("TRECHO:{0}", trechos[i].fim.trechoTermina.nome);
				}
				else
				{
					Console.WriteLine("NULL:{0}", trechos[i].fim.nome);
				}
			}
			bool sobe = true;
			trecho[] entreTrechoOriginal;
			if (__oldIndexStarted > trechos.IndexOf(__trechoModificado))
			{
				sobe = true;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) <= __oldIndexStarted && trechos.IndexOf(t) > trechos.IndexOf(__trechoModificado)).ToArray();
			}
			else
			{
				sobe = false;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) >= __oldIndexStarted && trechos.IndexOf(t) < trechos.IndexOf(__trechoModificado)).ToArray();
			}
			for (int i = 0; i < trechos.Count(); i++)
			{
				if (trechos[i].isHidrante)
				{
					if (trechos[i] == __trechoModificado)
					{
						trechos[i].SetFimSemCadeia(trechos[i].HidranteAtual(), trechos[i]);
						//trechos[i].fim = trechos[i].HidranteAtual();
					}
					else
					{
						hidrante hidranteAtual = trechos[i].HidranteAtual();
						int numeroHidranteAtual = hidranteAtual.Numero();
						int numero = (trechos[i].fim as hidrante).Numero();
						if (entreTrechoOriginal.Contains(trechos[i]))
						{
							if ((sobe && numeroHidranteAtual > numero) || (!sobe && numeroHidranteAtual < numero))
							{
								trechos[i].SetFimSemCadeia(hidranteAtual, trechos[i]);
								//trechos[i].fim = hidranteAtual;
							}
						}
						else
						{
							if (numeroHidranteAtual > numero)
							{
								trechos[i].SetFimSemCadeia(hidranteAtual, trechos[i]);
								//trechos[i].fim = hidranteAtual;
							}
						}
					}
				}
			}
			for (int i = 0; i < trechos.Count(); i++)
			{
				if (trechos[i].fim.trechoTermina != null)
				{
					Console.WriteLine("TRECHO:{0}", trechos[i].fim.trechoTermina.nome);
				}
				else
				{
					Console.WriteLine("NULL:{0}", trechos[i].fim.nome);
				}
			}
		}
		[ObsoleteAttribute("Não funciona.", true)]
		public void OrganizaLetras(trecho __trechoModificado, int __oldIndexStarted)
		{
			DebugAlert();
			OrganizaHidrantes(__trechoModificado, __oldIndexStarted);
			ponto pontoAtual = PrimeiroReservatorio();
			int trechoAcumulado = 0;
			return;
			for (int i = 0; i < pontosHidrantesOnlyEmUso.Count; i++)
			{
				//hidrante hidranteAtual = PrimeiroHidranteEmUso();
				hidrante hidranteAtual = pontosHidrantesOnlyEmUso[i] as hidrante;
				int numero = trechos.IndexOf(hidranteAtual.trechoTermina);
				Console.WriteLine("Hid:{0};Acu:{1};Num:{2}", hidranteAtual.nome, trechoAcumulado, numero);
				for (int j = trechoAcumulado; j < numero; j++)
				{
					Console.WriteLine(trechos[j].nome);
					int trechoIndex = j;
					trechos[trechoIndex].inicio = pontoAtual;
					pontoAtual = trechos[trechoIndex].inicio.Proximo();
					trechos[trechoIndex].fim = pontoAtual;/////////////////////////
				}
				hidranteAtual.trechoTermina.inicio = pontoAtual;
				trechoAcumulado = numero + 1;
			}
		}
		[ObsoleteAttribute("Não funciona.", true)]
		public void OrganizaLetrasOBSOLETE(trecho __trechoModificado, int __oldIndexStarted)
		{
			DebugAlert();
			bool sobe = true;
			trecho[] entreTrechoOriginal;
			if (__oldIndexStarted > trechos.IndexOf(__trechoModificado))
			{
				sobe = true;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) <= __oldIndexStarted && trechos.IndexOf(t) > trechos.IndexOf(__trechoModificado)).ToArray();
			}
			else
			{
				sobe = false;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) >= __oldIndexStarted && trechos.IndexOf(t) < trechos.IndexOf(__trechoModificado)).ToArray();
			}
			for (int i = 0; i < entreTrechoOriginal.Count(); i++)
			{
				ponto pontoInicio;
				if (sobe) { pontoInicio = entreTrechoOriginal[i].inicio.Proximo(false); } else { pontoInicio = entreTrechoOriginal[i].inicio.Anterior(false); }

				if (pontoInicio == entreTrechoOriginal[i].PontoAnterior())
				{
					Console.WriteLine("pontoInicio");
					entreTrechoOriginal[i].inicio = pontoInicio;
				}
				if (!__trechoModificado.isHidrante)
				{
					if (!entreTrechoOriginal[i].isHidrante)
					{
						ponto pontoFim;
						if (sobe) { pontoFim = entreTrechoOriginal[i].fim.Proximo(false); } else { pontoFim = entreTrechoOriginal[i].fim.Anterior(false); }
						if (pontoFim == entreTrechoOriginal[i].PontoPosterior())
						{
							Console.WriteLine("pontoFim");
							entreTrechoOriginal[i].fim = pontoFim;
						}
					}
				}
				else
				{
					if (entreTrechoOriginal[i].isHidrante)
					{
						ponto pontoHidrante;
						if (sobe) { pontoHidrante = entreTrechoOriginal[i].fim.Proximo(false); } else { pontoHidrante = entreTrechoOriginal[i].fim.Anterior(false); }

						Console.WriteLine(pontoHidrante.nome);
						Console.WriteLine(entreTrechoOriginal[i].HidrantePosterior().nome);

						if (pontoHidrante == entreTrechoOriginal[i].HidranteAtual())
						{
							Console.WriteLine("pontoHidrante");
							entreTrechoOriginal[i].fim = pontoHidrante;
						}
					}
				}
			}
			__trechoModificado.inicio = __trechoModificado.PontoAnterior();
			if (!__trechoModificado.isHidrante)
			{
				__trechoModificado.fim = __trechoModificado.PontoPosterior();
			}
			else
			{
				__trechoModificado.fim = __trechoModificado.HidranteAtual();
			}
		}
		[ObsoleteAttribute("Não funciona.", true)]
		public void OrganizaTrechos()
		{
			DebugAlert();
			return;
			int index = 0;
			int newIndex = 0;

			index = trechos.IndexOf(trechos.Where(t => t.inicio.tipo == tipos_de_ponto.Reservatorio).FirstOrDefault());
			trechos.Move(index, newIndex); newIndex++;

			for (int i = 0; i < pontos.Count; i++)
			{
				if (pontos[i].tipo == tipos_de_ponto.Ponto)
				{
					Console.WriteLine(pontos[i].trechoComeca.Count);
					for (int j = 0; j < pontos[i].trechoComeca.Count; j++)
					{
						Console.WriteLine(pontos[i].nome + " -- " + pontos[i].trechoComeca[j].fim.nome);
						index = trechos.IndexOf(pontos[i].trechoComeca[j]);
						trechos.Move(index, newIndex); newIndex++;
					}
				}

			}
		}
		#endregion
	}
}
