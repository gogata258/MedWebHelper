using System;

namespace MedHelper.Common.Extensions
{
	public static class Extensions
	{
		public static double ActualTime(this DateTime? dateTime) => (dateTime.Value.Hour * 60) + dateTime.Value.Minute;
		public static double ActualTime(this DateTime dateTime) => (dateTime.Hour * 60) + dateTime.Minute;
	}
}
