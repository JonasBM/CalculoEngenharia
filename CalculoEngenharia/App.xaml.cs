using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalculoEngenharia
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			Debug.WriteLine("App, OnStartup");
			base.OnStartup(e);
			VIEW.MainWindow window = new VIEW.MainWindow();
			VIEWMODEL.InicioVM InicioVM = new VIEWMODEL.InicioVM();
			string fullName = null;
			if (e.Args.Count() > 0)
			{
				Debug.WriteLine(e.Args[0]);
				fullName = e.Args[0];
			}
			InicioVM.Start(fullName);
			window.Show();
		}
	}
}
