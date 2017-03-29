using System;
using System.Globalization;

namespace AW.Application
{
    public class CommonFunction
    {
        

        public static string ConvertToShamsi(DateTime? miladi)
        {
            if (!miladi.HasValue)
                return null;

            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0} {1} {2} {3}", pc.GetYear(miladi.Value), GetNameMonth(pc.GetMonth(miladi.Value)), pc.GetDayOfMonth(miladi.Value), GetWeekDay(pc.GetDayOfWeek(miladi.Value)));
        }


        public static string GetWeekDay(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یک شنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    return "";
            }
        }

        public static string GetNameMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }
    }

     
}
