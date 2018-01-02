using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using KinniNet.Core.Demonio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                //Parámetros del compilador
                CompilerParameters objParametros = new CompilerParameters()
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false,
                    IncludeDebugInformation = false
                };

                //Clase
                string strClase =
                    "using System;" +
                    "namespace Scientia {" +
                    "public class Formula {" +
                    "public int IdPrueba = 5;" +
                        "public object Ejecutar() {" +
                            "return IdPrueba" +
                    ";}}}";
                //"return " + formula +

                //Compilo todo y ejecuto el método
                CodeDomProvider objCompiler = CodeDomProvider.CreateProvider("CSharp");
                //En .NET 1.1 usaba esta linea:
                //ICodeCompiler ICC = (new CSharpCodeProvider()).CreateCompiler();
                CompilerResults objResultados = objCompiler.CompileAssemblyFromSource(objParametros, strClase);
                object objClase = objResultados.CompiledAssembly.CreateInstance("Scientia.Formula", false, BindingFlags.CreateInstance, null, null, null, null);
                var x = objClase.GetType().InvokeMember("Ejecutar", BindingFlags.InvokeMethod, null, objClase, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [TestMethod]
        public void TesConsultas()
        {
            //TODO: Eliminar Comentarios
            try
            {
                new BusinessTicketMailService().RecibeCorreos();
                //new BusinessTicketMailService().RecibeCorreos();
                //new BusinessTicketMailService().EnviaCorreoTicketGenerado(1, "VLF0", "Este es el cuerpo del mensaje <b> Hola</b> <s>sub</s>", "ecerritos@kiininet.com");
                //new BusinessTicketMailService().RecibeCorreos();
                //new BusinessDemonio().EnvioNotificacion();
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
