using System.ServiceProcess;

namespace DemonioTicketsCorreo
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new ServiceTicketsCorreo(args[0]) };
            ServiceBase.Run(servicesToRun);

        }
    }
}