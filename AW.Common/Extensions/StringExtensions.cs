using System;
using AW.Common.SecurityToolkit;
using static System.Int64;

namespace AW.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetSha256Hash(this string str)
        {
            var hashValue = new Security().GetSha256Hash(str);

            return hashValue;
        }

        #region ConvertStringTo

        public static int GetInt(this string str)
        {
            int result;

            int.TryParse(GetString(str), out result);

            return result;
        }

        public static short GetInt16(this string str)
        {
            short result;

            short.TryParse(GetString(str), out result);

            return result;
        }

        public static long GetInt64(this string str)
        {
            long result;

            TryParse(GetString(str), out result);

            return result;
        }

        public static long? GetInt64Nullable(this string str)
        {
            long result;

            var ok = TryParse(GetString(str), out result);

            return !ok ? (long?)null : result;
        }

        public static float GetFloat(this string str)
        {
            float result;

            float.TryParse(GetString(str), out result);

            return result;
        }
        public static decimal GetDecimal(this string str)
        {
            decimal result;

            decimal.TryParse(GetString(str), out result);

            return result;
        }
        public static float? GetFloatNullable(this string str)
        {
            float result;

            var ok = float.TryParse(GetString(str), out result);

            return !ok ? (float?)null : result;
        }
        public static decimal? GetDecimalNullable(this string str)
        {
            decimal result;

            var ok = decimal.TryParse(GetString(str), out result);

            return !ok ? (decimal?)null : result;
        }
        public static int? GetIntNullable(this string str, int? initialValue = null)
        {
            int result;

            var ok = int.TryParse(GetString(str), out result);
            return initialValue.HasValue && result == initialValue.Value ? (int?)null : (!ok ? (int?)null : result);
        }

        public static DateTime GetDateTime(this string str)
        {
            var strStr = GetString(str);

            if (string.IsNullOrWhiteSpace(strStr)) return DateTime.Now;
            var result = Convert.ToDateTime(GetString((str)));


            return result;
        }

        public static DateTime? GetDateTimeNullable(this string str)
        {
            DateTime? result = null;
            var strValue = GetString(str);
            if (!string.IsNullOrWhiteSpace(strValue))
            {
                result = Convert.ToDateTime(strValue);
            }

            return result;
        }

        public static string GetString(this string str)
        {
            var strStr = str + "";

            return strStr;
        }

        public static string GetStringNullable(this string str)
        {
            var strStr = str + "";

            return string.IsNullOrWhiteSpace(strStr) ? null : strStr;
        }

        public static bool GetBool(this string str)
        {
            var strStr = GetString(str);

            return !string.IsNullOrWhiteSpace(strStr) && bool.Parse(strStr);
        }

        public static bool? GetBoolNullable(this string str)
        {
            var strStr = GetString(str);

            return !string.IsNullOrWhiteSpace(strStr) ? bool.Parse(strStr) : (bool?)null;

        }

        public static Guid GetGuid(this string str)
        {
            var strGuid = GetString(str);
            return string.IsNullOrWhiteSpace(strGuid) ? Guid.Empty : Guid.Parse(strGuid);
        }

        public static Guid? GetGuidNullable(this string str)
        {
            var strGuid = GetString(str);
            return string.IsNullOrWhiteSpace(strGuid) ? (Guid?)null : Guid.Parse(strGuid);
        }

        public static byte? GetByteNullable(this string str)
        {
            var strByte = GetString(str);
            return string.IsNullOrWhiteSpace(strByte) ? (byte?)null : byte.Parse(strByte);
        }

        public static byte GetByte(this string str)
        {
            var strByte = GetString(str);
            byte result = 0;
            byte.TryParse(strByte, out result);

            return result;
        }

        public static long? GetLongNullable(this string str)
        {
            var strLong = GetString(str);
            return string.IsNullOrWhiteSpace(strLong) ? (long?)null : Parse(strLong);
        }

        public static TimeSpan GetTimeSpan(this string str)
        {
            var strByte = GetString(str);
            return TimeSpan.Parse(strByte);
        }

        public static TimeSpan? GetTimeSpanNullable(this string str)
        {
            TimeSpan result;
            var ok = TimeSpan.TryParse(GetString(str), out result);

            return ok ? result : (TimeSpan?)null;
        }

        public static double GetDouble(this string str)
        {
            var strByte = GetString(str);
            return double.Parse(strByte);
        }

        public static double? GetDoubleNullable(this string str)
        {
            double result;
            var ok = double.TryParse(GetString(str), out result);

            return ok ? result : (double?)null;
        }


        #endregion

        #region ConvertObjectTo

        public static int GetInt(this object str)
        {
            return GetString(str).GetInt();

        }

        public static short GetInt16(this object str)
        {
            return GetString(str).GetInt16();

        }

        public static long GetInt64(this object str)
        {
            return GetString(str).GetInt64();

        }

        public static long? GetInt64Nullable(this object str)
        {
            return GetString(str).GetInt64Nullable();

        }

        public static float GetFloat(this object str)
        {
            return GetString(str).GetFloat();

        }
        public static decimal GetDecimal(this object str)
        {
            return GetString(str).GetDecimal();

        }
        public static float? GetFloatNullable(this object str)
        {
            return GetString(str).GetFloatNullable();

        }
        public static decimal? GetDecimalNullable(this object str)
        {
            return GetString(str).GetDecimalNullable();

        }
        public static int? GetIntNullable(this object str, int? initialValue = null)
        {
            return GetString(str).GetIntNullable(initialValue);

        }

        public static DateTime GetDateTime(this object str)
        {
            return GetString(str).GetDateTime();

        }

        public static DateTime? GetDateTimeNullable(this object str)
        {
            return GetString(str).GetDateTimeNullable();

        }

        public static string GetString(this object str)
        {
            var strStr = str + "";

            return strStr;
        }

        public static string GetStringNullable(this object str)
        {
            var strStr = str + "";

            return string.IsNullOrWhiteSpace(strStr) ? null : strStr;
        }

        public static bool GetBool(this object str)
        {
            return GetString(str).GetBool();

        }

        public static bool? GetBoolNullable(this object str)
        {
            return GetString(str).GetBoolNullable();


        }

        public static Guid GetGuid(this object str)
        {
            return GetString(str).GetGuid();

        }

        public static Guid? GetGuidNullable(this object str)
        {
            return GetString(str).GetGuidNullable();

        }

        public static byte? GetByteNullable(this object str)
        {
            return GetString(str).GetByteNullable();

        }

        public static byte GetByte(this object str)
        {
            return GetString(str).GetByte();

        }

        public static long? GetLongNullable(this object str)
        {
            return GetString(str).GetLongNullable();

        }

        public static TimeSpan GetTimeSpan(this object str)
        {
            return GetString(str).GetTimeSpan();

        }

        public static TimeSpan? GetTimeSpanNullable(this object str)
        {
            return GetString(str).GetTimeSpanNullable();

        }

        public static double GetDouble(this object str)
        {
            return GetString(str).GetDouble();

        }

        public static double? GetDoubleNullable(this object str)
        {
            return GetString(str).GetDoubleNullable();

        }


        #endregion


    }

}

