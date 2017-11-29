<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCambiarContrasena.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcCambiarContrasena" %>
<div class="panel panel-primary">
    <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel-heading">
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
                <h3>Cambiar Contraseña</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" class="col-sm-2 control-label">Contraseña Actual</asp:Label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtContrasenaActual" type="password" CssClass="form-control obligatorio" Style="text-transform: none"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" class="col-sm-2 control-label">Contraseña Nueva</asp:Label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtContrasenaNueva" type="password" CssClass="form-control obligatorio" Style="text-transform: none"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" class="col-sm-2 control-label">Confirma Contraseña Nueva</asp:Label>
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtConfirmaContrasenaNueva" type="password" CssClass="form-control obligatorio" Style="text-transform: none"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-footer text-center">
                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" OnClick="btnAceptar_OnClick" CssClass="btn btn-success" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
