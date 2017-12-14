using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
	public sealed partial class MapView : Page
	{
		ObservableCollection<Appointment> appoinments;
		AppointmentData _ad = new AppointmentData();
		public MapView()
		{
			this.InitializeComponent();
			appoinments = _ad.GetTodaysAppointments();
			MainMap.Loaded += Mapsample_Loaded;
			getMapObjects();
			
			StartTimers();

		}

		private DispatcherTimer t_DateTime;

		public void StartTimers()
		{
			t_DateTime = new DispatcherTimer();
			t_DateTime.Tick += UpdateDateTime;
			t_DateTime.Interval = TimeSpan.FromSeconds(1);
			t_DateTime.Start();

		}

		public void UpdateDateTime(Object sender, Object e)
		{
			DateTime datetime = DateTime.Now;

			txtCurrTime.Text = datetime.ToString("hh:mm");
			txtCurrDate.Text = datetime.ToString("ddd, d MMM yy");
		}

		private async void Mapsample_Loaded(object sender, RoutedEventArgs e)
		{
			var geoLocator = new Geolocator();
			geoLocator.DesiredAccuracy = PositionAccuracy.High;
			Geoposition pos = await geoLocator.GetGeopositionAsync();


			var center =
				new Geopoint(new BasicGeoposition()
				{
					Latitude = pos.Coordinate.Point.Position.Latitude,
					Longitude = pos.Coordinate.Point.Position.Longitude

				});
			await MainMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 10000));

			//Define MapIcon
			MapIcon myPOI = new MapIcon { Location = center, NormalizedAnchorPoint = new Point(0.5, 1.0), Title = "You", ZIndex = 0 };
			//// add to map and center it
			MainMap.MapElements.Add(myPOI);


		}
		private async void getMapObjects()
		{
			foreach (Appointment a in appoinments)
			{
				// The address or business to geocode.
				string addressToGeocode = a.Address.ToString();

				// The nearby location to use as a query hint.
				BasicGeoposition queryHint = new BasicGeoposition();
				queryHint.Latitude = -28;
				queryHint.Longitude = 23;
				Geopoint hintPoint = new Geopoint(queryHint);

				// Geocode the specified address, using the specified reference point
				// as a query hint. Return no more than 3 results.
				MapLocationFinderResult result =
					  await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint);

				// If the query returns results, display the coordinates
				// of the first result.
				if (result.Status == MapLocationFinderStatus.Success)
				{
					Debug.WriteLine("result = (" +
						  result.Locations[0].Point.Position.Latitude.ToString() + "," +
						  result.Locations[0].Point.Position.Longitude.ToString() + ")");

					var center =
					new Geopoint(new BasicGeoposition()
					{
						Latitude = result.Locations[0].Point.Position.Latitude,
						Longitude = result.Locations[0].Point.Position.Longitude

					});
				//	await MainMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 3000));

					//Define MapIcon
					MapIcon myPOI = new MapIcon { Location = center, NormalizedAnchorPoint = new Point(0.5, 1.0), Title = a.Customer.Name, ZIndex = 0 };
					//// add to map and center it
					MainMap.MapElements.Add(myPOI);

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
