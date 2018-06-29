<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="KiiniHelp.Registro" %>

<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiinisupport</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />

    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/elegant-icons.css" />
    <link rel="stylesheet" href="assets/css/bootstrap-markdown.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/styles_movil.css" />
    <link rel="stylesheet" href="assets/css/modales_movil.css" />
    <link rel="stylesheet" href="assets/css/main_movil.css" />


    <script type="text/javascript">
        function MostrarPopup(modalName) {
            $(modalName).on('shown.bs.modal', function () {
                $(this).find('[autofocus]').focus();
            });
            $(modalName).modal({ backdrop: 'static', keyboard: false });
            $(modalName).modal('show');
            return true;
        };
    </script>
</head>
<body class="bg-aqua">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
            <ProgressTemplate>
                <div class="progressBar">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upGeneral" runat="server">
            <ContentTemplate>
                <section class="bienvenido-section panelsection">
                    <div class="container">
                        <div class="row">
                            <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 col-xs-offset-0">
                                <h1 class="form-box-heading2 logo text-center"><span class="highlight">Regístrate
                                    <asp:Label runat="server" ID="lblBrandingModal" Visible="false" /></span> </h1>
                                <div class="form-box-inner2">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                            <div data-parsley-validate class="form-horizontal">
                                                <div class="form-group top15">
                                                    <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                                                </div>
                                                <div class="form-group top15">
                                                    <asp:Button CssClass="btn btn-default margin-top-15 margin-bottom-15" Width="73px" ID="btnCancelar" Text="Cancelar" runat="server" OnClick="btnCancelar_Click" CausesValidation="False" />
                                                    <asp:Button CssClass="btn btn-primary margin-top-15 margin-bottom-15" Width="73px" Text="Enviar" runat="server" OnClick="btnRegistrar_OnClick" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="copyright2 text-center">&copy; 2017 - Powered by Kiininet</div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="modal fade" id="modalRegistroExito" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h6 id="modal-new-ticket-label" class="modal-title">
                            Registro Exitoso
                        </h6>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <p class="h4 centered">
                                    <strong>En breve recibirás un correo electrónico para confirmar tu registro.</strong><br>
                                </p>
                                <p class="h4 centered">
                                    <asp:Button runat="server" Text="Aceptar" ID="btnAceptar" OnClick="btnCerrarExito_OnClick" CssClass="btn btn-primary" />
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
