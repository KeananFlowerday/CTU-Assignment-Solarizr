using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Solarizr
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
	{
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;


			SqliteEngine.UseWinSqlite3(); //Configuring library to use SDK version of SQLite
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_DB.db"))
			{
				db.Open();

				String _AddresstableCommand = "CREATE TABLE IF NOT EXISTS Address_tbl (" +
					"PK_ID INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Street text NULL, " +
					"Suburb text NULL, " +
					"City text NULL, " +
					"Postal_Code text NULL,"+
					"Country text NULL);";

				String _UsertableCommand = "CREATE TABLE IF NOT EXISTS User_tbl (" +
					"PK_ID INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Name text NULL, " +
					"Phone text NULL," +
					"FK_AdID INTEGER NULL," +
					"FOREIGN KEY (FK_AdID) REFERENCES Address_tbl(PK_ID));";

				String _AppointmenttableCommand = "CREATE TABLE IF NOT EXISTS Appointment_tbl (" +
					"PK_ID INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Date text NULL, " +
					"Status text NULL, " +
					"Submitted BLOB NULL, " +
					"FK_CID INTEGER NULL, " +
					"FK_SMID INTEGER NULL, " +
					"FK_AdID INTEGER NULL, " +
					"FOREIGN KEY (FK_CID) REFERENCES user_tbl(PK_ID)," +
					"FOREIGN KEY (FK_SMID) REFERENCES user_tbl(PK_ID)," +
					"FOREIGN KEY (FK_AdID) REFERENCES Address_tbl(PK_ID));";

				String _NotetableCommand = "CREATE TABLE IF NOT EXISTS Note_tbl (" +
					"PK_ID INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Note text NULL, " +
					"FK_ApID INTEGER NULL, " +
					"FOREIGN KEY (FK_ApID) REFERENCES Appointment_tbl(PK_ID));";

				String _PicturetableCommand = "CREATE TABLE IF NOT EXISTS Picture_tbl (" +
					"PK_ID INTEGER PRIMARY KEY AUTOINCREMENT," +
					"Path text NULL, " +
					"FK_ApID INTEGER NULL, " +
					"FOREIGN KEY (FK_ApID) REFERENCES Appointment_tbl(PK_ID));";

				SqliteCommand _createUserTable = new SqliteCommand(_AddresstableCommand, db);
				SqliteCommand _createAddressTable = new SqliteCommand(_UsertableCommand, db);
				SqliteCommand _createAppointmentTable = new SqliteCommand(_AppointmenttableCommand, db);
				SqliteCommand _createNoteTable = new SqliteCommand(_NotetableCommand, db);
				SqliteCommand _createPictureTable = new SqliteCommand(_PicturetableCommand, db);

				try
				{
					_createAddressTable.ExecuteReader();
					_createUserTable.ExecuteReader();
					_createAppointmentTable.ExecuteReader();
					_createNoteTable.ExecuteReader();
					_createPictureTable.ExecuteReader();
				}
				catch (SqliteException e)
				{
					Debug.WriteLine("DB CREATION FAILED");
					Debug.WriteLine("");
					Debug.WriteLine(e.Message + ": " + e.InnerException);
					Debug.WriteLine(e.SqliteErrorCode);
					Debug.WriteLine(e.StackTrace);

				}
				db.Close();
			}
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					// When the navigation stack isn't restored navigate to the first page,
					// configuring the new page by passing required information as a navigation
					// parameter
					rootFrame.Navigate(typeof(Dashboard), e.Arguments);
				}
				// Ensure the current window is active
				Window.Current.Activate();
			}
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}

		public void InsertData()
		{
		
			
			//Address _address = _user.Address;
			//using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			//{
			//	try
			//	{
			//		db.Open();
			//		SqliteCommand _insertAddress = new SqliteCommand();
			//		SqliteCommand _insertUser = new SqliteCommand();
			//		_insertUser.Connection = db;
			//		_insertAddress.Connection = db;
			//		_insertAddress.CommandText = "INSERT INTO Address_tbl (Street, Suburb, City, Postal_Code, Country) VALUES('" + _address.Street + "','" + _address.Suburb + "','" + _address.City + "','" + _address.PostalCode + "','" + _address.Country + "'); ";
			//		try
			//		{
			//			_insertAddress.ExecuteReader();

			//		}
			//		catch (SqliteException error)
			//		{
			//			Debug.WriteLine("Insert Address Error");
			//			return false;
			//		}

			//		int _adIndex = Address.LastIndex();

			//		if (_adIndex == -1)
			//		{
			//			throw new SqliteException("You Done Fucked Up", 500);
			//		}

			//		//Use parameterized query to prevent SQL injection attacks
			//		_insertUser.CommandText = "INSERT INTO User_tbl(Name, Phone, FK_AdID) VALUES ('" + _user.Name + "','" + _user.Phone + "','" + _adIndex + "');";

			//		try
			//		{
			//			//_insertAddress.ExecuteReader();
			//			_insertUser.ExecuteReader();
			//		}
			//		catch (SqliteException error)
			//		{
			//			Debug.WriteLine("Insert User Error");
			//			return false;
			//		}
			//		db.Close();
			//	}
			//	catch (Exception ex)
			//	{
			//		Debug.WriteLine("btnSave.Click() Error");
			//		return false;
			//	}
			//	return true;
			//}
		}
	}
}
