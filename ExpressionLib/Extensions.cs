using System;
using System.Linq;

namespace ExpressionLib
{
    static class Extensions
    {
        public static bool IsASCIIDigit(this char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsStartOfNumber(this char c)
        {
            return c.IsASCIIDigit() || c == '.';
        }

        public static bool IsASCIILetter(this char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public static bool IsBinOperator(this char c)
        {
            return "+-*/^".Contains(c);
        }

        public static void SkipSpaces(this string expression, ref int p)
        {
            while (p < expression.Length && expression[p] == ' ') p++;
        }

        /// <summary>
        /// Reads contiguous letters from a string starting from an index
        /// The index is passed by ref and on return points to the next non ASCII character
        /// </summary>
        /// <param name="expression">The string to parse.</param>
        /// <param name="p">Index from where to start reading</param>
        /// <returns>The string read</returns>
        public static string ReadLetters(this string expression, ref int p)
        {
            int a = p;
            while (p < expression.Length && expression[p].IsASCIILetter()) p++;
            return expression.Substring(a, p - a);
        }

        /// <summary>
        /// Reads a double from inside a string starting from an index
        /// The index is passed by ref and on return points to the first character after the double read
        /// </summary>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <param name="expression"></param>
        /// <param name="p">Index from where to parse the double</param>
        /// <returns>The Double read</returns>
        public static double ReadDouble(this string expression, ref int p)
        {
            int a = p;
            while (p < expression.Length && expression[p].IsASCIIDigit()) p++;
            if (p < expression.Length && expression[p] == '.')
            {
                p++;
                while (p < expression.Length && expression[p].IsASCIIDigit()) p++;
                if (p - a < 2)
                    throw new FormatException("Digit expected after the point");
            }
            if (p < expression.Length && char.ToLower(expression[p]) == 'e')
            {
                p++;
                if (p < expression.Length && (expression[p] == '+' || expression[p] == '-'))
                    p++;
                while (p < expression.Length && expression[p].IsASCIIDigit()) p++;
            }
            return Double.Parse(expression.Substring(a, p - a));         // Possible exceptions including OverflowException
        }
    }
}
