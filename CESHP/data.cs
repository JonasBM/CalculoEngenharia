using CESHP.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CESHP
{
	public class data
	{


		static public ObservableCollection<Norma> normas;

		static public ObservableCollection<jato> jatos;


		static public ObservableCollection<material> materiais;

		static data()
		{
			jatos = new ObservableCollection<jato>();
			jatos.Add(jato.Solido);
			jatos.Add(jato.Regulavel);



			normas = new ObservableCollection<Norma>();

			#region normas

			Norma CBM_SC_Velha = new Norma("CBM/SC-Velha");
			CBM_SC_Velha.riscos.Add(new Norma.Risco("Leve", 0, 4, 13, jato.Solido, 38));
			CBM_SC_Velha.riscos.Add(new Norma.Risco("Médio", 0, 15, 25, jato.Regulavel, 40));
			CBM_SC_Velha.riscos.Add(new Norma.Risco("Elevado", 0, 30, 25, jato.Regulavel, 65));
			normas.Add(CBM_SC_Velha);

			Norma CBM_SC_Nova = new Norma("CBM/SC-Nova");
			CBM_SC_Nova.riscos.Add(new Norma.Risco("Leve", 70, 4, 13, jato.Solido, 40, (float)0.3));
			CBM_SC_Nova.riscos.Add(new Norma.Risco("Leve-Mangotinho", 80, 15, -1, jato.Regulavel, 25, 10));
			CBM_SC_Nova.riscos.Add(new Norma.Risco("Médio", 300, 15, -1, jato.Regulavel, 40, 20));
			CBM_SC_Nova.riscos.Add(new Norma.Risco("Elevado", 600, 30, -1, jato.Regulavel, 65, 15));
			normas.Add(CBM_SC_Nova);

			Norma NBR_2000 = new Norma("NBR-13714/2000");
			NBR_2000.riscos.Add(new Norma.Risco("Tipo 1", 80, 4, 13, jato.Solido, 40));
			NBR_2000.riscos.Add(new Norma.Risco("Tipo 2", 80, 15, -1, jato.Regulavel, 25, 10));
			NBR_2000.riscos.Add(new Norma.Risco("Tipo 3", 300, 15, -1, jato.Regulavel, 40, 20));
			normas.Add(NBR_2000);

			#endregion

			

			materiais = new ObservableCollection<material>();
			string[] nomes_diametros;
			int[] diametros;
			string[] nomes_pecas;
			double[][] perdas_pecas;

			#region AÇO
			material aco = new material();
			aco.crt = 120;
			aco.nome = "Aço Galvanizado";
			aco.fabricante = "Tupy";

			#region DIAMETROS DE AÇO
			nomes_diametros = new string[] { "1/4", "3/8", "1/2", "3/4", "1", "1 1/4", "1 1/2", "2", "2 1/2", "3", "4", "5", "6" };
			diametros = new int[] { 8, 10, 15, 20, 25, 32, 40, 50, 65, 80, 100, 125, 150 };
			if (nomes_diametros.Length != diametros.Length)
			{
				Console.WriteLine("Erro: DIAMETROS DE AÇO, nomes={0}, diametros={1}", nomes_diametros.Length, diametros.Length);
				throw new NotImplementedException();
			}
			for (int i = 0; i < nomes_diametros.Length; i++)
			{
				aco.diametros.Insert(i, new diametro(i, nomes_diametros[i], diametros[i]));
			}
			aco.diametro_minimo_index = 8;
			#endregion

			#region PEÇAS DE AÇO
			

			#region PEÇAS DE AÇO - NOMES
			nomes_pecas = new string[]
			{
				"Joelho 90°", //0
				"Joelho 45°", //1
				"Tê de Passagem Direta", //2
				"Tê de Saída Lateral", //3
				"Tê de Saída Bilateral", //4
				"Luva", //5
				"União", //6
				"Saída da Tubulação", //7
				"Entrada Normal", //8
				"Entrada de Borda", //9
				"Registro de Gaveta Aberto", //10
				"Registro de Globo Aberto", //11
				"Registro de Angulo Aberto", //12
				"Válvula de Pe e Crivo Aberta", //13
				"Válvula Retenção Horizontal", //14
				"Válvula Retenção Vertical", //15
			};
			#endregion

			
			#region PEÇAS DE AÇO - PERDAS
			perdas_pecas = new double[nomes_pecas.Length][];
			//                               "1/4","3/8","1/2","3/4",  "1",11/4",11/2",  "2",21/2",  "3",  "4",  "5",  "6"
			perdas_pecas[00] = new double[] { 0.23, 0.35, 0.47, 0.70, 0.94, 1.17, 1.41, 1.88, 2.35, 2.82, 3.76, 4.70, 5.64 }; //Joelho 90°
			perdas_pecas[01] = new double[] { -1.0, 0.16, 0.22, 0.32, 0.43, 0.54, 0.65, 0.86, 1.08, 1.30, 1.73, 2.16, 2.59 }; //Joelho 45°
			perdas_pecas[02] = new double[] { 0.04, 0.06, 0.08, 0.12, 0.17, 0.21, 0.25, 0.33, 0.41, 0.50, 0.66, 0.83, 0.99 }; //Tê de Passagem Direta
			perdas_pecas[03] = new double[] { 0.34, 0.51, 0.69, 1.03, 1.37, 1.71, 2.06, 2.74, 3.43, 4.11, 5.49, 6.86, 8.23 }; //Tê de Saída Lateral
			perdas_pecas[04] = new double[] { 0.42, 0.62, 0.83, 1.25, 1.66, 2.08, 2.50, 3.33, 4.16, 4.99, 6.65, 8.32, 9.98 }; //Tê de Saída Bilateral
			perdas_pecas[05] = new double[] { 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.02, 0.02, 0.03 }; //Luva
			perdas_pecas[06] = new double[] { 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, 0.01, -1.0, -1.0 }; //União
			perdas_pecas[07] = new double[] { -1.0, -1.0, 0.40, 0.50, 0.70, 0.90, 1.00, 1.50, 1.90, 2.20, 3.20, 4.00, 5.00 }; //Saída da Tubulação
			perdas_pecas[08] = new double[] { -1.0, -1.0, 0.20, 0.20, 0.30, 0.40, 0.50, 0.70, 0.90, 1.10, 1.60, 2.00, 2.50 }; //Entrada Normal
			perdas_pecas[09] = new double[] { -1.0, -1.0, 0.40, 0.50, 0.70, 0.90, 1.00, 1.50, 1.90, 2.20, 3.20, 4.00, 5.00 }; //Entrada de Borda
			perdas_pecas[10] = new double[] { -1.0, -1.0, 0.10, 0.10, 0.20, 0.20, 0.30, 0.40, 0.40, 0.50, 0.70, 0.90, 1.10 }; //Registro de Gaveta Aberto
			perdas_pecas[11] = new double[] { -1.0, -1.0, 4.90, 6.70, 8.20, 11.3, 13.4, 17.4, 21.0, 26.0, 34.0, 43.0, 51.0 }; //Registro de Globo Aberto
			perdas_pecas[12] = new double[] { -1.0, -1.0, 2.60, 3.60, 4.60, 5.60, 6.70, 8.50, 10.0, 13.0, 17.0, 21.0, 26.0 }; //Registro de Angulo Aberto
			perdas_pecas[13] = new double[] { -1.0, -1.0, 3.60, 5.60, 7.30, 10.0, 11.6, 14.0, 17.0, 20.0, 23.0, 30.0, 39.0 }; //Válvula de Pe e Crivo Aberta
			perdas_pecas[14] = new double[] { -1.0, -1.0, 1.10, 1.60, 2.10, 2.70, 3.20, 4.20, 5.20, 6.30, 8.40, 10.4, 12.5 }; //Válvula Retenção Horizontal
			perdas_pecas[15] = new double[] { -1.0, -1.0, 1.60, 2.40, 3.20, 4.00, 4.80, 6.40, 8.10, 9.70, 12.9, 16.1, 19.3 }; //Válvula Retenção Vertical
			#endregion

			#region PEÇAS DE AÇO - APLICA
			if (nomes_pecas.Length != perdas_pecas.Length)
			{
				Console.WriteLine("Erro: PEÇAS DE AÇO, nomes={0}, diametros={1}", nomes_pecas.Length, perdas_pecas.Length);
				throw new NotImplementedException();
			}

			for (int i = 0; i < nomes_pecas.Length; i++)
			{
				aco.pecas.Insert(i, new peca(i, nomes_pecas[i]));
				if (diametros.Length != perdas_pecas[i].Length)
				{
					Console.WriteLine("Erro: PEÇAS DE AÇO, nome={0}, diametros={1}, perdas={2}", nomes_pecas[i], diametros.Length, perdas_pecas[i].Length);
					throw new NotImplementedException();
				}
				for (int j = 0; j < diametros.Length; j++)
				{
					aco.pecas[i].comprimentos_equivalentes.Add(new comprimento_equivalente(aco.diametros[j], (float)perdas_pecas[i][j]));
				}
			}
			#endregion

			#endregion

			materiais.Add(aco);
			#endregion

			#region PVC
			material pvc = new material();
			pvc.crt = 120;
			pvc.nome = "PVC";
			pvc.fabricante = "XXXXXXXXXXXXXXX";

			#region DIAMETROS DE PVC
			nomes_diametros = new string[] { "3/4", "1", "1 1/4", "1 1/2", "2", "2 1/2", "3", "4", "5", "6" };
			diametros = new int[] { 20, 25, 32, 40, 50, 65, 80, 100, 125, 150 };
			if (nomes_diametros.Length != diametros.Length)
			{
				Console.WriteLine("Erro: DIAMETROS DE PVC, nomes={0}, diametros={1}", nomes_diametros.Length, diametros.Length);
				throw new NotImplementedException();
			}
			for (int i = 0; i < nomes_diametros.Length; i++)
			{
				pvc.diametros.Insert(i, new diametro(i, nomes_diametros[i], diametros[i]));
			}
			pvc.diametro_minimo_index = 4;
			#endregion

			#region PEÇAS DE PVC

			#region PEÇAS DE PVC - NOMES
			nomes_pecas = new string[]
			{
				"Teste", //0
			};
			#endregion

			#region PEÇAS DE PVC - PERDAS
			perdas_pecas = new double[nomes_pecas.Length][];
			//                              "3/4",  "1",11/4",11/2",  "2",21/2",  "3",  "4",  "5",  "6"
			perdas_pecas[00] = new double[] {0.70, 0.94, 1.17, 1.41, 1.88, 2.35, 2.82, 3.76, 4.70, 5.64 }; //Teste
			#endregion

			#region PEÇAS DE PVC - APLICA
			if (nomes_pecas.Length != perdas_pecas.Length)
			{
				Console.WriteLine("Erro: PEÇAS DE PVC, nomes={0}, diametros={1}", nomes_pecas.Length, perdas_pecas.Length);
				throw new NotImplementedException();
			}

			for (int i = 0; i < nomes_pecas.Length; i++)
			{
				pvc.pecas.Insert(i, new peca(i, nomes_pecas[i]));
				if (diametros.Length != perdas_pecas[i].Length)
				{
					Console.WriteLine("Erro: PEÇAS DE PVC, nome={0}, diametros={1}, perdas={2}", nomes_pecas[i], diametros.Length, perdas_pecas[i].Length);
					throw new NotImplementedException();
				}
				for (int j = 0; j < diametros.Length; j++)
				{
					pvc.pecas[i].comprimentos_equivalentes.Add(new comprimento_equivalente(pvc.diametros[j], (float)perdas_pecas[i][j]));
				}
			}
			#endregion

			#endregion

			materiais.Add(pvc);
			#endregion

		}

		static public material GetMaterial(string __nome)
		{
			material material = materiais.Where(m => m.nome == __nome).FirstOrDefault();
			if (material != null)
			{
				return material;
			}
			else
			{
				return null;
			}
		}

		static public material GetMaterialFirst()
		{
			return materiais[0];
		}


		//static public material aco;
		//static public material cobre_pvc;

		//static public string[] alfabeto = new string[]{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "S", "T", "U", "V", "X", "Y", "Z" };

		static public class alfabeto
		{

			static private Regex rgx_hidrante = new Regex(@"^[hH]\d+[0-9]*$");

			static private Regex rgx = new Regex(@"^[a-zA-Z]*$");

			static private string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			static alfabeto() { }

			static public string primeira(bool maiuscula = true)
			{
				if (maiuscula)
				{
					return "A";
				}
				else
				{
					return "a";
				}
			}

			static public string proxima(string __letra, bool maiuscula = true)
			{
				__letra = __letra.ToUpper();

				int posicao = __letra.Length - 1;
				while (rgx.IsMatch(__letra.Substring(posicao, 1)) && posicao > 0)
				{
					posicao--;
				}

				string letra_comeco = "";
				if (posicao > 0)
				{
					letra_comeco = __letra.Substring(0, posicao + 1);
				}
				string letra_altera = __letra.Substring(posicao);

				int index = 0;
				for (int i = 0; i < letra_altera.Length; i++)
				{
					int multiplicador = letras.ToUpper().IndexOf(letra_altera.Substring(letra_altera.Length - 1 - i, 1)) + 1;
					index += multiplicador * (int)Math.Pow(letras.Length, i);
				}
				return letra_comeco + GetLetra(index);
			}
			static public string GetLetra(int __index, bool maiuscula = true)
			{
				string letra = "";
				if (__index < 26)
				{
					letra = (letras.ToUpper()[__index]).ToString();
				}
				else
				{
					int multiplicador = __index / letras.Length;
					letra = GetLetra(multiplicador - 1);
					letra += (letras.ToUpper()[__index - multiplicador * letras.Length]).ToString();
				}
				return letra;
			}
		}
	}
}
