<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcFormulario" %>
<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
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
                ErrorAlert('', 'Archivo con formato no valido');
                args.set_cancel(true);
                return false;
            }


            if (filesize > sizeallow) {
                ErrorAlert('', 'Archivo demasiado grande');
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
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfPreview" />
        <br>
        <asp:HiddenField runat="server" ID="hfTicketGenerado" />
        <asp:HiddenField runat="server" ID="hfRandomGenerado" />
        <asp:HiddenField runat="server" ID="hfFileTypes" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />

        <section class="module no-border">
            <div class="row">
                <div class="module-inner">
                    <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                        <div class="module-heading">
                            <h3 class="module-title18px">
                                <asp:Label runat="server" ID="lblTitle" />
                            </h3>
                            <p>
                                <asp:Label runat="server" ID="lblDescripcion"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner no-padding-top">
                        <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-12 col-md-12 col-sm-12" />
                        <div runat="server" id="divCaptcha" class="col-lg-12 col-md-12 col-sm-12">
                            <div class="text-right">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                                    <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4" CssClass="col-lg-2 col-md-2 col-sm-2 col-lg-offset-10 col-md-offset-10 col-sm-offset-10"
                                        CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                        FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <div class="col-lg-2 col-md-2 col-sm-2 col-lg-offset-10 col-md-offset-10 col-sm-offset-10">
                                        <asp:TextBox CssClass="form-control text-uppercase" ID="txtCaptcha" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="text-right col-lg-12 col-md-12 col-sm-12">
                            <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                            <asp:Button type="button" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                        </div>
                        <br />
                        <div runat="server" id="divControles">
                        </div>
                        <div runat="server" id="divRegistraUsuario">
                            <div class="margin-bottom-10">
                                <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="modal fade" id="modalExitoTicket" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h6 class="modal-title" id="myModalLabel">
                    <asp:Label runat="server" Text="Tu ticket se creo con éxito" />
                </h6>
            </div>
            <div class="modal-body">
                <hr />
                <p class="h4">
                    <strong>Tu no. de ticket:
                                       
                                <asp:Label runat="server" ID="lblNoTicket" /></strong><br />
                </p>
                <p class="h4">
                    <strong>Clave de registro:
                                   
                                <asp:Label runat="server" ID="lblRandom" /></strong>
                </p>
                <hr />
                En breve recibirás un correo con los datos de tu ticket para que puedas dar seguimiento.
            </div>
        </div>
    </div>
</div>
