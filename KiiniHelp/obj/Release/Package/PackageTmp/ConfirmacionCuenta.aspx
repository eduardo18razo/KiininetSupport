<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacionCuenta.aspx.cs" Inherits="KiiniHelp.ConfirmacionCuenta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Confirmacion Cuenta Usuario</title>
    <link href="BootStrap/css/bootstrap.css" rel="stylesheet" />
    <link href="BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="BootStrap/css/CheckBoxStyle.css" rel="stylesheet" />
    <link href="BootStrap/css/Calendar.css" rel="stylesheet" />
    <link href="BootStrap/css/DropDown.css" rel="stylesheet" />
    <link href="BootStrap/css/divs.css" rel="stylesheet" />
    <link href="BootStrap/css/FileInput.css" rel="stylesheet" />
    <link href="BootStrap/css/Headers.css" rel="stylesheet" />
    <link href="BootStrap/css/stylemainmenu.css" rel="stylesheet" />
    <link href="BootStrap/css/Defaults.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripMain" runat="server" EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/BootStrap/js/jquery-2.1.1.min.js" />
                <asp:ScriptReference Path="~/BootStrap/js/bootstrap.min.js" />
                <asp:ScriptReference Path="~/BootStrap/js/super-panel.js" />
                <asp:ScriptReference Path="~/BootStrap/js/validation.js" />
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer runat="server" ID="tmpSendNotificacion" OnTick="tmpSendNotificacion_OnTick" Interval="60000" Enabled="False"/>
                <header class="" id="panelAlertaGeneral" runat="server" visible="False" style="width: 600px; margin: 0 auto">
                    <div class="alert alert-danger">
                        <div>
                            <div style="float: left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div style="float: left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix" />
                        </div>
                        <hr />
                        <asp:Repeater runat="server" ID="rptErrorGeneral">
                            <ItemTemplate>
                                <%# Eval("Detalle")  %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <div class="panel panel-primary" style="width: 600px; margin: 0 auto">
                    <div class="panel-heading">
                        <h1>Confirma tu Cuenta</h1>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:Label runat="server" class="col-sm-2 control-label">Contraseña</asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox runat="server" ID="txtContrasena" type="password" CssClass="form-control obligatorio" Style="text-transform: none"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" class="col-sm-2 control-label">Confirma Contraseña</asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox runat="server" ID="txtConfirmaContrasena" type="password" CssClass="form-control obligatorio" Style="text-transform: none"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Repeater runat="server" ID="rptConfirmacion">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                                        <asp:Label runat="server" ID="lblTelefono" Text='<%# Eval("Numero") %>' Visible="False" />
                                        <asp:Label runat="server" ID="lblIdUsuario" Text='<%# Eval("IdUsuario") %>' Visible="False" />
                                        <asp:Label runat="server" Text="Codigo de confirmacion" class="col-sm-2 control-label" />
                                        <div class="col-sm-2">
                                            <asp:TextBox runat="server" CssClass="form-control obligatorio" ID="txtCodigo" onkeypress="return ValidaCampo(this,2)" MaxLength="5" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox runat="server" CssClass="form-control" Text='<%# Eval("Numero") %>' ID="txtNumeroEdit" ReadOnly="True" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                        </div>
                                        <asp:Button runat="server" Text="Cambiar Numero" CssClass="btn btn-sm btn-primary" ID="btnChangeNumber" CommandArgument="0" OnClick="btnChangeNumber_OnClick" />
                                        <asp:Button runat="server" Text="Enviar Codigo" CssClass="btn btn-sm btn-primary" ID="btnSendNotification" OnClick="btnSendNotification_OnClick" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="form-inline">
                                <asp:TextBox ID="txtIdPregunta" runat="server" CssClass="form-control" Visible="False"></asp:TextBox>
                                <br />
                                <div class="form-inline">

                                    <asp:Label ID="Label4" runat="server" Text="Pregunta" class="col-sm-2 control-label izquierda"></asp:Label>
                                    <asp:TextBox ID="txtPregunta" runat="server" CssClass="form-control" style="text-transform: none"></asp:TextBox>
                                </div>
                                <div class="form-inline margen-arriba">
                                    <asp:Label ID="Label5" runat="server" Text="Respuesta" class="col-sm-2 control-label izquierda"></asp:Label>
                                    <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control" style="text-transform: none"></asp:TextBox>
                                    <asp:Button ID="btnAddPregunta" runat="server" CssClass="btn btn-sm btn-success" Text="Agregar" OnClick="btnAddPregunta_OnClick" />
                                </div>
                            </div>

                            <asp:Repeater runat="server" ID="rptPreguntas">
                                <HeaderTemplate>
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-3">Pregunta</div>
                                        <div class="col-xs-6 col-sm-3">Respuesta</div>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="margen-arriba">
                                            <div class="col-xs-6 col-md-3">
                                                <asp:Label runat="server" ID="lblPregunta" Text='<%# Eval("Pregunta") %>'></asp:Label>
                                            </div>
                                            <div class="col-xs-5 col-md-3">
                                                <asp:Label runat="server" ID="lblRespuesta"><%# Eval("Respuesta") %></asp:Label>
                                            </div>
                                            <asp:LinkButton runat="server" Text="Editar" OnClick="OnClick" CommandArgument='<%# Eval("Id") %>' />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel-footer center-content-div">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
