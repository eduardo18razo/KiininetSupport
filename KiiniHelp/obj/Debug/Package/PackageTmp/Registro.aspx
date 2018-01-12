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
        function SuccsessAlert(title, msg) {
            $.notify({
                // options
                icon: 'glyphicon glyphicon-ok',
                title: title,
                message: msg,
                target: '_blank'
            }, {
                // settings
                element: 'body',
                position: null,
                type: "success",
                allow_dismiss: true,
                newest_on_top: false,
                showProgressbar: false,
                placement: {
                    from: "top",
                    align: "right"
                },
                offset: 20,
                spacing: 10,
                z_index: 1031,
                delay: 5000,
                timer: 1000,
                url_target: '_blank',
                mouse_over: null,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                onShow: null,
                onShown: null,
                onClose: null,
                onClosed: null,
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
            });
        }
        function ErrorAlert(title, msg) {
            $.notify({
                // options
                icon: 'glyphicon glyphicon-warning-sign',
                title: title,
                message: msg,
                target: '_blank'
            }, {
                // settings
                element: 'body',
                position: null,
                type: "danger",
                allow_dismiss: false,
                newest_on_top: false,
                showProgressbar: false,
                placement: {
                    from: "top",
                    align: "right"
                },
                offset: 20,
                spacing: 10,
                z_index: 99999,
                delay: 5000,
                timer: 1000,
                url_target: '_blank',
                mouse_over: null,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                onShow: null,
                onShown: null,
                onClose: null,
                onClosed: null,
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert" style="z-index=9999999">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
            });
        }
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
                                <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <asp:Label runat="server" ID="lblBrandingModal" /></span> </h1>
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
                                                    
                                                    <p class="text-center" style="margin-top:15px; font-size:22px;">
                                                        <asp:Label runat="server" Font-Bold="true" ID="lblconfirmar" Text="Te llegará un correo electrónico, favor de confirmar tu registro." Visible="false"></asp:Label>
                                                    </p>

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
    </form>
</body>
</html>
