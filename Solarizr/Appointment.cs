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

    }

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
}
