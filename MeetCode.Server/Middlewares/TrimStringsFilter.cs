using Microsoft.AspNetCore.Mvc.Filters;

namespace MeetCode.Server.Middlewares
{
    public class TrimStringsFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var arg in context.ActionArguments.Values)
            {
                TrimStringsInObject(arg);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private void TrimStringsInObject(object? obj)
        {
            if (obj == null) return;

            var stringProps = obj.GetType()
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

            foreach(var prop in stringProps)
            {
                var val = (string?)prop.GetValue(obj);
                if (!string.IsNullOrWhiteSpace(val))
                {
                    prop.SetValue(obj, val.Trim());
                }
            }

            var complexProps = obj.GetType()
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(p => p.PropertyType.IsClass && p.PropertyType == typeof(string));

            foreach (var prop in complexProps)
            {
                var val = prop.GetValue(obj);
                if (val is IEnumerable<object> list)
                {
                    foreach(var item in list)
                    {
                        TrimStringsInObject(item);
                    }
                } else
                {
                    TrimStringsInObject(val);
                }
            }
        }
    }
}
