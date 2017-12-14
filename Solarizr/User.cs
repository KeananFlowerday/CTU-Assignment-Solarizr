using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
    public class  User
    {
		public User()
		{

		}
		public User( string name, string phone, Address address)
		{
			
			Name = name;
			Phone = phone;
			Address = address;
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public Address Address { get; set; }
	}
}
