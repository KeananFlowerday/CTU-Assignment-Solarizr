using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
		ObservableCollection<User> _out;
		public SiteManager()
		{
			this.InitializeComponent();
			try
			{
				ProjectSiteData _ps = new ProjectSiteData();
				_out = _ps.GetAllUsers();
				ListV_Upcoming.ItemsSource = _out;
          
			}
      catch(Exception e)
{
				Debug.WriteLine(e.Message);
			}

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
				try
				{
					db.Open();
					SqliteCommand _insertAddress = new SqliteCommand();
					SqliteCommand _insertUser = new SqliteCommand();
					_insertUser.Connection = db;
					_insertAddress.Connection = db;
					_insertAddress.CommandText = "INSERT INTO Address_tbl (Street, Suburb, City, Postal_Code, Country) VALUES('" + _address.Street + "','" + _address.Suburb + "','" + _address.City + "','" + _address.PostalCode + "','" + _address.Country + "'); ";
					try
					{
						_insertAddress.ExecuteReader();

					}
					catch (SqliteException error)
					{
						Debug.WriteLine("Insert Address Error");
						return;
					}

					int _adIndex = Address.LastIndex();

					if (_adIndex == -1)
					{
						throw new SqliteException("You Done Fucked Up", 500);
					}

					//Use parameterized query to prevent SQL injection attacks
					_insertUser.CommandText = "INSERT INTO User_tbl(Name, Phone, FK_AdID) VALUES ('" + _Site.Name + "','" + _Site.Phone + "','" + _adIndex + "');";

					try
					{
						//_insertAddress.ExecuteReader();
						_insertUser.ExecuteReader();
					}
					catch (SqliteException error)
					{
						Debug.WriteLine("Insert User Error");
						return;
					}
					db.Close();
				}catch(Exception ex)
				{
					Debug.WriteLine("btnSave.Click() Error");
				}
			}
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
