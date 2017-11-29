<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaArboles.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaArboles" %>
<div style="height: 100%;">
    <script>
        function dbClic(e) {
            $('#tblHeader').find('tr').dblclick(function (e) {
                alert(e.target.parentElement.id);
            });
        };

        function ContextMenu() {
            var $contextMenu = $("#contextMenuArbol");
            $("body").on("click", function (e) {
                $contextMenu.hide();
                var table = $("#tblHeader");
                table.find('tr').each(function (i, ev) {
                    $(this).css('background', "transparent");
                });
            });
            $("body").on("contextmenu", "table tr", function (e) {
                $contextMenu.css({
                    display: "block",
                    left: e.pageX,
                    top: e.pageY
                });
                var baja = false;
                var alta;
                var parent = e.target.parentElement;
                var nodos = parent.parentElement.childNodes;
                for (var fondo = 0; fondo < nodos.length; fondo++) {
                    if (nodos[fondo].nodeType === 1)
                        parent.parentElement.childNodes[fondo].removeAttribute("style");
                }

                parent.parentElement.parentElement.style.background = 'transparent';
                parent.style.background = "gray";
                var columnas = e.target.parentElement.childNodes;
                for (var z = 0; z < columnas.length; z++) {
                    if (columnas[z].id === "colHabilitado") {
                        baja = (columnas[z].textContent === 'SI');
                    }
                }
                alta = !baja;
                document.getElementById("<%= FindControl("btnBaja").ClientID %>").style.display = baja ? 'block' : 'none';
                document.getElementById("<%= FindControl("btnAlta").ClientID %>").style.display = alta ? 'block' : 'none';
                var elementId = document.getElementById("<%= FindControl("hfId").ClientID %>");
                elementId.value = e.target.parentElement.id;
                return false;
            });

            $contextMenu.on("click", "button", function () {
                debugger;
                $contextMenu.hide();
            });
        };
    </script>
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <div id="contextMenuArbol" class="panel-heading contextMenu">
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfModal" />
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfId" />
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Baja" ID="btnBaja" OnClick="btnBaja_OnClick" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Alta" ID="btnAlta" OnClick="btnAlta_OnClick" />

                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Editar" ID="btnEditar" OnClick="btnEditar_OnClick" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" />
                </div>
            </div>
            <div class="modal-header" id="panelAlertaGeneral" runat="server" visible="false">
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
                    <asp:Repeater runat="server" ID="rptErrorGeneral">
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
                    <h3>Opciones de menú</h3>
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
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label Width="16%" runat="server" class="col-sm-3 control-label">Area de Atención</asp:Label>
                                                <asp:Label Width="16%" runat="server" class="col-sm-3 control-label">Tipo de Usuario Autorizado</asp:Label>
                                                <asp:Label Width="16%" runat="server" class="col-sm-3 control-label">Tipo de Servicio</asp:Label>
                                            </div>

                                            <div class="form-group">
                                                <asp:DropDownList runat="server" Width="16%" ID="ddlArea" CssClass="DropSelect" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true" />
                                                <asp:DropDownList runat="server" Width="16%" CssClass="DropSelect" ID="ddlTipoUsuario" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                                <asp:DropDownList runat="server" Width="16%" CssClass="DropSelect" ID="ddlTipoArbol" OnSelectedIndexChanged="ddlTipoArbol_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            </div>
                                            <div class="form-group">

                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 1</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 2</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 3</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 4</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 5</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 6</asp:Label>
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">SubMenu/Opcion 7</asp:Label>
                                            </div>
                                            <div class="form-group">

                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel1" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel2" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel3" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel4" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel5" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel6" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel7" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
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
                                <asp:Repeater runat="server" ID="rptResultados">
                                    <HeaderTemplate>
                                        <table border="1" class="table table-bordered table-hover table-responsive" id="tblHeader" style="table-layout: fixed">
                                            <thead>
                                                <tr align="center">
                                                    <td><asp:Label runat="server">Producto</asp:Label></td>
                                                    <td><asp:Label runat="server">Tipo Usuario</asp:Label></td>
                                                    <td><asp:Label runat="server">Tipo Servicio</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 1</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 2</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 3</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 4</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 5</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 6</asp:Label></td>
                                                    <td><asp:Label runat="server">Nivel 7</asp:Label></td>
                                                    <td><asp:Label runat="server">Tipo Opcion</asp:Label></td>
                                                    <td><asp:Label runat="server">Habilitado</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Area.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("TipoUsuario.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("TipoArbolAcceso.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel1.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel2.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel3.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel4.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel5.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel6.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# Eval("Nivel7.Descripcion")%></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()"><%# (bool) Eval("EsTerminal") ? "Opcion" : "Menu"%></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="ContextMenu()" ondblclick="dbClic()" id="colHabilitado"><%# (bool) Eval("Habilitado") ? "SI" : "NO"%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--MODAL CATALOGOS--%>
<div class="modal fade" id="editOpcion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upOcion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<uc1:UcAltaArbolAcceso runat="server" ID="UcAltaArbolAcceso" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
