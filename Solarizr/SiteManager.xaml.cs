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
		ProjectSiteData _ps = new ProjectSiteData();
		public SiteManager()
		{
			this.InitializeComponent();
			try
			{
				
				_out = _ps.GetAllSites();
				ListV_Upcoming.ItemsSource = _out;

			}
			catch (Exception e)
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
			Address _address = new Address(_Street, _Suburb, _City, _PCode, _Country);
			User _Site = new User(_name, _phone, _address);
			bool _complete = _ps.InsertUser(_Site);
			if (_complete)
			{
				Debug.WriteLine("User Saved");
			}
			else
			{
				Debug.WriteLine("Error with User Save");
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
