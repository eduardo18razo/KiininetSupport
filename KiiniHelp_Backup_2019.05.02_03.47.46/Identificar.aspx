﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Identificar.aspx.cs" Inherits="KiiniHelp.Identificar" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Kiininet CPX</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/styles_movil.css" />
    <link rel="stylesheet" href="assets/css/divs.css" />
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
<body class="bodyBackgroundPassword" >
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
        <asp:UpdatePanel ID="upGeneral" runat="server" class="heigth100">
            <ContentTemplate>
                <div runat="server" class="heigth100">
                    <section class="panelsectionbacground panelsection">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-lg-8 col-md-8 col-sm-12 col-xs-12 col-lg-offset-2 col-md-offset-2 col-sm-offset-0 col-xs-offset-0">
                                    <h1 class="form-box-heading2">
                                        <span>Recuperar Contraseña </span>
                                    </h1>
                                    <div class="form-box-inner2">
                                        <h2 class="title">¿Olvidaste tu contraseña?</h2>
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div data-parsley-validate class="form-horizontal">
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:Label runat="server" CssClass="text-left col-lg-12 no-padding-left">Ingresa tu usuario, correo electrónico o telefono celular:</asp:Label>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="sr-only" for="login-email">Email or username</label>
                                                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control text-no-transform" onkeydown="return (event.keyCode!=13);"/>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                                                        <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4"
                                                            CssClass="col-lg-4 col-md-4 col-sm-12 col-xs-12 margin-bottom-5" 
                                                            CaptchaHeight="60" CaptchaWidth="150" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                            FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                                        <asp:Label runat="server" CssClass="text-left col-lg-12 no-padding-left">Ingresa el texto de la imagen:</asp:Label>
                                                        <asp:TextBox class="form-control text-uppercase" ID="txtCaptcha" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off"/>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Button CssClass="btn btn-default" ID="btnCancelar" Width="73px" Text="Cancelar" runat="server" OnClick="btnCancelar_Click" />
                                                        <asp:Button CssClass="btn btn-primary" Text="Continuar" Width="73px" runat="server" OnClick="btnBuscar_OnClick" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                            <div class="row copyright2">
                                <img src="assets/images/logo_kinninet_blanco.png"><br />
                            </div>

                        </div>
                    </section>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="modal fade" id="modalRegistroExito" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h6 id="modal-new-ticket-label" class="modal-title">Registro Exitoso
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
