﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.AulaPratica4
{
    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Aula Prática de C# 4!");
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Tasks e Async/Await com IO Bound");
            Console.WriteLine("---------------------------------------");

            PraticaIOAsync praticaIOAsync = new PraticaIOAsync();
            await praticaIOAsync.Exercicio2Async();
        }

       
    }
}
