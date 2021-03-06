﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaInformacionConsulta" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<script>
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
    function uploadComplete() {
        $get("<%=ReloadThePanel.ClientID %>").click();
    }
</script>
<asp:HiddenField runat="server" ID="hfFileName" />
<asp:HiddenField runat="server" ID="hfEsAlta" Value="true" />
<asp:HiddenField runat="server" ID="hfIdInformacionConsulta" />
<asp:HiddenField runat="server" ID="hfValueText" />

<ol class="breadcrumb">
    <li>
        <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
    <li>Centro de Soporte</li>
    <li>Artículos</li>
    <li class="active">Nuevo</li>
</ol>

<asp:UpdatePanel runat="server" ID="upAltaInformacionConsulta" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfComentario" />
        <asp:HiddenField runat="server" ID="hfFileTypes" />
        <asp:HiddenField runat="server" ID="hfMaxSizeAllow" Value="0" />
        <section class="module">
            <div class="row">
                <div class="col-lg-8 col-md-8">
                    <div class="module-inner">

                        <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Título del Artículo</label>
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left no-padding-right">
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control no-padding-left no-margin-left" MaxLength="50" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                        </div>

                    </div>

                </div>
                <div class="col-lg-4 col-md-6 margin-top-btn-consulta no-padding-top">
                    <div class="module-inner text-center">
                        <asp:Button runat="server" CssClass="btn btn-default" Height="29px" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary margin-left-5" Height="29px" Text="Previsualizar" ID="btnPreview" OnClick="btnPreview_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-success margin-left-5" Height="29px" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-8 col-md-8">
                    <div class="module-inner">
                        <asp:TextBox runat="server" ID="txtSummerEditor" Height="350px" ClientIDMode="Static" Style="width: 100%" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4">
                    <div class="module-inner">
                        <asp:Label runat="server" Text="Palabras de Búsqueda" />
                        <hr />
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtBusqueda" ClientIDMode="Static" TextMode="MultiLine" Rows="5" CssClass="form-control widht100" MaxLength="500"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Text="Etiquetas" />
                        <hr />
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtTags" ClientIDMode="Static" TextMode="MultiLine" Rows="5" CssClass="form-control widht100" MaxLength="500"></asp:TextBox>
                        </div>
                        <asp:Label runat="server" Text="Adjuntos" />
                        <br />
                        <hr />
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upArchivos">
                            <ContentTemplate>
                                <div class="form-group">
                                    <asp:Repeater runat="server" ID="rptFiles">
                                        <ItemTemplate>
                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                <span class="row">
                                                    <span class="col-lg-10 col-md-10 col-sm-10"><i class="fa fa-file-o"></i>
                                                        <asp:Label runat="server" ID="lblFile" Text='<%# Eval("NombreArchivo")%>' />
                                                    </span>
                                                    <asp:LinkButton runat="server" CssClass="col-lg-1 col-md-1 col-sm-1" ID="btnRemoveFile" CommandArgument='<%# Eval("NombreArchivo")%>' OnClick="btnRemoveFile_OnClick"><i class="fa fa-remove"></i></asp:LinkButton>

                                                    <asp:Label runat="server" ID="Label1" CssClass="col-lg-10 col-md-10 col-sm-10" Text='<%# Eval("Tamaño")%>' />
                                                </span>
                                                <hr />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="form-group">
                                    <span class="span-upload">
                                        <ajax:AsyncFileUpload ID="afuArchivo" runat="server" CssClass="FileUploadClass" UploaderStyle="Traditional" OnClientUploadStarted="UploadStart" OnClientUploadComplete="uploadComplete" OnUploadedComplete="afuArchivo_OnUploadedComplete" OnClientUploadError="uploadError" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" />
                                        Cargar archivos (max 10 MB)
                                                <br />
                                        <br />
                                        <asp:Button ID="ReloadThePanel" runat="server" Text="hidden" OnClick="ReloadThePanel_OnClick" Style="visibility: hidden" />
                                    </span>

                                    <div class="clearfix"></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </section>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(SetComment);
            function SetComment() {
                $('#txtSummerEditor').summernote({
                    toolbar: [
   // [groupName, [list of button]]
   ['estilos', ['fontname', 'fontsize', 'forecolor', 'bold', 'iatlic', 'underline', 'strikethrough', 'superscript', 'subscript']],
   ['insertar', ['picture', 'link', 'video', 'table', 'hr']],
   ['parrafos', ['style', 'ol', 'ul', 'paragraph', 'height']],
   ['utilerias', ['undo', 'redo', 'fullscreen', 'codeview', 'clear']]
                    ],
                    lang: 'es-ES',
                    tabsize: 2,
                    height: 350
                });
                $("#txtSummerEditor").on('summernote.blur', function () {
                    $('#txtSummerEditor').html($('#txtSummerEditor').summernote('code'));
                });
            }

            $(document).ready(SetTags);
            function SetTags() {
                $('#txtSummerEditor').summernote({
                    toolbar: [
   // [groupName, [list of button]]
   ['estilos', ['fontname', 'fontsize', 'forecolor', 'bold', 'iatlic', 'underline', 'strikethrough', 'superscript', 'subscript']],
   ['insertar', ['picture', 'link', 'video', 'table', 'hr']],
   ['parrafos', ['style', 'ol', 'ul', 'paragraph', 'height']],
   ['utilerias', ['undo', 'redo', 'fullscreen', 'codeview', 'clear']]
                    ],
                    lang: 'es-ES',
                    tabsize: 2,
                    height: 350
                });
                $("#txtSummerEditor").on('summernote.blur', function () {
                    $('#txtSummerEditor').html($('#txtSummerEditor').summernote('code'));
                });
                $('#txtBusqueda').tagsInput({ width: 'auto', defaultText: 'Agregar', delimiter: '|' });
                $('#txtTags').tagsInput({ width: 'auto', defaultText: 'Agregar', delimiter: '|' });
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTags);
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
