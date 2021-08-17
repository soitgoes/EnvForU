using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvForU;

namespace ExampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new Settings("../../.env");
            //inject into IoC for use throughout the application

            //Remember Environment Variables take precedence to .env file unless the first line of the file is !!

            foreach (var key in env.Keys)
                Console.WriteLine($"{key}={env[key]}");
            Console.ReadLine();
        }
    }
}
