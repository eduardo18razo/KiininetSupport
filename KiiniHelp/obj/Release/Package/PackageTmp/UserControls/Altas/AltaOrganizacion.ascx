<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaOrganizacion" %>
<asp:UpdatePanel ID="upOrganizacion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:HiddenField runat="server" ID="hfTipoUsuario" />
                <div class="modal-header" id="panelAlertaOrganizacion" runat="server" visible="false">
                    <div class="alert alert-danger" role="alert">
                        <div>
                            <div style="float: left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div style="float: left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix"></div>
                        </div>
                        <hr />
                        <asp:Repeater runat="server" ID="rptErrorOrganizacion">
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
                                <asp:Label for="ddlHolding" class="col-sm-3 control-label" runat="server">Tipo de Usuario</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlHolding" class="col-sm-3 control-label" runat="server">Holding</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlHolding" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlHolding_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnAltaHolding" CommandName="Holding" CommandArgument="99" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlCompañia" class="col-sm-3 control-label" runat="server">Compañia</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlCompañia" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlCompañia_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnCompania" CommandName="Compañia" CommandArgument="9" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlDirecion" class="col-sm-3 control-label" runat="server">Direccion</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlDireccion" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlDirecion_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnDireccion" CommandName="Direccion" CommandArgument="10" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlSubDireccion" class="col-sm-3 control-label" runat="server">Sub Direccion</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlSubDireccion" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlSubDireccion_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnSubDireccion" CommandName="Sub Direccion" CommandArgument="11" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlGerencia" class="col-sm-3 control-label" runat="server">Gerencia</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlGerencia" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlGerencia_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnGerencia" CommandName="Gerencia" CommandArgument="12" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlSubGerencia" class="col-sm-3 control-label" runat="server">Sub Gerencia</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlSubGerencia" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlSubGerencia_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnSubGerencia" CommandName="Sub Gerencia" CommandArgument="13" />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="col-sm-offset-1">
                                <asp:Label for="ddlJefatura" class="col-sm-3 control-label" runat="server">Jefatura</asp:Label>
                                <asp:DropDownList runat="server" ID="ddlJefatura" Width="450px" CssClass="DropSelect" AutoPostBack="True" AppendDataBoundItems="True" />
                                <asp:Button runat="server" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="OnClick" ID="btnJefatura" CommandName="Jefatura" CommandArgument="14" />
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button ID="btnAceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptarOrganizacion_OnClick" />
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<%--MODAL CATALOGOS--%>
<div class="modal fade" id="editCatalogoOrganizacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
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
                                        <asp:TextBox runat="server" ID="txtDescripcionCatalogo" placeholder="DESCRIPCION" class="form-control obligatorio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
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
