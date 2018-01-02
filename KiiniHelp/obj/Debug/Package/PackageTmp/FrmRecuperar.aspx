<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRecuperar.aspx.cs" Inherits="KiiniHelp.FrmRecuperar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Recuperar Cuenta</title>
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />

    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/bootstrap-markdown.css" />
    <link rel="stylesheet" href="assets/css/elegant-icons.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <style>
        .radio label {
            padding-left: 5px;
        }
    </style>
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
     <%--   <asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
                <asp:HiddenField runat="server" ID="hfEsLink" Value="false" />
                <asp:HiddenField runat="server" ID="hfParametrosConfirmados" Value="false" />
                <asp:HiddenField runat="server" ID="hfValueNotivicacion" Value="false" />
                <asp:Button runat="server" ID="btncontinuar" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-block btn-primary" Visible="False" />
                <div runat="server" id="divQuestion">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <% Response.Write(ConfigurationManager.AppSettings["Brand"]); %></span> </h1>
                                    <div class="form-box-inner">
                                        <h2 class="title text-center">¿Cómo quieres cambiar tu contraseña?</h2>
                                        <div class="row">
                                            <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                                <div data-parsley-validate id="" class="form-horizontal">
                                                    <div class="form-inline" runat="server">
                                                        <asp:RadioButton runat="server" ID="rbtnCorreo" Text="Quiero recibir un correo con un enlace" GroupName="Options" CssClass="radio" Style="font-weight: normal" OnCheckedChanged="rbtnCorreo_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:RadioButton runat="server" ID="rbtnSms" Text="Quiero recibir un SMS con mi codigo" GroupName="Options" CssClass="radio" Style="font-weight: normal" OnCheckedChanged="rbtnSms_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:RadioButton runat="server" ID="rbtnPreguntas" Text="Quiero contestar preguntas Reto" GroupName="Options" CssClass="radio" Style="font-weight: normal" OnCheckedChanged="rbtnPreguntas_OnCheckedChanged" AutoPostBack="True" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divCodigoVerificacion" visible="False">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <% Response.Write(ConfigurationManager.AppSettings["Brand"]); %></span> </h1>
                                    <div class="form-box-inner">
                                        <h2 class="title text-center">¿Cómo quieres cambiar tu contraseña?</h2>
                                        <div class="row">
                                            <asp:HiddenField runat="server" ID="hfIdSend" />
                                            <asp:HiddenField runat="server" ID="hfValueSend" />
                                            <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                                <div data-parsley-validate id="" class="form-horizontal">
                                                    <div class="form-horizontal">
                                                        <div><span>Ingresa el código que recibiste:</span></div>
                                                        <br>

                                                        <label class="sr-only" for="login-email">Email or username</label>

                                                        <asp:TextBox required="required" runat="server" placeholder="Código de verificación" CssClass="form-control login-email" ID="txtCodigo" />

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
                                                    <asp:Label runat="server" Text=""></asp:Label>
                                                    <div style="margin-top: 15px">
                                                        <asp:Button runat="server" ID="btnContinuarCodigo" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-block btn-primary" />
                                                        <p class="alt-path">
                                                            <asp:HyperLink CssClass="signup-link" NavigateUrl="~/Default.aspx" runat="server">Regresar al inicio</asp:HyperLink>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divPreguntas" visible="False">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <% Response.Write(ConfigurationManager.AppSettings["Brand"]); %></span> </h1>
                                    <div class="form-box-inner">
                                        <h2 class="title text-center">¿Cómo quieres cambiar tu contraseña?</h2>
                                        <div class="row">
                                            <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                                <div data-parsley-validate id="" class="form-horizontal">
                                                    <div class="form-horizontal">
                                                        <asp:Repeater runat="server" ID="rptPreguntas">
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <asp:Label runat="server" Text='<%# Eval("Id") %>' ID="lblId" Visible="False" />
                                                                    <asp:Label runat="server" Text='<%# Eval("IdUsuario") %>' ID="lblIdUsuario" Visible="False" />
                                                                    <asp:Label runat="server" Text='<%# Eval("Pregunta") %>' class="col-xs-6 col-md-3" ID="lblPregunta" />
                                                                    <div class="col-sm-9">
                                                                        <asp:TextBox required="required" runat="server" ID="txtRespuesta" CssClass="form-control obligatorio" Style="text-transform: none" />
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div style="margin-top: 15px">
                                                        <asp:Button runat="server" ID="btnContinuarPreguntas" Text="Continuar" OnClick="btncontinuar_OnClick" CssClass="btn btn-block btn-primary" />
                                                        <p class="alt-path">
                                                            <asp:HyperLink CssClass="signup-link" NavigateUrl="~/Default.aspx" runat="server">Regresar al inicio</asp:HyperLink>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                <div runat="server" id="divChangePwd" visible="False">
                    <section class="login-section auth-section">
                        <div class="container">
                            <div class="row">
                                <div class="form-box col-md-8 col-sm-12 col-xs-12 col-md-offset-2 col-sm-offset-0 xs-offset-0">
                                    <h1 class="form-box-heading logo text-center"><span class="highlight">SOPORTE <% Response.Write(ConfigurationManager.AppSettings["Brand"]); %></span> </h1>
                                    <div class="form-box-inner">
                                        <h2 class="title text-center">Cambia tu contraseña</h2>
                                        <div class="row">
                                            <p>Bienvenido, Rubén González Camarena</p>
                                            <div class="form-container col-md-6 col-sm-12 col-xs-12">
                                                <div data-parsley-validate id="" class="form-horizontal">
                                                    <div class="form-group password">
                                                        <div><span>Ingresa tu nueva contraseña:</span></div>
                                                    </div>
                                                    <br>
                                                    <div class="form-group password">
                                                        <label class="sr-only" for="login-password">Password</label>
                                                        <span class="fa fa-lock icon"></span>
                                                        <asp:TextBox required="required" runat="server" ID="txtContrasena" type="password" CssClass="form-control login-password" Style="text-transform: none" placeholder="Contraseña"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group password">
                                                        <label class="sr-only" for="login-password">Password</label>
                                                        <span class="fa fa-lock icon"></span>
                                                        <asp:TextBox required="required" runat="server" ID="txtConfirmar" type="password" CssClass="form-control login-password" Style="text-transform: none" placeholder="Confirma tu contraseña"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group password">
                                                        <asp:Button class="btn btn-block btn-primary" runat="server" Text="Cambiar password" OnClick="btncontinuar_OnClick" />
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
                            <div class="copyright text-center">&copy; 2017 - Powered by Kiininet</div>
                        </div>
                    </section>
                </div>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>


    </form>
</body>
</html>
