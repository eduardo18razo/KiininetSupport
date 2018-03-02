<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Identificar.aspx.cs" Inherits="KiiniHelp.Identificar" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiininet CPX</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />

    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/styles_movil.css" />

    <link rel="stylesheet" href="assets/css/divs.css" />
    <link rel="stylesheet" href="assets/css/checkBox.css" />

    <link rel="stylesheet" href="assets/css/sumoselect.css" />

    <link rel="stylesheet" href="assets/css/controls.css" />
    <link rel="stylesheet" href="assets/tmp/jquery.tagsinput.min.css" />
    <link rel="stylesheet" href="assets/css/elusive-icons.css" />

    <link rel="stylesheet" href="assets/css/modales_movil.css" />
    <link rel="stylesheet" href="assets/css/main_movil.css" />

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
<body style="height: 100%;">
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
        <asp:UpdatePanel ID="upGeneral" runat="server" style="height: 100%">
            <ContentTemplate>
                <section class="login-section auth-section">
                    <div class="container">
                        <div class="row">
                            <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                <h1 class="form-box-heading2"><span>Recuperar Contraseña </span></h1>
                                <div class="form-box-inner2">
                                    <h2 class="title">¿Olvidaste tu contraseña?</h2>
                                    <div class="row">
                                        <div class="form-container col-md-12 col-sm-12 col-xs-12">
                                            <div data-parsley-validate class="form-horizontal">
                                                <div class="form-group email no-margin-bottom">
                                                    <asp:Label runat="server" CssClass="text-left col-lg-12 no-padding-left">Ingresa tu usuario, correo electrónico o número celular:</asp:Label>
                                                </div>

                                                <div class="form-group email">
                                                    <label class="sr-only" for="login-email">Email or username</label>
                                                    <span class="fa fa-user icon"></span>
                                                    <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control login-email" Style="text-transform: none" />
                                                </div>

                                                <div class="form-group">
                                                    <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                                                    <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4"
                                                        CssClass="col-sm-4 margin-bottom-5"
                                                        CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                        FontColor="#D20B0C" NoiseColor="#B1B1B1" />

                                                    <asp:Label runat="server" CssClass="text-left col-lg-12 no-padding-left">Ingresa el texto de la imagen:</asp:Label>
                                                    <asp:TextBox class="form-control" ID="txtCaptcha" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                </div>

                                                <div class="form-group email">
                                                    <asp:Button CssClass="btn btn-default" ID="btnCancelar" Text="Cancelar" runat="server" OnClick="btnCancelar_Click" />

                                                    <asp:Button CssClass="btn btn-primary" Text="Continuar" runat="server" OnClick="btnBuscar_OnClick" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="copyright2 center">
                            <img src="assets/images/logo_kinninet_blanco.png" class="center"><br>
                            &copy; 2018
                       
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
