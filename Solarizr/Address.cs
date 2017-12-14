using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
	public class Address
	{
		public Address()
		{

		}
		public Address(string street, string suburb, string city, string postalCode, string country)
		{

			Street = street;
			Suburb = suburb;
			City = city;
			PostalCode = postalCode;
			Country = country;
		}

		public int ID { get; set; }
		public string Street { get; set; }
		public string Suburb { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }

		public static int LastIndex()
		{
			int _return = -1;
			using (SqliteConnection db = new SqliteConnection("Filename=Solarizr_db.db"))
			{
				db.Open();
				SqliteCommand selectCommand = new SqliteCommand("SELECT PK_ID FROM Address_tbl ORDER BY PK_ID DESC LIMIT 1;", db);
				SqliteDataReader query;
				try
				{
					query = selectCommand.ExecuteReader();
				}
				catch (SqliteException error)
				{
					//Handle error
					return -1;
				}
				while (query.Read())
				{
					string _ret = (query.GetString(0));
					int.TryParse(_ret, out _return);
				}
				db.Close();
			}
			return _return;
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3}", Street, Suburb, City, Country);
		}
	}

}
