using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Entities
{
    public static class EntityExtensions
    {
        public static string ToGenericString(this object entity)
        {
            if (entity == null)
                return "null";

            var type = entity.GetType();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType)
                    || p.PropertyType == typeof(string))
                .Select(p => $"{p.Name}='{p.GetValue(entity) ?? "null"}'");

            return $"{type.Name} {{ {string.Join(", ", props)} }}";
        }
    }
}
