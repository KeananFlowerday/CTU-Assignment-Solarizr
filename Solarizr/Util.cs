using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solarizr
{
	class Util
	{

		public static string GetDateTime()
		{
			var shortDateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("shortdate");
			var shortTimeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("shorttime");

			var dateTimeToFormat = DateTime.Now;

			var shortDate = shortDateFormatter.Format(dateTimeToFormat);
			var shortTime = shortTimeFormatter.Format(dateTimeToFormat);

			return   shortDate + " " + shortTime;
		}
	}
}
