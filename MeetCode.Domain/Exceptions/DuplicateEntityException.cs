using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Exceptions
{
    public class DuplicateEntityException<TEntity> : Exception
    {
        public string FieldName { get; }
        public string FieldValue { get; }
        public DuplicateEntityException(string fieldName, string fieldValue)
            : base($"{typeof(TEntity).Name} with {fieldName} '{fieldValue}' already exists") 
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}
