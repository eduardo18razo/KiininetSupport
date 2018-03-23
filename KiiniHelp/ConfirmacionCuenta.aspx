<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacionCuenta.aspx.cs" Inherits="KiiniHelp.ConfirmacionCuenta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmación Cuenta Usuario</title>
    <link rel="Shortcut Icon" href="favicon.ico" type="image/x-icon" />

    <link rel='stylesheet' href="assets/css/font.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" href="assets/css/pe-7-icons.css" />
    <%--<link rel="stylesheet" href="assets/css/pe-7-icons.css" />--%>

    <link rel="stylesheet" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" href="assets/css/styles.css" />
    <link rel="stylesheet" href="assets/css/menuStyle.css" />
    <link rel="stylesheet" href="assets/css/divs.css" />
    <link rel="stylesheet" href="assets/css/checkBox.css" />
    <link rel="stylesheet" href="assets/tmp/chosen.css" />
    <link rel="stylesheet" href="assets/css/sumoselect.css" />
    <link rel="stylesheet" href="assets/tmp/editor/ui/trumbowyg.css" />
    <link rel="stylesheet" href="assets/tmp/editor/ui/trumbowyg.min.css" />
    <link rel="stylesheet" href="assets/css/controls.css" />
    <link rel="stylesheet" href="assets/tmp/jquery.tagsinput.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/assets/js/jquery.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap.js" />
                <asp:ScriptReference Path="~/assets/js/imagesloaded.js" />
                <asp:ScriptReference Path="~/assets/js/masonry.js" />
                <asp:ScriptReference Path="~/assets/js/main.js" />
                <asp:ScriptReference Path="~/assets/js/modernizr.custom.js" />
                <asp:ScriptReference Path="~/assets/js/pmenu.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.js" />
                <asp:ScriptReference Path="~/assets/js/bootstrap-notify.min.js" />
                <asp:ScriptReference Path="~/assets/js/Notificaciones.js" />
                <asp:ScriptReference Path="~/assets/tmp/chosen.jquery.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/trumbowyg.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/plugins/base64/trumbowyg.base64.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/editor/plugins/upload/trumbowyg.upload.min.js" />
                <asp:ScriptReference Path="~/assets/tmp/jquery.tagsinput.min.js" />
                <asp:ScriptReference Path="~/assets/js/jquery.sumoselect.min.js" />
                <asp:ScriptReference Path="~/assets/js/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" runat="server" ClientIDMode="Static">
            <ProgressTemplate>
                <div class="progressBar">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Images/loading.gif" AlternateText="Loading ..." ToolTip="Loading ..."/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer runat="server" ID="tmpSendNotificacion" OnTick="tmpSendNotificacion_OnTick" Interval="60000" Enabled="False" />
                <div class="panel panel-primary no-margin-top no-margin-bottom margin-left-auto margin-right-auto widht800px">
                    <div class="no-padding-top no-padding-bottom padding-20-left padding-20-right">
                        <h4 class="h2">Confirma tus Datos</h4>
                        <hr />
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:Label runat="server" class="col-sm-4 control-label">Contraseña</asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtContrasena" type="password" CssClass="form-control obligatorio text-no-transform"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" class="col-sm-4 control-label">Confirma Contraseña</asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox runat="server" ID="txtConfirmaContrasena" type="password" CssClass="form-control obligatorio text-no-transform"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12 text-right">
                                <asp:Repeater runat="server" ID="rptConfirmacion">
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                                            <asp:Label runat="server" ID="lblTelefono" Text='<%# Eval("Numero") %>' Visible="False" />
                                            <asp:Label runat="server" ID="lblIdUsuario" Text='<%# Eval("IdUsuario") %>' Visible="False" />
                                            <table>
                                                <tr>
                                                    <td colspan="3" class="padding-65-left text-left padding-40-right">
                                                        <br />
                                                        <asp:Label runat="server" Text="Si tu número celular es correcto envía el código para verificar tu cuenta, en caso contrario puedes modificar tu número celular" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="col-sm-4 padding-10-bottom">
                                                        <asp:Label runat="server" ID="Label1" Text="Número Celular" CssClass="control-label" />
                                                    </td>
                                                    <td class="col-sm-4 padding-10-bottom">
                                                        <div>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text='<%# Eval("Numero") %>' ID="txtNumeroEdit" ReadOnly="True" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                        </div>
                                                    </td>
                                                    <td class="col-sm-4 padding-10-bottom" style="padding-right: 8.5%">
                                                        <asp:Button runat="server" Text="Cambiar Número" CssClass="btn btn-primary" ID="btnChangeNumber" CommandArgument="0" OnClick="btnChangeNumber_OnClick" Width="130px" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="col-sm-4">
                                                        <asp:Label runat="server" Text="Código de confirmación" class="control-label" />
                                                    </td>
                                                    <td class="col-sm-4">
                                                        <div>
                                                            <asp:TextBox runat="server" CssClass="form-control obligatorio" ID="txtCodigo" onkeypress="return ValidaCampo(this,2)" MaxLength="5" />
                                                        </div>
                                                    </td>
                                                    <td class="col-sm-4" style="padding-right: 8.5%">
                                                        <asp:Button runat="server" Text="Enviar Código" CssClass="btn btn-primary" ID="btnSendNotification" OnClick="btnSendNotification_OnClick" Width="130px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="form-group">
                                <asp:TextBox ID="txtIdPregunta" runat="server" CssClass="form-control" Visible="False" />
                                <asp:Label ID="Label4" runat="server" Text="Ingresa tu pregunta secreta" class="col-sm-4 control-label" />
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPregunta" runat="server" CssClass="form-control text-no-transform" />
                                </div>

                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="Ingresa la respuesta de tu pregunta secreta" class="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control text-no-transform"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="center-content-div">
                        <%--panel-footer--%>
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
