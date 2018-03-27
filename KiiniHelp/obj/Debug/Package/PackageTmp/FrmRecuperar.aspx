<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRecuperar.aspx.cs" Inherits="KiiniHelp.FrmRecuperar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Recuperar Cuenta</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />

    <link rel='stylesheet' href="assets/css/font.css" />
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
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" class="heigth100">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfEsLink" Value="false" />
                <asp:HiddenField runat="server" ID="hfParametrosConfirmados" Value="false" />
                <asp:HiddenField runat="server" ID="hfValueNotivicacion" Value="false" />
                <asp:Button runat="server" ID="btncontinuar" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-block btn-primary" Visible="False" />
                
                <div runat="server" id="divQuestion" class="heigth100">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading2">
                                        <span>Recuperar contraseña</span>
                                    </h1>
                                    <div class="form-box-inner2">
                                        <h2 class="title">¿Cómo quieres recuperar tu contraseña?</h2>
                                        <div class="row">
                                            <div class="form-container col-md-12 col-sm-12 col-xs-12 margin-bottom-10">
                                                <div data-parsley-validate class="form-horizontal">
                                                    <div class="form-inline text-left" runat="server">
                                                        <asp:RadioButton runat="server" ID="rbtnCorreo" Text="Quiero recibir un correo con un enlace" GroupName="Options" CssClass="radio" OnCheckedChanged="rbtnCorreo_OnCheckedChanged" AutoPostBack="True" />
                                                        <br />
                                                        <asp:RadioButton runat="server" ID="rbtnSms" Text="Quiero recibir un SMS con mi codigo" GroupName="Options" CssClass="radio" OnCheckedChanged="rbtnSms_OnCheckedChanged" AutoPostBack="True" />
                                                        <br />
                                                        <asp:RadioButton runat="server" ID="rbtnPreguntas" Text="Quiero contestar preguntas Reto" GroupName="Options" CssClass="radio" OnCheckedChanged="rbtnPreguntas_OnCheckedChanged" AutoPostBack="True" />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row copyright2 center">
                                <img src="assets/images/logo_kinninet_blanco.png" class="center"><br/>
                                &copy; 2018                       
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divCodigoVerificacion" visible="False" class="heigth100">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading2">
                                        <span>Recuperar contraseña</span>
                                    </h1>
                                    <div class="form-box-inner2">
                                        <h2 class="title">Ingresa el código que recibiste:</h2>
                                        <div class="row">
                                            <asp:HiddenField runat="server" ID="hfIdSend" />
                                            <asp:HiddenField runat="server" ID="hfValueSend" />
                                            <div class="form-container col-md-12 col-sm-12 col-xs-12 margin-bottom-10">
                                                <div data-parsley-validate class="form-horizontal">
                                                    <div class="form-horizontal">

                                                        <label class="sr-only" for="login-email">Email or username</label>

                                                        <asp:TextBox runat="server" CssClass="form-control login-email margin-bottom-25" ID="txtCodigo" />

                                                        <div class="panel-body" runat="server" visible="False">
                                                            <div class="form-horizontal">
                                                                <div class="form-group">
                                                                    <asp:Label runat="server" Text="Comprueba si recibiste un correo electrónico con tu código, que debe tener cinco dígitos." CssClass="col-sm-12" />
                                                                </div>
                                                                <div class="form-group">
                                                                    <asp:Label runat="server" Text="Te enviamos el código a:" CssClass="col-sm-12" />
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-sm-2">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-group email">
                                                        <asp:Button CssClass="btn btn-default" ID="btnCancelar" Text="Cancelar" runat="server" OnClick="btnCancelar_Click" />
                                                        <asp:Button runat="server" ID="btnContinuarCodigo" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-primary" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row copyright2 center">
                                <img src="assets/images/logo_kinninet_blanco.png" class="center"><br/>
                                &copy; 2018                       
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divPreguntas" visible="False" class="heigth100">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading2"><span>Responde tu pregunta secreta</span> </h1>
                                    <div class="form-box-inner2">
                                        <div class="row">
                                            <div class="form-container col-md-offset-2 col-md-8 col-sm-12 col-xs-12">
                                                <div data-parsley-validate class="form-horizontal">
                                                    <div class="form-horizontal margin-bottom-25">
                                                        <asp:Repeater runat="server" ID="rptPreguntas">
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <asp:Label runat="server" Text='<%# Eval("Id") %>' ID="lblId" Visible="False" />
                                                                    <asp:Label runat="server" Text='<%# Eval("IdUsuario") %>' ID="lblIdUsuario" Visible="False" />
                                                                    <asp:Label runat="server" Text='<%# Eval("Pregunta") %>' class="col-xs-12 col-md-12 text-left" ID="lblPregunta" />

                                                                    <div class="col-sm-12">
                                                                        <asp:TextBox runat="server" ID="txtRespuesta" CssClass="form-control obligatorio text-no-transform"/>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>

                                                    <div class="form-group email">
                                                        <asp:Button CssClass="btn btn-default" ID="btnCancelar2" Text="Cancelar" runat="server" OnClick="btnCancelar_Click" />
                                                        <asp:Button runat="server" ID="btnContinuarPreguntas" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-primary" />
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row copyright2 center">
                                <img src="assets/images/logo_kinninet_blanco.png" class="center"><br/>
                                &copy; 2018                       
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divChangePwd" visible="False" class="heigth100">
                    <section class="login-section auth-section ">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading2">
                                        <span>Cambia tu contraseña</span>
                                    </h1>
                                    <div class="form-box-inner2">
                                        <div class="row">
                                            <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                                <div data-parsley-validate class="row">
                                                    <asp:Label runat="server" Text="Ingresa tu nueva contraseña" CssClass="col-lg-12 text-left" />
                                                    <br/>
                                                    <div class="form-group password">
                                                        <label class="sr-only" for="txtContrasena">Password</label>
                                                        <span class="fa fa-lock icon"></span>
                                                        <asp:TextBox runat="server" ID="txtContrasena" type="password" CssClass="form-control login-password text-no-transform" />
                                                    </div>
                                                    <asp:Label runat="server" Text="Confirma tu contraseña" CssClass="col-lg-12 text-left"/>
                                                    <br/>
                                                    <div class="form-group password">
                                                        <label class="sr-only" for="txtConfirmar">Password</label>
                                                        <span class="fa fa-lock icon"></span>
                                                        <asp:TextBox runat="server" ID="txtConfirmar" type="password" CssClass="form-control login-password text-no-transform"/>
                                                    </div>
                                                    <div class="form-group password">
                                                        <asp:Button class="btn btn-block btn-primary" runat="server" Text="Cambiar contraseña" OnClick="btncontinuar_OnClick" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="social-btns col-md-5 col-sm-12 col-xs-12 col-md-offset-1 col-sm-offset-0 col-sm-offset-0">
                                                <div class="divider"><span>IMPORTANTE</span></div>
                                                <ul class="list-unstyled social-login">
                                                    <li>
                                                        <p>Recuerda que para que tu nueva contraseña sea segura debe contenir mínimo 8 caractéres, mayúsculas y minúsculas y algún caracter especial</p>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row copyright2 center">
                                <img src="assets/images/logo_kinninet_blanco.png" class="center"><br/>
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
