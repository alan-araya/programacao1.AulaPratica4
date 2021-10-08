using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.AulaPratica4
{
    public class ThreadPing
    {
        private string endereco = "google.com";
        int countPing = 0;
        bool executePing = false;

        public void StartPing()
        {
            Console.WriteLine("Digite: S a qualquer momento para interromper o ping");

            var threadPingger = new Thread(ExecutePing);
            threadPingger.Start();
            executePing = true; //mantém a thread em execução continua

            var comandoSair = "S";
            var comandoLido = string.Empty;

            while (!comandoSair.Equals(comandoLido))
            {
                comandoLido = Console.ReadLine();
            }
            executePing = false;

            //Espera finalizar
            while (threadPingger.IsAlive)
            {
                Console.WriteLine("Esperando finalizar...");
            }

            Console.WriteLine("Thread finalizada!");
            Console.WriteLine("-----------------------------------");
        }

        public void ExecutePing()
        {
            while (executePing)
            {
                Ping pingger = new Ping();

                var pingResponse = pingger.Send(endereco);

                Console.WriteLine($"Ping {countPing}: {endereco} | Status: {pingResponse.Status} - {pingResponse.RoundtripTime}ms");
                countPing++;

                //espera alguns segundos
                Thread.Sleep(2000);
            }
        }
    }
}
