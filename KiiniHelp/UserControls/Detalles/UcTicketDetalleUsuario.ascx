<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketDetalleUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcTicketDetalleUsuario" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleMascaraCaptura.ascx" TagPrefix="ucMascara" TagName="UcDetalleMascaraCaptura" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="ucCambioEstatus" TagName="UcCambiarEstatusTicket" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<script type="text/javascript">
    var uploadCustomerError = "";
    function UploadStart(sender, args) {
        debugger;
        if (sender._inputFile.files.length > 0) {
            var filesize = ((sender._inputFile.files[0].size) / 1024) / 1024;
            var fileId = sender.get_id();
            var container = document.getElementById('AjaxFileUpload1_FileInfoContainer_' + fileId);
            var sizeallow = document.getElementById("<%= FindControl("hfMaxSizeAllow").ClientID %>").value;
            var validFilesTypes = document.getElementById("<%= FindControl("hfFileTypes").ClientID %>").value.split('|');

            var ext = "." + sender._inputFile.files[0].name.split('.').pop().toLowerCase();

            var isValidFile = false;
            for (var i = 0; i < validFilesTypes.length; i++) {
                if (ext == validFilesTypes[i]) {
                    isValidFile = true;
                    break;
                }
            }
            if (!isValidFile) {
                ErrorAlert('', 'Archivo con formato no valido, formatos permitidos: ' + validFilesTypes.join(' '));
                args.set_cancel(true);
                return false;
            }

            if (filesize > sizeallow) {
                ErrorAlert('', 'Archivo excede el tamaño permitido');
                args.set_cancel(true);
                return false;
            }
        }
        ShowLanding();
        return true;

    };
    function uploadError(sender, args) {

        //alert("You have selected a file: " + args._fileName);
        //if (uploadCustomerError != "") {
        //    alert(+ "The ErrorMessage is " + uploadCustomerError);
        //    uploadCustomerError = "";
        //}
    }
