<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketPortal.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcTicketPortal" %>
<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
<%@ Register TagPrefix="ms" Namespace="MSCaptcha" Assembly="MSCaptcha, Version=2.0.1.36094, Culture=neutral, PublicKeyToken=b9ff12f28cdcf412" %>

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
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfFileTypes" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />
        
        <div class="margin-bottom-10">
                <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
            </div>

        <div runat="server" id="divControles">
        </div>
        <div>
            
            <div class="margin-bottom-10 text-center">
                <div class="form-group">
                    <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                    <ms:CaptchaControl ID="captchaTicket" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4" CssClass="col-lg-12 col-md-12 col-sm-12 col-xs-12"
                        CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                        FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                </div>
                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="form-group col-lg-2 col-md-2 col-lg-offset-5 col-md-offset-5 col-sm-12 col-xs-12">
                        <asp:TextBox CssClass="form-control text-uppercase" ID="txtCaptcha" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off" />
                    </div>
                </div>
                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12 text-left">
                    <span style="color: red">* Campos obligatorios </span>
                </div>
                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button type="button text-left" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hfTicketGenerado" />
        <asp:HiddenField runat="server" ID="hfRandomGenerado" />
    </ContentTemplate>
</asp:UpdatePanel>
