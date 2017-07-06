using System;
using ExpressionLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string expString;
            Expression exp= null;
            while ((expString = Console.ReadLine()) != string.Empty)
            {
                try
                {
                    exp = new Expression(expString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine(exp.Evaluate());
            }
        }
    }
}
