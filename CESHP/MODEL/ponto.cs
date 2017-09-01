using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CESHP.MODEL
{
	[Serializable]
	public enum tipos_de_ponto { Nulo, Ponto, Bomba, Hidrante, Esguicho, Redutor_de_Pressao, Reservatorio };

	[Serializable]
	public class ponto : baseModel //, IEquatable<ponto>
	{
		#region DADOS
		private int _index;
		[Description("identificação do ponto")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		private string _nome;
		[Description("nome do ponto")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private shp _shpParent;
		public shp shpParent { get { return _shpParent; } set { _shpParent = value; OnPropertyChanged(); } }

		private tipos_de_ponto _tipo;
		[Description("tipo de ponto (Ponto, Bomba, Hidrante, Esguicho, Redutor_de_Pressao)")]
		public tipos_de_ponto tipo { get { return _tipo; } set { _tipo = value; OnPropertyChanged(); } }

		private ObservableCollection<trecho> _trechoComeca;
		[Description("peças no trecho")]
		public ObservableCollection<trecho> trechoComeca
		{
			get
			{
				if (_trechoComeca == null) { _trechoComeca = new ObservableCollection<trecho>(); }
				return _trechoComeca;
			}
			set
			{
				_trechoComeca = value;
				OnPropertyChanged();
			}
		}

		private trecho _trechoTermina;
		[Description("peças no trecho")]
		public trecho trechoTermina { get { return _trechoTermina; } set { _trechoTermina = value; OnPropertyChanged(); } }

		public bool isLast
		{
			get
			{
				if (tipo == tipos_de_ponto.Hidrante)
				{
					if (this == shpParent.pontosHidrantesOnly.LastOrDefault()) { return true; }
				}
				else
				{
					if (this == shpParent.pontosPontosOnly.LastOrDefault()) { return true; }
				}
				return false;
			}
		}
		public bool hasTrechoTermina { get { if (trechoTermina == null) { return false; } else { return true; } } }

		private float _pressao;
		[Description("pressão no ponto")]
		public float pressao { get { return _pressao; } set { _pressao = value; OnPropertyChanged(); } }

		private float _cota;
		[Description("cota do ponto")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float cota { get { return _cota; } set { _cota = value; OnPropertyChanged(); } }

		private float _vazaoCriada;
		[Description("vazão de saída no ponto")]
		public float vazaoCriada { get { return _vazaoCriada; } set { _vazaoCriada = value; OnPropertyChanged(); } }

		private float _vazaoAcumulada;
		[Description("vazão acumulada no ponto")]
		public float vazaoAcumulada { get { return _vazaoAcumulada; } set { _vazaoAcumulada = value; OnPropertyChanged(); } }

		private float _perdaDePressao;
		[Description("perda de pressão no ponto")]
		public float perdaDePressao { get { return _perdaDePressao; } set { _perdaDePressao = value; OnPropertyChanged(); } }
		#endregion

		public ponto(string __nome, shp __shpParent, tipos_de_ponto __tipo = tipos_de_ponto.Ponto)
		{
			Debug.WriteLine("ponto, ponto(nome,tipo)");
			shpParent = __shpParent;
			nome = __nome;
			tipo = __tipo;
			shpParent.pontos.Add(this);
			index = shpParent.pontos.IndexOf(this);
			shpParent.Refresh();
		}
		public static ponto Novo(shp __shpParent, tipos_de_ponto __tipo = tipos_de_ponto.Ponto)
		{
			Debug.WriteLine("ponto, Novo");
			if (__tipo == tipos_de_ponto.Ponto)
			{

				ponto ultimoPonto = __shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Ponto).LastOrDefault();
				if (ultimoPonto == null)
				{
					return __shpParent.PrimeiroPonto();
				}
				else
				{
					data.alfabeto.Proxima(ultimoPonto.nome);
					return new ponto(data.alfabeto.Proxima(ultimoPonto.nome), __shpParent, __tipo);
				}
			}
			else if (__tipo == tipos_de_ponto.Hidrante)
			{
				hidrante ultimoHidrante = __shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante).LastOrDefault() as hidrante;
				if (ultimoHidrante == null)
				{
					return __shpParent.PrimeiroHidrante();
				}
				else
				{
					string stringNumero = Regex.Replace(ultimoHidrante.nome, @"[^0-9]", "");
					int numero = int.Parse(stringNumero);
					return new hidrante(shp.letraHidrante + (numero + 1), __shpParent);
				}
			}
			else
			{
				Debug.WriteLine("ponto, novo, PROBLEMA NULL");
				return null;
			}
		}
		public ponto Proximo(bool __force = true)
		{
			Debug.WriteLine("ponto, Proximo");
			if (tipo == tipos_de_ponto.Ponto)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Ponto && shpParent.pontos.IndexOf(p) > shpParent.pontos.IndexOf(this)).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipo); }
				return pontoPosterior;
			}
			else if (tipo == tipos_de_ponto.Hidrante)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && shpParent.pontos.IndexOf(p) > shpParent.pontos.IndexOf(this)).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipo); }
				return pontoPosterior;
			}
			else if (tipo == tipos_de_ponto.Reservatorio)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Ponto).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipos_de_ponto.Ponto); }
				return pontoPosterior;
			}
			else
			{
				Debug.WriteLine("ponto, proximo, PROBLEMA NULL");
				return null;
			}
		}
		public ponto ProximoHidrante(bool __force = true)
		{
			Debug.WriteLine("ponto, ProximoHidrante");

			ponto pontoPosterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && shpParent.pontos.IndexOf(p) > shpParent.pontos.IndexOf(this)).FirstOrDefault();
			if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipos_de_ponto.Hidrante); }
			return pontoPosterior;
		}
		public ponto ProximoSemUso()
		{
			Debug.WriteLine("ponto, ProximoSemUso");
			if (tipo == tipos_de_ponto.Ponto)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => !p.hasTrechoTermina && p.tipo == tipos_de_ponto.Ponto && shpParent.pontos.IndexOf(p) > shpParent.pontos.IndexOf(this)).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipo); }
				return pontoPosterior;
			}
			else if (tipo == tipos_de_ponto.Hidrante)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => !p.hasTrechoTermina && p.tipo == tipos_de_ponto.Hidrante && shpParent.pontos.IndexOf(p) > shpParent.pontos.IndexOf(this)).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipo); }
				return pontoPosterior;
			}
			else if (tipo == tipos_de_ponto.Reservatorio)
			{
				ponto pontoPosterior = shpParent.pontos.Where(p => !p.hasTrechoTermina && p.tipo == tipos_de_ponto.Ponto).FirstOrDefault();
				if (pontoPosterior == null) { pontoPosterior = Novo(shpParent, tipo); }
				return pontoPosterior;
			}
			else
			{
				Debug.WriteLine("ponto, ProximoSemUso, PROBLEMA NULL");
				return null;
			}
		}
		public ponto Anterior(bool __force = true)
		{
			Debug.WriteLine("ponto, Anterior");
			if (tipo == tipos_de_ponto.Ponto)
			{
				ponto pontoAnterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Ponto && shpParent.pontos.IndexOf(p) < shpParent.pontos.IndexOf(this)).LastOrDefault();
				if (pontoAnterior == null) { pontoAnterior = shpParent.PrimeiroReservatorio(); }
				return pontoAnterior;
			}
			else if (tipo == tipos_de_ponto.Hidrante)
			{
				ponto pontoAnterior = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && shpParent.pontos.IndexOf(p) < shpParent.pontos.IndexOf(this)).LastOrDefault();
				if (pontoAnterior == null) { pontoAnterior = shpParent.PrimeiroHidrante(); }
				return pontoAnterior;
			}
			else
			{
				Debug.WriteLine("ponto, anterior, PROBLEMA NULL");
				return null;
			}
		}
	}

	[Serializable]
	public class nulo : ponto
	{
		public nulo(shp __shpParent) : base("--", __shpParent, tipos_de_ponto.Nulo) { Debug.WriteLine("nulo, nulo"); }
	}

	[Serializable]
	public class bomba : ponto
	{
		public bomba(string __nome, shp __shpParent) : base(__nome, __shpParent, tipos_de_ponto.Bomba) { Debug.WriteLine("bomba, bomba"); }
	}

	[Serializable]
	public class hidrante : ponto
	{
		public hidrante(string __nome, shp __shpParent) : base(__nome, __shpParent, tipos_de_ponto.Hidrante) { Debug.WriteLine("hidrante, hidrante"); }
		public int Numero()
		{
			string stringNumero = Regex.Replace(nome, @"[^0-9]", "");
			return int.Parse(stringNumero);
		}
	}

	[Serializable]
	public class esguicho : ponto
	{
		public esguicho(string __nome, shp __shpParent) : base(__nome, __shpParent, tipos_de_ponto.Esguicho) { Debug.WriteLine("esguicho, esguicho"); }
	}

	[Serializable]
	public class redutor : ponto
	{
		public redutor(string __nome, shp __shpParent) : base(__nome, __shpParent, tipos_de_ponto.Redutor_de_Pressao) { Debug.WriteLine("redutor, redutor"); }
	}

	[Serializable]
	public class reservatorio : ponto
	{
		public reservatorio(string __nome, shp __shpParent) : base(__nome, __shpParent, tipos_de_ponto.Reservatorio)
		{
			Debug.WriteLine("reservatorio, reservatorio");
			pressao = 0;
			vazaoCriada = 0;
			vazaoAcumulada = 0;
		}
	}
}
