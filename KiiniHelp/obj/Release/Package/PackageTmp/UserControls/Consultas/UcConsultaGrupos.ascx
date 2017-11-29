<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaGrupos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaGrupos" %>
<%@ Register TagPrefix="uc" TagName="altagrupousuario" Src="~/UserControls/Altas/AltaGrupoUsuario.ascx" %>
<div style="height: 100%;">
    <script>
        function dbClic(e) {
            //debugger;
            //$('#tblHeader').find('tr').dblclick(function (e) {
            //    alert(e.target.parentElement.id);
            //});
        };

        function contextMenuGrupo() {
            var $contextMenuGrupo = $("#contextMenuGrupo");
            $("body").on("click", function (e) {
                debugger;
                $contextMenuGrupo.hide();
                var table = $("#tblHeader");
                table.find('tr').each(function (i, ev) {
                    $(this).css('background', "transparent");
                });
            });
            $("body").on("contextmenu", "table tr", function (e) {
                debugger;
                $contextMenuGrupo.css({
                    display: "block",
                    left: e.pageX,
                    top: e.pageY
                });
                var baja = false;
                var alta = false;
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
                document.getElementById("<%= this.FindControl("btnBaja").ClientID %>").style.display = baja ? 'block' : 'none';
                document.getElementById("<%= this.FindControl("btnAlta").ClientID %>").style.display = alta ? 'block' : 'none';
                var elementId = document.getElementById("<%= this.FindControl("hfId").ClientID %>");
                elementId.value = e.target.parentElement.id;
                return false;
            });

            $contextMenuGrupo.on("click", "button", function () {
                debugger;
                $contextMenuGrupo.hide();
            });
        };
    </script>
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <div id="contextMenuGrupo" class="panel-heading contextMenu">
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
                    <h3>Grupos</h3>
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
                                            <asp:Label Width="14%" for="ddlTipoUsuario" class="col-xs-1 control-label" runat="server">Tipo de Usuario</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label Width="14%" for="ddlTipoGrupo" class="col-xs-1 control-label" runat="server">Rol</asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlTipoGrupo" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoGrupo_OnSelectedIndexChanged" />
                                            </div>
                                            <div class="form-group">
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
                                        <table border="1" class="table table-bordered table-hover table-responsive" id="tblHeader">
                                            <thead>
                                                <tr align="center">
                                                    <td><asp:Label runat="server">Tipo Usuario</asp:Label></td>
                                                    <td><asp:Label runat="server">Rol</asp:Label></td>
                                                    <td><asp:Label runat="server">Grupo</asp:Label></td>
                                                    <td><asp:Label runat="server">Supervisor</asp:Label></td>
                                                    <td><asp:Label runat="server">Habilitado</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="contextMenuGrupo()" ondblclick="dbClic()" contextmenu="contextMenu"><%# Eval("TipoUsuario.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="contextMenuGrupo()" ondblclick="dbClic()"><%# Eval("TipoGrupo.Descripcion")%></td>
                                            <td style="padding: 0; text-align: left; font-size: 10px;" oncontextmenu="contextMenuGrupo()" ondblclick="dbClic()"><%# Eval("Descripcion")%></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="contextMenuGrupo()" ondblclick="dbClic()"><%# (bool) Eval("TieneSupervisor") ? "SI" : "NO" %></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="contextMenuGrupo()" ondblclick="dbClic()" id="colHabilitado"><%# (bool) Eval("Habilitado") ? "SI" : "NO"%></td>
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
    <%--MODAL GRUPO USUARIO--%>
    <div class="modal fade" id="modalAltaGrupoUsuarios" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaGrupo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 780px; width: 650px">
                    <div class="modal-content">
                        <uc:altagrupousuario runat="server" ID="ucAltaGrupoUsuario" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL SELECCION DE ROL--%>
    <div class="modal fade" id="modalSeleccionRol" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upSubRoles" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-md">
                    <div class="modal-content">
                        <div class="modal-header" id="panelAlertaSeleccionRol" runat="server" visible="false">
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
                                <asp:Repeater runat="server" ID="rptErrorSeleccionRol">
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
                                <asp:Label runat="server" ID="lblTitleSubRoles"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:HiddenField runat="server" ID="hfOperacion" />
                                <div>
                                    <div class="form-group">
                                        <div class="form-group">
                                            <asp:CheckBoxList runat="server" ID="chklbxSubRoles" Checked="True" Visible="True" OnSelectedIndexChanged="chklbxSubRoles_OnSelectedIndexChanged" AutoPostBack="True" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>


