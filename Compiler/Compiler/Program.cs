using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //nick1 or nickv
                //using (StreamReader sr = new StreamReader("C:/Users/Joep/Documents/Visual Studio 2015/Projects/Tokenizer/Tokenizer/Voorbeeldscript.txt"))
                //{
                string[] lines = File.ReadAllLines("C:/Users/Joep/Documents/Visual Studio 2015/Projects/Tokenizer/Tokenizer/Voorbeeldscript.txt");

                Tokenizer tokenizer = new Tokenizer(lines);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }
    }
}
