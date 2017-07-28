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
	public class material : baseModel
	{
		public material()
		{
			Debug.WriteLine("material, material");
		}
		public material(string __nome)
		{
			Debug.WriteLine("material, material(nome)");
			_nome = __nome;

		}

		private string _nome;
		[Description("nome do material")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private string _fabricante;
		[Description("fabricante do material")]
		public string fabricante { get { return _fabricante; } set { _fabricante = value; OnPropertyChanged(); } }

		private float _crt;
		[Description("coeficiente \"C\" de Hazen Willians")]
		public float crt { get { return _crt; } set { _crt = value; OnPropertyChanged(); } }

		private ObservableCollection<diametro> _diametros;
		[Description("diametros para este material")]
		public ObservableCollection<diametro> diametros
		{
			get
			{
				if (_diametros == null)
				{
					_diametros = new ObservableCollection<diametro>();
				}
				return _diametros;
			}
			set
			{
				_diametros = value;
				OnPropertyChanged();
			}
		}

		private int _diametro_minimo_index;
		[Description("index do diametro minimo para este material")]
		public int diametro_minimo_index { get { return _diametro_minimo_index; } set { _diametro_minimo_index = value; OnPropertyChanged(); } }

		private ObservableCollection<peca> _pecas;
		[Description("peças para este material")]
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
	}
	[Serializable]
	public class peca : baseModel
	{
		public peca()
		{
			Debug.WriteLine("peca, peca");
		}
		public peca(int __index, string __nome)
		{
			Debug.WriteLine("peca, peca(index,nome)");
			_index = __index;
			_nome = __nome;
		}
		private int _index;
		[Description("identificação da peça")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		private string _nome;
		[Description("nome da peça")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private ObservableCollection<comprimento_equivalente> _comprimentos_equivalentes;
		[Description("relação diametro x perda de carga")]
		public ObservableCollection<comprimento_equivalente> comprimentos_equivalentes
		{
			get
			{
				if (_comprimentos_equivalentes == null)
				{
					_comprimentos_equivalentes = new ObservableCollection<comprimento_equivalente>();
				}
				return _comprimentos_equivalentes;
			}
			set
			{
				_comprimentos_equivalentes = value;
				OnPropertyChanged();
			}
		}
	}

	[Serializable]
	public class comprimento_equivalente : baseModel
	{
		public comprimento_equivalente() { Debug.WriteLine("comprimento_equivalente, comprimento_equivalente"); }
		public comprimento_equivalente(diametro __diametro, float __perda)
		{
			Debug.WriteLine("comprimento_equivalente, comprimento_equivalente(diametro,perda)");
			_diametro = __diametro;
			_perda = __perda;
		}

		private diametro _diametro;
		[Description("diametro da peça")]
		public diametro diametro { get { return _diametro; } set { _diametro = value; OnPropertyChanged(); } }

		private float _perda;
		[Description("comprimento equivalente da peça")]
		public float perda { get { return _perda; } set { _perda = value; OnPropertyChanged(); } }
	}

	[Serializable]
	public class diametro : baseModel
	{
		public diametro() { Debug.WriteLine("diametro, diametro"); }
		public diametro(int __index, string __nome, float __diametro_nominal)
		{
			Debug.WriteLine("diametro, diametro(index,nome,diametro_nominal");
			_index = __index;
			_nome = __nome;
			_diametro_nominal = __diametro_nominal;
		}

		private int _index;
		[Description("identificação do diametro")]
		public int index { get { return _index; } set { _index = value; OnPropertyChanged(); } }

		private string _nome;
		[Description("nome do diametro (em polegadas)")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private float _diametro_nominal;
		[Description("diametro nominal em milímetro")]
		public float diametro_nominal { get { return _diametro_nominal; } set { _diametro_nominal = value; OnPropertyChanged(); OnPropertyChanged("diametro_nominal_M"); } }

		[Description("diametro nominal em metro")]
		public float diametro_nominal_M { get { return _diametro_nominal * 1000; } }

	}
}
