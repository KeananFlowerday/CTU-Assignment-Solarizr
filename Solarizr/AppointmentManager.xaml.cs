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

		public AppointmentManager()
		{
			this.InitializeComponent();

			try
			{
				appoinments = Grab_Appointments();
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
			int cmbxItem = cmbxApptSitePicker.SelectedIndex;
			// ProjectSite pSite = SiteList[cmbxItem];


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

			//  Appointment newAppointment = new Appointment(apptDateTime, pSite);
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

		private ObservableCollection<Appointment> Grab_Appointments()
		{
			ObservableCollection<Appointment> entries = new ObservableCollection<Appointment>();
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				db.Open();
				SqliteCommand selectCommand = new SqliteCommand("SELECT * from Appointment_tbl as Ap INNER JOIN User_tbl as Ut ON Ap.FK_CID = Ut.PK_ID INNER JOIN Address_tbl as Ad ON Ut.FK_AdID = Ad.PK_ID INNER JOIN User_tbl as SM ON Ap.FK_SMID = SM.PK_ID INNER JOIN Address_tbl as SMAd ON SM.FK_AdID = SMAd.PK_ID INNER JOIN Address_tbl as AAd ON Ap.FK_AdID = AAd.PK_ID", db);
				SqliteDataReader query;
				try
				{
					query = selectCommand.ExecuteReader();
				}
				catch (SqliteException error)
				{
					Debug.WriteLine("SelectCommand Error");
					return entries;
				}
				while (query.Read())
				{
					try
					{
						Appointment _app = new Appointment();
						_app.ID = int.Parse(query.GetString(0));
						_app.Date = DateTime.Parse(query.GetString(1));
						switch (query.GetString(2))
						{
							case ("Done"):
								_app.Status = AppointmentStatus.Done;
								break;
							default:
								_app.Status = AppointmentStatus.Done;
								break;
						}
						switch (int.Parse(query.GetString(3)))
						{
							case (0):
								_app.Submitted = false;
								break;
							case (1):
								_app.Submitted = true;
								break;
							default:
								_app.Status = AppointmentStatus.Done;
								break;
						}
						_app.Customer = new User
						{
							ID = int.Parse(query.GetString(7)),
							Name = query.GetString(8),
							Phone = query.GetString(9),
							Address = new Address
							{
								ID = int.Parse(query.GetString(11)),
								Street = query.GetString(12),
								Suburb = query.GetString(13),
								City = query.GetString(14),
								PostalCode = query.GetString(15),
								Country = query.GetString(16)
							}
						};
						_app.SiteManager = new User
						{
							ID = int.Parse(query.GetString(17)),
							Name = query.GetString(18),
							Phone = query.GetString(19),
							Address = new Address
							{
								ID = int.Parse(query.GetString(21)),
								Street = query.GetString(22),
								Suburb = query.GetString(23),
								City = query.GetString(24),
								PostalCode = query.GetString(25),
								Country = query.GetString(26)
							}
						};
						_app.Address = new Address
						{
							ID = int.Parse(query.GetString(27)),
							Street = query.GetString(28),
							Suburb = query.GetString(29),
							City = query.GetString(30),
							PostalCode = query.GetString(31),
							Country = query.GetString(32)
						};

						entries.Add(_app);
					}
					catch (Exception e)
					{
						Debug.WriteLine("AssignUser Error");
					}
				}
				db.Close();
			}
			return entries;
		}
	}
}
