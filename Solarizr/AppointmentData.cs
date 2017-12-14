using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
	class AppointmentData
	{

		public ObservableCollection<Appointment> GetAllAppointments()
		{
			ObservableCollection<Appointment> _Apps = new ObservableCollection<Appointment>();
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
					return _Apps;
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
							case ("Pending"):
								_app.Status = AppointmentStatus.Pending;
								break;
							case ("Approved"):
								_app.Status = AppointmentStatus.Approved;
								break;
							case ("Denied"):
								_app.Status = AppointmentStatus.Denied;
								break;
							case ("Skipped"):
								_app.Status = AppointmentStatus.Skipped;
								break;
							default:
								_app.Status = AppointmentStatus.Pending;
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
								_app.Submitted = false;
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

						_Apps.Add(_app);
					}
					catch (Exception e)
					{
						Debug.WriteLine("AssignUser Error");
					}
				}
				db.Close();
			}
			return _Apps;
		}

		public ObservableCollection<Appointment> GetUpcomingAppointments()
		{
			DateTime _dt = DateTime.Now;
			ObservableCollection<Appointment> _Apps = new ObservableCollection<Appointment>();
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
					return _Apps;
				}
				while (query.Read())
				{
					if (DateTime.Parse(query.GetString(1)) >= DateTime.Now)
					{


						try
						{
							Appointment _app = new Appointment();
							_app.ID = int.Parse(query.GetString(0));
							_app.Date = DateTime.Parse(query.GetString(1));
							switch (query.GetString(2))
							{
								case ("Pending"):
									_app.Status = AppointmentStatus.Pending;
									break;
								case ("Approved"):
									_app.Status = AppointmentStatus.Approved;
									break;
								case ("Denied"):
									_app.Status = AppointmentStatus.Denied;
									break;
								case ("Skipped"):
									_app.Status = AppointmentStatus.Skipped;
									break;
								default:
									_app.Status = AppointmentStatus.Pending;
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
									_app.Submitted = false;
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

							_Apps.Add(_app);
						}
						catch (Exception e)
						{
							Debug.WriteLine("AssignUser Error");
						}
					}
				}
				db.Close();
			}
			return _Apps;
		}

		public ObservableCollection<Appointment> GetTodaysAppointments()
		{
			DateTime _dt = DateTime.Now;
			ObservableCollection<Appointment> _Apps = new ObservableCollection<Appointment>();
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
					return _Apps;
				}
				while (query.Read())
				{
					DateTime _today = DateTime.Today.Date;
					DateTime _appToday = DateTime.Parse(query.GetString(1)).Date;
					if ( _appToday== DateTime.Today)
					{


						try
						{
							Appointment _app = new Appointment();
							_app.ID = int.Parse(query.GetString(0));
							_app.Date = DateTime.Parse(query.GetString(1));
							switch (query.GetString(2))
							{
								case ("Pending"):
									_app.Status = AppointmentStatus.Pending;
									break;
								case ("Approved"):
									_app.Status = AppointmentStatus.Approved;
									break;
								case ("Denied"):
									_app.Status = AppointmentStatus.Denied;
									break;
								case ("Skipped"):
									_app.Status = AppointmentStatus.Skipped;
									break;
								default:
									_app.Status = AppointmentStatus.Pending;
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
									_app.Submitted = false;
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

							_Apps.Add(_app);
						}
						catch (Exception e)
						{
							Debug.WriteLine("AssignUser Error");
						}
					}
				}
				db.Close();
			}
			return _Apps;
		}

		public bool InsertAppointment(Appointment _App)
		{
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				try
				{
					db.Open();
					SqliteCommand _insertApp = new SqliteCommand();
					SqliteCommand _insertUser = new SqliteCommand();
					_insertUser.Connection = db;
					_insertApp.Connection = db;
					_insertApp.CommandText = "insert into Appointment_tbl (Date, Status, Submitted,FK_CID,FK_SMID,FK_AdID ) values  ('" + _App.Date + "', '" + _App.Status.ToString() + "', " + _App.Submitted + "," + _App.Customer.ID + "," + _App.SiteManager.ID + "," + _App.Address.ID + " )";
					try
					{
						_insertApp.ExecuteReader();

					}
					catch (SqliteException error)
					{
						Debug.WriteLine("Insert Address Error");
						return false;
					}

			
					db.Close();
				}
				catch (Exception ex)
				{
					Debug.WriteLine("btnSave.Click() Error");
					return false;
				}
				return true;
			}
		}
	}
}
