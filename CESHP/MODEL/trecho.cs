using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CESHP.MODEL
{
	[Serializable]
	public class trecho : baseModel //, IEquatable<trecho>
	{
		#region VISIBILIDADE
		public Visibility showHidrante
		{
			get
			{
				if (isHidrante)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}

		public Visibility showNotHidrante
		{
			get
			{
				if (!isHidrante)
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

		#region DADOS
		private int _index;
		[Description("identificação do trecho")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		[Description("nome do trecho")]
		public string nome { get { return inicio.nome + "-" + fim.nome; } }

		private shp _shpParent;
		public shp shpParent { get { return _shpParent; } set { _shpParent = value; OnPropertyChanged(); } }

		private ponto _inicio;
		[Description("ponto inicial do trecho")]
		public ponto inicio
		{
			get { return _inicio; }
			set
			{
				if (value != null)
				{
					if (_inicio != null) { if (_inicio.trechoComeca.Contains(this)) { _inicio.trechoComeca.Remove(this); } }
					_inicio = value;
					if (_inicio != null) { _inicio.trechoComeca.Add(this); }
					if (_inicio.isLast) { ponto.Novo(shpParent, _inicio.tipo); }
					OnPropertyChanged();
				}
			}
		}
		public bool isHidrante
		{
			get
			{
				if (fim != null) { if (fim is hidrante) { return true; } }
				return false;
			}
			set
			{
				if (value) { fim = HidranteAtual(true); } else { fim = PontoPosterior(true); }
			}
		}
		private ponto _fim;
		[Description("ponto final do trecho")]
		public ponto fim
		{
			get { return _fim; }
			set
			{
				if (value != null)
				{
					if (_fim != null) { _fim.trechoTermina = null; }
					if (value.trechoTermina != null) { value.trechoTermina.SetFimSemCadeia(_fim, value.trechoTermina); }
					_fim = value;
					value.trechoTermina = this;
					if (_fim.isLast) { ponto.Novo(shpParent, _fim.tipo); }
					OnPropertyChanged();
					OnPropertyChanged("isHidrante");
					OnPropertyChanged("showHidrante");
					OnPropertyChanged("showNotHidrante");
				}
			}
		}
		public void SetFimSemCadeia(ponto __fim, trecho __trechoTermina)
		{
			_fim = __fim;
			if (_fim != null) { _fim.trechoTermina = __trechoTermina; }
			OnPropertyChanged("fim");
			OnPropertyChanged("isHidrante");
			OnPropertyChanged("showHidrante");
			OnPropertyChanged("showNotHidrante");
		}

		private int _diametroIndex;
		[Description("diametro em mm")]
		public int diametroIndex { get { return _diametroIndex; } set { _diametroIndex = value; OnPropertyChanged(); } }

		private int _materialIndex;
		[Description("material")]
		public int materialIndex
		{
			get { return _materialIndex; }
			set
			{
				if (value != _materialIndex)
				{
					_materialIndex = value;
					OnPropertyChanged();
					OnPropertyChanged("material");
					diametroIndex = data.materiais[materialIndex].diametro_minimo_index;
				}
			}
		}

		public material material { get { return data.materiais[materialIndex]; } }

		private float _comprimento;
		[Description("comprimento do trecho")]
		public float comprimento { get { return _comprimento; } set { _comprimento = value; OnPropertyChanged(); } }

		private float _comprimentoHorizontal;
		[Description("comprimento horizontal do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float comprimentoHorizontal { get { return _comprimentoHorizontal; } set { _comprimentoHorizontal = value; OnPropertyChanged(); } }

		private float _comprimentoVertical;
		[Description("comprimento vertical do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float comprimentoVertical { get { return _comprimentoVertical; } set { _comprimentoVertical = value; OnPropertyChanged(); } }

		private float _desnivel;
		[Description("desnivel do trecho")]
		public float desnivel { get { return _desnivel; } set { _desnivel = value; OnPropertyChanged(); } }

		private int _MaterialMangueiraIndex;
		[Description("desnivel do trecho")]
		public int MaterialMangueiraIndex { get { return _MaterialMangueiraIndex; } set { _MaterialMangueiraIndex = value; OnPropertyChanged(); } }

		public mangueira MaterialMangueira { get { return data.mangueiras[MaterialMangueiraIndex]; } }

		private int _comprimentoDaMangueira;
		[Description("comprimento da mangueira")]
		public int comprimentoDaMangueira { get { return _comprimentoDaMangueira; } set { _comprimentoDaMangueira = value; OnPropertyChanged(); } }

		private float _desnivelDaMangueira;
		[Description("desnivel da mangueira")]
		public float desnivelDaMangueira { get { return _desnivelDaMangueira; } set { _desnivelDaMangueira = value; OnPropertyChanged(); } }

		private float _vazao;
		[Description("vazão de saída no ponto")]
		public float vazao { get { return _vazao; } set { _vazao = value; OnPropertyChanged(); } }

		private ObservableCollection<int> _pecasIndexes;
		[Description("peças no trecho")]
		public ObservableCollection<int> pecasIndexes
		{
			get
			{
				if (_pecasIndexes == null) { _pecasIndexes = new ObservableCollection<int>(); }
				return _pecasIndexes;
			}
			set
			{
				_pecasIndexes = value;
				OnPropertyChanged();
			}
		}

		public float comprimentoEquivalente
		{
			get
			{
				float total = 0;
				for (int i = 0; i < pecasIndexes.Count; i++)
				{
					total += (data.materiais[materialIndex].pecas[pecasIndexes[i]]).comprimentos_equivalentes.Where(ce => ce.diametro.diametro_nominal == data.materiais[materialIndex].diametros[diametroIndex].diametro_nominal).FirstOrDefault().perda;
				}
				return total;
			}
		}

		public float j
		{
			get
			{
				float j = 0;
				j = (float)((10.65 * Math.Pow(vazao, 1.852)) / (Math.Pow(data.materiais[materialIndex].crt, 1.852) * Math.Pow(data.materiais[materialIndex].diametros[diametroIndex].diametro_nominal_M, 4.87)));
				return j;
			}
		}

		private float _cota;
		[Description("cota do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float cota { get { return _cota; } set { _cota = value; OnPropertyChanged(); } }

		public float comprimentoTotal { get { return comprimento + comprimentoEquivalente; } }

		#endregion
		public trecho(ponto __inicio, ponto __fim, float __comprimento, float __desnivel, shp __shpParent)
		{
			Debug.WriteLine("trecho, trecho");
			shpParent = __shpParent;
			inicio = __inicio;
			fim = __fim;
			comprimento = __comprimento;
			desnivel = __desnivel;
			materialIndex = data.materiaisDefaultIndex;
			diametroIndex = data.materiais[materialIndex].diametro_minimo_index;
			MaterialMangueiraIndex = data.mangueirasDefaultIndex;
			comprimentoDaMangueira = 30;
			desnivelDaMangueira = (float)0.00;
			vazao = 0;
			shpParent.trechos.Add(this);
			index = shpParent.trechos.IndexOf(this);
			shpParent.Refresh();
		}
		public ponto PontoAnterior()
		{
			Debug.WriteLine("trecho, PontoAnterior");
			trecho trechoAnterior = shpParent.trechos.Where(t => !t.isHidrante && shpParent.trechos.IndexOf(t) < shpParent.trechos.IndexOf(this)).LastOrDefault();
			if (trechoAnterior == null)
			{
				return shpParent.PrimeiroReservatorio();
			}
			else
			{
				return trechoAnterior.fim;
			}
		}
		public ponto PontoPosterior(bool __semUSo = false)
		{
			Debug.WriteLine("trecho, PontoPosterior");
			trecho trechoPosterior = shpParent.trechos.Where(t => !t.isHidrante && shpParent.trechos.IndexOf(t) > shpParent.trechos.IndexOf(this)).FirstOrDefault();
			if (trechoPosterior == null)
			{
				return inicio.Proximo();
			}
			else
			{
				if (__semUSo)
				{
					return inicio.ProximoSemUso();
				}
				else
				{
					return trechoPosterior.inicio;
				}
			}
		}
		public hidrante HidranteAnterior()
		{
			Debug.WriteLine("trecho, HidranteAnterior");
			trecho trechoHidranteAnterior = shpParent.trechos.Where(t => t.isHidrante && shpParent.trechos.IndexOf(t) < shpParent.trechos.IndexOf(this)).LastOrDefault();
			if (trechoHidranteAnterior == null)
			{
				return null;
			}
			else
			{
				return trechoHidranteAnterior.fim as hidrante;
			}
		}
		public hidrante HidrantePosterior()
		{
			Debug.WriteLine("trecho, HidrantePosterior");
			trecho trechoHidrantePosterior = shpParent.trechos.Where(t => t.isHidrante && shpParent.trechos.IndexOf(t) > shpParent.trechos.IndexOf(this)).FirstOrDefault();
			if (trechoHidrantePosterior == null)
			{
				return fim.Proximo() as hidrante;
			}
			else
			{
				return trechoHidrantePosterior.fim as hidrante;
			}
		}
		public hidrante HidranteAtual(bool __semUSo = false)
		{
			Debug.WriteLine("trecho, HidranteAtual");
			hidrante pontoHidrante = HidranteAnterior();
			if (pontoHidrante == null)
			{
				return shpParent.PrimeiroHidrante();
			}
			else
			{
				if (__semUSo)
				{
					return pontoHidrante.ProximoSemUso() as hidrante;
				}
				else
				{
					return pontoHidrante.Proximo() as hidrante;
				}
			}
		}
	}
}
