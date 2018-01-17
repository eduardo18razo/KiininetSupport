<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="KiiniHelp.Registro" %>

<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Kiinisupport</title>
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link href="assets/css/elegant-icons.css" rel="stylesheet" />
    <link href="assets/css/bootstrap-markdown.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/css/styles.css" />
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
<body class="layout_no_leftnav">
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
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; background-color: rgba(0,0,0, .1); opacity: 1.0;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 25%; left: 40%; border: 10px;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="upGeneral" runat="server">
            <ContentTemplate>
                <section class="auth-section">
                    <div class="container">
                        <div class="row">
                            <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE
                                    <asp:Label runat="server" ID="lblBrandingModal" /></span> </h1>
                                <div class="form-box-inner">
                                    <h2 class="title text-center">Regístrate</h2>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                            <div data-parsley-validate id="" class="form-horizontal">
                                                <div class="form-group email" style="top: 15px">
                                                    <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                                                </div>
                                                <div class="form-group email" style="top: 15px">
                                                    <asp:Button CssClass="btn btn-block btn-primary" Text="Enviar" runat="server" Style="margin-top: 30px" OnClick="btnRegistrar_OnClick" />



                                                    <p class="alt-path text-right">
                                                        <asp:HyperLink CssClass="signup-link" NavigateUrl="~/Default.aspx" runat="server">Regresar al inicio</asp:HyperLink>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="copyright text-center">&copy; 2017 - Powered by Kiininet</div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="modal fade" id="modalRegistroExito" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h3 class="modal-title" id="myModalLabel">
                            <br />
                            Registro Exitoso
                        </h3>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <hr />
                                <p class="h4">
                                    <strong>Te llegará un correo electrónico, favor de confirmar tu registro.
                                        <asp:Label runat="server" ID="lblNoTicket" /></strong><br>
                                </p>
                                <p class="h4">
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
