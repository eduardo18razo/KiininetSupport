<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltroCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.UcFiltroCatalogos" %>
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
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfFileTypes" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label runat="server" ID="lblDescCatalogo"></asp:Label>
            </div>
            <div class="panel-body">
                <div runat="server" id="divControles">
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
