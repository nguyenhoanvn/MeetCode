using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.Enums
{
    public static class TestCaseVisibility
    {
        public const string Sample = "sample";
        public const string Public = "public";
        public const string Hidden = "hidden";

        public static bool IsValid(string visibility) => visibility is Sample or Public or Hidden;
    }
}
