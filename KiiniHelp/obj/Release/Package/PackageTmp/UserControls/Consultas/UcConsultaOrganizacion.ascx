<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaOrganizacion" %>
<div style="height: 100%;">
    <script>
        function dbClicOrganizacion(e) {
            debugger;
            var idSeleccion = document.getElementById("<%= this.FindControl("hfIdSeleccion").ClientID %>");
            idSeleccion.value = e.parentElement.id;
            var modalName = document.getElementById("<%= this.FindControl("hfModalName").ClientID %>");
            $(modalName.value).modal('hide');
            document.getElementById("<%= this.FindControl("btnCerrar").ClientID %>").click();

        };
        function SeleccionaOrganizacion(id) {
            var table = $("#tblHeader");
            table.find('tr').each(function (i, ev) {
                if (ev.id === id)
                    ev.style.background = "gray";
            });
        }

    </script>
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfModalName" />
            <asp:HiddenField runat="server" ID="hfIdSeleccion" />
            <div id="contextMenuOrganizacion" class="panel-heading contextMenu">
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfModal" />
            </div>
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
                    <asp:Label runat="server" ID="lbotest"></asp:Label>
                    <h3><asp:Label runat="server" Text="Organizaciones" ID="lblTitleOrganizacion"/></h3>
                </div>
                <div class="panel-body">
                    <div class="panel panel-primary">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingFiltros">
                                <h4 class="panel-title">
                                    <div role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFiltros" aria-expanded="true" aria-controls="collapseFiltros" style="cursor: pointer">
                                        Ocultar/Mostrar Filtros
                                    </div>
                                </h4>
                            </div>
                            <div id="collapseFiltros" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingFiltros">
                                <div class="panel-body">
                                    <div class="form-horizontal">

                                        <div class="form-group">
                                            <asp:Label Width="14%" for="ddlTipoUsuario" class="col-xs-1 control-label" runat="server">Selecciona tipo de Usuario</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                            <asp:Label Width="14%" runat="server" Text="Filtrar por" class="col-xs-1 control-label"></asp:Label>
                                            <asp:TextBox Width="14%" runat="server" ID="txtFiltroDecripcion" CssClass=" col-xs-1 form-control" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"></asp:TextBox>
                                            <asp:Button runat="server" Text="Buscar" ID="btnBuscar" CssClass="col-xs-1 btn btn-sm btn-primary" OnClick="btnBuscar_OnClick"></asp:Button>
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label Width="14%" ID="lblNivel1" class="col-xs-1 control-label" for="ddlHolding" runat="server">Nivel 1</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel2" class="col-xs-1 control-label" for="ddlCompañia" runat="server">Nivel 2</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel3" class="col-xs-1 control-label" for="ddlDirecion" runat="server">Nivel 3</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel4" class="col-xs-1 control-label" for="ddlSubDireccion" runat="server">Nivel 4</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel5" class="col-xs-1 control-label" for="ddlGerencia" runat="server">Nivel 5</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel6" class="col-xs-1 control-label" for="ddlSubGerencia" runat="server">Nivel 6</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel7" class="col-xs-1 control-label" for="ddlJefatura" runat="server">Nivel 7</asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <asp:DropDownList runat="server" ID="ddlHolding" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlHolding_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlCompañia" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCompañia_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlDireccion" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDirecion_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlSubDireccion" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSubDireccion_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlGerencia" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlGerencia_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlSubGerencia" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSubGerencia_OnSelectedIndexChanged" />
                                                <asp:DropDownList runat="server" ID="ddlJefatura" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlJefatura_OnSelectedIndexChanged" />
                                            </div>
                                            <div class="form-group">
                                                <asp:Button runat="server" CssClass="col-xs-1 btn btn-primary" ID="btnNew" Text="Agregar Holding" Width="14%" OnClick="btnNew_OnClick" Visible="False" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-primary">
                            <div class="panel-body">
                                <asp:Repeater runat="server" ID="rptResultados" OnItemCreated="rptResultados_OnItemCreated">
                                    <HeaderTemplate>
                                        <table border="1" class="table table-bordered table-hover table-responsive" id="tblHeader">
                                            <thead>
                                                <tr align="center">
                                                    <td><asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblHolding">Nivel 1</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCompania">Nivel 2</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblDireccion">Nivel 3</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSubDireccion">Nivel 4</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblGerencia">Nivel 5</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSubGerencia">Nivel 6</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblJefatura">Nivel 7</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Habilitado</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Editar</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <%--oncontextmenu="ContextMenuOrganizacion()" --%>
                                            <td style='font-weight: normal; padding: 0; text-align: left; font-size: 10px;' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("TipoUsuario.Descripcion")%></td>
                                            <td style='<%# Eval("Compania") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("Holding.Descripcion")%></td>
                                            <td style='<%# Eval("Direccion") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("Compania.Descripcion")%></td>
                                            <td style='<%# Eval("SubDireccion") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("Direccion.Descripcion")%></td>
                                            <td style='<%# Eval("Gerencia") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("SubDireccion.Descripcion")%></td>
                                            <td style='<%# Eval("SubGerencia") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("Gerencia.Descripcion")%></td>
                                            <td style='<%# Eval("Jefatura") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("SubGerencia.Descripcion")%></td>
                                            <td style='<%# Eval("Jefatura") != null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>' ondblclick='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) == 1 ? "" : "dbClicOrganizacion(this)" %>'><%# Eval("Jefatura.Descripcion")%></td>
                                            <td style="padding: 0; font-size: 10px;">
                                                <asp:Button Style="width: 50px;" CssClass='<%# (bool) Eval("Habilitado") ? "btn btn-sm btn-success" : "btn btn-sm btn-danger"%>' runat="server" ID="btnBajaAlta"
                                                    Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' CommandArgument='<%# (bool) Eval("Habilitado") ? "true" : "false"%>' CommandName='<%# Eval("Id")%>' OnClick="btnBajaAlta_OnClick" />
                                            </td>
                                            <td style="padding: 0; font-size: 10px;">
                                                <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Editar" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Repeater ID="rptPager" runat="server">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                            OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Button runat="server" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" Style="visibility: hidden" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
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
                            <asp:HiddenField runat="server" ID="hfAlta" />
                            <div class="form-horizontal">
                                <div class="form-group" runat="server" visible="False">
                                    <label class="col-sm-2 control-label">Tipo de Usuario</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuarioCatalogo" CssClass="DropSelect" Enabled="False" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Descripcion</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="txtDescripcionCatalogo" placeholder="DESCRIPCION" class="form-control" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" autofocus="autofocus"/>
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
