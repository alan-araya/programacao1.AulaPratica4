using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.AulaPratica4
{
    public class PessoaFinder
    {
        public async Task ExercicioPessoaAsync()
        {
            Console.WriteLine("Gerando 10M pessoas....");
            var arrayPessoas = new Pessoa[10000000];

            for (int i = 0; i < arrayPessoas.Length; i++)
            {
                arrayPessoas[i] = new Pessoa
                {
                    Id = i,
                    Nome = $"Pessoa {i}"
                };
            }

            List<Pessoa> listaPessoas = arrayPessoas.ToList();
            HashSet<Pessoa> hashSetPessoas = new HashSet<Pessoa>(arrayPessoas);


            var comandoSair = "S";
            var comandoLido = string.Empty;

            while (true)
            {
                Console.WriteLine("Informe o numero de uma pessoa de 0 a 10M:");

                comandoLido = Console.ReadLine();
                if (comandoSair.Equals(comandoLido))
                {
                    break;
                }

                var numeroPessoa = int.Parse(comandoLido);

                var pessoa = new Pessoa()
                {
                    Id = numeroPessoa,
                    Nome = $"Pessoa {numeroPessoa}"
                };


                var task1 = Task.Run(() => 
                {
                    var cronometro = new Stopwatch();
                    cronometro.Start();

                    for (int i = 0; i < arrayPessoas.Length; i++)
                    {
                        if (arrayPessoas[i] == pessoa)
                        {
                            break;
                        }
                    }
                    cronometro.Stop();

                    return cronometro.Elapsed.TotalSeconds;
                });

                var task2 = Task.Run(() =>
                {
                    var cronometro = new Stopwatch();
                    cronometro.Start();

                    var pessoaFound = listaPessoas.Find((p) => { return p == pessoa; });

                    cronometro.Stop();

                    return cronometro.Elapsed.TotalSeconds;
                });

                var task3 = Task.Run(() =>
                {
                    var cronometro = new Stopwatch();
                    cronometro.Start();

                    var pessoaFound = hashSetPessoas.Contains(pessoa);

                    cronometro.Stop();

                    return cronometro.Elapsed.TotalSeconds;
                });


                await Task.WhenAll(task1, task2, task3);


                Console.WriteLine($"Duração Task1:{task1.Result}s");
                Console.WriteLine($"Duração Task2:{task2.Result}s");
                Console.WriteLine($"Duração Task3:{task3.Result}s");
            }




        }
    }

    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not Pessoa)
            {
                return false;
            }
            return ((Pessoa)obj).Id == Id;
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }

}
