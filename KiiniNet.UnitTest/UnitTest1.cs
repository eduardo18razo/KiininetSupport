using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using DemonioCierreTicket;
using DemonioTicketsCorreo;
using EASendMail;
using KinniNet.Business.Utils;
using KinniNet.Core.Demonio;
using KinniNet.Core.Operacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MailAddress = System.Net.Mail.MailAddress;
using MailMessage = System.Net.Mail.MailMessage;
using SmtpClient = EASendMail.SmtpClient;

namespace KiiniNet.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private object GetProxyInstance(ref CompilerResults compilerResults)
        {

            object proxyInstance = null;

            // Define the WSDL Get address, contract name and parameters, with this we can extract WSDL details any time
            Uri address = new Uri("http://localhost:15277/ServiceArea.svc?wsdl");
            // For HttpGet endpoints use a Service WSDL address a mexMode of .HttpGet and for MEX endpoints use a MEX address and a mexMode of .MetadataExchange
            MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;
            string contractName = "IServiceArea";

            // Get the metadata file from the service.
            MetadataExchangeClient metadataExchangeClient = new MetadataExchangeClient(address, mexMode);
            metadataExchangeClient.ResolveMetadataReferences = true;

            //One can also provide credentials if service needs that by the help following two lines.
            //ICredentials networkCredential = new NetworkCredential("", "", "");
            //metadataExchangeClient.HttpCredentials = networkCredential;
            metadataExchangeClient.MaximumResolvedReferences = 200;
            //Gets the meta data information of the service.
            MetadataSet metadataSet = metadataExchangeClient.GetMetadata();

            // Import all contracts and endpoints.
            WsdlImporter wsdlImporter = new WsdlImporter(metadataSet);

            //Import all contracts.
            Collection<ContractDescription> contracts = wsdlImporter.ImportAllContracts();

            //Import all end points.
            ServiceEndpointCollection allEndpoints = wsdlImporter.ImportAllEndpoints();

            // Generate type information for each contract.
            ServiceContractGenerator serviceContractGenerator = new ServiceContractGenerator();

            //Dictinary has been defined to keep all the contract endpoints present, contract name is key of the dictionary item.
            var endpointsForContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

            foreach (ContractDescription contract in contracts)
            {
                serviceContractGenerator.GenerateServiceContractType(contract);
                // Keep a list of each contract's endpoints.
                endpointsForContracts[contract.Name] = allEndpoints.Where(ep => ep.Contract.Name == contract.Name).ToList();
            }

            // Generate a code file for the contracts.
            CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions();
            codeGeneratorOptions.BracingStyle = "C";

            // Create Compiler instance of a specified language.
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

            // Adding WCF-related assemblies references as copiler parameters, so as to do the compilation of particular service contract.
            CompilerParameters compilerParameters = new CompilerParameters(new string[] { "System.dll", "System.ServiceModel.dll", "System.Runtime.Serialization.dll" });
            compilerParameters.GenerateInMemory = true;

            //Gets the compiled assembly.
            compilerResults = codeDomProvider.CompileAssemblyFromDom(compilerParameters, serviceContractGenerator.TargetCompileUnit);

            if (compilerResults.Errors.Count <= 0)
            {
                // Find the proxy type that was generated for the specified contract (identified by a class that implements the contract and ICommunicationbject - this is contract
                //implemented by all the communication oriented objects).
                Type proxyType = compilerResults.CompiledAssembly.GetTypes().First(t => t.IsClass && t.GetInterface(contractName) != null &&
                    t.GetInterface(typeof(ICommunicationObject).Name) != null);

                // Now we get the first service endpoint for the particular contract.
                ServiceEndpoint serviceEndpoint = endpointsForContracts[contractName].First();

                // Create an instance of the proxy by passing the endpoint binding and address as parameters.
                proxyInstance = compilerResults.CompiledAssembly.CreateInstance(proxyType.Name, false, BindingFlags.CreateInstance, null,
                    new object[] { serviceEndpoint.Binding, serviceEndpoint.Address }, CultureInfo.CurrentCulture, null);

            }

            return proxyInstance;
        }

        [TestMethod]
        public void Test()
        {
            try
            {
                //    ServiceBase[] servicesToRun = new ServiceBase[] 
                //{ 
                //    new ServiceTicketsCorreo("pruebas") 
                //};
                //    ServiceBase.Run(servicesToRun);
                //    try
                //    {
                //        string logname = "Lalosss";
                //        string mensaje = "Mensaje para el log";

                //        if (!EventLog.SourceExists(logname))
                //            EventLog.CreateEventSource(logname, logname);

                //        EventLog.WriteEntry(logname, mensaje, EventLogEntryType.SuccessAudit);
                //    }
                //    catch (Exception ex)
                //    {
                //        //LogError("Kiininet", _serviceName, ex.Message);
                //    }
                while (true)
                    new BusinessTicketMailService().RecibeCorreos();
                //MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("Kiininet.desarrollo@gmail.com", "DEsde Desarrollo", Encoding.UTF8);
                //mail.To.Add("Kiininet.desarrollo@gmail.com");

                //mail.SubjectEncoding = Encoding.UTF8;
                //mail.Subject = "Asunto";

                //mail.Headers.Add("Reply-To", "Reply-To__ <" + "Kiininet.desarrollo@gmail.com" + ">");
                //mail.Headers.Add("Sender", "Kiininet.desarrollo@gmail.com");
                //mail.Headers.Add("Return-Path", "Kiininet.desarrollo@gmail.com");
                //mail.Headers.Add("MIME-Version", "1.0");
                //mail.Headers.Add("guidticket", "idticket");

                //string boundary = Guid.NewGuid().ToString();
                //mail.Headers.Add("Content-Type", "multipart/mixed; boundary=--" + boundary);

                ////mail.Headers.Add("", Environment.NewLine);
                ////mail.Headers.Add("", Environment.NewLine);

                ////mail.Headers.Add("", "--" + boundary);
                ////mail.Headers.Add("Content-Type", "text/html; charset=utf-8");
                ////mail.Headers.Add("Content-Transfer-Encoding", "base64");
                ////mail.Headers.Add("", Environment.NewLine);
                ////var bytes = Encoding.UTF8.GetBytes(html_body);
                ////var base64 = Convert.ToBase64String(bytes);
                ////mail.Headers.Add("", base64.ToString());
                ////mail.Headers.Add("", "--" + boundary);

                //SmtpClient smtp = new SmtpClient();
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential("Kiininet.desarrollo@gmail.com", "Knnet2018");
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.Timeout = 60000;

                //smtp.SendCompleted += (s, e) =>
                //{
                //    if (e.Cancelled)
                //    {
                //        MessageBox.Show("sending email was canceled");
                //    }
                //    if (e.Error != null)
                //    {
                //        MessageBox.Show("sending email was failed -> Error : " + e.Error.ToString());
                //    }
                //    else
                //    {
                //        MessageBox.Show("email was sent successfully");
                //    }

                //    mail.Dispose();
                //};

                //try
                //{
                //    smtp.SendAsync(mail, null);
                //}
                //catch (System.Net.Mail.SmtpException exp)
                //{
                //    MessageBox.Show("sending email was failed, SmtpException -> Error : " + exp.ToString());

                //    mail.Dispose();
                //}



                //using (var pop = new AE.Net.Mail.Pop3Client("smtp.gmail.com", "kiininet.desarrollo@gmail.com", "Knnet2018", 587, true))
                //{
                //    for (var i = pop.GetMessageCount() - 1; i >= 0; i--)
                //    {
                //        var msg = pop.GetMessage(i, false);
                //        Assert.AreEqual(msg.Subject, "Standard API between different protocols?  Yes, please!");
                //        pop.DeleteMessage(i); //WE DON'T NEED NO STINKIN' EMAIL!
                //    }
                //}

                //using (var imap = new AE.Net.Mail.ImapClient("smtp.gmail.com", "kiininet.desarrollo@gmail.com", "Knnet2018", AE.Net.Mail.AuthMethods.Login, 587, true))
                //{

                //    var msgs = imap.SearchMessages(
                //      SearchCondition.Undeleted().And(
                //        SearchCondition.From("david"),
                //        SearchCondition.SentSince(new DateTime(2000, 1, 1))
                //      ).Or(SearchCondition.To("andy"))
                //    );

                //    Assert.AreEqual(msgs[0].Value.Subject, "This is cool!");

                //    imap.NewMessage += (sender, e) =>
                //    {
                //        var msg = imap.GetMessage(e.MessageCount - 1);
                //        Assert.AreEqual(msg.Subject, "IDLE support?  Yes, please!");
                //    };
                //}



                //var ServicesToRun = new ServiceTicketsCorreo();
                //ServicesToRun.st
                //ServiceBase.Run(ServicesToRun);

                //new BusinessDashboards().GetDashboardAgente(null, null);
                //DataTable dt = new DataTable("dt");
                //dt.Columns.Add(new DataColumn("Ocupado"));
                //dt.Columns.Add(new DataColumn("Libre"));
                //dt.Rows.Add(350, 1000);
                //BusinessGraficosDasboard.Administrador.GeneraGraficoBarraApilada(new Chart(), dt);
                //DirectoryInfo dInfo = new DirectoryInfo(@"C:\Users\Eduardo Cerritos\Desktop\Repositorio\");
                //var z = BusinessFile.DirectorySize(dInfo, true);
                //var y = BusinessFile.DirectoryFilesCount(dInfo, true);
                //DashboardAdministrador variable = new BusinessDashboards().GetDashboardAdministrador();
                ////Parámetros del compilador
                //CompilerParameters objParametros = new CompilerParameters()
                //{
                //    GenerateInMemory = true,
                //    GenerateExecutable = false,
                //    IncludeDebugInformation = false
                //};

                ////Clase
                //string strClase =
                //    "using System;" +
                //    "namespace Scientia {" +
                //    "public class Formula {" +
                //    "public int IdPrueba = 5;" +
                //        "public object Ejecutar() {" +
                //            "return IdPrueba" +
                //    ";}}}";
                ////"return " + formula +

                ////Compilo todo y ejecuto el método
                //CodeDomProvider objCompiler = CodeDomProvider.CreateProvider("CSharp");
                ////En .NET 1.1 usaba esta linea:
                ////ICodeCompiler ICC = (new CSharpCodeProvider()).CreateCompiler();
                //CompilerResults objResultados = objCompiler.CompileAssemblyFromSource(objParametros, strClase);
                //object objClase = objResultados.CompiledAssembly.CreateInstance("Scientia.Formula", false, BindingFlags.CreateInstance, null, null, null, null);
                //var x = objClase.GetType().InvokeMember("Ejecutar", BindingFlags.InvokeMethod, null, objClase, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [TestMethod]
        public void TesConsultas()
        {
            #region comentarios
            //new BusinessDemonio().EnvioNotificacion();
            //Program.Main(new[] { "Cierre TIcket Calidad" });
            //string hora = DateTime.Now.ToString("hh:mm:ss");
            //hora = DateTime.Now.ToString("HH:mm:ss");
            //new BusinessDemonio().EnvioNotificacion();
            //try
            //{
            //    SmtpServer result = new SmtpServer("kiininetcxp.com")
            //    {
            //        Port = 25,
            //        ConnectType = SmtpConnectType.ConnectSSLAuto,
            //        User = "soporte.calidad@kiininetcxp.com",
            //        Password = "SuppCalidad#.2019"
            //    };

            //    SmtpMail oMail = new SmtpMail("TryIt");
            //    SmtpClient oSmtp = new SmtpClient();

            //    oMail.From = "soporte.calidad@kiininetcxp.com";
            //    oMail.To = "ecerritos@kiininet.com";
            //    oMail.Subject = "c# project";
            //    oMail.TextBody = "Contenido";

            //    try
            //    {
            //        Console.WriteLine("Enviando correro");
            //        oSmtp.SendMail(result, oMail);
            //        Console.WriteLine("email enviado!");
            //    }
            //    catch (Exception ep)
            //    {
            //        Console.WriteLine("fallo el envio:");
            //        Console.WriteLine(ep.Message);
            //    }


            //    //SmtpClient smtpClient = new SmtpClient("webmail.kiininetcxp.com", 25);

            //    //smtpClient.Credentials = new System.Net.NetworkCredential("soporte.calidad@kiininetcxp.com", "SuppCalidad#.2019");
            //    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    //MailMessage mailMessage = new MailMessage("soporte.calidad@kiininetcxp.com", "ecerritos@kiininet.com");
            //    //mailMessage.Subject = "Subject";
            //    //mailMessage.Body = "body";

            //    //try
            //    //{
            //    //    smtpClient.Send(mailMessage);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //}
            //catch (Exception e)
            //{

            //}
            #endregion comentarios

            new BusinessTicketMailService().RecibeCorreos();
            new BusinessInformacionConsulta().ObtenerReporteInformacionConsulta(1, new Dictionary<string, DateTime>
                        {
                            {"inicio", Convert.ToDateTime("01/01/2018")},
                            {"fin", Convert.ToDateTime("27/06/2018")}
                        }, 1);
            new BusinessInformacionConsulta().ObtenerReporteInformacionConsulta(1, null, 1);
            new BusinessMascaras().Test(34);
            new BusinessDemonio().EnvioNotificacion();
            new BusinessDemonio().CierraTicketsResueltos();


            ////TODO: Eliminar Comentarios
            //try
            //{
            //    SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            //    MailAddress fromAddress = new MailAddress("support@kiininet.com", "Eduardo Cerritos");
            //    MailAddress toAddress = new MailAddress("eduardo18razo@gmail.com", "Kiininet Support");

            //    var smtp = new SmtpClient
            //    {
            //        Host = section.Network.Host,//"smtp.gmail.com",
            //        Port = section.Network.Port,
            //        EnableSsl = section.Network.EnableSsl,
            //        DeliveryMethod = SmtpDeliveryMethod.Network,
            //        UseDefaultCredentials = section.Network.DefaultCredentials,
            //        Credentials = new NetworkCredential(fromAddress.Address, section.Network.Password)
            //    };
            //    using (var message = new MailMessage(fromAddress, toAddress)
            //    {
            //        Subject = "Alias",
            //        IsBodyHtml = true,
            //        Body = "content"
            //    })
            //    {
            //        smtp.Send(message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString());
            //}

            try
            {
                //SmtpClient mailClient = new SmtpClient("smtp.gmail.com");
                //MailMessage msgMail = new MailMessage();
                //msgMail.From = new MailAddress("ecerritos@kiininet.com", "support@kiininet.com");
                //mailClient.UseDefaultCredentials = false;
                //mailClient.Credentials = new NetworkCredential("ecerritos@kiininet.com", "Eyleen231012");
                //mailClient.EnableSsl = true;
                //MailAddress sendMailTo = new MailAddress("eduardo18razo@gmail.com", "Mark Twain");
                //msgMail.To.Add(sendMailTo);
                //msgMail.Subject = "Test Subject Alias";

                //msgMail.Body = "Email content";
                //msgMail.IsBodyHtml = true;

                //mailClient.Send(msgMail);
                //msgMail.Dispose();
                //new BusinessCatalogos().CrearCatalogoExcel(new Catalogos(), false, "file", "hoja");
                //new BusinessTicketMailService().RecibeCorreos();
                //new BusinessTicketMailService().RecibeCorreos();
                //new BusinessTicketMailService().EnviaCorreoTicketGenerado(1, "VLF0", "Este es el cuerpo del mensaje <b> Hola</b> <s>sub</s>", "ecerritos@kiininet.com");
                //new BusinessTicketMailService().RecibeCorreos();

                //new KiiniNet.Services.Windows.ServiceNotificacion();
                //new BusinessDemonio().EnvioNotificacion();
                //new BusinessDemonio().ActualizaSla();
                //Usuario datosUsuario = new Usuario();
                //datosUsuario.IdTipoUsuario = (int) BusinessVariables.EnumTiposUsuario.Cliente;
                //datosUsuario.ApellidoMaterno = "Apellido Materno Prueba";
                //datosUsuario.ApellidoPaterno = "Apellido Paterno Prueba";
                //datosUsuario.Nombre = "Nombre Prueba";
                //datosUsuario.TelefonoUsuario = new List<TelefonoUsuario>
                //{
                //    new TelefonoUsuario
                //    {
                //        IdTipoTelefono = (int) BusinessVariables.EnumTipoTelefono.Celular,
                //        Numero = "1234560000",
                //        Obligatorio = true
                //    }
                //};
                //datosUsuario.CorreoUsuario = new List<CorreoUsuario>();
                //datosUsuario.CorreoUsuario.Add(new CorreoUsuario
                //{
                //    Correo = "correos@correos.com",
                //    Obligatorio = true
                //});
                //new BusinessUsuarios().RegistrarCliente(datosUsuario);
                //CompilerResults compilerResults = null;

                ////Create the proxy instance and returns it back, One parameter to this method is compiler compilerResults(Reference Type) this has been done so that the assemblies are
                ////compiled only once, one can change the implementation adhering to coding guidelines and as per the implementation and requirement
                //object proxyInstance = GetProxyInstance(ref compilerResults);
                //string operationName = "ObtenerAreas";

                //// Get the operation's method
                //var methodInfo = proxyInstance.GetType().GetMethod(operationName);

                ////Paramaters if any required by the method are added into optional paramaters
                //object[] operationParameters = new object[] { true };
                //var z = methodInfo.Invoke(proxyInstance, BindingFlags.InvokeMethod, null, operationParameters, null);
                //Type t = z.GetType();

                //foreach (var prop in z.GetType().GetProperties())
                //{
                //    MessageBox.Show(string.Format("{0}={1}", prop.Name, prop.GetValue(z, null)));
                //}

                ////Invoke Method, and get the return value
                //var y = methodInfo.Invoke(proxyInstance, BindingFlags.InvokeMethod, null, operationParameters, null).ToString();
                ////lblUserMessage.Text = methodInfo.Invoke(proxyInstance, BindingFlags.InvokeMethod, null, operationParameters, null).ToString();
                //BusinessCorreo.SendMail("ecerritos@kiininet.com", "prueba envio", "Correo de prueba");

                //DataBaseModelContext db = new DataBaseModelContext();
                //string textoOriginal = "¿Cuál es tu solicitud?";//transformación UNICODE
                ////textoOriginal = QuitAccents(textoOriginal);
                //var tmp = BusinessCadenas.Cadenas.FormatoBaseDatos(textoOriginal);
                //var tmp = Regex.Replace(BusinessCadenas.Cadenas.ReemplazaAcentos(textoOriginal), "[^0-9a-zA-Z]+", "");
                //Area area = new BusinessArea().ObtenerAreaById(1);

                //List<InfoClass> contenClass = ObtenerPropiedadesObjeto(area);

                //string formatoArea = NamedFormat.Format("El Area {Descripcion} tiene el identificador {Id}", area);
                //new BusinessTicketMailService().RecibeCorreos();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<InfoClass> ObtenerPropiedadesObjeto(object obj)
        {
            List<InfoClass> result;
            try
            {
                var propertiesArea = GetProperties(obj);
                result = (from info in propertiesArea
                          where info.PropertyType.Name == "String" || info.PropertyType.Name == "Int32" || info.PropertyType.Name == "DateTime"
                          select new InfoClass
                          {
                              Name = info.Name,
                              Type = info.PropertyType.Name
                          }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        private IEnumerable<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }

        public class InfoClass
        {
            public string Name { get; set; }
            public string Type { get; set; }

        }
    }
}
