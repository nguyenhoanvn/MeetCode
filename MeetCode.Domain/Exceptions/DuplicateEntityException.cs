using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Exceptions
{


     public class DuplicateEntityException<TEntity> : Exception
     {
        public IReadOnlyDictionary<string, string> FieldPairs { get; }

        public DuplicateEntityException(
            IDictionary<string, string> fieldPairs)
            : base(BuildMessage(fieldPairs))
        {
            FieldPairs = new Dictionary<string, string>(fieldPairs);
        }

        private static string BuildMessage(IDictionary<string, string> fieldPairs)
        {
            var pairs = string.Join(", ", fieldPairs.Select(kv => $"{kv.Key}='{kv.Value}'"));
            return $"{typeof(TEntity).Name} with {pairs} already exists.";
        }
     }
}
