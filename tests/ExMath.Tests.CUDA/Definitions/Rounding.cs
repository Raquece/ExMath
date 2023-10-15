using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.CUDA.Definitions
{
    public static class Rounding
    {
        public static double Truncate(this double value, int sigFigures)
        {
            var digits = value.ToString();
            digits = digits.Replace(".", "");

            if (digits.Length > sigFigures)
            {
                double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1 - sigFigures);
                return scale * Math.Truncate(value / scale);
            }

            return value;
        }
    }
}
