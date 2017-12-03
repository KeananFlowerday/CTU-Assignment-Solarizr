using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
    class Note
		
    {
		public Note()
		{

		}
		public Note( string content, Appointment appointment)
		{
			
			Content = content;
			Appointment = appointment;
		}

		public int ID { get; set; }
		public string Content { get; set; }
		public  Appointment Appointment { get; set; }
	}
}
