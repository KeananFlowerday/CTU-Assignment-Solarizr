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
    class ProjectSiteData
    {
		public User GetUserById(int _id)
		{
			User _u = new User();
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				db.Open();
				SqliteCommand selectCommand = new SqliteCommand("SELECT * from User_tbl INNER JOIN Address_tbl on User_tbl.FK_AdID = Address_tbl.PK_ID where User_tbl.PK_ID = "+_id+";", db);
				SqliteDataReader query;
				try
				{
					query = selectCommand.ExecuteReader();
				}
				catch (SqliteException error)
				{
					Debug.WriteLine("SelectCommand Error");
					return _u;
				}
				while (query.Read())
				{
					try
					{
						Address _a = new Address();
						_a.ID = int.Parse(query.GetString(4));
						_a.Street = query.GetString(5);
						_a.Suburb = query.GetString(6);
						_a.City = query.GetString(7);
						_a.PostalCode = query.GetString(8);
						_a.Country = query.GetString(9);
						_u = new User(query.GetString(1), query.GetString(2), _a);
						_u.ID = int.Parse(query.GetString(0));
						
					}
					catch (Exception e)
					{
						Debug.WriteLine("AssignUser Error");
					}
				}
				db.Close();
			}
			return _u;
		
			
		}

		public ObservableCollection<User> GetUsersById(int _id)
		{
			//Will be added if needed
			return null;
		}

		public ObservableCollection<User> GetAllSites()
		{
			ObservableCollection<User> _users = new ObservableCollection<User>();
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				db.Open();
				SqliteCommand selectCommand = new SqliteCommand("SELECT * from User_tbl INNER JOIN Address_tbl on User_tbl.FK_AdID = Address_tbl.PK_ID;", db);
				SqliteDataReader query;
				try
				{
					query = selectCommand.ExecuteReader();
				}
				catch (SqliteException error)
				{
					Debug.WriteLine("SelectCommand Error");
					return _users;
				}
				while (query.Read())
				{
					try
					{
						Address _a = new Address();
						_a.ID = int.Parse(query.GetString(4));
						_a.Street = query.GetString(5);
						_a.Suburb = query.GetString(6);
						_a.City = query.GetString(7);
						_a.PostalCode = query.GetString(8);
						_a.Country = query.GetString(9);
						User _u = new User(query.GetString(1), query.GetString(2), _a);
						_u.ID = int.Parse(query.GetString(0));
						_users.Add(_u);
					}
					catch (Exception e)
					{
						Debug.WriteLine("AssignUser Error");
					}
				}
				db.Close();
			}
			return _users;
		}

		public bool InsertUser(User _user)
		{
			Address _address = _user.Address;
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
						return false;
					}

					int _adIndex = Address.LastIndex();

					if (_adIndex == -1)
					{
						throw new SqliteException("You Done Fucked Up", 500);
					}

					//Use parameterized query to prevent SQL injection attacks
					_insertUser.CommandText = "INSERT INTO User_tbl(Name, Phone, FK_AdID) VALUES ('" + _user.Name + "','" + _user.Phone + "','" + _adIndex + "');";

					try
					{
						//_insertAddress.ExecuteReader();
						_insertUser.ExecuteReader();
					}
					catch (SqliteException error)
					{
						Debug.WriteLine("Insert User Error");
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
