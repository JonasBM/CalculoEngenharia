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
	public class mangueira : baseModel
	{
		public mangueira()
		{
			Debug.WriteLine("mangueira, mangueira");
		}
		public mangueira(string __nome)
		{
			Debug.WriteLine("mangueira, mangueira{0}", __nome);
			_nome = __nome;
		}

		public mangueira(string __nome, string __fabricante, float __crt, diametro __diametro)
		{
			Debug.WriteLine("mangueira, mangueira({0},{1},{2},{3})", __nome, __fabricante, __crt, __diametro);
			nome = __nome;
			fabricante = __fabricante;
			crt = __crt;
			diametro = __diametro;
		}

		private string _nome;
		[Description("nome da mangueira")]
		public string nome { get { return _nome; } set { _nome = value; OnPropertyChanged(); } }

		private string _fabricante;
		[Description("fabricante da mangueira")]
		public string fabricante { get { return _fabricante; } set { _fabricante = value; OnPropertyChanged(); } }

		private float _crt;
		[Description("coeficiente \"C\" de Hazen Willians")]
		public float crt { get { return _crt; } set { _crt = value; OnPropertyChanged(); } }

		private diametro _diametro;
		public diametro diametro { get { return _diametro; } set { _diametro = value; OnPropertyChanged(); } }
	}
}
