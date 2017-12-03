using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
    class Address
    {
		public Address()
		{

		}
		public Address( string street, string suburb, string city, string postalCode, string country)
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
	}
}
