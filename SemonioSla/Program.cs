using System.ServiceProcess;

namespace DemonioSla
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new ServiceSla()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
