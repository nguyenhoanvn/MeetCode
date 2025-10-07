using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Domain.Enums
{
    public static class ProblemDifficulty
    {
        public const string Easy = "easy";
        public const string Medium = "medium";
        public const string Hard = "hard";

        public static bool IsValid(string difficulty) => difficulty is Easy or Medium or Hard;
    }
}
