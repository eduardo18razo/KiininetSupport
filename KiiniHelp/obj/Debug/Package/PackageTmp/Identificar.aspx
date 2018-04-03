<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Identificar.aspx.cs" Inherits="KiiniHelp.Identificar" %>

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


</head>
<body class="heigth100">
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
                <div class="progressBar">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..." />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upGeneral" runat="server" class="heigth100">
            <ContentTemplate>
                <div runat="server" class="heigth100">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-lg-8 col-md-8 col-sm-12 col-xs-12 col-lg-offset-2 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading2">
                                        <span>Recuperar Contraseña </span>
                                    </h1>
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
                                                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control login-email text-no-transform" />
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:CustomValidator ErrorMessage="" OnServerValidate="OnServerValidate" runat="server" />
                                                        <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="4"
                                                            CssClass="col-lg-4 col-md-4 col-sm-12 col-xs-12 margin-bottom-5" 
                                                            CaptchaHeight="60" CaptchaWidth="150" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                            FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                                        <asp:Label runat="server" CssClass="text-left col-lg-12 no-padding-left">Ingresa el texto de la imagen:</asp:Label>
                                                        <asp:TextBox class="form-control text-uppercase" ID="txtCaptcha" runat="server" />
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
                            <div class="row copyright2 center">
                                <img src="assets/images/logo_kinninet_blanco.png" class="center"><br />
                                &copy; 2018
                            </div>

                        </div>
                    </section>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
