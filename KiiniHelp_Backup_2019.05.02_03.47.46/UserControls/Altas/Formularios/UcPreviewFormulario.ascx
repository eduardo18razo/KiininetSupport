<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcPreviewFormulario" %>
<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
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
        <asp:HiddenField runat="server" ID="hfPreview" />
        <asp:HiddenField runat="server" ID="hfFileTypesPreview" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllowPreview" Value="0" />

        <ol class="breadcrumb">
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
            <li>Centro de Soporte</li>
            <li>Formularios</li>
            <li>Nuevo Formulario</li>
            <li class=" active">Previsualizar</li>
        </ol>

        <section class="module no-border">
            <div class="row">
                <div class="module-inner">
                    <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                        <div class="module-heading">
                            <h3 class="module-title18px">
                                <asp:Label runat="server" ID="lblTitle" />
                            </h3>
                            <p>
                                <asp:Label runat="server" ID="lblDescripcionMascara" />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner no-padding-top">
                        <asp:Label runat="server" ID="Label1" class="col-lg-12 col-md-12 col-sm-12" />

                        <br />
                        <div runat="server" id="divRegistraUsuario">
                            <div class="margin-bottom-10">
                                <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                            </div>
                        </div>
                        <div runat="server" id="divControles">
                        </div>

                    </div>
                </div>
            </div>
            <div class="row">
                <div class="row widht100 text-center">
                    <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" />
                    <asp:Button type="button" class="btn btn-success" runat="server" Text="Crear ticket" ID="btnGuardar" />
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>

