using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EAGetMail;
using EASendMail;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Core.Security;
using KinniNet.Data.Help;
using Attachment = EAGetMail.Attachment;
using ServerProtocol = EAGetMail.ServerProtocol;
using SmtpClient = EASendMail.SmtpClient;

namespace KinniNet.Core.Demonio
{
    public class Imap4Mail
    {
        public class Retrieve
        {
            private string GetPathInboxSaveMail()
            {
                string result;
                try
                {
                    result = String.Format(BusinessVariables.Directorios.RepositorioCorreo);
                    if (!Directory.Exists(result))
                    {
                        Directory.CreateDirectory(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            private MailServer GetServer(BusinessVariables.EnumtServerImap server)
            {
                MailServer result = null;
                try
                {
                    switch (server)
                    {
                        case BusinessVariables.EnumtServerImap.Hotmail:
                            result = new MailServer("imap-mail.outlook.com", BusinessVariables.Correo.HotmailAccount, BusinessVariables.Correo.HotmailPassword, ServerProtocol.Imap4)
                            {
                                SSLConnection = true,
                                Port = 993
                            };
                            break;
                        case BusinessVariables.EnumtServerImap.Gmail:
                            result = new MailServer("imap.gmail.com", BusinessVariables.Correo.GmailAccount, BusinessVariables.Correo.GmailPassword, ServerProtocol.Imap4)
                            {
                                SSLConnection = true,
                                Port = 993
                            };
                            result.Port = 993;
                            break;
                        case BusinessVariables.EnumtServerImap.Yahoo:
                            result = new MailServer("pop.mail.yahoo.com", BusinessVariables.Correo.YahooAccount, BusinessVariables.Correo.YahooPassword, ServerProtocol.Imap4)
                            {
                                SSLConnection = true,
                                Port = 995
                            };
                            break;
                        case BusinessVariables.EnumtServerImap.Other:
                            result = new MailServer(BusinessVariables.Correo.OtherSmtp, BusinessVariables.Correo.OtherAccount, BusinessVariables.Correo.OtherPassword, ServerProtocol.Imap4)
                            {
                                Port = BusinessVariables.Correo.OtherPort
                            };
                            break;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            private MailClient GetMailClient(BusinessVariables.EnumtServerImap server)
            {
                MailClient result;
                try
                {
                    result = new MailClient("TryIt");
                    result.Connect(GetServer(server));
                    result.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            private void MarcarLeidoNoLeido(MailClient client, MailInfo mailInfo, bool asRead = true)
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

            private void SaveMailRepository(Mail mail)
            {
                try
                {
                    DateTime d = DateTime.Now;
                    CultureInfo cur = new CultureInfo("en-US");
                    string sdate = d.ToString("yyyyMMddHHmmss", cur);
                    string fileName = String.Format("{0}\\{1}{2}{3}.eml", GetPathInboxSaveMail(), sdate, d.Millisecond.ToString("d3"), mail.Subject);
                    mail.SaveAs(fileName, true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            private Imap4Folder[] GetFolders(MailClient oClient)
            {
                Imap4Folder[] result;
                try
                {
                    result = oClient.Imap4Folders;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public void GetMails(BusinessVariables.EnumtServerImap cliente)
            {
                try
                {
                    MailClient oClient = GetMailClient(cliente);
                    MailInfo[] infoMails = oClient.GetMailInfos();
                    if (infoMails != null)
                        foreach (MailInfo infoMail in infoMails)
                        {
                            MailInfo info = infoMail;
                            Mail oMail = oClient.GetMail(info);
                            if (string.IsNullOrEmpty(oMail.Headers.GetValueOfKey("References")))
                                Smtp.SendNotificationNewTicket(cliente, oMail);
                            else
                            {
                                string references = oMail.Headers.GetValueOfKey("References");
                                string[] referencesArray = references.Split('~');
                                if (referencesArray.Any(w => w.Contains("ticket")))
                                    Smtp.SendNotificationCommentTicket(cliente, oMail);
                            }
                            MarcarLeidoNoLeido(oClient, infoMail);
                        }
                    oClient.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public void SendMailTicket(BusinessVariables.EnumtServerImap cliente, int idTicket, string clave, string body, string to)
            {
                try
                {
                    MailClient oClient = GetMailClient(cliente);
                    Smtp.SendNotificationTicketGenerated(cliente, idTicket, clave, body, to);

                    oClient.Quit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static class Smtp
        {
            private static SmtpServer GetServer(BusinessVariables.EnumtServerImap server)
            {
                SmtpServer result = null;
                try
                {
                    switch (server)
                    {
                        case BusinessVariables.EnumtServerImap.Hotmail:
                            result = new SmtpServer("smtp.live.com")
                            {
                                Port = 587,
                                ConnectType = SmtpConnectType.ConnectSSLAuto,
                                User = BusinessVariables.Correo.HotmailAccount,
                                Password = BusinessVariables.Correo.HotmailPassword
                            };
                            break;
                        case BusinessVariables.EnumtServerImap.Gmail:
                            result = new SmtpServer("smtp.gmail.com")
                            {
                                Port = 465,
                                ConnectType = SmtpConnectType.ConnectSSLAuto,
                                User = BusinessVariables.Correo.GmailAccount,
                                Password = BusinessVariables.Correo.GmailPassword
                            };
                            break;
                        case BusinessVariables.EnumtServerImap.Yahoo:
                            result = new SmtpServer("smtp.mail.yahoo.com")
                            {
                                Port = 465,
                                ConnectType = SmtpConnectType.ConnectSSLAuto,
                                User = BusinessVariables.Correo.YahooAccount,
                                Password = BusinessVariables.Correo.YahooPassword
                            };
                            break;
                        case BusinessVariables.EnumtServerImap.Other:
                            result = new SmtpServer("smtp.gmail.com")
                            {
                                Port = 993,
                                ConnectType = SmtpConnectType.ConnectSSLAuto,
                                User = BusinessVariables.Correo.OtherAccount,
                                Password = BusinessVariables.Correo.OtherPassword
                            };
                            break;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public static void SendNotificationNewTicket(BusinessVariables.EnumtServerImap server, Mail source)
            {
                try
                {
                    SmtpClient oSmtp = new SmtpClient();
                    oSmtp.SendMail(GetServer(server), ManagerMessage.NewMail.CreateReplyNewTicket(source));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public static void SendNotificationCommentTicket(BusinessVariables.EnumtServerImap server, Mail source)
            {
                try
                {
                    SmtpClient oSmtp = new SmtpClient();
                    oSmtp.SendMail(GetServer(server), ManagerMessage.ResponseMail.CreateReplyComentarioTicket(source));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public static void SendNotificationTicketGenerated(BusinessVariables.EnumtServerImap server, int idTicket, string clave, string body, string to)
            {
                try
                {
                    SmtpClient oSmtp = new SmtpClient();
                    SmtpServer servidor = GetServer(server);
                    oSmtp.SendMail(servidor, ManagerMessage.NewMail.SenMailTicket(servidor, server, idTicket, clave, body, to));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public static class ManagerMessage
        {
            private static ParametroCorreo ObtenerCorreo(BusinessVariables.EnumTipoCorreo tipoCorreo)
            {
                ParametroCorreo result;
                DataBaseModelContext db = new DataBaseModelContext();
                try
                {
                    result = db.ParametroCorreo.SingleOrDefault(s => s.IdTipoCorreo == (int)tipoCorreo && s.Habilitado);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    db.Dispose();
                }
                return result;
            }
            public static class NewMail
            {
                public static SmtpMail SenMailTicket(SmtpServer servidor, BusinessVariables.EnumtServerImap server, int idTicket, string clave, string body, string to)
                {
                    SmtpMail reply = new SmtpMail("TryIt");
                    try
                    {
                        string references = reply.Headers.GetValueOfKey("References");

                        if (string.IsNullOrEmpty(references))
                            references += "~ticket&" + idTicket + "~";

                        reply.Headers.Add("References", references);
                        reply.From.Address = servidor.User;
                        reply.Subject += "Ticket " + idTicket;
                        reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        reply.To = to;
                        reply.HtmlBody = GeneraCorreoTicket("eduardo18razo@gmail.com", "Kiininet", string.Empty, DateTime.Now.ToShortDateString().ToString(CultureInfo.InvariantCulture), idTicket, clave, body);
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


                public static SmtpMail CreateReplyNewTicket(Mail source)
                {
                    SmtpMail reply = new SmtpMail("TryIt");

                    try
                    {
                        string attname;
                        ArbolAcceso arbol;
                        Mascara mascara;

                        ObtenerFormulario(out arbol, out mascara);

                        //ParametroCorreo correo = ObtenerCorreo(BusinessVariables.EnumTipoCorreo.ResponderTicket);
                        //String body = NamedFormat.Format(correo.Contenido, usuario);

                        Attachment att = source.Attachments.Any() ? source.Attachments[0] : null;
                        string subject = source.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        List<HelperCampoMascaraCaptura> capturaMascara = GeneraCaptura(mascara, source.From.Address, subject, source.TextBody, source.Attachments, out attname);

                        Usuario user = new BusinessUsuarios().GetUsuarioByCorreo(source.From.Address);
                        if (user == null)
                        {
                            user = new Usuario
                            {
                                IdTipoUsuario = (int)BusinessVariables.EnumTiposUsuario.Cliente,
                                ApellidoPaterno = source.From.Name.Trim() == string.Empty ? source.From.Address.Split('@')[0] : source.From.Name.Trim(),
                                ApellidoMaterno = source.From.Name.Trim() == string.Empty ? source.From.Address.Split('@')[0] : source.From.Name.Trim(),
                                Nombre = source.From.Name.Trim(),
                                DirectorioActivo = false,
                                Vip = false,
                                PersonaFisica = false,
                                NombreUsuario = new BusinessUsuarios().GeneraNombreUsuario(source.From.Name.Trim() == string.Empty ? source.From.Address.Split('@')[0] : source.From.Name.Trim(), source.From.Name.Trim() == string.Empty ? source.From.Address.Split('@')[0] : source.From.Name.Trim()),
                                Password = ConfigurationManager.AppSettings["siteUrl"] + "/ConfirmacionCuenta.aspx",
                                Autoregistro = true,
                                Habilitado = true
                            };
                            if (source.From.Address.Trim() != string.Empty)
                                user.CorreoUsuario = new List<CorreoUsuario>
                        {
                            new CorreoUsuario
                            {
                                Correo = source.From.Address.Trim(),
                                Obligatorio = true,
                            }
                        };
                            user = new BusinessUsuarios().ObtenerDetalleUsuario(new BusinessUsuarios().RegistrarCliente(user));
                        }
                        Ticket ticket = new BusinessTicket().CrearTicket(user.Id, user.Id, arbol.Id, capturaMascara, (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Correo, mascara.Random, true, true);
                        if (att != null)
                        {
                            File.Move(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname, ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname.Replace("ticketid", ticket.Id.ToString()));
                            attname = attname.Replace("ticketid", ticket.Id.ToString());
                            BusinessFile.MoverTemporales(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], ConfigurationManager.AppSettings["RepositorioCorreos"], new List<string> { attname });
                            BusinessFile.CopiarArchivo(ConfigurationManager.AppSettings["RepositorioCorreos"], ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"], attname);
                        }

                        reply.From.Address = source.To[0].Address;
                        reply.To = source.From.Address;
                        reply.ReplyTo.Address = source.From.Address;
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
                        reply.HtmlBody = GeneraRespuesta(source.From.Address, source.From.Name, source.HtmlBody, source.SentDate.ToString(CultureInfo.InvariantCulture), ticket.Id, ticket.ClaveRegistro);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return reply;
                }
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

                private static List<HelperCampoMascaraCaptura> GeneraCaptura(Mascara mascara, string from, string subject, string contenido, Attachment[] atacchment, out string attname)
                {
                    List<HelperCampoMascaraCaptura> result = new List<HelperCampoMascaraCaptura>();
                    try
                    {
                        Attachment att;
                        attname = string.Empty;
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
                                            attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Name.Replace(extension, string.Empty), "ticketid", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                            result.Add(new HelperCampoMascaraCaptura
                                            {
                                                IdCampo = campo.Id,
                                                NombreCampo = campo.NombreCampo,
                                                Valor = attname
                                            });
                                            att.SaveAs(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname), true);
                                        }
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

                private static string GeneraRespuesta(string fromAddress, string fromName, string sourceBody, string sentDate, int idTicket, string claveRegistro)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<hr/>");
                        result.Append("<p>Gracias por tu correo!</p>");
                        result.Append(string.Format("<p>Hemos generado su ticket <br>No.: {0} <br>Clave: {1}.</p>", idTicket, claveRegistro));
                        result.Append("<p>Responderemos lo mas pronto posible, Si tienes algun comentario contesta este correo .</p>");
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
            }

            public static class ResponseMail
            {
                private static string GeneraRespuesta(string fromAddress, string fromName, string sourceBody, string sentDate)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<p>Gracias Por tu correo!</p>");
                        result.Append("<p>Hemos recibido tus comentarios.</p>");
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
                public static SmtpMail CreateReplyComentarioTicket(Mail source)
                {

                    SmtpMail reply;
                    try
                    {

                        reply = new SmtpMail("TryIt")
                        {
                            From = { Address = source.To[0].Address },
                            To = source.From.Address,
                            ReplyTo = { Address = source.From.Address }
                        };

                        string respuesta = source.TextBody.Split(new[] { "De: eduardo18razo@gmail.com" }, StringSplitOptions.None)[0];
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
                        if (attachment.Any())
                        {
                            string attname;
                            List<string> archivos = new List<string>();
                            foreach (Attachment att in attachment)
                            {
                                string extension = Path.GetExtension(att.Name);
                                if (extension != null)
                                {
                                    attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Name.Replace(extension, string.Empty), "ticketid", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                    archivos.Add(attname);
                                    att.SaveAs(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname), true);
                                }
                            }
                            new BusinessAtencionTicket().AgregarComentarioConversacionTicket(idticket, user.Id, respuesta, false, archivos, false, false);
                            foreach (string archivo in archivos)
                            {
                                File.Move(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + archivo, ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + archivo.Replace("ticketid", idticket.ToString()));
                                attname = archivo.Replace("ticketid", idticket.ToString());
                                BusinessFile.MoverTemporales(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], ConfigurationManager.AppSettings["RepositorioCorreos"], new List<string> { attname });
                                BusinessFile.CopiarArchivo(ConfigurationManager.AppSettings["RepositorioCorreos"], ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"], attname);
                            }
                        }
                        else
                            new BusinessAtencionTicket().AgregarComentarioConversacionTicket(idticket, user.Id, respuesta, false, null, false, false);



                        if (string.IsNullOrEmpty(references))
                            references += "~ticket&" + idticket + "~";

                        reply.Headers.Add("References", references);

                        if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                            reply.Subject = "Re: ";
                        reply.Subject += source.Subject;

                        reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        reply.HtmlBody = GeneraRespuesta(source.From.Address, source.From.Address, source.HtmlBody, source.SentDate.ToString(CultureInfo.InvariantCulture));

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return reply;
                }
            }
        }
    }
}