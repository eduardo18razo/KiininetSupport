<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaArea.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaArea" %>
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
<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdArea" />
        <asp:HiddenField runat="server" ID="hfFileName" />
        <asp:HiddenField runat="server" ID="hfFileTypes" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblOperacion" Font-Bold="true" />
            </h6>
        </div>
        <div class="modal-body">
            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div>
                            <div class="form-group margin-top">
                                Escribe el nombre de la Categoría<br />
                                <asp:TextBox runat="server" ID="txtDescripcionAreas" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50" />
                            </div>
                            <ajax:AsyncFileUpload ID="afDosnload" runat="server" UploaderStyle="Traditional" OnClientUploadStarted="UploadStart" OnUploadedComplete="afDosnload_OnUploadedComplete" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" Visible="False" />
                            <p class="margin-top-20">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardarArea" OnClick="btnGuardar_OnClick" />
                            </p>
                        </div>
                        <p class="text-right margin-top-40">
                            <asp:Button runat="server" CssClass="btn btn-success" Text="Terminar" ID="btnTerminar" visible="False" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
