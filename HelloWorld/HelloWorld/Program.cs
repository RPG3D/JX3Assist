using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Learn C Sharp";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("Hello World From C Sharp!");
            Console.WriteLine("The Main() has {0} parameters.", args.Length);

            DatabaseReader dr = new DatabaseReader();
            int? i = dr.GetIntFromDatabase();
            if(i.HasValue)
            {
                Console.WriteLine("Value of i is {0}", i.Value);
            }
            else
            {
                Console.WriteLine("Value of i is undefined");
         
            }

            bool? b = dr.GetBoolFromDatabase();
            if(b!= null)
            {
                Console.WriteLine("value of b is {0}", b.Value);
            }
            else
            {
                Console.WriteLine("Value of b is undefined");
            }

            Console.ReadLine();
            MessageBox.Show("Application End");
        }
    }
}
