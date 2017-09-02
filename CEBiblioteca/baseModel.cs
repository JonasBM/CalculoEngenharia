using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CEBiblioteca
{
	[Serializable]
	public class baseModel : INotifyPropertyChanged
	{
		[field: NonSerializedAttribute()]
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			//Debug.WriteLine("baseModel, OnPropertyChanged("+ propertyName + ")");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		protected bool SetField<T>(ref T field, T value,
		[CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
		public static void DebugAlert([CallerMemberName] string memberName = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
		{
			Debug.WriteLine("Arquivo: " + Path.GetFileName(sourceFilePath) + " /Função: " + memberName + "() /Linha: " + sourceLineNumber);
		}

		public static void DebugAlertMessage(string message, [CallerMemberName] string memberName = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = 0)
		{
			Debug.WriteLine(message);
			Debug.WriteLine("Arquivo: " + Path.GetFileName(sourceFilePath) + " /Função: " + memberName + "() /Linha: " + sourceLineNumber);
		}
	}
}
