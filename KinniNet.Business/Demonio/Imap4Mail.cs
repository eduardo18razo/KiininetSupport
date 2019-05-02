using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using EAGetMail;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Core.Parametros;
using Attachment = EAGetMail.Attachment;
using MailAddress = System.Net.Mail.MailAddress;
using ServerProtocol = EAGetMail.ServerProtocol;
using SmtpClient = EASendMail.SmtpClient;


namespace KinniNet.Core.Demonio
{
    public class Imap4Mail
    {
        public class Retrieve
        {
            private readonly List<string> _valuesIgnored = ConfigurationManager.AppSettings["valuesIgnored"].Split(',').Select(s => s.Trim().ToLower()).ToList();
            #region Privado
            private MailServer GetServer()
            {
                MailServer result;
                try
                {
                    var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                    string username = smtpSection.Network.UserName;
                    string pdw = smtpSection.Network.Password;
                    string host = smtpSection.Network.Host;
                    result = new MailServer(host, username, pdw, ServerProtocol.Imap4)
                    {
                        Port = smtpSection.Network.Port
                    };

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }


            private MailClient GetMailClient()
            {
                MailClient result;
                try
                {
                    result = new MailClient("TryIt");
                    result.Connect(GetServer());
                    result.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            #endregion Privado

            private bool ValidaHeader(string referencia, List<string> excludeHeader)
            {
                bool result = true;
                try
                {
                    if (referencia != null)
                    {
                        referencia = referencia.Trim().ToLower();
                        if (excludeHeader.Contains(referencia))
                        {
                            result = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return result;
            }

            public string GetMails()
            {
                StringBuilder result = new StringBuilder();
                try
                {
                    result.AppendLine("Conecta con correo");
                    MailClient oClient = GetMailClient();
                    result.AppendLine("Conexión exitosa");
                    result.AppendLine("Obtiene correos nuevos");
                    MailInfo[] infoMails = oClient.GetMailInfos();
                    
                    if (infoMails != null)
                    {
                        result.AppendLine(string.Format("Se encontraron {0} correos nuevos", infoMails.Length));
                        foreach (MailInfo infoMail in infoMails)
                        {
                            Mail oMail = null;
                            try
                            {
                                oMail = oClient.GetMail(infoMail);
                                List<string> headersIngored = new List<string>
                                {
                                    "Auto-Submitted",
                                    "X-Auto-Response-Suppress",
                                    "Precedence"
                                };
                                result.AppendLine(string.Format("Procesando correo {0} recibido {1}", oMail.Subject, oMail.ReceivedDate));
                                bool correovalido = true;
                                foreach (string header in headersIngored)
                                {
                                    correovalido = ValidaHeader(oMail.Headers.GetValueOfKey(header), _valuesIgnored);
                                    if (!correovalido)
                                        break;
                                }

                                if (correovalido)
                                {
                                    result.AppendLine(string.Format("Correo {0} recibido {1} procesado como nuevo", oMail.Subject, oMail.ReceivedDate));
                                    if (string.IsNullOrEmpty(oMail.Headers.GetValueOfKey("References")))
                                    {
                                        result.AppendLine(string.Format("Correo {0} recibido {1} procesado como nuevo", oMail.Subject, oMail.ReceivedDate));
                                        Smtp.SendNotificationNewTicket(oMail);
                                    }
                                    else
                                    {
                                        result.AppendLine(string.Format("Correo {0} recibido {1} procesado como comentario", oMail.Subject, oMail.ReceivedDate));
                                        string references = oMail.Headers.GetValueOfKey("References");
                                        string[] referencesArray = references.Split('~');
                                        if (referencesArray.Any(w => w.Contains("ticket")))
                                            Smtp.SendNotificationCommentTicket(oMail);
                                        else
                                            Smtp.SendNotificationNewTicket(oMail);
                                    }
                                }
                                else
                                {
                                    result.AppendLine(string.Format("Correo {0} recibido {1} se descarto por motivo de autorespuesa", oMail.Subject, oMail.ReceivedDate));
                                }
                                ManagerMessage.MarcarLeidoNoLeido(oClient, infoMail);
                            }
                            catch (Exception e)
                            {
                                if (oMail != null)
                                    result.AppendLine("error al recibir correo: " + oMail.Subject);
                                else
                                {
                                    result.AppendLine("error al recibir correo: " + e.Message);
                                }
                            }
                        }
                    }
                    oClient.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result.ToString().Trim();
            }

            public void SendMailTicket(int idTicket, string clave, string body, string to)
            {
                try
                {
                    MailClient oClient = GetMailClient();
                    Smtp.SendNotificationTicketGenerated(idTicket, clave, body, to);

                    oClient.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public static class ManagerMessage
        {
            public static class SmtpMessage
            {
                private static void ObtenerFormulario(out ArbolAcceso arbol, out Mascara mascara)
                {
                    try
                    {
                        int arbolParametro = int.Parse(ConfigurationManager.AppSettings["ServicioCorreo"]);
                        arbol = new BusinessArbolAcceso().ObtenerArbolAcceso(arbolParametro);
                        int? idMascara = arbol.InventarioArbolAcceso.First().IdMascara;
                        if (idMascara != null)
                            mascara = new BusinessMascaras().ObtenerMascaraCaptura((int)idMascara);
                        else
                            throw new Exception("No se encontro la Mascara de captura");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                private static List<HelperCampoMascaraCaptura> GeneraCaptura(Mascara mascara, string from, string subject, string contenido, Attachment[] atacchment, out string attname, out bool agregaArchivo, out bool eliminoarchivo)
                {
                    List<HelperCampoMascaraCaptura> result = new List<HelperCampoMascaraCaptura>();
                    try
                    {
                        Attachment att;
                        attname = string.Empty;
                        agregaArchivo = false;
                        eliminoarchivo = false;
                        foreach (CampoMascara campo in mascara.CampoMascara.OrderBy(o => o.Id))
                        {
                            switch (campo.NombreCampo.ToUpper())
                            {
                                case "NOMBRE":
                                    result.Add(new HelperCampoMascaraCaptura
                                    {
                                        IdCampo = campo.Id,
                                        NombreCampo = campo.NombreCampo,
                                        Valor = from
                                    });
                                    break;
                                case "CORREO":
                                    result.Add(new HelperCampoMascaraCaptura
                                    {
                                        IdCampo = campo.Id,
                                        NombreCampo = campo.NombreCampo,
                                        Valor = from
                                    });
                                    break;
                                case "ASUNTO":
                                    result.Add(new HelperCampoMascaraCaptura
                                    {
                                        IdCampo = campo.Id,
                                        NombreCampo = campo.NombreCampo,
                                        Valor = subject
                                    });
                                    break;
                                case "COMENTARIO":
                                    result.Add(new HelperCampoMascaraCaptura
                                    {
                                        IdCampo = campo.Id,
                                        NombreCampo = campo.NombreCampo,
                                        Valor = contenido
                                    });
                                    break;
                                case "ARCHIVOADJUNTO":
                                    if (atacchment.Any())
                                    {
                                        att = atacchment.First();
                                        string extension = Path.GetExtension(att.Name);

                                        if (extension != null)
                                        {
                                            ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                                            double tamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                                            List<ArchivosPermitidos> archivospermitidos = new BusinessParametros().ObtenerArchivosPermitidos();
                                            if (archivospermitidos.Any(a => a.Extensiones.Contains(extension)))
                                            {
                                                attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Name.Replace(extension, string.Empty), "ticketid", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                                att.SaveAs(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname), true);
                                                long length = new FileInfo(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname)).Length;

                                                if (((length / 1024) / 1024) <= tamañoArchivo)
                                                {
                                                    result.Add(new HelperCampoMascaraCaptura
                                                    {
                                                        IdCampo = campo.Id,
                                                        NombreCampo = campo.NombreCampo,
                                                        Valor = attname
                                                    });
                                                    eliminoarchivo = false;
                                                    agregaArchivo = true;
                                                }
                                                else
                                                {
                                                    File.Delete(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname));
                                                    attname = "";
                                                    eliminoarchivo = true;
                                                    agregaArchivo = false;
                                                }
                                            }
                                            else
                                            {
                                                attname = "";
                                                eliminoarchivo = true;
                                                agregaArchivo = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new HelperCampoMascaraCaptura
                                        {
                                            IdCampo = campo.Id,
                                            NombreCampo = campo.NombreCampo,
                                            Valor = string.Empty
                                        });
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return result;
                }

                private static string GeneraRespuestaTicket(string fromAddress, string fromName, string sourceBody, string sentDate, int idTicket, string claveRegistro, bool eliminoArchivo)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<hr/>");
                        result.Append("<p>Gracias por tu correo!</p>");
                        if (eliminoArchivo)
                            result.Append("<p>El archivo adjunto no fue recibido correctamente ya que incumple con el tamaño o formato permitido!</p>");

                        result.Append(string.Format("<p>Hemos generado su ticket <br>No.: {0} <br>Clave: {1}.</p>", idTicket, claveRegistro));
                        result.Append("<p>Responderemos lo mas pronto posible, Si tienes algun comentario contesta este correo .</p>");
                        result.Append("<p>Saludos,<br>");
                        result.Append("KiiniHelp");
                        result.Append("</p>");
                        result.Append("<br>");

                        result.Append("<div>");
                        result.AppendFormat("Fecha {0}, ", sentDate);

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

                        result.AppendFormat("<<a href=\"mailto:{0}\">{0}</a>> wrote:<br/>", fromAddress);

                        if (!string.IsNullOrEmpty(sourceBody))
                        {
                            result.Append("<blockqoute style=\"margin: 0 0 0 5px;border-left:2px blue solid;padding-left:5px\">");
                            result.Append(sourceBody);
                            result.Append("</blockquote>");
                        }

                        result.Append("</div>");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return result.ToString();
                }

                private static string GeneraRespuestaPreTicketCorreo(string fromAddress, string fromName, string sourceBody, string sentDate, string guid, bool eliminoArchivo)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<hr/>");
                        result.Append("<p>Gracias por tu correo!</p>");
                        if (eliminoArchivo)
                            result.Append("<p>El archivo adjunto no fue recibido correctamente ya que incumple con el tamaño o formato permitido!</p>");

                        result.Append(string.Format("<p>Confirma tu solicitud haciendo click <a href='{0}{1}'>aqui</a></p>",
                            ConfigurationManager.AppSettings["siteUrl"]
                            + "/" +
                            (string.IsNullOrEmpty(ConfigurationManager.AppSettings["siteUrlfolder"]) ? string.Empty : ConfigurationManager.AppSettings["siteUrlfolder"] + "/") + "FrmConfirmarTicket.aspx?confirmacionguid=", guid));
                        result.Append("<p>Saludos,<br>");
                        //TODO: Change Name Sender
                        result.Append("KiiniHelp");
                        result.Append("</p>");
                        result.Append("<br>");

                        result.Append("<div>");
                        result.AppendFormat("Fecha {0}, ", sentDate);

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

                        result.AppendFormat("<<a href=\"mailto:{0}\">{0}</a>> wrote:<br/>", fromAddress);

                        if (!string.IsNullOrEmpty(sourceBody))
                        {
                            result.Append("<blockqoute style=\"margin: 0 0 0 5px;border-left:2px blue solid;padding-left:5px\">");
                            result.Append(sourceBody);
                            result.Append("</blockquote>");
                        }

                        result.Append("</div>");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return result.ToString();
                }

                public static MailMessage CreateReplyNewTicket(Mail source)
                {
                    MailMessage reply = new MailMessage();

                    try
                    {
                        string attname;
                        ArbolAcceso arbol;
                        Mascara mascara;

                        ObtenerFormulario(out arbol, out mascara);

                        Attachment att = source.Attachments.Any() ? source.Attachments[0] : null;
                        bool agregaArchivo;
                        bool eliminoArchivo;
                        string subject = source.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        List<HelperCampoMascaraCaptura> capturaMascara = GeneraCaptura(mascara, source.From.Address, subject, source.TextBody, source.Attachments, out attname, out agregaArchivo, out eliminoArchivo);

                        Usuario user = new BusinessUsuarios().GetUsuarioByCorreo(source.From.Address);
                        if (user == null)
                        {
                            string[] nombreCompleto = source.From.Name.Trim().Split(' ');
                            string nombre = nombreCompleto.Any() ? nombreCompleto.Length > 3 ? string.Format("{0} {1}", nombreCompleto[0], 1) : nombreCompleto[0] : string.Empty;
                            string apellidoPaterno = nombreCompleto.Length > 4 ? nombreCompleto[nombreCompleto.Length - 2] : nombreCompleto.Length > 1 ? nombreCompleto[1] : string.Empty;
                            string apellidoMaterno = nombreCompleto.Length > 4 ? nombreCompleto[nombreCompleto.Length - 1] : nombreCompleto.Length > 2 ? nombreCompleto[2] : string.Empty;
                            PreTicketCorreo preticket = new BusinessTicket().GeneraPreticketCorreo(nombre, apellidoPaterno, apellidoMaterno, source.From.Address.Trim(), subject, source.TextBody, attname);
                            if (preticket != null)
                            {
                                if (att != null && agregaArchivo)
                                {
                                    File.Move(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname, ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname.Replace("ticketid", preticket.Guid));
                                    attname = attname.Replace("ticketid", preticket.Guid);
                                    BusinessFile.MoverTemporales(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], ConfigurationManager.AppSettings["RepositorioCorreos"], new List<string> { attname });
                                    BusinessFile.CopiarArchivo(ConfigurationManager.AppSettings["RepositorioCorreos"], ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"], attname);
                                }
                                reply.From = new MailAddress(source.To[0].Address);
                                reply.To.Add(source.From.Address);
                                reply.ReplyToList.Add(source.To[0].Address);
                                string id = source.Headers.GetValueOfKey("Message-ID");
                                reply.Headers.Add("In-Reply-To", id);
                                string references = source.Headers.GetValueOfKey("References");

                                if (string.IsNullOrEmpty(references))
                                    references += "~guidpreticket&" + preticket.Guid + "~";

                                reply.Headers.Add("References", references);

                                reply.Subject = "Confirmar Ticket";

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaPreTicketCorreo(source.From.Address, source.From.Name, source.HtmlBody, source.SentDate.ToString(CultureInfo.InvariantCulture), preticket.Guid, eliminoArchivo);
                            }
                        }
                        else
                        {
                            Ticket ticket = new BusinessTicket().CrearTicket(user.Id, user.Id, arbol.Id, capturaMascara, (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Correo, mascara.Random, true, true);
                            if (ticket != null)
                            {
                                if (att != null && agregaArchivo)
                                {
                                    File.Move(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname, ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname.Replace("ticketid", ticket.Id.ToString()));
                                    attname = attname.Replace("ticketid", ticket.Id.ToString());
                                    BusinessFile.MoverTemporales(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], ConfigurationManager.AppSettings["RepositorioCorreos"], new List<string> { attname });
                                    BusinessFile.CopiarArchivo(ConfigurationManager.AppSettings["RepositorioCorreos"], ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"], attname);
                                }
                                reply.From = new MailAddress(source.To[0].Address);
                                reply.To.Add(source.From.Address);
                                reply.ReplyToList.Add(source.To[0].Address);
                                string id = source.Headers.GetValueOfKey("Message-ID");
                                reply.Headers.Add("In-Reply-To", id);
                                string references = source.Headers.GetValueOfKey("References");

                                if (string.IsNullOrEmpty(references))
                                    references += "~ticket&" + ticket.Id + "~";

                                reply.Headers.Add("References", references);

                                if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                                    reply.Subject = "Re: ";

                                reply.Subject += source.Subject;

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaTicket(source.From.Address, source.From.Name, source.HtmlBody, source.SentDate.ToString(CultureInfo.InvariantCulture), ticket.Id, ticket.ClaveRegistro, eliminoArchivo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return reply;
                } 
            }
            public static class NewMail
            {
                public static MailMessage SenMailTicket(SmtpClient servidor, int idTicket, string clave, string body, string to)
                {
                    MailMessage reply = new MailMessage();
                    try
                    {
                        string references = reply.Headers.GetValues("References").ToString();

                        if (string.IsNullOrEmpty(references))
                            references += "~ticket&" + idTicket + "~";

                        reply.Headers.Add("References", references);
                        reply.From = new MailAddress(servidor.Credentials.GetCredential(servidor.Host, servidor.Port, "").UserName);
                        reply.Subject += "Ticket " + idTicket;
                        reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        reply.To.Add(to);
                        reply.IsBodyHtml = true;
                        reply.Body = GeneraCorreoTicket(servidor.Credentials.GetCredential(servidor.Host, servidor.Port, "").UserName, "Kiininet", string.Empty, DateTime.Now.ToShortDateString().ToString(CultureInfo.InvariantCulture), idTicket, clave, body);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return reply;
                }

                private static string GeneraCorreoTicket(string fromAddress, string fromName, string sourceBody, string sentDate, int idTicket, string claveRegistro, string body)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<hr/>");
                        result.Append(string.Format("<p>Hemos generado su ticket <br>No.: {0} <br>Clave: {1}.</p>", idTicket, claveRegistro));
                        //result.Append("<p>Responderemos lo mas pronto posible, Si tienes algun comentario contesta este correo .</p>");
                        //result.Append("<p>Saludos,<br>");
                        ////TODO: Change Name Sender
                        //result.Append("KiiniHelp");
                        result.Append("<p>" + body.Replace("~replaceTicket~", string.Format("Ticket #: {0}<br>Clave: {1}", idTicket, claveRegistro)) + "</p>");
                        result.Append("<br>");

                        result.Append("<div>");
                        result.AppendFormat("Fecha {0}, ", sentDate);

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

                        result.AppendFormat("<<a href=\"mailto:{0}\">{0}</a>> wrote:<br/>", fromAddress);

                        if (!string.IsNullOrEmpty(sourceBody))
                        {
                            result.Append("<blockqoute style=\"margin: 0 0 0 5px;border-left:2px blue solid;padding-left:5px\">");
                            result.Append(sourceBody);
                            result.Append("</blockquote>");
                        }

                        result.Append("</div>");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return result.ToString();
                }
            }

            public static class ResponseMail
            {
                private static string GeneraRespuesta(string fromAddress, string fromName, string sourceBody, string sentDate, bool eliminoArchivo)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<p>Gracias Por tu correo!</p>");
                        result.Append("<p>Hemos recibido tus comentarios.</p>");
                        if (eliminoArchivo)
                            result.Append("<p>Los archivos adjuntos no fueron recibidos correctamente ya que incumple con el tamaño o formato permitido!</p>");
                        result.Append("<p>Saludos,<br>");
                        //TODO: Change Name Sender
                        result.Append("KiiniHelp");
                        result.Append("</p>");
                        result.Append("<br>");
                        result.Append("<div>");
                        result.AppendFormat("On {0}, ", sentDate);

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

                        result.AppendFormat("<<a href=\"mailto:{0}\">{0}</a>> wrote:<br/>", fromAddress);

                        if (!string.IsNullOrEmpty(sourceBody))
                        {
                            result.Append("<blockqoute style=\"margin: 0 0 0 5px;border-left:2px blue solid;padding-left:5px\">");
                            result.Append(sourceBody);
                            result.Append("</blockquote>");
                        }

                        result.Append("</div>");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return result.ToString();
                }
                public static MailMessage CreateReplyComentarioTicket(Mail source)
                {

                    MailMessage reply;
                    try
                    {

                        reply = new MailMessage()
                        {
                            From = new MailAddress(source.To[0].Address),
                            
                        };
                        reply.To.Add(source.From.Address);
                        reply.ReplyToList.Add(source.From.Address);
                        //todo cambiar el correo al que se usa 
                        string respuesta = source.TextBody.Split(new[] { "De: kiininet.desarrollo@gmail.com" }, StringSplitOptions.None)[0];
                        Attachment[] attachment = source.Attachments;

                        string id = source.Headers.GetValueOfKey("Message-ID");
                        reply.Headers.Add("In-Reply-To", id);

                        string references = source.Headers.GetValueOfKey("References");
                        string[] referencesArray = references.Split('~');
                        List<string> lstTickets = referencesArray.Where(w => w.Contains("ticket")).Distinct().ToList();
                        if (lstTickets.Count > 1)
                            return null;
                        string stringTicket = lstTickets.First();
                        string[] ticketContains = stringTicket.Split('&');

                        if (ticketContains.Count() <= 0)
                            return null;

                        int idticket = int.Parse(ticketContains[1]);
                        if (idticket == 0) return null;
                        HelperDetalleTicket ticket = new BusinessTicket().ObtenerDetalleTicket(idticket);
                        //new BusinessSecurity.Autenticacion().GetUserInvitadoDataAutenticate((int)BusinessVariables.EnumTiposUsuario.Cliente);
                        Usuario user = new BusinessUsuarios().ObtenerDetalleUsuario(ticket.IdUsuarioLevanto);
                        bool eliminoarchivo = false;
                        if (attachment.Any())
                        {
                            string attname;
                            List<string> archivos = new List<string>();
                            foreach (Attachment att in attachment)
                            {
                                string extension = Path.GetExtension(att.Name);
                                if (extension != null)
                                {
                                    ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                                    double tamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                                    List<ArchivosPermitidos> archivospermitidos = new BusinessParametros().ObtenerArchivosPermitidos();
                                    if (archivospermitidos.Any(a => a.Extensiones.Contains(extension)))
                                    {
                                        attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Name.Replace(extension, string.Empty), idticket, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                        att.SaveAs(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname), true);

                                        if (File.Exists(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname)))
                                        {
                                            long length = new FileInfo(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname)).Length;

                                            if (((length / 1024) / 1024) <= tamañoArchivo)
                                            {
                                                archivos.Add(attname);
                                            }
                                            else
                                            {
                                                if (File.Exists(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname)))
                                                    File.Delete(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname));
                                                eliminoarchivo = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        eliminoarchivo = true;
                                    }

                                }
                            }
                            new BusinessAtencionTicket().AgregarComentarioConversacionTicket(idticket, user.Id, respuesta, false, archivos, false, false);
                        }
                        else
                            new BusinessAtencionTicket().AgregarComentarioConversacionTicket(idticket, user.Id, respuesta, false, null, false, false);

                        if (string.IsNullOrEmpty(references))
                            references += "~ticket&" + idticket + "~";
                        if (!references.Contains("ticket&"))
                            references += "~ticket&" + idticket + "~";

                        reply.Headers.Add("References", references);

                        if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                            reply.Subject = "Re: ";
                        reply.Subject += source.Subject;

                        reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        reply.IsBodyHtml = true;
                        reply.Body = GeneraRespuesta(source.From.Address, source.From.Address, source.HtmlBody, source.SentDate.ToString(CultureInfo.InvariantCulture), eliminoarchivo);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return reply;
                }
            }

            public static void MarcarLeidoNoLeido(MailClient client, MailInfo mailInfo, bool asRead = true)
            {
                try
                {
                    client.MarkAsRead(mailInfo, asRead);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }

    public class Smtp
    {
        private static SmtpClient GetServer()
        {
            SmtpClient result;
            try
            {
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string host = smtpSection.Network.Host;
                int port = int.Parse(smtpSection.Network.TargetName);
                bool ssl = bool.Parse(ConfigurationManager.AppSettings["smtpSSL"]);
                string username = smtpSection.Network.UserName;
                string pwd = smtpSection.Network.Password;

                result = new SmtpClient("kiininetcxp.com", 25)
                {
                    Credentials = new System.Net.NetworkCredential("soporte.calidad@kiininetcxp.com", "SuppCalidad#.2019"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                //if (ssl)
                //{
                //    result = new SmtpServer(host)
                //    {
                //        Port = port,
                //        ConnectType = SmtpConnectType.ConnectSSLAuto,
                //        User = username,
                //        Password = pwd,

                //    };
                //}
                //else
                //{
                //    host = ConfigurationManager.AppSettings["smtp"];
                //    port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);

                //    result = new SmtpServer(host);
                //    result.ConnectType = SmtpConnectType.ConnectNormal;
                //    result.Port = port;
                //    result.User = username;
                //    result.Password = pwd;
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public static void SendNotificationNewTicket(Mail source)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("kiininetcxp.com", 25)
                {
                    Credentials = new System.Net.NetworkCredential("soporte.calidad@kiininetcxp.com", "SuppCalidad#.2019"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtpClient.Send(Imap4Mail.ManagerMessage.SmtpMessage.CreateReplyNewTicket(source));
                
                //SmtpClient oSmtp = new SmtpClient();
                //SmtpServer server = GetServer();
                //if (server.ConnectType == SmtpConnectType.ConnectNormal)
                //{
                //    oSmtp.NativeSSLMode = false;
                //}
                //SmtpMail oMail = new SmtpMail("TryIt");
                //oMail.From = new MailAddress("soporte.calidad@kiininetcxp.com");
                //oMail.To.Add(new MailAddress("ecerritos@kiininet.com"));
                //oMail.Subject = "first test email";
                //oMail.TextBody = "test body";

                //oSmtp.SendMail(server, oMail);
                //oSmtp.SendMail(server, Imap4Mail.ManagerMessage.NewMail.CreateReplyNewTicket(source));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SendNotificationCommentTicket(Mail source)
        {
            try
            {
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.Send(Imap4Mail.ManagerMessage.ResponseMail.CreateReplyComentarioTicket(source));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SendNotificationTicketGenerated(int idTicket, string clave, string body, string to)
        {
            try
            {
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.Send(Imap4Mail.ManagerMessage.NewMail.SenMailTicket(oSmtp, idTicket, clave, body, to));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}