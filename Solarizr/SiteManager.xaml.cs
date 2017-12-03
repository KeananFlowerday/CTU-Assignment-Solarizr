using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Solarizr
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SiteManager : Page
	{
		public SiteManager()
		{
			this.InitializeComponent();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Add_Site(object sender, RoutedEventArgs e)
		{
			using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
				//insertCommand.Parameters.AddWithValue("@Entry", Input_Box.Text);

				try
				{
					insertCommand.ExecuteReader();
				}
				catch (SqliteException error)
				{
					//Handle errord
					return;
				}
				db.Close();
			}
			//Output.ItemsSource = Grab_Entries();
		}

		private void AppBarHome_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Dashboard), e);
		}

		private void AppBarProjSite_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(SiteManager), e);
		}

		private void AppBarAppointment_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(AppointmentManager), e);
		}

		private void AppBarMap_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MapView), e);
		}
	}
}
