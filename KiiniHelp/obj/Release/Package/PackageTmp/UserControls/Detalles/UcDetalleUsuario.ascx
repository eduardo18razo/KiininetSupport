<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleUsuario" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUbicacion.ascx" TagPrefix="uc1" TagName="UcDetalleUbicacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleOrganizacion.ascx" TagPrefix="uc1" TagName="UcDetalleOrganizacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGrupoUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleGrupoUsuario" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
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
                        <ul>
                            <li><%# Container.DataItem %></li>
                        </ul>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3>
                    <asp:Label runat="server" ID="lblUserName"></asp:Label></h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-sm-2 control-label" Text="Nombre:"></asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label runat="server" CssClass="form-control" ID="lblNombre" Enabled="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-sm-2 control-label" Text="Usuario:"></asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label runat="server" CssClass="form-control" ID="lblUsuario" Enabled="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-sm-2 control-label" Text="Puesto:"></asp:Label>
                        <div class="col-sm-10 ">
                            <asp:Label runat="server" CssClass="form-control" ID="lblPuesto"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2 ">
                            <asp:CheckBox runat="server" ID="chkVip" Text="Vip" Enabled="False" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-sm-3 control-label" Text="Datos Adicionales"></asp:Label>
                    </div>
                    <%--TELEFONOS--%>
                    <div class="form-group">
                        <div class="col-sm-10 ">
                            <div class="panel panel-primary">
                                <div class="panel-heading" role="tab" id="headingTelefono">
                                    <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTelefonos" aria-expanded="true" aria-controls="collapseOne" style="cursor: pointer">
                                        <div class="col-xs-6 col-sm-3">Telefono</div>
                                        <div class="col-xs-6 col-sm-3">
                                            Numero telefono
                                        </div>
                                        <div class="col-xs-6 col-sm-3">
                                            Extensiones
                                        </div>
                                    </div>
                                </div>
                                <div id="collapseTelefonos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTelefono">
                                    <div class="panel-body">
                                        <asp:Repeater runat="server" ID="rptTelefonos">
                                            <ItemTemplate>
                                                <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                    <div class="row">
                                                        <div class="col-xs-5 col-md-3">
                                                            <asp:Label runat="server"><%# Eval("TipoTelefono.Descripcion") %></asp:Label>
                                                        </div>
                                                        <div class="col-xs-5 col-md-3">
                                                            <asp:Label runat="server" ID="txtNumero" Text='<%# Eval("Numero") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                        </div>
                                                        <div class="col-xs-4 col-md-3" runat="server" visible='<%# Eval("TipoTelefono.Extension") %>'>
                                                            <asp:Label runat="server" ID="txtExtension" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="40" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--CORREOS--%>
                    <div class="form-group">
                        <div class="col-sm-10 ">
                            <div class="panel panel-primary">
                                <div class="panel-heading" role="tab" id="headingCorreos">
                                    <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseCorreos" aria-expanded="false" aria-controls="collapseTwo" style="cursor: pointer">
                                        <div class="col-xs-6 col-sm-3">Correo</div>
                                    </div>
                                </div>
                                <div id="collapseCorreos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingCorreos">
                                    <div class="panel-body">
                                        <asp:Repeater runat="server" ID="rptCorreos">
                                            <ItemTemplate>
                                                <asp:Label runat="server" CssClass="form-control" Text='<%# Eval("Correo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--ORGANIZACION--%>
                    <div class="form-group">
                        <div class="col-sm-10 ">
                            <div class="panel panel-primary">
                                <div class="panel-heading" role="tab" id="headingOrganizacion">
                                    <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOrganizacion" aria-expanded="true" aria-controls="collapseOne" style="cursor: pointer">
                                        <div class="col-xs-6 col-sm-3">Organizacion</div>
                                    </div>
                                </div>
                                <div id="collapseOrganizacion" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOrganizacion">
                                    <div class="panel-body">
                                        <uc1:UcDetalleOrganizacion runat="server" ID="UcDetalleOrganizacion" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--UBICACION--%>
                    <div class="form-group">
                        <div class="col-sm-10 ">
                            <div class="panel panel-primary">
                                <div class="panel-heading" role="tab" id="headingUbicacion">
                                    <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseUbicacion" aria-expanded="true" aria-controls="collapseOne" style="cursor: pointer">
                                        <div class="col-xs-6 col-sm-3">Ubicación</div>
                                    </div>
                                </div>
                                <div id="collapseUbicacion" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingUbicacion">
                                    <div class="panel-body">
                                        <uc1:UcDetalleUbicacion runat="server" ID="UcDetalleUbicacion" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--GRUPOS--%>
                    <div class="form-group">
                        <div class="col-sm-10 ">
                            <div class="panel panel-primary">
                                <div class="panel-heading" role="tab" id="headingGrupos">
                                    <div class="row collapsed panel-title" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseGrupos" aria-expanded="true" aria-controls="collapseOne" style="cursor: pointer">
                                        <div class="col-xs-6 col-sm-3">Roles y Grupos</div>
                                    </div>
                                </div>
                                <div id="collapseGrupos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingGrupos">
                                    <div class="panel-body">
                                        <uc1:UcDetalleGrupoUsuario runat="server" ID="UcDetalleGrupoUsuario" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer" style="text-align: center">
                    <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btnCerrarModal" Text="Cerrar" OnClick="btnCerrarModal_OnClick" />
                    <%--
                    <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="btnModificar" Text="Cerrar" OnClick="btnCerrarModal_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-lg btn-danger" ID="Button2" Text="Cerrar" OnClick="btnCerrarModal_OnClick" />--%>
                </div>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
