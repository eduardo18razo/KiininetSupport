<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleUbicacion.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleUbicacion" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="panel panel-primary">
            <%--<div class="panel-heading">
                Ubicacion
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label class="col-sm-2 control-label" runat="server">Pais</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblPais"></asp:Label>

                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlCompañia" class="col-sm-2 control-label" runat="server">Campus</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblCampus" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlDirecion" class="col-sm-2 control-label" runat="server">Torre</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblTorre" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlSubDireccion" class="col-sm-2 control-label" runat="server">Piso</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblPiso" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlGerencia" class="col-sm-2 control-label" runat="server">Zona</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblZona" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlSubGerencia" class="col-sm-2 control-label" runat="server">Sub Zona</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblSubZona" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label for="ddlJefatura" class="col-sm-2 control-label" runat="server">SiteRack</asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label CssClass="form-control" runat="server" ID="lblsite" />
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="panel-footer" style="text-align: center">
                <asp:Button ID="btnCerrarOrganizacion" runat="server" CssClass="btn btn-success btn-lg" Text="Aceptar" OnClick="btnCerrarOrganizacion_OnClick" />
                <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger btn-lg" Text="Cerrar" data-dismiss="modal" />
            </div>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
