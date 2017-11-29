<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaUsuarios.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaUsuarios" %>
<%@ Register TagPrefix="uc1" TagName="UcDetalleUsuario" Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" %>
<%@ Register Src="~/UserControls/Altas/UcAltaUsuario.ascx" TagPrefix="uc1" TagName="UcAltaUsuario" %>

<div style="height: 100%;">
    <script>
        function ContextMenu() {
            debugger;
            var $contextMenu = $("#contextMenuUsuario");
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
                debugger;
                var baja = false;
                var alta;
                var parent = e.target.nodeName === "A" ? e.target.parentElement.parentElement : e.target.parentElement;
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
                elementId.value = e.target.nodeName === "A" ? e.target.parentElement.parentElement.id : e.target.parentElement.id;
                return false;
            });

            $contextMenu.on("click", "button", function () {
                $contextMenu.hide();
            });
        };
    </script>
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <div id="contextMenuUsuario" class="panel-heading contextMenu">
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
                    <h3>Usuarios</h3>
                </div>
                <div class="panel-body">
                    <div class="panel panel-primary">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingFiltros">
                                <h4 class="panel-title">
                                    <div role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFiltros" aria-expanded="true" aria-controls="collapseFiltros" style="cursor: pointer">
                                        Filtros
                                    </div>
                                </h4>
                            </div>
                            <div id="collapseFiltros" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingFiltros">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label Width="14%" runat="server" class="col-sm-3 control-label">Tipo de Usuario</asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlTipoUsuario" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                            </div>
                                            <div class="form-group">
                                                <asp:Button runat="server" CssClass="col-xs-1 btn btn-primary" ID="btnNew" Text="Agregar Usuario" Width="14%" OnClick="btnNew_OnClick" Visible="False" />
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
                                                    <td>
                                                        <asp:Label runat="server">Nombre Completo</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Tipo Usuario</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Activo</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <td oncontextmenu="ContextMenu()" style="padding: 0; text-align: left; font-size: 10px;" >
                                                <asp:LinkButton Style="padding: 0;" runat="server" Text='<%#Eval("NombreCompleto") %>' ID="LinkButton1" OnClick="btnUsuario_OnClick" CommandArgument='<%#Eval("Id") %>' /></td>
                                            <td oncontextmenu="ContextMenu()" style="padding: 0; text-align: left; font-size: 10px;" ><%# Eval("TipoUsuario.Descripcion")%></td>
                                            <td oncontextmenu="ContextMenu()" style="padding: 0; font-size: 10px;" id="colHabilitado"><%# (bool) Eval("Habilitado") ? "SI" : "NO"%></td>
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
    <%--MODAL DETALLE--%>
<div class="modal fade" id="modalDetalleUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario1" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>


<%--MODAL CATALOGOS--%>
<div class="modal fade" id="editUser" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upUser" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" style="width: 1250px; height: 940px; overflow: hidden" >
                <div class="modal-content">
                    <uc1:UcAltaUsuario runat="server" ID="UcAltaUsuario" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>





