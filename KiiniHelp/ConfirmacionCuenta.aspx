<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacionCuenta.aspx.cs" Inherits="KiiniHelp.ConfirmacionCuenta" %>

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
    <link rel="stylesheet" href="assets/css/pe-7-icons.css" />
    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/elegant-icons.css" />
    <link rel="stylesheet" href="assets/css/bootstrap-markdown.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/styles_movil.css" />
    <link rel="stylesheet" href="assets/css/modales_movil.css" />
    <link rel="stylesheet" href="assets/css/main_movil.css" />
</head>
<body class="bodyBackgroundPassword">
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
        <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <section class="panelsectionbacground panelsection">
                    <div class="container">
                        <div class="row">
                            <div class="form-box col-lg-8 col-md-8 col-sm-12 col-xs-12 col-lg-offset-2 col-md-offset-2 col-sm-offset-0 col-xs-offset-0">
                                <h1 class="form-box-heading2">
                                    <span>Confirma tus Cuenta</span>
                                </h1>
                                <div class="form-box-inner2">
                                    <h2 class="title">Verifica tus Datos</h2>
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12">
                                            <div class="form-horizontal">
                                                <div class="form-group no-margin-bottom text-left">
                                                    <asp:Label runat="server" ID="lblCaracteristicas" Text="La contraseña debe tener:" CssClass="control-label text-bold2" />
                                                    <ul runat="server" id="listParamtros" class="list styleList text-bold2">
                                                        <li runat="server" ID="paramLongitud">
                                                            <asp:Label runat="server" ID="lblLongitud" Text="Longitud minima de 8 caracteres"></asp:Label></li>
                                                        <li runat="server" ID="paramMayuscula">
                                                            <asp:Label runat="server" ID="Label3" Text="1 Mayuscula"></asp:Label></li>
                                                        <li runat="server" ID="paramNumero">
                                                            <asp:Label runat="server" ID="Label6" Text="1 Numero"></asp:Label></li>
                                                        <li runat="server" ID="paramEspecial">
                                                            <asp:Label runat="server" ID="Label7" Text="1 Caracter especial"></asp:Label></li>
                                                    </ul>
                                                </div>

                                                <div class="form-group no-margin-bottom">
                                                    <asp:Label runat="server" class="text-left col-lg-12 no-padding-left" Text="Contraseña" />
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:TextBox runat="server" ID="txtContrasena" type="password" CssClass="form-control text-no-transform"></asp:TextBox>
                                                </div>

                                                <div class="form-group no-margin-bottom">
                                                    <asp:Label runat="server" class="text-left col-lg-12 no-padding-left" Text="Confirma Contraseña" />
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:TextBox runat="server" ID="txtConfirmaContrasena" type="password" CssClass="form-control text-no-transform"></asp:TextBox>
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:Label runat="server" class="text-left col-lg-12 no-padding-left" Text="Si tu número celular es correcto envía el código para verificar tu cuenta, en caso contrario puedes modificar tu número celular" />
                                                </div>
                                                <asp:Label runat="server" ID="lblId" Visible="False" />
                                                <asp:Label runat="server" ID="lblTelefono" Visible="False" />
                                                <asp:Label runat="server" ID="lblIdUsuario" Visible="False" />
                                                <div class="form-group no-margin-bottom">
                                                    <asp:Label runat="server" ID="Label1" Text="Número Celular" CssClass="control-label" />
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:TextBox runat="server" CssClass="form-control" Text='<%# Eval("Numero") %>' ID="txtNumeroEdit" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:Label runat="server" ID="Label2" Text="Confirma tu número Celular" CssClass="control-label" />
                                                </div>
                                                <div class="form-group no-margin-bottom">
                                                    <asp:TextBox runat="server" CssClass="form-control" Text='<%# Eval("Numero") %>' ID="txtNumeroConfirmEdit" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                </div>
                                                <div class="form-group">
                                                    <div class="row">
                                                        <asp:Button runat="server" Text="Cambiar Número" CssClass="btn btn-primary " ID="btnChangeNumber" CommandArgument="0" OnClick="btnChangeNumber_OnClick" Width="130px" Visible="False" />
                                                        <asp:Button runat="server" Text="Confirmar Número" CssClass="btn btn-primary" ID="btnSendNotification" OnClick="btnSendNotification_OnClick" Width="130px" />
                                                    </div>
                                                </div>


                                                <div runat="server" id="divConfirmacion" visible="False">
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:Label runat="server" Text="Recibiras un código de confirmación en tu celular, favor de digitarlo" class="text-left col-lg-12 no-padding-left text-bold2" />
                                                    </div>

                                                    <div class="form-group no-margin-bottom">
                                                        <asp:Label runat="server" Text="Código de confirmación" class="text-left col-lg-10 no-padding-left" />
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upReenvio">
                                                            <ContentTemplate>
                                                                <asp:Timer runat="server" ID="timebtn" Interval="5000" OnTick="timebtn_OnTick" Enabled="False"></asp:Timer>
                                                                <asp:Button runat="server" ID="btnReenviarcodigo" CssClass="btn btn-primary col-lg-2" Text="Reenviar código" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCodigo" onkeypress="return ValidaCampo(this,2)" MaxLength="5" />
                                                    </div>

                                                    <div class="form-group no-margin-bottom">
                                                        <asp:Label ID="Label4" runat="server" Text="Ingresa tu pregunta secreta" class="text-left col-lg-12 no-padding-left" />
                                                    </div>
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:TextBox ID="txtIdPregunta" runat="server" CssClass="form-control" Visible="False" />
                                                        <asp:TextBox ID="txtPregunta" runat="server" CssClass="form-control text-no-transform" />
                                                    </div>
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:Label ID="Label5" runat="server" Text="Ingresa la respuesta de tu pregunta secreta" class="text-left col-lg-12 no-padding-left"></asp:Label>
                                                    </div>
                                                    <div class="form-group no-margin-bottom">
                                                        <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control text-no-transform"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row copyright2">
                                        <img src="assets/images/logo_kinninet_blanco.png"><br />
                                        &copy; 2018
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
