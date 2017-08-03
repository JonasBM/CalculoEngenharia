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

namespace CESHP.MODEL
{
	[Serializable]
	public enum classe_de_risco { Leve, Medio, Elevado };

	[Serializable]
	public enum jato { Regulavel, Solido };

	[Serializable]
	public class shp : baseModel //, IEquatable<shp>
	{
		#region ARQUIVO
		private FileInfo _arquivo;
		public FileInfo arquivo { get { return _arquivo; } set { _arquivo = value; OnPropertyChanged(); } }



		#endregion

		public Visibility show_requinte
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
		public Visibility show_k
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

		private bool _IsEnabled = true;
		public bool IsEnabled { get { return _IsEnabled; } set { _IsEnabled = value; OnPropertyChanged(); } }

		public int minWidthPonto
		{
			get
			{
				return pontos.Max(p => p.nome.Length) * 6 + 25;
			}
		}

		public static string letraHidrante = "H";

		#region DADOS
		private string _obra;
		public string obra { get { return _obra; } set { _obra = value; OnPropertyChanged(); } }

		private string _observacao;
		public string observacao { get { return _observacao; } set { _observacao = value; OnPropertyChanged(); } }

		private jato _jato;
		public jato jato { get { return _jato; } set { _jato = value; OnPropertyChanged(); OnPropertyChanged("show_requinte"); OnPropertyChanged("show_k"); } }

		private int _numero_hidrantes;
		public int numero_hidrantes { get { return _numero_hidrantes; } set { _numero_hidrantes = value; OnPropertyChanged(); } }

		private float _vazao_minima;
		public float vazao_minima { get { return _vazao_minima; } set { _vazao_minima = value; OnPropertyChanged(); } }

		private float _pressao_minima;
		public float pressao_minima { get { return _pressao_minima; } set { _pressao_minima = value; OnPropertyChanged(); } }

		private float _requinte;
		public float requinte { get { return _requinte; } set { _requinte = value; OnPropertyChanged(); } }

		private float _k;
		public float k { get { return _k; } set { _k = value; OnPropertyChanged(); } }

		private classe_de_risco _tipo;
		[Description("classe de risco (Leve, Medio, Elevado)")]
		public classe_de_risco tipo { get { return _tipo; } set { _tipo = value; OnPropertyChanged(); } }
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

		private ObservableCollection<trecho> _trechos;
		[Description("trechos a calcular")]
		public ObservableCollection<trecho> trechos { get { return _trechos; } set { _trechos = value; OnPropertyChanged(); } }

		private trecho _trechosSelected;
		public trecho trechosSelected { get { return _trechosSelected; } set { _trechosSelected = value; OnPropertyChanged(); } }

		private ObservableCollection<ponto> _pontos;

		[Description("pontos a calcular")]
		public ObservableCollection<ponto> pontos { get { return _pontos; } set { _pontos = value; OnPropertyChanged(); } }

		public IEnumerable<ponto> pontosPontosOnly
		{
			get
			{
				return _pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante);
			}
		}

