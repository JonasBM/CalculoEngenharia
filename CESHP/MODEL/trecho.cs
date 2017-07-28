using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CESHP.MODEL
{
	[Serializable]
	public class trecho : baseModel //, IEquatable<trecho>
	{
		public Visibility show_hidrante
		{
			get
			{
				if (fim is hidrante)
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}
		}

		private int _index;
		[Description("identificação do trecho")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		[Description("nome do trecho")]
		public string nome { get { return inicio.nome + "-" + fim.nome; } }

		private shp _shpParent;
		[Description("diametro em mm")]
		public shp shpParent { get { return _shpParent; } set { _shpParent = value; OnPropertyChanged(); } }

		private ponto _inicio;
		[Description("ponto inicial do trecho")]
		public ponto inicio
		{
			get { return _inicio; }
			set
			{
				if (_inicio != null)
				{
					if (_inicio.trecho_comeca.Contains(this))
					{
						_inicio.trecho_comeca.Remove(this);
					}
				}
				_inicio = value;
				if (_inicio != null)
				{
					_inicio.trecho_comeca.Add(this);
				}
				OnPropertyChanged();
				if (shpParent.IsEnabled)
				{
					shpParent.IsEnabled = false;
					shpParent.OrganizaTrechos();
					shpParent.IsEnabled = true;
				}
			}
		}

		private ponto _fim;
		[Description("ponto final do trecho")]
		public ponto fim
		{
			get { return _fim; }
			set
			{
				if (_fim != null)
				{
					if (_fim.trecho_termina != null)
					{
						_fim.trecho_termina = null;
					}
					_fim.trecho_termina = this;
				}
				_fim = value;
				OnPropertyChanged();
				OnPropertyChanged("show_hidrante");
			}
		}

		private int _diametro_index;
		[Description("diametro em mm")]
		public int diametro_index { get { return _diametro_index; } set { _diametro_index = value; OnPropertyChanged(); } }

		private int _material_index;
		[Description("material")]
		public int material_index
		{
			get { return _material_index; }
			set
			{
				if (value != _material_index)
				{
					_material_index = value;
					OnPropertyChanged();
					OnPropertyChanged("material");
					diametro_index = data.materiais[material_index].diametro_minimo_index;
				}
			}
		}

		public material material { get { return data.materiais[material_index]; } }

		private float _comprimento;
		[Description("comprimento do trecho")]
		public float comprimento { get { return _comprimento; } set { _comprimento = value; OnPropertyChanged(); } }

		private float _comprimento_horizontal;
		[Description("comprimento horizontal do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float comprimento_horizontal { get { return _comprimento_horizontal; } set { _comprimento_horizontal = value; OnPropertyChanged(); } }

		private float _comprimento_vertical;
		[Description("comprimento vertical do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float comprimento_vertical { get { return _comprimento_vertical; } set { _comprimento_vertical = value; OnPropertyChanged(); } }

		private float _desnivel;
		[Description("desnivel do trecho")]
		public float desnivel { get { return _desnivel; } set { _desnivel = value; OnPropertyChanged(); } }

		private float _vazao;
		[Description("vazão de saída no ponto")]
		public float vazao { get { return _vazao; } set { _vazao = value; OnPropertyChanged(); } }

		private ObservableCollection<int> _pecas_indexes;
		[Description("peças no trecho")]
		public ObservableCollection<int> pecas_indexes
		{
			get
			{
				if (_pecas_indexes == null)
				{
					_pecas_indexes = new ObservableCollection<int>();
				}
				return _pecas_indexes;
			}
			set
			{
				_pecas_indexes = value;
				OnPropertyChanged();
			}
		}

		public float comprimento_equivalente
		{
			get
			{
				float total = 0;
				for (int i = 0; i < pecas_indexes.Count; i++)
				{
					total += (data.materiais[material_index].pecas[pecas_indexes[i]]).comprimentos_equivalentes.Where(ce => ce.diametro.diametro_nominal == data.materiais[material_index].diametros[diametro_index].diametro_nominal).FirstOrDefault().perda;
				}
				return total;
			}
		}

		public float J
		{
			get
			{
				float j = 0;
				j = (float)((10.65 * Math.Pow(vazao, 1.852)) / (Math.Pow(data.materiais[material_index].crt, 1.852) * Math.Pow(data.materiais[material_index].diametros[diametro_index].diametro_nominal_M, 4.87)));
				return j;
			}
		}

		private float _cota;
		[Description("cota do trecho")]
		[ObsoleteAttribute("Não implementado.", true)]
		public float cota { get { return _cota; } set { _cota = value; OnPropertyChanged(); } }

		public float comprimento_total { get { return comprimento + comprimento_equivalente; } }

		public trecho(ponto __inicio, ponto __fim, float __comprimento, float __desnivel, shp __shpParent)
		{
			Debug.WriteLine("trecho, trecho");
			shpParent = __shpParent;
			inicio = __inicio;
			fim = __fim;
			comprimento = __comprimento;
			desnivel = __desnivel;
			material_index = 0;
			diametro_index = data.materiais[material_index].diametro_minimo_index;
			vazao = 0;

		}

		public bool Equals(trecho other)
		{
			Debug.WriteLine("trecho, Equals");
			if (other != null)
			{
				if (
				index.Equals(other.index) &&
				nome.Equals(other.nome) &&
				inicio.Equals(other.inicio) &&
				fim.Equals(other.fim) &&
				diametro_index.Equals(other.diametro_index) &&
				material_index.Equals(other.material_index) &&
				comprimento.Equals(other.comprimento) &&
				desnivel.Equals(other.desnivel) &&
				vazao.Equals(other.vazao)
				)
				{
					bool pecas_indexesOk = true;
					if (pecas_indexes.Count == other.pecas_indexes.Count)
					{
						for (int i = 0; i < pecas_indexes.Count; i++)
						{
							if (!pecas_indexes[i].Equals(other.pecas_indexes[i]))
							{
								pecas_indexesOk = false;
							}
						}
					}
					else
					{
						pecas_indexesOk = false;
					}
					if (pecas_indexesOk)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
