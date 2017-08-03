using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CESHP.MODEL
{
	[Serializable]
	public enum tipos_de_ponto { Nulo, Ponto, Bomba, Hidrante, Esguicho, Redutor_de_Pressao, Reservatorio };

	[Serializable]
	public class ponto : baseModel //, IEquatable<ponto>
	{
		public ponto()
		{
			Debug.WriteLine("ponto, ponto");
			tipo = tipos_de_ponto.Ponto;
		}

		public ponto(string __nome, shp __shpParent, tipos_de_ponto __tipo = tipos_de_ponto.Ponto)
		{
			Debug.WriteLine("ponto, ponto(nome,tipo)");
			shpParent = __shpParent;
			nome = __nome;
			tipo = __tipo;
		}


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

		private ObservableCollection<trecho> _trecho_comeca;
		[Description("peças no trecho")]
		public ObservableCollection<trecho> trecho_comeca
		{
			get
			{
				if (_trecho_comeca == null)
				{
					_trecho_comeca = new ObservableCollection<trecho>();
				}
				return _trecho_comeca;
			}
			set
			{
				_trecho_comeca = value;
				OnPropertyChanged();
			}
		}

		private trecho _trecho_termina;
		[Description("peças no trecho")]
		public trecho trecho_termina { get { return _trecho_termina; } set { _trecho_termina = value; OnPropertyChanged(); } }

		private float _pressao;
		[Description("pressão no ponto")]
		public float pressao { get { return _pressao; } set { _pressao = value; OnPropertyChanged(); } }

		private float _cota;
		[Description("cota do ponto")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float cota { get { return _cota; } set { _cota = value; OnPropertyChanged(); } }

		private float _vazao_criada;
		[Description("vazão de saída no ponto")]
		public float vazao_criada { get { return _vazao_criada; } set { _vazao_criada = value; OnPropertyChanged(); } }

		private float _vazao_acumulada;
		[Description("vazão acumulada no ponto")]
		public float vazao_acumulada { get { return _vazao_acumulada; } set { _vazao_acumulada = value; OnPropertyChanged(); } }

		private float _perda_de_pressao;
		[Description("perda de pressão no ponto")]
		public float perda_de_pressao { get { return _perda_de_pressao; } set { _perda_de_pressao = value; OnPropertyChanged(); } }

		public bool Equals(ponto other)
		{
			Debug.WriteLine("ponto, Equals");
			Debug.WriteLine(other.trecho_comeca);
			if (other != null)
			{
				if (
				index.Equals(other.index) &&
				nome.Equals(other.nome) &&
				tipo.Equals(other.tipo) &&
				trecho_termina.Equals(other.trecho_termina) &&
				pressao.Equals(other.pressao) &&
				vazao_criada.Equals(other.vazao_criada) &&
				vazao_acumulada.Equals(other.vazao_acumulada) &&
				perda_de_pressao.Equals(other.perda_de_pressao)
				)
				{
					bool trecho_comecaOk = true;
					if (trecho_comeca.Count == other.trecho_comeca.Count)
					{
						for (int i = 0; i < trecho_comeca.Count; i++)
						{
							if (!trecho_comeca[i].Equals(other.trecho_comeca[i]))
							{
								trecho_comecaOk = false;
							}
						}
					}
					else
					{
						trecho_comecaOk = false;
					}
					if (trecho_comecaOk)
					{
						return true;
					}
				}
			}
			return false;
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
		public hidrante proximo(bool __novo = true)
		{
			Debug.WriteLine("hidrante, proximo");
			string stringNumero = Regex.Replace(nome, @"[^0-9]", "");
			int numero = int.Parse(stringNumero);
			hidrante proximoHidrante = shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante && p.nome == shp.letraHidrante + (numero + 1)).FirstOrDefault() as hidrante;
			if (__novo && proximoHidrante == null)
			{
				return novo(shpParent);
			}
			else
			{
				return proximoHidrante;
			}
		}
		public static hidrante novo(shp __shpParent)
		{
			Debug.WriteLine("hidrante, novo");
			hidrante ultimoHidrante = __shpParent.pontos.Where(p => p.tipo == tipos_de_ponto.Hidrante).LastOrDefault() as hidrante;
			string stringNumero = Regex.Replace(ultimoHidrante.nome, @"[^0-9]", "");
			int numero = int.Parse(stringNumero);
			return new hidrante(shp.letraHidrante + (numero + 1), __shpParent);
		}
		public int numero()
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
			index = 0;
			pressao = 0;
			vazao_criada = 0;
			vazao_acumulada = 0;

		}
	}
}
