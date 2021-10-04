using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.AulaPratica4
{
    public class PraticaIOAsync
    {
        public class Diretorio
        {
            public string Nome { get; set; }
            public decimal TamanhoEmMB { get; set; }

            public List<Arquivo> Arquivos { get; set; }
        }
        public class Arquivo
        {
            public string Nome { get; set; }
            public decimal TamanhoEmMB { get; set; }
        }


        public async Task Exercicio2Async()
        {
            Console.WriteLine("Informe uma lisa de diretórios para contarmos a quantidade de arquivos....");
            Console.WriteLine("Digito 'S' para finalizar o input:");

            string diretorio = string.Empty;
            string comandoStop = "S";
            var listaDiretorios = new List<string>();

            do
            {
                try
                {
                    diretorio = Console.ReadLine();

                    if (!Directory.Exists(diretorio))
                    {
                        Console.WriteLine("-> Diretório não encontrado! Verifique o input novamente!");
                        continue;
                    }

                    listaDiretorios.Add(diretorio);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Erro ao processar o input de diretórios. Tente novamente.");
                }

            } while (!comandoStop.Equals(diretorio));


            //Inicia uma task para cada dir
            var diretoriosCalculado = new List<Diretorio>();
            var listaTasks = new List<Task>(listaDiretorios.Count);
            foreach (var dir in listaDiretorios)
            {
                var taskDir = Task.Run(async ()=> 
                {
                    var diretorioCalculado = new Diretorio
                    {
                        Nome = dir
                    };

                    var arquivos = await FindDirFilesAsync(dir);

                    diretorioCalculado.Arquivos = arquivos;
                    diretorioCalculado.TamanhoEmMB = arquivos.Sum(a => a.TamanhoEmMB);
                    
                    diretoriosCalculado.Add(diretorioCalculado);
                });

                listaTasks.Add(taskDir);
            }

            await Task.WhenAll(listaTasks);

            Console.WriteLine("***********************");
            Console.WriteLine("Processo finalizado!");
            Console.WriteLine("***********************");

            foreach (var diretorioCalc in diretoriosCalculado)
            {
                Console.WriteLine($"Dir: {diretorioCalc.Nome} | Quantidade de Arquivos: {diretorioCalc.Arquivos.Count} | Tamanho: {diretorioCalc.TamanhoEmMB} MB");
            }

        }

        public async Task<List<Arquivo>> FindDirFilesAsync(string path)
        {
            var files = await Task.Run(() => 
            {
                var filesDir = Directory.GetFiles(path, "*", new EnumerationOptions() { RecurseSubdirectories = true });
                return filesDir;
            });

            var arquivos = new List<Arquivo>(files.Length);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                arquivos.Add(new Arquivo
                {
                    Nome = fileInfo.Name,
                    TamanhoEmMB = ToSize(fileInfo.Length, SizeUnits.MB)
                });
            }

            return arquivos;
        }

        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public decimal ToSize(long value, SizeUnits unit)
        {
            return Math.Round(Convert.ToDecimal((value / (double)Math.Pow(1024, (long)unit))),2);
        }
    }
}
