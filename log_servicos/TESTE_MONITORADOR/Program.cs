using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;

namespace TESTE_MONITORADOR
{
    public partial class Program : ServiceBase
    {
        public static string fileName = @"C:\Teste_log\teste_log.txt";
        private static System.Timers.Timer temporizador;
        public static string ServicoMonitorar = "XboxGipSvc"; //nome do serviço para monitorar
        

        static void Main()
        {
            CriarPasta(@"C:\Teste_Log");
            Escrever($"Serviço de monitoramento iniciado em: {DateTime.Now}");
            temporizador = new System.Timers.Timer();
            temporizador.Interval = 3000;
            temporizador.Elapsed += Monitorador;           //(object sender, System.Timers.ElapsedEventArgs e) => Controlador(NomeServicoMonitorar);
            temporizador.Enabled = true;
            Console.ReadLine();
        }
        public static void CriarPasta(string pastaPath) //Cria a pasta e armazenará o log
        {
            try
            {
                if (Directory.Exists(pastaPath))
                {
                    Console.WriteLine($"Diretório {pastaPath} já existe.");
                    return;
                }
                Directory.CreateDirectory(pastaPath);
                Console.WriteLine("Diretório criado com sucesso.");
            }
            catch (Exception e)
            {
                Console.WriteLine("A criação do diretório falhou.");
            }
            finally { }
        }
        public static void Escrever(string menssagem)
        {
            try
            {
                if (File.Exists(fileName)) //o arquivo de log já existe{Atenção ao !}
                {
                    using(StreamWriter escrever = new StreamWriter(fileName,true))
                    {
                        escrever.WriteLine(menssagem);
                        Console.WriteLine("ESCREVI AQUI");
                        escrever.Flush();
                    }
                }
                else //o arquivo de log não existe e será criado
                {
                    using (StreamWriter escrever = File.CreateText(fileName))
                    {
                        escrever.WriteLine(menssagem);
                        Console.WriteLine("ESCREVI ALI");
                        escrever.Flush();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Erro ao escrever no arquivo : {e}");
            }
            finally {  }
        }
        public static void Monitorador(object source, System.Timers.ElapsedEventArgs e) //Executa o monitoramento do serviço e é chamado pelo Event
        {
            ServiceController servico = new ServiceController(ServicoMonitorar);
            if (servico.Status != ServiceControllerStatus.Running)
            {
                Escrever($"Serviço {ServicoMonitorar} parado -> {e.SignalTime} ");
            }
            else
            {
                Escrever($"Serviço {ServicoMonitorar} rodando -> {e.SignalTime} ");
            }
        }
    }   
}
