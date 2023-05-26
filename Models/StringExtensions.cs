using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingCalculator.Models {
    public static class StringExtensions {
        public static string Repeat(this string s, int repeats)
            => new StringBuilder().Insert(0, s, repeats).ToString();
    }
}
