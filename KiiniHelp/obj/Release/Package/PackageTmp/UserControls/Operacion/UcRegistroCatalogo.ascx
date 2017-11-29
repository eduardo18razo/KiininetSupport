<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcRegistroCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcRegistroCatalogo" %>
<asp:UpdatePanel ID="upNivel" runat="server">
    <ContentTemplate>
        <div class="panel panel-primary">
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
            <div class="panel-heading">

                <h3>Agregar registro
                        <asp:Label runat="server" ID="lblTitle" /></h3>
            </div>
            <div class="panel-body">
                <asp:HiddenField runat="server" ID="hfEsAlta" />
                <asp:HiddenField runat="server" ID="hfIdCatalogo" />
                <div class="form-group">
                    <asp:Label runat="server" Text="Descripción" />
                    <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" />
                </div>
            </div>
            <div class="panel-footer">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_OnClick" CssClass="btn btn-success" />
                <asp:Button runat="server" ID="btnLimpiar" Text="Limpiar" OnClick="btnLimpiar_OnClick" CssClass="btn btn-primary" />
                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" OnClick="btnCancelar_OnClick" CssClass="btn btn-danger" />

            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
