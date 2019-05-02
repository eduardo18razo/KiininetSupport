namespace DemonioCierreTicket
{
    public static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main(string[] args)
        {
            ScheduleCierreTicket schedule = new ScheduleCierreTicket(args[0]);
            schedule.ExecutaProceso();
            //ServiceBase[] servicesToRun = new ServiceBase[] 
            //{ 
            //    new ServiceCierreTicket(args[0])

            //};
            //ServiceBase.Run(servicesToRun);
        }
    }
}
