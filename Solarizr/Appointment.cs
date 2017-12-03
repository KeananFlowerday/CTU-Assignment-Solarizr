using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
    class Appointment
    {
		public Appointment()
		{

		}
		public Appointment (DateTime date, AppointmentStatus status, User customer, Address address, User siteManager)
		{
		
			Date = date;
			Status = status;
			Customer = customer;
			Address = address;
			SiteManager = siteManager;
		}

		public int ID { get; set; }
		public DateTime Date { get; set; }
		public AppointmentStatus Status { get; set; }
		public User Customer { get; set; }
		public Address Address { get; set; }
		public User SiteManager { get; set; }

	}

	public enum AppointmentStatus
	{
		UnSubmitted,
		Submitted
	}
}
