using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
			window.DataContext = new VIEWMODEL.InicioVM();
			window.Show();
		}
	}
}
