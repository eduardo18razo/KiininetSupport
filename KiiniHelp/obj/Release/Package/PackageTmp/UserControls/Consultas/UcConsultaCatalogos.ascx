<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaCatalogos" %>
<%@ Register Src="~/UserControls/Altas/UcAltaCatalogo.ascx" TagPrefix="uc1" TagName="UcAltaCatalogo" %>
<%@ Register Src="~/UserControls/Altas/UcCargaCatalgo.ascx" TagPrefix="uc1" TagName="UcCargaCatalgo" %>


<div style="height: 100%;">
    <script>
        function dbClic(e) {
            debugger;
            $('#tblHeader').find('tr').dblclick(function (e) {
                alert(e.target.parentElement.id);
            });
        };

        function contextMenuCatalogos() {
            var $contextMenuCatalogos = $("#contextMenuCatalogos");
            $("body").on("click", function (e) {
                debugger;
                $contextMenuCatalogos.hide();
                var table = $("#tblHeader");
                table.find('tr').each(function (i, ev) {
                    $(this).css('background', "transparent");
                });
            });
            $("body").on("contextmenu", "table tr", function (e) {
                debugger;
                $contextMenuCatalogos.css({
                    display: "block",
                    left: e.pageX,
                    top: e.pageY
                });
                var baja = false;
                var alta = false;
                var parent = e.target.parentElement;
                var nodos = parent.parentElement.childNodes;
                var columnArchivo = parent.childNodes[3];
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
                if (columnArchivo.textContent === "True") {
                    document.getElementById("divEditar").style.visibility = 'visible';
                    document.getElementById("divEditar").style.display = 'block';
                }
                else {
                    document.getElementById("divEditar").style.visibility = 'hidden';
                    document.getElementById("divEditar").style.display = 'none';
                }
                document.getElementById("<%= this.FindControl("btnBaja").ClientID %>").style.display = baja ? 'block' : 'none';
                document.getElementById("<%= this.FindControl("btnAlta").ClientID %>").style.display = alta ? 'block' : 'none';
                var elementId = document.getElementById("<%= this.FindControl("hfId").ClientID %>");
                var elementEsArchivo = document.getElementById("<%= this.FindControl("hfEsArchivo").ClientID %>");
                elementId.value = e.target.parentElement.id;
                elementEsArchivo.value = columnArchivo.textContent;
                return false;
            });

            $contextMenuCatalogos.on("click", "button", function () {
                debugger;
                $contextMenuCatalogos.hide();
            });
        };
    </script>
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <div id="contextMenuCatalogos" class="panel-heading contextMenu">
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfId" />
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfEsArchivo" />
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Baja" ID="btnBaja" OnClick="btnBaja_OnClick" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Alta" ID="btnAlta" OnClick="btnAlta_OnClick" />
                </div>
                <div class="form-group" id="divEditar">
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
                    <h3>Catálogos </h3>
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
                                        <div class="form-group">
                                            <asp:Label class="col-xs-1 control-label" runat="server">Catálogo</asp:Label>
                                            <div class="col-xs-2">
                                                <asp:DropDownList runat="server" ID="ddlCatalogos" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCatalogos_OnSelectedIndexChanged" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-xs-1 control-label"></div>
                                            <div class="col-xs-1">
                                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnNew" Text="Agregar Catálogo" OnClick="btnNew_OnClick" />
                                            </div>
                                            <div style="width: 45px"></div>
                                            <div class="col-xs-1">
                                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnCargarCatalogo" Text="Cargar Catálogo" OnClick="btnCargarCatalogo_OnClick" />
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
                                        <table border="1" class="table table-bordered table-hover table-responsive" id="tblHeader">
                                            <thead>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label runat="server">Descripción</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Habilitado</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="contextMenuCatalogos()"><%# Eval("Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px; display: none" oncontextmenu="contextMenuCatalogos()" id="colArchivo"><%# Eval("Archivo")%></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="contextMenuCatalogos()" id="colHabilitado"><%# (bool) Eval("Habilitado") ? "SI" : "NO"%></td>
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
    <%--MODAL ALTA--%>
    <div class="modal fade" id="modalAltaCatalogo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaCatalogo" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcAltaCatalogo runat="server" id="ucAltaCatalogo" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--MODAL CARGAR--%>
    <div class="modal fade" id="modalCargaCatalogo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCargaCatalogo" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcCargaCatalgo runat="server" id="ucCargaCatalgo" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
