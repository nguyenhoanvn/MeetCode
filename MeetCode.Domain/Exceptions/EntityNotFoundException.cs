using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string FieldName { get; }
        public string FieldValue { get; }

        public EntityNotFoundException(string entityName, string fieldName, string fieldValue)
            : base($"{entityName} with '{fieldName}' {fieldValue} cannot found")
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}
