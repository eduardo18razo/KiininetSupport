<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUbicacion.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaUbicacion" %>
<asp:UpdatePanel ID="upUbicacion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:HiddenField runat="server" ID="hfTipoUsuario" />
                <div class="modal-header" id="panelAlertaUbicacion" runat="server" visible="false">
                    <div class="alert alert-danger" role="alert">
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
                        <asp:Repeater runat="server" ID="rptErrorUbicacion">
                            <ItemTemplate>
                                <ul>
                                    <li><%# Container.DataItem %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Organizacion
                    </div>
                    <div class="panel-body">
                        <div class="form-group" runat="server" id="divTipoUsuario">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlHolding" class="col-sm-2 control-label" runat="server">Tipo de Usuario</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlpais" class="col-sm-2 control-label" runat="server">País</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlpais" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlpais_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlCampus" class="col-sm-2 control-label" runat="server">Campus</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlCampus" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlCampus_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" data-toggle="modal" data-target="#editCampus" data-backdrop="static" data-keyboard="false" CommandName="Campus" CommandArgument="0" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlTorre" class="col-sm-2 control-label" runat="server">Torre</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlTorre" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlTorre_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAddTorre" data-toggle="modal" data-target="#editCatalogoUbicacion" data-backdrop="static" data-keyboard="false" CommandName="Torre" CommandArgument="3" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlPiso" class="col-sm-2 control-label" runat="server">Piso</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlPiso" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlPiso_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAddPiso" data-toggle="modal" data-target="#editCatalogoUbicacion" data-backdrop="static" data-keyboard="false" CommandName="Piso" CommandArgument="4" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddllZona" class="col-sm-2 control-label" runat="server">Zona</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlZona" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlZona_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAddZona" data-toggle="modal" data-target="#editCatalogoUbicacion" data-backdrop="static" data-keyboard="false" CommandName="Zona" CommandArgument="5" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlSubZona" class="col-sm-2 control-label" runat="server">Sub Zona</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlSubZona" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlSubZona_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAddSubZona" data-toggle="modal" data-target="#editCatalogoUbicacion" data-backdrop="static" data-keyboard="false" CommandName="Sub Zona" CommandArgument="6" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlSiteRack" class="col-sm-2 control-label" runat="server">Site/Rack</asp:Label>
                                <asp:DropDownList Enabled="False" runat="server" ID="ddlSiteRack" Width="450px" CssClass="DropSelect" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAddSite" data-toggle="modal" data-target="#editCatalogoUbicacion" data-backdrop="static" data-keyboard="false" CommandName="Site Rack" CommandArgument="7" />
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptar_OnClick" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<%--MODAL CATALOGOS--%>
<div class="modal fade" id="editCatalogoUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upCatlogos" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" id="panelAlertaCatalogo" runat="server" visible="false">
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
                            <asp:Repeater runat="server" ID="rptErrorCatalogo">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <asp:Label runat="server" ID="lblTitleCatalogo"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <asp:HiddenField runat="server" ID="hfCatalogo" />
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="ddlTipoUsuarioCatalogo" class="col-sm-2 control-label">Tipo de Usuario</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuarioCatalogo" CssClass="DropSelect" Enabled="False" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="txtDescripcionCatalogo" class="col-sm-2 control-label">Descripcion</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="txtDescripcionCatalogo" placeholder="DESCRIPCION" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                </div>
                            </div>
                            <asp:CheckBox runat="server" ID="chkHabilitado" Checked="True" Visible="False" />
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button ID="btnGuardarCatalogo" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnGuardarCatalogo_OnClick" />
                        <asp:Button ID="btnCancelarCatalogo" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelarCatalogo_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCancelarCatalogo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>

<%--MODAL CAMPUS--%>
<div class="modal fade" id="editCampus" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <asp:UpdatePanel ID="upCampus" runat="server">
                <ContentTemplate>
                    <div class="modal-header" id="panelAlertaCampus" runat="server" visible="false">
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
                            <asp:Repeater runat="server" ID="rptErrorCampus">
                                <ItemTemplate>
                                    <ul>
                                        <li><%# Container.DataItem %></li>
                                    </ul>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Datos generales
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                    <asp:DropDownList runat="server" ID="ddlTipoUsuarioCampus" CssClass="DropSelect" Enabled="False"/>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Descripcion</label>
                                    <asp:TextBox runat="server" ID="txtDescripcionCampus" placeholder="DESCRIPCION" class="form-control OBLIGATORIO" onkeydown="return (event.keyCode!=13);" />
                                </div>
                                <div class="form-group">
                                    <asp:CheckBox runat="server" ID="CheckBox1" Checked="True" Visible="False" />
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Codigo Postal</label>
                                    <asp:TextBox runat="server" ID="txtCp" placeholder="CODIGO POSTAL" AutoPostBack="True" OnTextChanged="txtCp_OnTextChanged" class="form-control" onkeypress="return ValidaCampo(this,2)" onkeydown="return (event.keyCode!=13);" />
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Colonia</label>
                                    <asp:DropDownList runat="server" ID="ddlColonia" CssClass="DropSelect" OnSelectedIndexChanged="ddlColonia_OnSelectedIndexChanged" AutoPostBack="True"/>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Municipio</label>
                                    <asp:Label runat="server" class="col-sm-3 control-label" ID="lblMunicipio"/>
                                    <label class="col-sm-2 control-label">Estado</label>
                                    <asp:Label runat="server" class="col-sm-3 control-label" ID="lblEstado"/>
                                </div>
                                <div class="form-group">

                                    <label class="col-sm-3 control-label">Calle</label>
                                    <asp:TextBox runat="server" ID="txtCalle" placeholder="CALLE" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Numero Exterior</label>
                                    <asp:TextBox runat="server" ID="txtNoExt" placeholder="NUMERO EXTERIOR" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Numero Interior</label>
                                    <asp:TextBox runat="server" ID="txtNoInt" placeholder="NUMERO INTERIOR" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button ID="btnCrearCampus" runat="server" CssClass="btn btn-success" Text="Aceptar" ValidationGroup="vsData" OnClick="btnCrearCampus_OnClick" />
                        <asp:Button ID="btnCancelarCampus" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelarCampus_OnClick" />
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarCampus" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
