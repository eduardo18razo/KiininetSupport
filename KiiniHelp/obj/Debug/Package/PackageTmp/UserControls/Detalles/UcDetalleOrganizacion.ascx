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
        <%--<div class="panel panel-primary">
            <%--<div class="panel-heading">
                Organizacion
            </div>--%>
        <%--<div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label class="col-sm-2 control-label" runat="server">Holding</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblPais"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">

                        <asp:Label for="ddlCompañia" class="col-sm-2 control-label" runat="server">Compaía</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblCampus" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlDirecion" class="col-sm-2 control-label" runat="server">Direccion</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblTorre" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlSubDireccion" class="col-sm-2 control-label" runat="server">Sub Direccion</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblPiso" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlGerencia" class="col-sm-2 control-label" runat="server">Gerencia</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblZona" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlSubGerencia" class="col-sm-2 control-label" runat="server">Sub Gerencia</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblSubZona" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlJefatura" class="col-sm-2 control-label" runat="server">Jefatura</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblsite" />
                        </div>
                    </div>
                </div>
            </div>--%>
        <%--<div class="panel-footer" style="text-align: center">
                <asp:Button ID="btnCerrarOrganizacion" runat="server" CssClass="btn btn-success btn-lg" Text="Aceptar" OnClick="btnCerrarOrganizacion_OnClick" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger btn-lg" Text="Cerrar" data-dismiss="modal" />
            </div>--%>
        <%--</div>--%>
    </ContentTemplate>
</asp:UpdatePanel>
