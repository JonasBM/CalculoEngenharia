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
	
	public class Norma : CEBiblioteca.baseModel
	{

		private string _nome;
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private ObservableCollection<Risco> _riscos;
		public ObservableCollection<Risco> riscos
		{
			get
			{
				if (_riscos == null)
				{
					_riscos = new ObservableCollection<Risco>();
				}
				return _riscos;
			}
			set
			{
				_riscos = value;
				OnPropertyChanged();
			}
		}

		public Norma(string __nome)
		{
			DebugAlert();
			nome = __nome;
		}

		public class Risco : CEBiblioteca.baseModel
		{
			private string _nome;
			public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

			private float _vazão;
			public float vazão { get { return _vazão; } set { _vazão = value; OnPropertyChanged(); } }

			private float _pressao;
			public float pressao { get { return _pressao; } set { _pressao = value; OnPropertyChanged(); } }

			private float _requinte;
			public float requinte { get { return _requinte; } set { _requinte = value; OnPropertyChanged(); } }

			private jato _jato;
			public jato jato { get { return _jato; } set { _jato = value; OnPropertyChanged(); } }

			private int _mangueira;
			public int mangueira { get { return _mangueira; } set { _mangueira = value; OnPropertyChanged(); } }

			private float _perda_esguicho;
			public float perda_esguicho { get { return _perda_esguicho; } set { _perda_esguicho = value; OnPropertyChanged(); } }

			private Action _calculo;
			public Action calculo { get { return _calculo; } set { _calculo = value; OnPropertyChanged(); } }

			public Risco(string __nome, float __vazão, float __pressao, float __requinte, jato __jato, int __mangueira, float __perda_esguicho = 0)
			{
				DebugAlert();
				nome = __nome;
				vazão = __vazão;
				pressao = __pressao;
				requinte = __requinte;
				jato = __jato;
				mangueira = __mangueira;
				perda_esguicho = __perda_esguicho;

			}
		}
	}

	
}
