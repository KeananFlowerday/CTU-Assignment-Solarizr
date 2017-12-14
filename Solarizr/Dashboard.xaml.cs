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
using Windows.UI.Notifications;
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
	public sealed partial class Dashboard : Page
	{   
        //appointments - lists and data context
		AppointmentData apptData = new AppointmentData();
		ObservableCollection<Appointment> todaysAppointments;
        ObservableCollection<Appointment> allAppointments;
        ObservableCollection<Appointment> upcomingAppointments = new ObservableCollection<Appointment>();

        //counters for todays appointments
        double numAppointments = 0;
        double numComplete = 0;
        double numRemaining = 0;

        //Project Sites - lists and data context
        ProjectSiteData psData = new ProjectSiteData();
        ObservableCollection<User> projectSites;

 

		Geolocator geoLocator;
		//initialised below
		public Dashboard()

		{
			this.InitializeComponent();
            txt_Percent.Text = "---%";

			todaysAppointments = apptData.GetTodaysAppointments();
            projectSites = psData.GetAllSites();

            //foreach(appt a in list) if a.date == today create marker on map
            

			SmallMap.Loaded += Mapsample_Loaded;
            getMapObjects();

            //foreach(appointment for today) if status == pending then add to upcomingAppointments.
            foreach (Appointment a in todaysAppointments)
            {
                if (a.Status == AppointmentStatus.Pending)
                {
                    upcomingAppointments.Add(a);
                }
            }
            //order the appointments by date/time.
            var tempList = upcomingAppointments.OrderBy(a => a.Date);

            tempList.ToList().ForEach(q =>
            {
                upcomingAppointments.Remove(q);
                upcomingAppointments.Add(q);
            });

            //adding combobox items to form
            cmbx_Status.Items.Add("Pending");
            cmbx_Status.Items.Add("Approved");
            cmbx_Status.Items.Add("Denied");
            cmbx_Status.Items.Add("Skipped");

            //add upcoming appt data to form
            InitializeForm();
            
            //add data to progress bar
            numAppointments = todaysAppointments.Count;
            numRemaining = upcomingAppointments.Count;
            numComplete = numAppointments - numRemaining;

            //initial setting of progress bar
            SetProgressBar();


            StartTimers();
            //add apointment combobox.
            foreach (User u in projectSites)

			StartTimers();
			//add apointment combobox.
			foreach (User u in projectSites)

            {
                cmbxApptSitePicker.Items.Add(u.Name);
            }

            //foreach( appt a in list) create marker on calander
            allAppointments = apptData.GetAllAppointments();
            //foreach (Appointment a in allAppointments)
            //{
                

            //}
            
            //initialize webview for weather - link from dian
          

		}

        private void InitializeForm()
        {
            Appointment curr = upcomingAppointments[0];
            NextAppt_txtblock.Text = curr.Customer.Name + ", at " + curr.Date.Hour + ":" + curr.Date.Minute;
            txtCust.Text = curr.Customer.Name;
            txtPhone.Text = curr.Customer.Phone;
            txtManager.Text = curr.SiteManager.Name;

        }

        private void SetProgressBar()
        {
            txt_Remaining.Text = "Appointments Left: " + numRemaining;

            if (numAppointments > 0)
            {
                PB_Appointments.Value = numComplete;
                PB_Appointments.Maximum = numAppointments;
                txt_Percent.Text = (Math.Round(numComplete / numAppointments * 100)).ToString() + "%";
            }
        }

        private async void getMapObjects()
		{
			foreach (Appointment a in todaysAppointments)
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
					//await SmallMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 3000));

					//Define MapIcon
					MapIcon myPOI = new MapIcon { Location = center, NormalizedAnchorPoint = new Point(0.5, 1.0), Title = a.Customer.Name, ZIndex = 0 };
					// add to map and center it
					SmallMap.MapElements.Add(myPOI);

				}
			}

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
			txtCurrDate.Text = datetime.ToString("dddd, d MMMM yyyy");
            try
            {
                Appointment curr = upcomingAppointments[0];
                if (curr.Date > DateTime.Now)
                {
                    TimeSpan timebefore = curr.Date - datetime;
                    if ((timebefore.TotalHours <= 24) && (timebefore.TotalSeconds > 0))
                    {

                        txtTimeBefore.Text = timebefore.Hours.ToString() + ":" + timebefore.Minutes.ToString();
                    }
                    else if (timebefore.TotalDays > 0)
                    {
                        txtTimeBefore.Text = timebefore.TotalDays.ToString();
                    }
                    else
                    {
                        txtTimeBefore.Text = "Error";
                    }
                }
            }
            catch (Exception execption)
            {
                txtTimeBefore.Text = "No Appts";
                Debug.WriteLine(execption.ToString());
                throw;
            }
            
        }

		private void BtnApptCreate_Click(object sender, RoutedEventArgs e)
		{
			//int cmbxItem = cmbxApptSitePicker.SelectedIndex;
			//ProjectSite pSite = SiteList[cmbxItem];


			DateTimeOffset _date = dateApptDatePicker.Date;
			TimeSpan _time = timeApptTimePicker.Time;

			DateTime apptDateTime;

			apptDateTime = _date.DateTime;
			apptDateTime.Add(_time);

			#region Notes
			//To convert back to offset and bind to datetimepicker   
			//DateTime newBookingDate;
			//newBookingDate = DateTime.SpecifyKind(apptDateTime, DateTimeKind.Utc);
			//DateTimeOffset bindTime = newBookingDate;
			//dateApptDatePicker.Date = bindTime;
			#endregion

			//Appointment newAppointment = new Appointment(apptDateTime, pSite);
		}

		private async void Mapsample_Loaded(object sender, RoutedEventArgs e)
		{
			 geoLocator = new Geolocator();
			geoLocator.DesiredAccuracy = PositionAccuracy.High;
			Geoposition pos = await geoLocator.GetGeopositionAsync();
			WebView_Weather.Navigate(new Uri("http://forecast.io/embed/#lat=" + pos.Coordinate.Point.Position.Latitude.ToString() +"&lon="+pos.Coordinate.Point.Position.Longitude.ToString()+"&name=the Job Site&color=#00aaff&font=Segoe UI&units=ca"));

			var center =
				new Geopoint(new BasicGeoposition()
				{
					Latitude = pos.Coordinate.Point.Position.Latitude,
					Longitude = pos.Coordinate.Point.Position.Longitude

				});
			await SmallMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 5000));

			//Define MapIcon
			MapIcon myPOI = new MapIcon { Location = center, NormalizedAnchorPoint = new Point(0.5, 1.0), Title = "You", ZIndex = 0 };
			//add to map and center it
			SmallMap.MapElements.Add(myPOI);


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

		private void SetAlarm(object sender, RoutedEventArgs e)
		{
			//toastnotifications
		}

        public void CountDown()
        {
            Appointment curr = upcomingAppointments[0];
            if (curr.Date < DateTime.Now)
            {

            }
        }
	}
}
