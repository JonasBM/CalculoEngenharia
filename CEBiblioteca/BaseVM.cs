using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CEBiblioteca
{
	public abstract class BaseVM : baseModel, INotifyPropertyChanged, IDisposable
	{
		private bool _ativo;
		public bool ativo { get { return _ativo; } set { _ativo = value; OnPropertyChanged(); } }

		private string _titulo;
		public string titulo { get { return _titulo; } set { _titulo = value; OnPropertyChanged(); } }

		private string _descricao;
		public string descricao { get { return _descricao; } set { _descricao = value; OnPropertyChanged(); } }

		private UserControl _conteudo;
		public UserControl conteudo { get { return _conteudo; } set { _conteudo = value; OnPropertyChanged(); } }

		private double _minHeight;
		public double minHeight
		{
			get
			{
				if (conteudo != null)
				{
					return conteudo.MinHeight + 60;
				}
				return 0;
			}
		}

		private double _minWidth;
		public double minWidth
		{
			get
			{
				if (conteudo != null)
				{
					return conteudo.MinWidth + 40;
				}
				return 0;
			}
		}

		
		public void Dispose()
		{
			OnDispose();
		}
		protected virtual void OnDispose()
		{
			throw new NotImplementedException();
		}
		internal virtual void Update()
		{
			throw new NotImplementedException();
		}

		protected virtual bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				return true;
			}
			finally
			{
				if (stream != null)
				{ stream.Close(); }	
			}
			//file is not locked
			return false;
		}
	}
}
