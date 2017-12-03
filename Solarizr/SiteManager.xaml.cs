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
			string _name = txtSiteName.Text;
			string _phone = txtPhoneNumber.Text;

			string _Street = txtStreet.Text;
			string _Suburb = txtSuburb.Text;
			string _City = txtCity.Text;
			string _PCode = txtPostalCode.Text;
			string _Country = txtCountry.Text;
			Address _address = new Address(_Street,_Suburb,_City,_PCode,_Country);
			User _Site = new User(_name,_phone, _address);
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				db.Open();
				SqliteCommand _insertAddress = new SqliteCommand();
				SqliteCommand _insertUser = new SqliteCommand();
				_insertUser.Connection = db;
				_insertAddress.Connection = db;
				int _adIndex = Address.LastIndex();
				if (_adIndex == -1)
				{
					throw new SqliteException("You Done Fucked Up", 500);
				}
				//Use parameterized query to prevent SQL injection attacks
				_insertUser.CommandText = "INSERT INTO User_tbl VALUES (NULL, "+_Site.Name+","+_Site.Phone+","+_adIndex+");";
				_insertAddress.CommandText ="INSERT INTO Address_tbl VALUES(NULL, );";
				try
				{
					_insertAddress.ExecuteReader();
					_insertUser.ExecuteReader();
				}
				catch (SqliteException error)
				{
					//Handle errord
					return;
				}
				db.Close();
			}
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
