<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleUbicacion.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleUbicacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:Repeater runat="server" ID="rptUbicacion">
            <HeaderTemplate>
                <table class="table table-striped display" id="tblHeader">
                    <thead>
                        <tr>
                            <th>
                                <asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel1">Nivel 1</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel2">Nivel 2</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel3">Nivel 3</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel4">Nivel 4</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel5">Nivel 5</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel6">Nivel 6</asp:Label></th>
                            <th>
                                <asp:Label runat="server" ID="lblNivel7">Nivel 7</asp:Label></th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblIdUbicacion" Text='<%# Eval("Id")%>' Visible="False" />
                        <button type="button" class="btn btn-default-alt btn-circle"><%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                    </td>
                    <td><%# Eval("Pais.Descripcion")%></td>
                    <td><%# Eval("Campus.Descripcion")%></td>
                    <td><%# Eval("Torre.Descripcion")%></td>
                    <td><%# Eval("Piso.Descripcion")%></td>
                    <td><%# Eval("Zona.Descripcion")%></td>
                    <td><%# Eval("SubZona.Descripcion")%></td>
                    <td><%# Eval("SiteRack.Descripcion")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                                            </table>
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
