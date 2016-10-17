using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace acopy
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                if (args.Length != 3) {
                    // TODO: Criar um metodo mais elegante para uma tela de help da aplicação
                    Console.WriteLine("Precisa de Ajuda?");
                    throw new Exception("Número de argumentos inválidos.");
                }

                string strOrigem, strDestino, strLogDir;
                strOrigem  = args[0];
                strDestino = args[1];
                strLogDir  = args[2];

                if (!Directory.Exists(strOrigem)) {
                    throw new Exception("Diretório de origem não existe ou não está acessivel.");
                }

                if (!Directory.Exists(strDestino)) {
                    try {
                        Directory.CreateDirectory(strDestino);
                    } catch (Exception ex) {
                        throw new Exception("Erro ao tentar criar o diretório de destino. Verifique se o caminho informado é um camingo válido para um diretório");
                    }
                }

                if (!Directory.Exists(strLogDir)) {
                    try {
                        Directory.CreateDirectory(strLogDir);
                    } catch (Exception ex) {
                        throw new Exception("Erro ao tentar criar o diretório do Log. Verifique se o caminho informado é um camingo válido para um diretório");
                    }
                }

                DirectoryInfo dirOrigem = new DirectoryInfo(@strOrigem);

                FileInfo[] arquivos = dirOrigem.GetFiles("*", SearchOption.AllDirectories);

                string strPathDestino, strDisPathDestino, logName;
                int countFile = 0;
                int countFileCppy = 0;

                logName = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

                var arquivoLog = File.CreateText(strLogDir + "\\log_" + logName + ".txt");
                Console.WriteLine("Copiando arquivos...");
                foreach (FileInfo arquivo in arquivos) {
                    countFile++;
                    strPathDestino = arquivo.FullName.Replace(strOrigem, strDestino);
                    if (!File.Exists(strPathDestino)) {
                        strDisPathDestino = arquivo.DirectoryName.Replace(strOrigem, strDestino);
                        if (!Directory.Exists(strDisPathDestino)) {
                            Directory.CreateDirectory(strDisPathDestino);
                        }
                        Console.WriteLine(" - Copiando Arquivo " + arquivo.Name);
                        arquivo.CopyTo(strPathDestino, true);
                        arquivoLog.WriteLine("Arquivo [" + arquivo.FullName + "] copiado com sucesso.");
                        countFileCppy++;
                    }
                }
                arquivoLog.WriteLine("");
                arquivoLog.WriteLine("Foram copiados " + countFileCppy.ToString() + " arquivos de um total de " + countFile.ToString() + " arquivos localizados");
                Console.WriteLine("Foram copiados " + countFileCppy.ToString() + " arquivos de um total de " + countFile.ToString() + " arquivos localizados");
                arquivoLog.Close();


            } catch (Exception ex) {
                Console.WriteLine("Erro: " + ex.Message.ToString());
            }
            //Console.ReadKey();
        }
    }
}
