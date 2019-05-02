using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using AE.Net.Mail;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Core.Parametros;
using Attachment = AE.Net.Mail.Attachment;
using MailMessage = AE.Net.Mail.MailMessage;
using MailMessageReply = System.Net.Mail.MailMessage;


namespace KinniNet.Core.Demonio
{
    public class ManejadorCorreo : IDisposable
    {

        private readonly bool _proxy;
        public void Dispose()
        {

        }
        public ManejadorCorreo(bool proxy = false)
        {
            _proxy = proxy;
        }

        private class ServidorCliente
        {
            public string Servidor { get; set; }
            public int Puerto { get; set; }
            public bool Ssl { get; set; }
            public string Usuario { get; set; }
            public string Contraseña { get; set; }
        }

        public class ClienteImap
        {
            private readonly List<string> _valuesIgnored = ConfigurationManager.AppSettings["valuesIgnored"].Split(',').Select(s => s.Trim().ToLower()).ToList();
            private ServidorCliente ObtenerServidor()
            {
                ServidorCliente result;
                try
                {
                    var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                    if (smtpSection != null)
                    {
                        result = new ServidorCliente
                        {
                            Servidor = smtpSection.Network.Host,
                            Puerto = smtpSection.Network.Port,
                            Ssl = smtpSection.Network.EnableSsl,
                            Usuario = smtpSection.Network.UserName,
                            Contraseña = smtpSection.Network.Password
                        };
                    }
                    else
                    {
                        throw new Exception("Debe configurar Imap de Correo");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
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

            private void MarcarLeido(ImapClient imap, MailMessage mail)
            {
                try
                {
                    MailMessage[] mails = { mail };
                    imap.SetFlags(Flags.Seen, mails);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public string LeeCorreo()
            {
                StringBuilder result = new StringBuilder();
                try
                {
                    result.AppendLine("Obteniendo datos de servidor");
                    ServidorCliente servidor = ObtenerServidor();
                    List<string> ids = new List<string>();
                    List<MailMessage> mails = new List<MailMessage>();

                    result.AppendLine(string.Format("Conectando a {0}", servidor.Servidor));
                    using (var imap = new ImapClient(servidor.Servidor, servidor.Usuario, servidor.Contraseña, AuthMethods.Login, servidor.Puerto, servidor.Ssl))
                    {
                        result.AppendLine("Conexion Exitosa");
                        result.AppendLine("Obteniendo mensajes nuevos");
                        var msgs = imap.SearchMessages(SearchCondition.Unseen());
                        for (int i = 0; i < msgs.Length; i++)
                        {
                            string msgId = msgs[i].Value.Uid;
                            ids.Add(msgId);
                        }

                        foreach (string id in ids)
                        {
                            mails.Add(imap.GetMessage(id, false, false));
                        }
                        result.AppendLine(string.Format("Se encontraron {0} correos nuevos", mails.Count));
                        foreach (MailMessage mail in mails)
                        {
                            try
                            {
                                List<string> headersIngored = new List<string>
                                {
                                    "Auto-Submitted",
                                    "X-Auto-Response-Suppress",
                                    "Precedence"
                                };
                                result.AppendLine(string.Format("Procesando correo {0} recibido {1}", mail.Subject, mail.Date));
                                bool correovalido = true;
                                foreach (string header in headersIngored)
                                {
                                    HeaderValue headerContent;
                                    mail.Headers.TryGetValue(header, out headerContent);
                                    correovalido = ValidaHeader(headerContent.Value, _valuesIgnored);
                                    if (!correovalido)
                                        break;
                                }

                                if (correovalido)
                                {
                                    HeaderValue headerReference;
                                    mail.Headers.TryGetValue("References", out headerReference);
                                    if (string.IsNullOrEmpty(headerReference.Value))
                                    {
                                        result.AppendLine(string.Format("Correo {0} recibido {1} procesado como nuevo", mail.Subject, mail.Date));
                                        result.AppendLine(ClienteSmtp.SendNotificationNewTicket(mail));
                                    }
                                    else
                                    {
                                        if (headerReference.Value.Contains("AutoReply"))
                                        {
                                            result.AppendLine(string.Format("Correo {0} recibido {1} se descarto por motivo de autorespuesa", mail.Subject, mail.Date));
                                        }
                                        else
                                        {
                                            int idTicket = 0;

                                            string[] referencesArray = headerReference.Value.Split('~');
                                            if (referencesArray.Any(w => w.Contains("ticket")))
                                            {
                                                List<string> lstTickets = referencesArray.Where(w => w.Contains("ticket")).Distinct().ToList();
                                                if (lstTickets.Count > 1)
                                                    return null;
                                                string stringTicket = lstTickets.First();
                                                string[] ticketContains = stringTicket.Split('&');

                                                if (ticketContains.Count() <= 0)
                                                    return null;

                                                idTicket = int.Parse(ticketContains[1]);
                                                result.AppendLine(ClienteSmtp.SendNotificationCommentTicket(mail, idTicket));
                                                result.AppendLine(string.Format("Correo {0} recibido {1} procesado como comentario", mail.Subject, mail.Date));
                                            }
                                            else
                                            {
                                                string[] subject = mail.Subject.Split('[');
                                                if (subject.Any(a => a.Contains("Ticket:")))
                                                {
                                                    idTicket = int.Parse(subject.Last().Replace("[", string.Empty).Replace("]", string.Empty).Split(':').Last());
                                                    result.AppendLine(ClienteSmtp.SendNotificationCommentTicket(mail, idTicket));
                                                }
                                                else
                                                {
                                                    result.AppendLine(ClienteSmtp.SendNotificationNewTicket(mail));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result.AppendLine(string.Format("Correo {0} recibido {1} se descarto por motivo de autorespuesa", mail.Subject, mail.Date));
                                }

                                MarcarLeido(imap, mail);
                                result.AppendLine(string.Format("Correo {0} recibido {1} se marco como leido", mail.Subject, mail.Date));
                            }
                            catch (Exception e)
                            {
                                if (mail != null)
                                    result.AppendLine("error al recibir correo: " + mail.Subject + "\n" + e.Message + "\n" + e.InnerException);
                                else
                                {
                                    result.AppendLine("error al recibir correo: " + e.Message + "\n" + e.InnerException);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AppendLine(ex.Message);
                }

                return result.ToString().Trim();
            }
        }
        public class ClienteSmtp
        {
            private static ServidorCliente Servidor()
            {
                ServidorCliente result;
                try
                {
                    var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                    if (smtpSection != null)
                    {
                        result = new ServidorCliente
                        {
                            Servidor = smtpSection.Network.Host,
                            Puerto = int.Parse(smtpSection.Network.TargetName),
                            Ssl = smtpSection.Network.EnableSsl,
                            Usuario = smtpSection.Network.UserName,
                            Contraseña = smtpSection.Network.Password
                        };

                    }
                    else
                    {
                        throw new Exception("Debe configurar Imap de Correo");
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }
            public static string SendNotificationNewTicket(MailMessage source)
            {
                string result;
                try
                {
                    ServidorCliente servidor = Servidor();
                    SmtpClient smtpClient = new SmtpClient(servidor.Servidor, servidor.Puerto)
                    {
                        Credentials = new System.Net.NetworkCredential(servidor.Usuario, servidor.Contraseña),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    smtpClient.Send(ManejadorMensajes.CreateReplyNewTicket(source));
                    result = string.Format("Se envio correo Ticket Nuevo {0} - {1}", source.From.Address, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public static string SendNotificationCommentTicket(MailMessage source, int idTicket)
            {
                string result;
                try
                {
                    ServidorCliente servidor = Servidor();
                    SmtpClient smtpClient = new SmtpClient(servidor.Servidor, servidor.Puerto)
                    {
                        Credentials = new System.Net.NetworkCredential(servidor.Usuario, servidor.Contraseña),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    smtpClient.Send(ManejadorMensajes.CreateReplyComentarioTicket(source, idTicket));
                    result = string.Format("Se envio correo Ticket Comentario {0} - {1}", source.From.Address, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return result;
            }

            public static void SendNotificationTicketGenerated(int idTicket, string clave, string subject, string body, string to)
            {
                try
                {
                    ServidorCliente servidor = Servidor();
                    SmtpClient smtpClient = new SmtpClient(servidor.Servidor, servidor.Puerto)
                    {
                        Credentials = new System.Net.NetworkCredential(servidor.Usuario, servidor.Contraseña),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    smtpClient.Send(ManejadorMensajes.NewMail.SenMailTicket(smtpClient, idTicket, clave, subject, body, to));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public void SendMailTicket(int idTicket, string clave, string subject, string body, string to)
            {
                try
                {
                    ServidorCliente servidor = Servidor();
                    SendNotificationTicketGenerated(idTicket, clave, subject, body, to);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public class ManejadorMensajes
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

                private static List<HelperCampoMascaraCaptura> GeneraCaptura(Mascara mascara, string from, string subject, string contenido, AE.Net.Mail.Attachment[] atacchment, out string attname, out bool agregaArchivo, out bool eliminoarchivo)
                {
                    List<HelperCampoMascaraCaptura> result = new List<HelperCampoMascaraCaptura>();
                    try
                    {
                        AE.Net.Mail.Attachment att;
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
                                        string extension = Path.GetExtension(att.Filename);

                                        if (extension != null)
                                        {
                                            ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                                            double tamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                                            List<ArchivosPermitidos> archivospermitidos = new BusinessParametros().ObtenerArchivosPermitidos();
                                            if (archivospermitidos.Any(a => a.Extensiones.Contains(extension)))
                                            {
                                                attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Filename.Replace(extension, string.Empty), "ticketid", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                                att.Save(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname));
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

                        result.AppendFormat("<a href=\"mailto:{0}\">{0}</a> wrote:<br/>", fromAddress);

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

                public static MailMessageReply CreateReplyNewTicket(MailMessage source)
                {
                    MailMessageReply reply = new MailMessageReply();

                    try
                    {
                        string attname;
                        ArbolAcceso arbol;
                        Mascara mascara;

                        ObtenerFormulario(out arbol, out mascara);

                        AE.Net.Mail.Attachment att = source.Attachments.Any() ? source.Attachments.ToArray()[0] : null;
                        bool agregaArchivo;
                        bool eliminoArchivo;
                        string subject = source.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        List<HelperCampoMascaraCaptura> capturaMascara = GeneraCaptura(mascara, source.From.Address, subject, source.Body, source.Attachments.ToArray(), out attname, out agregaArchivo, out eliminoArchivo);

                        Usuario user = new BusinessUsuarios().GetUsuarioByCorreo(source.From.Address);
                        if (user == null)
                        {
                            string[] nombreCompleto = source.From.DisplayName.Trim().Split(' ');
                            string nombre = nombreCompleto.Any() ? nombreCompleto.Length > 3 ? string.Format("{0} {1}", nombreCompleto[0], 1) : nombreCompleto[0] : string.Empty;
                            string apellidoPaterno = nombreCompleto.Length > 4 ? nombreCompleto[nombreCompleto.Length - 2] : nombreCompleto.Length > 1 ? nombreCompleto[1] : string.Empty;
                            string apellidoMaterno = nombreCompleto.Length > 4 ? nombreCompleto[nombreCompleto.Length - 1] : nombreCompleto.Length > 2 ? nombreCompleto[2] : string.Empty;
                            PreTicketCorreo preticket = new BusinessTicket().GeneraPreticketCorreo(nombre, apellidoPaterno, apellidoMaterno, source.From.Address.Trim(), subject, source.Body, attname);
                            if (preticket != null)
                            {
                                if (att != null && agregaArchivo)
                                {
                                    File.Move(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname, ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"] + attname.Replace("ticketid", preticket.Guid));
                                    attname = attname.Replace("ticketid", preticket.Guid);
                                    BusinessFile.MoverTemporales(ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"], ConfigurationManager.AppSettings["RepositorioCorreos"], new List<string> { attname });
                                    BusinessFile.CopiarArchivo(ConfigurationManager.AppSettings["RepositorioCorreos"], ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"], attname);
                                }
                                reply.From = new MailAddress(source.To.First().Address);
                                reply.To.Add(source.From);
                                reply.ReplyToList.Add("noreply@kiininetcxp.com");
                                HeaderValue headerMessageId;
                                source.Headers.TryGetValue("Message-ID", out headerMessageId);
                                string id = headerMessageId.Value;

                                HeaderValue headerInReplyTo;
                                source.Headers.TryGetValue("In-Reply-To", out headerInReplyTo);
                                reply.Headers.Add("In-Reply-To", id);
                                HeaderValue headerReference;
                                source.Headers.TryGetValue("References", out headerReference);
                                string references = headerReference.Value;

                                if (string.IsNullOrEmpty(references))
                                    references += "~guidpreticket&" + preticket.Guid + "~AutoReply";

                                reply.Headers.Add("References", references);

                                reply.Subject = "Confirmar Ticket";

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaPreTicketCorreo(source.From.Address, source.From.DisplayName, source.Body, source.Date.ToString(CultureInfo.InvariantCulture), preticket.Guid, eliminoArchivo);
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
                                //foreach (MailAddress mailAddress in source.To)
                                //{
                                //    reply.From = new MailAddress(mailAddress.Address);
                                //}
                                reply.From = new MailAddress(source.To.First().Address);
                                reply.To.Add(source.From.Address);
                                reply.ReplyToList.Add(source.To.First().Address);

                                HeaderValue headerMessageId;
                                source.Headers.TryGetValue("Message-ID", out headerMessageId);
                                string id = headerMessageId.Value;

                                HeaderValue headerReference;
                                source.Headers.TryGetValue("References", out headerReference);
                                string references = headerReference.Value;

                                if (string.IsNullOrEmpty(references))
                                    references += "~ticket&" + ticket.Id + "~";

                                reply.Headers.Add("References", references);

                                if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                                    reply.Subject = "Re: ";

                                reply.Subject += string.Format("{0} [Ticket: {1}]", source.Subject, ticket.Id);

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaTicket(source.From.Address, source.From.DisplayName, source.Body, source.Date.ToString(CultureInfo.InvariantCulture), ticket.Id, ticket.ClaveRegistro, eliminoArchivo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return reply;
                }

                private static string GeneraRespuestaComentario(string fromAddress, string fromName, string sourceBody, string sentDate, bool eliminoArchivo)
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

                        result.AppendFormat("<a href=\"mailto:{0}\">{0}</a> wrote:<br/>", fromAddress);

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

                private static string GeneraRespuestaTicketInvalido(string fromAddress, string fromName, string sourceBody)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<p>Lo sentimos el Ticket al que haces referencia no existe. !</p>");

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

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

                private static string GeneraRespuestaTicketUsuarioInvalido(string fromAddress, string fromName, string sourceBody)
                {
                    StringBuilder result = new StringBuilder();
                    try
                    {
                        result.Append("<p>Lo sentimos este Número de Ticket pertenece a otro usuario</p>");

                        if (!string.IsNullOrEmpty(fromName))
                            result.Append(fromName + ' ');

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

                public static MailMessageReply CreateReplyComentarioTicket(MailMessage source, int idTicket)
                {

                    MailMessageReply reply;
                    try
                    {

                        reply = new MailMessage()
                        {
                            From = new MailAddress(source.To.First().Address),

                        };
                        reply.To.Add(source.From);

                        //todo cambiar el correo al que se usa 
                        string respuesta = source.Body.Split(new[] { "De: kiininet.desarrollo@gmail.com" }, StringSplitOptions.None)[0];
                        Attachment[] attachment = source.Attachments.ToArray();

                        HeaderValue headerMessageId;
                        source.Headers.TryGetValue("Message-ID", out headerMessageId);
                        reply.Headers.Add("In-Reply-To", headerMessageId.Value);

                        HeaderValue headerReference;
                        source.Headers.TryGetValue("References", out headerReference);


                        int idticket = idTicket;
                        if (idticket == 0) return null;
                        HelperDetalleTicket ticket = new BusinessTicket().ObtenerDetalleTicket(idticket);
                        if (ticket == null)
                        {
                            reply.ReplyToList.Add("noreply@kiininet.com");
                            if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                                reply.Subject = "Re: ";
                            reply.Subject += source.Subject;

                            reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                            reply.IsBodyHtml = true;
                            reply.Body = GeneraRespuestaTicketInvalido(source.From.Address, source.From.Address, source.Body);
                        }
                        else
                        {
                            Usuario user = new BusinessUsuarios().ObtenerDetalleUsuario(ticket.IdUsuarioLevanto);
                            if (user.CorreoPrincipal != source.From.Address)
                            {
                                reply.ReplyToList.Add("noreply@kiininet.com");
                                if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                                    reply.Subject = "Re: ";
                                reply.Subject += source.Subject;

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaTicketUsuarioInvalido(source.From.Address, source.From.Address, source.Body);
                            }
                            else
                            {
                                foreach (MailAddress address in source.To)
                                {
                                    reply.ReplyToList.Add(address);
                                }

                                bool eliminoarchivo = false;
                                if (attachment.Any())
                                {
                                    string attname;
                                    List<string> archivos = new List<string>();
                                    foreach (Attachment att in attachment)
                                    {
                                        string extension = Path.GetExtension(att.Filename);
                                        if (extension != null)
                                        {
                                            ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                                            double tamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                                            List<ArchivosPermitidos> archivospermitidos = new BusinessParametros().ObtenerArchivosPermitidos();
                                            if (archivospermitidos.Any(a => a.Extensiones.Contains(extension)))
                                            {
                                                attname = String.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", att.Filename.Replace(extension, string.Empty), idticket, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                                                att.Save(String.Format("{0}{1}", ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"], attname));

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

                                string reference = headerReference.Value;
                                if (string.IsNullOrEmpty(reference))
                                    reference += "~ticket&" + idticket + "~";
                                if (!headerReference.Value.Contains("ticket&"))
                                    reference += "~ticket&" + idticket + "~";

                                reply.Headers.Add("References", reference);

                                if (!source.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                                    reply.Subject = "Re: ";
                                reply.Subject += source.Subject;

                                reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                                reply.IsBodyHtml = true;
                                reply.Body = GeneraRespuestaComentario(source.From.Address, source.From.Address, source.Body, source.Date.ToString(CultureInfo.InvariantCulture), eliminoarchivo);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return reply;
                }

                public static class NewMail
                {
                    public static MailMessageReply SenMailTicket(SmtpClient servidor, int idTicket, string clave, string subject, string body, string to)
                    {
                        MailMessageReply reply = new MailMessageReply();
                        try
                        {
                            string references = string.Empty;
                            if (reply.Headers.GetValues("References") != null)
                            {
                                references = reply.Headers.GetValues("References").ToString();
                            }

                            if (string.IsNullOrEmpty(references))
                                references += "~ticket&" + idTicket + "~";

                            reply.Headers.Add("References", references);
                            reply.From = new MailAddress(servidor.Credentials.GetCredential(servidor.Host, servidor.Port, "").UserName);
                            reply.Subject += string.Format("{0}", subject);
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
                            result.Append("<p>" + body.Replace("~replaceTicket~", string.Format("Ticket #: {0}<br>Clave: {1}", idTicket, claveRegistro)) + "</p>");
                            result.Append("<br>");

                            result.Append("<div>");
                            result.AppendFormat("Fecha {0}, ", sentDate);

                            if (!string.IsNullOrEmpty(fromName))
                                result.Append(fromName + ' ');

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
            }
        }
    }
}
