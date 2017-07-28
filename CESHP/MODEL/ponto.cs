using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

		public ponto(string __nome)
		{
			Debug.WriteLine("ponto, ponto(nome)");
			nome = __nome;
			tipo = tipos_de_ponto.Ponto;
		}


		private int _index;
		[Description("identificação do ponto")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		private string _nome;
		[Description("nome do ponto")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

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

		public static ponto ProximoHidrante(ObservableCollection<ponto> __pontos, ponto __atual)
		{
			//__pontos.Where.FirstOrDefault();
			return null;
		}
	}

	[Serializable]
	public class nulo : ponto
	{
		public nulo()
		{
			nome = "--";
			tipo = tipos_de_ponto.Nulo;
		}
	}

	[Serializable]
	public class bomba : ponto
	{
		public bomba()
		{
			tipo = tipos_de_ponto.Bomba;
		}
	}

	[Serializable]
	public class hidrante : ponto
	{
		public hidrante(string __nome)
		{
			nome = __nome;
			tipo = tipos_de_ponto.Hidrante;
		}
	}

	[Serializable]
	public class esguicho : ponto
	{
		public esguicho()
		{
			tipo = tipos_de_ponto.Esguicho;
		}
	}

	[Serializable]
	public class redutor : esguicho
	{
		public redutor()
		{
			tipo = tipos_de_ponto.Redutor_de_Pressao;
		}
	}

	[Serializable]
	public class reservatorio : ponto
	{
		public reservatorio()
		{
			index = 0;
			nome = "Res.";
			pressao = 0;
			vazao_criada = 0;
			tipo = tipos_de_ponto.Reservatorio;
			vazao_acumulada = 0;
		}
	}
}
