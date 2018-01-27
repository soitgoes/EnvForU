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
            var env = new Settings();
            //inject into IoC for use throughout the application

            //Remember Environment Variables take precedence to .env file!

            //IF git is detected we get the latest tag available and insert it into version
            Console.WriteLine("Version: " + env.Get("VERSION"));

        }
    }
}
