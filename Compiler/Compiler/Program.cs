﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compiler.Compilers;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = (File.ReadAllLines(Environment.CurrentDirectory + @"\..\..\Language.txt"));

            Tokenizer tokenizer = new Tokenizer(lines);

            Compilers.Compiler compiler = new Compilers.Compiler();
            compiler.TokenList = tokenizer.tokenList;

            VirtualMachine vm = new VirtualMachine();

            vm.Run(compiler.compile());

            Console.ReadKey();

        }
    }
}
