<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCatalogoNiveles.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcCatalogoNiveles" %>
<div class="modal fade" id="editNivel" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="width: 100%">
    <asp:UpdatePanel ID="upNivel" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" style="width: 100%">
                <div class="modal-content" style="width: 100%">
                    <div class="modal-header">
                        <div class="alert alert-warning" id="panelAlertaNivel" runat="server" visible="false">
                            <div>
                                <div style="float: left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div style="float: left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <asp:Repeater runat="server" ID="rptErrorNivel">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <h4 class="modal-title">
                            <asp:Label runat="server" ID="lblTitleCatalogo"></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <asp:HiddenField runat="server" ID="hfCatalogo" />
                            <div>
                                <div class="form-group">
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuarioNivel" CssClass="DropSelect" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Descripcion</label>
                                        <asp:TextBox runat="server" ID="txtDescripcionNivel" placeholder="DESCRIPCION" class="form-control"  onkeydown="return (event.keyCode!=13);" MaxLength="50" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox runat="server" ID="chkNivelHabilitado" Text="Habilitado" Checked="True" Visible="False" />
                                        <asp:CheckBox runat="server" ID="chkNivelTerminal" Text="Es Nodo terminal" Checked="False" Visible="True" AutoPostBack="True" OnCheckedChanged="chkNivelTerminal_OnCheckedChanged" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div runat="server" id="divInfoConculta" visible="False">
                            <div class="panel-primary">
                                <div class="panel-heading">
                                    <div class="col-sm-3 control-label">
                                        Tipo Información
                                    </div>
                                    <div class="col-sm-3 control-label">
                                        Información
                                    </div>
                                    <div class="clear-fix clearfix"></div>
                                </div>
                                <div class="panel-body">
                                    <asp:Repeater runat="server" ID="rptInformacion" OnItemDataBound="rptInformacion_OnItemDataBound" OnItemCreated="rptInformacion_OnItemCreated">
                                        <ItemTemplate>
                                            <div class="row">
                                                <asp:Label runat="server" ID="lblIndex" Text='<%# Container.ItemIndex %>' Visible="False"></asp:Label>
                                                <div class="col-sm-3 control-label">
                                                    <asp:CheckBox runat="server" Text='<%# Eval("TipoInfConsulta.Descripcion") %>' Checked="False" ID="chkInfoConsulta" OnCheckedChanged="chkInfoConsulta_OnCheckedChanged" />
                                                </div>
                                                <div runat="server" class="col-sm-4 control-label margen-arriba">
                                                    <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsBaseDatos")) %>'>
                                                        <asp:DropDownList runat="server" ID="ddlPropietario" CssClass="DropSelect" />
                                                        <asp:Button runat="server" Text="+" />
                                                    </div>
                                                    <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsDirectorio")) %>'>
                                                        <asp:DropDownList runat="server" ID="ddlDocumento" CssClass="DropSelect" />
                                                        <asp:Button runat="server" Text="+" />
                                                    </div>
                                                    <div runat="server" visible='<%# Convert.ToBoolean(Eval("TipoInfConsulta.EsUrl")) %>'>
                                                        <asp:DropDownList runat="server" ID="ddlUrl" CssClass="DropSelect" />
                                                        <asp:Button runat="server" Text="+" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGuardarNivel" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnGuardarNivel_OnClick" />
                        <asp:Button ID="btnCancelarNivel" runat="server" CssClass="btn btn-danger" Text="Cancelar" data-dismiss="modal" OnClick="btnCancelarNivel_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCancelarNivel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>
