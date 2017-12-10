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
		public User GetUser(int _id)
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

		public ObservableCollection<User> GetUsers(int _id)
		{
			return null;
		}

		public ObservableCollection<User> GetAllUsers()
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
	}
}
