using System.ServiceProcess;

namespace DemonioSla
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new ServiceSla(args[0])
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
