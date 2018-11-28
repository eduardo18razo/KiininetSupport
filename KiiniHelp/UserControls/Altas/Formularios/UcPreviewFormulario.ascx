<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcPreviewFormulario" %>
<script type="text/javascript">
    var uploadCustomerError = "";
    function UploadStart(sender, args) {
        debugger;
        if (sender._inputFile.files.length > 0) {
            var filesize = ((sender._inputFile.files[0].size) / 1024) / 1024;
            var fileId = sender.get_id();
            var container = document.getElementById('AjaxFileUpload1_FileInfoContainer_' + fileId);
            var sizeallow = document.getElementById("<%= FindControl("hfMaxSizeAllowPreview").ClientID %>").value;
            var validFilesTypes = document.getElementById("<%= FindControl("hfFileTypesPreview").ClientID %>").value.split('|');

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
        <asp:HiddenField runat="server" ID="hfFileTypesPreview" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllowPreview" Value="0" />

        <ol class="breadcrumb">
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
            <li>Help Center</li>
            <li>Formularios</li>
            <li>Nuevo Formulario</li>
            <li class="breadcrumb-item active">Previsualizar</li>

        </ol>

        <section class="module">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <div class="row">
                            <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-10 col-md-10 col-sm-10 TitulosAzul" />
                            <div class="col-lg-2 col-md-2 col-sm-2 text-right no-padding-right">
                                <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" />
                                <asp:Button type="button" class="btn btn-success" runat="server" Text="Crear ticket" ID="btnGuardar" />
                            </div>
                        </div>
                        <hr />
                        <div runat="server" id="divControles" class="margin-top-40">
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>

