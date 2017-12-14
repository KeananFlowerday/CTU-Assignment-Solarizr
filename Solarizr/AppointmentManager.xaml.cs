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
	public sealed partial class AppointmentManager : Page
	{
		ObservableCollection<Appointment> appoinments;
		AppointmentData _ad = new AppointmentData();
		public AppointmentManager()
		{
			this.InitializeComponent();

			try
			{
				appoinments = _ad.GetUpcomingAppointments();
				//cmb_Sites.ItemsSource = _out;
				ListV_Upcoming.ItemsSource = appoinments;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		private void BtnApptSave_Click(object sender, RoutedEventArgs e)
		{
			#region Notes
			//To convert back to offset and bind to datetimepicker   
			//DateTime newBookingDate;
			//newBookingDate = DateTime.SpecifyKind(apptDateTime, DateTimeKind.Utc);
			//DateTimeOffset bindTime = newBookingDate;
			//dateApptDatePicker.Date = bindTime;
			#endregion

			
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
