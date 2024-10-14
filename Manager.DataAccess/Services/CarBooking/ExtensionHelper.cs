using System.Globalization;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Manager.DataAccess.Services.CarBooking
{
    public static class ExtensionHelper
    {
        public static string format_datetime(string input)
        {
            DateTime value = DateTime.ParseExact(input, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return value.ToString("dd-MM-yyyy");
        }
        public static string format_currency(decimal amount)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            return string.Format(culture, "{0:C0}", amount);
        }
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static int get_month(string dateRange)
        {
            //string dateRange = "2024/4/1 - 2024/4/30";

            // Tách chuỗi thành các phần bằng dấu cách
            string[] parts = dateRange.Split(' ');

            // Lấy phần tháng từ phần tử đầu tiên trong mảng phân tách
            string startDate = parts[0];
            string[] startDateParts = startDate.Split('/');
            int month = int.Parse(startDateParts[1]);
            return month;
        }
    }
}