		public IEnumerable<ponto> pontosHidrantesOnly
		{
			get
			{
				return _pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante);
			}
		}

		public shp()
		{
			Debug.WriteLine("shp, shp");
			arquivo = null;
			tipo = classe_de_risco.Leve;
			pressao_minima = 4;
			requinte = 13;
			k = 0;

			pontos = new ObservableCollection<ponto>();
			trechos = new ObservableCollection<trecho>();
		}

		public shp(int __numero_hidrantes, float __altura_piso_a_piso)
		{
			IsEnabled = false;

			Debug.WriteLine("shp, shp(numero_hidrantes,altura_piso_a_piso)");
			arquivo = null;
			tipo = classe_de_risco.Leve;
			vazao_minima = 70;
			pressao_minima = 4;
			requinte = 13;
			jato = jato.Solido;
			k = 0;
			numero_hidrantes = __numero_hidrantes;

			pontos = new ObservableCollection<ponto>();
			trechos = new ObservableCollection<trecho>();

			ponto ponto1;
			ponto ponto2;
			ponto ponto3;
			trecho trecho1;
			trecho trecho2;

			ponto1 = new nulo(this);
			pontos.Add(ponto1);
			ponto1.index = pontos.IndexOf(ponto1);

			ponto1 = new reservatorio("Res.", this);
			pontos.Add(ponto1);
			ponto1.index = pontos.IndexOf(ponto1);

			for (int i = 0; i < __numero_hidrantes; i++)
			{
				ponto2 = new ponto(data.alfabeto.GetLetra(i), this);
				pontos.Add(ponto2);
				ponto2.index = pontos.IndexOf(ponto2);

				trecho1 = new trecho(pontos[ponto1.index], pontos[ponto2.index], __altura_piso_a_piso, -1 * __altura_piso_a_piso, this);
				trechos.Add(trecho1);
				trecho1.index = trechos.IndexOf(trecho1);

				ponto3 = new hidrante(letraHidrante + (i + 1), this);
				pontos.Add(ponto3);
				ponto3.index = pontos.IndexOf(ponto3);

				trecho2 = new trecho(pontos[ponto2.index], pontos[ponto3.index], (float)0.20, (float)0.00, this);
				trechos.Add(trecho2);
				trecho2.index = trechos.IndexOf(trecho2);

				ponto1 = ponto2;
			}

			ponto1 = new ponto(data.alfabeto.GetLetra(__numero_hidrantes), this);
			pontos.Add(ponto1);
			ponto1.index = pontos.IndexOf(ponto1);

			ponto1 = new hidrante(letraHidrante + (__numero_hidrantes + 1), this);
			pontos.Add(ponto1);
			ponto1.index = pontos.IndexOf(ponto1);

			IsEnabled = true;
		}

		public static shp novo()
		{
			Debug.WriteLine("shp, novo");
			NovoSHPDialogVM NovoSHPDialog = new NovoSHPDialogVM();
			NovoSHPDialog.NovoSHP();
			if (NovoSHPDialog.salvo)
			{
				return new shp(NovoSHPDialog.numero_hidrantes, NovoSHPDialog.altura_piso_a_piso);
			}
			else
			{
				return new shp(1, 0);
			}
		}



		public void moveTrechos(trecho __sourceTrecho, trecho __targetTrecho)
		{
			Debug.WriteLine("shp, moveTrechos(sourceTrecho,targetTrecho)");
			int oldIndex = trechos.IndexOf(__sourceTrecho);
			int newIndex = trechos.IndexOf(__targetTrecho);
			if (__sourceTrecho.fim != null)
			{
				//moveTrechos(trechos.IndexOf(__sourceTrecho), trechos.IndexOf(__targetTrecho));
				if (__sourceTrecho.fim.tipo == tipos_de_ponto.Hidrante && newIndex == 0) { newIndex = 1; }
				if (oldIndex != newIndex)
				{
					moveTrechos(oldIndex, newIndex);
				}
				for (int i = 0; i < trechos.Count; i++)
				{
					trechos[i].index = i;
				}
			}
			
		}

		public void moveTrechos(int __oldIndex, int __newIndex)
		{
			Debug.WriteLine("shp, moveTrechos(oldIndex,newIndex)");
			bool sobe = true;
			trecho trechoModificado = trechos[__newIndex];
			trecho[] entreTrechoOriginal;


			trechos.Move(__oldIndex, __newIndex);
			trechosSelected = trechos[__newIndex];

			if (__oldIndex > __newIndex)
			{
				sobe = true;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) <= __oldIndex && trechos.IndexOf(t) >= __newIndex).ToArray();
			}
			else
			{
				sobe = false;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) >= __oldIndex && trechos.IndexOf(t) <= __newIndex).ToArray();
			}

			hidrante ultimoHidrante = trechos.Where(t => t.isHidrante && trechos.IndexOf(t) < trechos.IndexOf(entreTrechoOriginal.FirstOrDefault())).FirstOrDefault().fim as hidrante;
			ponto ultimoPonto = trechos.Where(t => !t.isHidrante && trechos.IndexOf(t) < trechos.IndexOf(entreTrechoOriginal.FirstOrDefault())).FirstOrDefault().fim;

			if (!trechoModificado.isHidrante)
			{
				for (int i = 0; i < entreTrechoOriginal.Count(); i++)
				{
					
					if (!entreTrechoOriginal[i].isHidrante)
					{
						entreTrechoOriginal[i].inicio = trechos.Where(t => !t.isHidrante && trechos.IndexOf(t) < trechos.IndexOf(entreTrechoOriginal[i])).FirstOrDefault().fim;
						entreTrechoOriginal[i].fim = ultimoHidrante.proximo();
					}
				}
			}
		}

		public void OrganizaLetras(trecho __trechoModificado, int __oldIndexStarted)
		{
			Debug.WriteLine("shp, OrganizaLetras");
			bool sobe = true;
			trecho[] anteriorTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) < trechos.IndexOf(__trechoModificado)).ToArray();
			trecho[] posteriorTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) > trechos.IndexOf(__trechoModificado)).ToArray();
			trecho[] entreTrechoOriginal;
			if (__oldIndexStarted > trechos.IndexOf(__trechoModificado))
			{
				sobe = true;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) <= __oldIndexStarted && trechos.IndexOf(t) >= trechos.IndexOf(__trechoModificado)).ToArray();
			}
			else
			{
				sobe = false;
				entreTrechoOriginal = trechos.Where(t => trechos.IndexOf(t) >= __oldIndexStarted && trechos.IndexOf(t) <= trechos.IndexOf(__trechoModificado)).ToArray();
			}
			hidrante ultimoHidrante = trechos.Where(t => t.isHidrante && trechos.IndexOf(t) < trechos.IndexOf(entreTrechoOriginal.FirstOrDefault())).FirstOrDefault().fim as hidrante;
			ponto ultimoPonto = trechos.Where(t => !t.isHidrante && trechos.IndexOf(t) < trechos.IndexOf(entreTrechoOriginal.FirstOrDefault())).FirstOrDefault().fim;

			if (!__trechoModificado.isHidrante)
			{
				for (int i = 0; i < entreTrechoOriginal.Count(); i++)
				{
					entreTrechoOriginal[i].inicio = ultimoPonto;
					if (!entreTrechoOriginal[i].isHidrante)
					{
						entreTrechoOriginal[i].fim = ultimoHidrante.proximo();
					}
				}
			}


			return;




			__trechoModificado.inicio = anteriorTrechoOriginal.Where(t => t.fim.tipo != tipos_de_ponto.Hidrante).LastOrDefault().fim;

			//hidrante proximoHidrante = (anteriorTrechoOriginal.Where(t => t.fim.tipo != tipos_de_ponto.Hidrante).LastOrDefault().fim as hidrante).proximo();
			//ponto hidranteAtual = pontos.Where(t => t.fim.tipo != tipos_de_ponto.Hidrante).LastOrDefault().fim;

			if (!__trechoModificado.isHidrante)
			{
				if (posteriorTrechoOriginal.Where(t => t.fim.tipo != tipos_de_ponto.Hidrante).FirstOrDefault() != null)
				{
					//__trechoModificado.fim = posteriorTrechoOriginal.Where(t => t.fim.tipo != tipos_de_ponto.Hidrante).FirstOrDefault().fim;
				}
			}
			else
			{
				//__trechoModificado.fim = posteriorTrechoOriginal.Where(t => t.isHidrante).FirstOrDefault().fim;
			}

			if (__oldIndexStarted > trechos.IndexOf(__trechoModificado))
			{
				for (int i = 0; i < posteriorTrechoOriginal.Count(); i++)
				{
					if (trechos.IndexOf(posteriorTrechoOriginal[i]) <= __oldIndexStarted)
					{
						if (!__trechoModificado.isHidrante)
						{
							posteriorTrechoOriginal[i].inicio = pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante && pontos.IndexOf(p) > pontos.IndexOf(posteriorTrechoOriginal[i].inicio)).FirstOrDefault();
							if (!posteriorTrechoOriginal[i].isHidrante)
							{
								posteriorTrechoOriginal[i].fim = pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante && pontos.IndexOf(p) > pontos.IndexOf(posteriorTrechoOriginal[i].fim)).FirstOrDefault();
							}
						}
						else
						{
							if (posteriorTrechoOriginal[i].fim.tipo == tipos_de_ponto.Hidrante)
							{
								posteriorTrechoOriginal[i].fim = pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && pontos.IndexOf(p) > pontos.IndexOf(posteriorTrechoOriginal[i].fim)).FirstOrDefault();
							}
						}
					}
				}
			}

			if (__oldIndexStarted < trechos.IndexOf(__trechoModificado))
			{
				for (int i = 0; i < anteriorTrechoOriginal.Count(); i++)
				{
					if (trechos.IndexOf(anteriorTrechoOriginal[i]) >= __oldIndexStarted)
					{
						if (!__trechoModificado.isHidrante)
						{
							anteriorTrechoOriginal[i].inicio = pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante && pontos.IndexOf(p) < pontos.IndexOf(anteriorTrechoOriginal[i].inicio)).LastOrDefault();
							if (!anteriorTrechoOriginal[i].isHidrante)
							{
								anteriorTrechoOriginal[i].fim = pontos.Where(p => p.tipo != tipos_de_ponto.Hidrante && pontos.IndexOf(p) < pontos.IndexOf(anteriorTrechoOriginal[i].fim)).LastOrDefault();
							}
						}
						else
						{
							if (anteriorTrechoOriginal[i].fim.tipo == tipos_de_ponto.Hidrante)
							{
								anteriorTrechoOriginal[i].fim = pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && pontos.IndexOf(p) < pontos.IndexOf(anteriorTrechoOriginal[i].fim)).LastOrDefault();
							}
						}
					}
				}
			}

		}

		public void OrganizaTrechos()
		{
			Debug.WriteLine("shp, OrganizaTrechos");
			int index = 0;
			int newIndex = 0;

			index = trechos.IndexOf(trechos.Where(t => t.inicio.tipo == tipos_de_ponto.Reservatorio).FirstOrDefault());
			trechos.Move(index, newIndex); newIndex++;

			for (int i = 0; i < pontos.Count; i++)
			{
				if (pontos[i].tipo == tipos_de_ponto.Ponto)
				{
					Console.WriteLine(pontos[i].trecho_comeca.Count);
					for (int j = 0; j < pontos[i].trecho_comeca.Count; j++)
					{
						Console.WriteLine(pontos[i].nome + " -- " + pontos[i].trecho_comeca[j].fim.nome);
						index = trechos.IndexOf(pontos[i].trecho_comeca[j]);
						trechos.Move(index, newIndex); newIndex++;
					}
				}

			}
		}

		public bool Equals(shp other)
		{
			Debug.WriteLine("shp, Equals");
			if (other != null)
			{
				if (
				obra.Equals(other.obra) &&
				observacao.Equals(other.observacao) &&
				jato.Equals(other.jato) &&
				numero_hidrantes.Equals(other.numero_hidrantes) &&
				vazao_minima.Equals(other.vazao_minima) &&
				pressao_minima.Equals(other.pressao_minima) &&
				requinte.Equals(other.requinte) &&
				k.Equals(other.k) &&
				tipo.Equals(other.tipo) &&
				trechos.Equals(other.trechos) &&
				pontos.Equals(other.pontos)
				)
				{
					bool trechosOk = true;
					if (trechos.Count == other.trechos.Count)
					{
						for (int i = 0; i < trechos.Count; i++)
						{
							if (!trechos[i].Equals(other.trechos[i]))
							{
								trechosOk = false;
							}
						}
					}
					else
					{
						trechosOk = false;
					}

					bool pontosOk = true;
					if (pontos.Count == other.pontos.Count)
					{
						for (int i = 0; i < pontos.Count; i++)
						{
							if (!pontos[i].Equals(other.pontos[i]))
							{
								pontosOk = false;
							}
						}
					}
					else
					{
						pontosOk = false;
					}

					if (trechosOk && pontosOk)
					{
						return true;
					}
				}
				//object.ReferenceEquals(this.observacao, other.observacao) ||
				//this.observacao != null &&
				//this.observacao.Equals(other.observacao)
			}
			return false;

		}
	}
}
