using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using static System.DateTime;
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
            Console.WriteLine(Now.Date);

            Car c = new Car("Yu", 100, 10);
            c.RegisterWithCarEngine(new Car.CarEngineHandler(c.OnCarEngineEvent));
            Console.WriteLine("Speed Up");
            for(int i = 0; i < 6; ++i)
            {
                c.Accelerate(20);
            }

            Form mainForm = new MainForm();
            mainForm.Show();
            Console.ReadLine();
            MessageBox.Show("Application End");
        }
    }
}