</script>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdTicket" />
            <asp:HiddenField runat="server" ID="hfIdEstatusActual" />
            <asp:HiddenField runat="server" ID="hfIdUsuario" />
            <asp:HiddenField runat="server" ID="hfIdGrupoUsuario" />
            <asp:HiddenField runat="server" ID="hfPropietario" />
            <asp:HiddenField runat="server" ID="hfTipoTicket" />
            <asp:HiddenField runat="server" ID="hfTieneEncuesta" Value="false" />
            <asp:HiddenField runat="server" ID="hfEncuestaRespondida" Value="false" />
            <asp:HiddenField runat="server" ID="hfFileTypes" />
            <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12">

                    <section class="module">
                        <div class="module-inner">
                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom">
                                    <span class="fa fa-envelope fa-30x padding-10 " />
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-10">
                                    <div class="row">
                                        <label class="module-title titulosBandeja">
                                            <asp:Label runat="server" ID="lblNoticket" />:
                                               
                                            <asp:Label runat="server" ID="lblTituloTicket"></asp:Label></label>
                                        <br />
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblNombreCorreo" /><br />
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblFechaAlta"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-lg-7 col-md-3 col-sm-12">
                                    <div class="col-lg-12 col-md-12 col-sm-12 table-responsive no-border">
                                        <table class="table no-border tableHeadTicket">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Asignación" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Agente" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Prioridad" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="SLA" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Fecha estimada <br>de solución"></asp:Label></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Estatus" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblAsignacion" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row ocultaTexto" style="min-width: 86px; max-width: 86px;">
                                                        <asp:Label runat="server" ID="lblAgenteAsignado" CssClass="ocultaTexto" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row">
                                                        <i runat="server" id="iPrioridad" class="fa fa-exclamation fontRed iconoFont margin-top-5"></i>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row text-center">
                                                        <i runat="server" id="iSLA" class="fa fa-bomb iconoFont margin-top-5"></i>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row text-center">
                                                        <asp:Label runat="server" ID="lblFechaSla" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="text-center" style="margin: auto; width: 100%; color: white; border-radius: 5px" runat="server" id="divEstatus">
                                                        <asp:Label runat="server" ID="lblEstatus" Text="Abierto" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section class="module">
                        <div class="module-inner">

                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-12 col-lg-1Custom">
                                </div>
                                <div class="col-lg-11 col-md-10 col-sm-10 ">
                                    <div class="row no-padding-top margin-top0" runat="server">
                                        <div class="col-lg-12 col-md-12 col-sm-12 no-padding-top margin-top0">
                                            <label>Escribe  un nuevo comentario</label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-12 col-lg-1Custom">
                                    <asp:Image runat="server" ID="imgProfileNewComment" CssClass="imageCircle image40" ImageUrl="~/assets/images/profiles/profile-1.png" />
                                </div>
                                <div class="col-lg-11 col-md-10 col-sm-10">
                                    <div class="row no-padding-top margin-top0" runat="server">
                                        <div class="col-lg-12 col-md-12 col-sm-12 no-padding-top margin-top0">
                                            <asp:TextBox ID="txtConversacion" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control no-padding-top no-margin-top" MaxLength="999" />
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12 text-right padding-10-top padding-10-bottom">
                                            <ajax:AsyncFileUpload ID="afuArchivo" runat="server" UploaderStyle="Traditional" OnClientUploadStarted="UploadStart" OnUploadedComplete="afuArchivo_OnUploadedComplete" OnClientUploadComplete="HideLanding" OnClientUploadError="uploadError" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" />
                                            <asp:Button ID="btnEstatus" runat="server" Text="Modificar estado del Ticket" CssClass="btn btn-guardar" OnClick="btnCambiarEstatus_OnClick" />
                                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-guardar" OnClick="btnEnviar_OnClick" />
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <hr />

                            <div class="row margin-top-5" runat="server" id="divHistorial">
                                <asp:Repeater runat="server" ID="rptConversaciones" OnItemDataBound="rptConversaciones_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="row" style="border-top: 1px solid #f3f3f7; border-bottom: 1px solid #f3f3f7">
                                            <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom">
                                                <asp:Image runat="server" ImageUrl='<%# Eval("Foto") != null ? "~/DisplayImages.ashx?id=" + Eval("IdUsuario") : "~/assets/images/profiles/profile-1.png"%>' ID="imgAgente" CssClass="imageCircle image40" />
                                            </div>
                                            <div class='<%# (bool) Eval("Privado") ? "col-lg-11 col-md-10 col-sm-10 private" : "col-lg-11 col-md-10 col-sm-10 public" %>'>
                                                <div class="col-lg-12 col-md-12 col-sm-12 no-margin-bottom no-padding-bottom">
                                                    <h4 class="no-margin-bottom no-padding-bottom titulosBandeja14">
                                                        <asp:Label runat="server" ID="lblNombre" Text='<%# Eval("Nombre") %>'></asp:Label></h4>
                                                </div>
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Label runat="server" ID="lblFecha" Text='<%# Eval("FechaHora") %>'></asp:Label>
                                                </div>

                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Label runat="server" ID="lblMensaje" Text='<%# Eval("Comentario") %>'></asp:Label>
                                                </div>
                                                <asp:Repeater runat="server" ID="rptArchivos">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" NavigateUrl='<%# ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioMascara + "~" + Eval("Archivo"))) %>' Text='<%# Eval("Archivo") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="row">
                                <div class="row" style="border-bottom: 1px solid #f3f3f7">
                                    <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom ">
                                        <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-1.png" ID="imgUsuarioTicket" CssClass="imageCircle image40" />
                                    </div>
                                    <div class="col-lg-11 col-md-10 col-sm-10">
                                        <div class="col-lg-12 col-md-12 col-sm-12 no-margin-bottom no-padding-bottom">
                                            <h4 class="no-margin-bottom no-padding-bottom titulosBandeja14">
                                                <asp:Label runat="server" ID="lblNombreU" Text="" /></h4>
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <asp:Label runat="server" ID="lblFecha" Text='<%# Eval("FechaHora") %>' />
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <ucMascara:UcDetalleMascaraCaptura runat="server" ID="UcDetalleMascaraCaptura" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <ucCambioEstatus:UcCambiarEstatusTicket runat="server" ID="ucCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
