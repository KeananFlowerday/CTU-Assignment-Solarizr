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
<<<<<<< HEAD

        public Appointment(DateTime date, User customer, Address address, User siteManager)
        {

            Date = date;
            Status = AppointmentStatus.Pending;
            Customer = customer;
            Address = address;
            SiteManager = siteManager;
            Submitted = Submitted.No;
        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }
        public User Customer { get; set; }
        public Address Address { get; set; }
        public User SiteManager { get; set; }
        public Submitted Submitted { get; set; }
=======
		public Appointment (DateTime date, AppointmentStatus status, User customer, Address address, User siteManager)
		{
		
			Date = date;
			Status = status;
			Customer = customer;
			Address = address;
			SiteManager = siteManager;
			Submitted = false;
		}

		public int ID { get; set; }
		public DateTime Date { get; set; }
		public AppointmentStatus Status { get; set; }
		public User Customer { get; set; }
		public Address Address { get; set; }
		public User SiteManager { get; set; }
		public bool Submitted { get; set; }
>>>>>>> e8a434f979499d4b360e9e13bae929c4412890e2

    }

<<<<<<< HEAD
    public enum AppointmentStatus
    {
        Pending,
        Approved,
        Denied,
        Skipped
    }

    public enum Submitted
    {
        Yes,
        No
    }
=======
	public enum AppointmentStatus
	{
		Pending,
		Approved,
		Denied,
		Skipped
	}
>>>>>>> e8a434f979499d4b360e9e13bae929c4412890e2
}
