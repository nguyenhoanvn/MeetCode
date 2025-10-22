using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public string FieldName { get; }
        public string FieldValue { get; }

        public EntityNotFoundException(string fieldName, string fieldValue)
            : base($"{typeof(TEntity).Name} with '{fieldName}' {fieldValue} cannot found")
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}
