<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleOrganizacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:Repeater runat="server" ID="rptOrganizacion">
            <HeaderTemplate>
                <table class="table table-striped display">
                    <thead>
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblHolding">Nivel 1</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblCompania">Nivel 2</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblDireccion">Nivel 3</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblSubDireccion">Nivel 4</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblGerencia">Nivel 5</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblSubGerencia">Nivel 6</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblJefatura">Nivel 7</asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblIdOrganizacion" Text='<%# Eval("Id")%>' Visible="False" />
                        <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                    </td>
                    <td><%# Eval("Holding.Descripcion")%></td>
                    <td><%# Eval("Compania.Descripcion")%></td>
                    <td><%# Eval("Direccion.Descripcion")%></td>
                    <td><%# Eval("SubDireccion.Descripcion")%></td>
                    <td><%# Eval("Gerencia.Descripcion")%></td>
                    <td><%# Eval("SubGerencia.Descripcion")%></td>
                    <td><%# Eval("Jefatura.Descripcion")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                                            </table>
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
