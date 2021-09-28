using System;
using System.Linq;

namespace HotelFunctions
{
    public static class ObjectExtensions
    {
        public static object BindStringMembersFromConnectionString(this object obj, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            var dict = connectionString
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Split(new[] { '=' }, 2))
                .ToDictionary(key => key[0].Trim(), value => value[1].Trim());

            foreach (var property in obj.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                if (dict.TryGetValue(property.Name, out var name))
                {
                    property.SetValue(obj, name);
                }
            }

            return obj;
        }
    }
}