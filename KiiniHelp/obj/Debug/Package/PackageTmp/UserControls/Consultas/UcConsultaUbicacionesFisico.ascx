<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaUbicacionesFisico.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaUbicacionesFisico" %>
<div style="height: 100%;">
    <script>
        function dbClicUbicacion(e) {
            //__doPostBack('SeleccionarOrganizacion', e.parentElement.id);
            var idSeleccion = document.getElementById("<%= this.FindControl("hfIdSeleccion").ClientID %>");
            idSeleccion.value = e.parentElement.id;
            var modalName = document.getElementById("<%= this.FindControl("hfModalName").ClientID %>");
            $(modalName.value).modal('hide');
            document.getElementById("<%= this.FindControl("btnCerrar").ClientID %>").click();
        };

        function contextMenuUbicaciones() {

            var $contextMenuUbicaciones = $("#contextMenuUbicaciones");
            $("body").on("click", function (e) {
                $contextMenuUbicaciones.hide();
                var table = $("#tblHeader");
                table.find('tr').each(function (i, ev) {
                    $(this).css('background', "transparent");
                });
            });
            $("body").on("contextmenu", "table tr", function (e) {
                if (document.getElementById("<%= this.FindControl("hfModal").ClientID %>").value.toLowerCase() === "true") {
                    if (e.pageX > 239)
                        positionx = e.pageX - 239;
                    else
                        positionx = 239 - e.pageX;
                    if (e.pageY > 31)
                        positiony = e.pageY - 31;
                    else
                        positiony = 31 - e.pageY;
                } else {
                    positionx = e.pageX;
                    positiony = e.pageY;
                }
                $contextMenuUbicaciones.css({
                    display: "block",
                    left: positionx,
                    top: positiony
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

            $contextMenuUbicaciones.on("click", "button", function () {
                $contextMenuUbicaciones.hide();
            });
        };
    </script>
    <asp:UpdatePanel runat="server" style="height: 100%" ID="upGeneral">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfModalName" />
            <asp:HiddenField runat="server" ID="hfIdSeleccion" />
            <div id="contextMenuUbicaciones" class="panel-heading contextMenu">
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfModal" />
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfId" />
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Baja" ID="btnBaja" OnClick="btnBaja_OnClick" ClientIDMode="Inherit" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Alta" ID="btnAlta" OnClick="btnAlta_OnClick" ClientIDMode="Static" />

                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Editar" ID="btnEditar" OnClick="btnEditar_OnClick" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" Visible="False" />
                </div>
            </div>
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
                    <asp:Label runat="server" ID="lbotest"></asp:Label>
                    <h3>
                        <asp:Label runat="server" Text="Ubicaciones" ID="lblTitleUbicacion" /></h3>
                </div>
                <div class="panel-body">
                    <div class="panel panel-primary">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingFiltros">
                                <h4 class="panel-title">
                                    <div role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFiltros" aria-expanded="true" aria-controls="collapseFiltros" style="cursor: pointer">
                                        Mostrar/Ocultar Filtros
                                    </div>
                                </h4>
                            </div>
                            <div id="collapseFiltros" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingFiltros">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label Width="14%" for="ddlTipoUsuario" class="col-xs-1 control-label" runat="server">Tipo de Usuario</asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="14%" CssClass="col-xs-1 DropSelect" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                            <asp:Label Width="14%" runat="server" Text="Filtrar por" class="col-xs-1 control-label"></asp:Label>
                                            <asp:TextBox Width="14%" runat="server" ID="txtFiltroDecripcion" CssClass=" col-xs-1 form-control" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);"></asp:TextBox>
                                            <asp:Button runat="server" Text="Buscar" ID="btnBuscar" CssClass="col-xs-1 btn btn-sm btn-primary" OnClick="btnBuscar_OnClick"></asp:Button>
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label Width="14%" ID="lblNivel1" class="col-xs-1 control-label" runat="server">Pais</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel2" class="col-xs-1 control-label" runat="server">Campus</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel3" class="col-xs-1 control-label" runat="server">Torre</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel4" class="col-xs-1 control-label" runat="server">Piso</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel5" class="col-xs-1 control-label" runat="server">Zona</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel6" class="col-xs-1 control-label" runat="server">Sub Zona</asp:Label>
                                                <asp:Label Width="14%" ID="lblNivel7" class="col-xs-1 control-label" runat="server">Site Rack</asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlpais" CssClass="DropSelect" OnSelectedIndexChanged="ddlpais_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlCampus" CssClass="DropSelect" OnSelectedIndexChanged="ddlCampus_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlTorre" CssClass="DropSelect" OnSelectedIndexChanged="ddlTorre_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlPiso" CssClass="DropSelect" OnSelectedIndexChanged="ddlPiso_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlZona" CssClass="DropSelect" OnSelectedIndexChanged="ddlZona_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlSubZona" CssClass="DropSelect" OnSelectedIndexChanged="ddlSubZona_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                <asp:DropDownList runat="server" Width="14%" ID="ddlSiteRack" CssClass="DropSelect" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSiteRack_OnSelectedIndexChanged" AutoPostBack="True" />
                                            </div>
                                            <div class="form-group">
                                                <asp:Button runat="server" CssClass="col-xs-1 btn btn-primary" ID="btnNew" Text="Agregar Campus" Width="14%" OnClick="btnNew_OnClick" Visible="False" />
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
                                                    <td>
                                                        <asp:Label runat="server" ID="Label1">Tipo Usuario</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel1">Nivel 1</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel2">Nivel 2</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel3">Nivel 3</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel4">Nivel 4</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel5">Nivel 5</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel6">Nivel 6</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNivel7">Nivel 7</asp:Label></td>
                                                    <td>
                                                        <asp:Label runat="server">Habilitado</asp:Label></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr align="center" id='<%# Eval("Id")%>'>
                                            <td style='font-weight: normal; padding: 0; text-align: left; font-size: 10px;'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("TipoUsuario.Descripcion")%></td>
                                            <td style='<%# Eval("Campus") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("Pais.Descripcion")%></td>
                                            <td style='<%# Eval("Torre") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("Campus.Descripcion")%></td>
                                            <td style='<%# Eval("Piso") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("Torre.Descripcion")%></td>
                                            <td style='<%# Eval("Zona") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("Piso.Descripcion")%></td>
                                            <td style='<%# Eval("SubZona") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("Zona.Descripcion")%></td>
                                            <td style='<%# Eval("SiteRack") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("SubZona.Descripcion")%></td>
                                            <td style='<%# Eval("SiteRack") == null ? "font-weight: bold; padding: 0; text-align: left; font-size: 10px;": "font-weight: normal; padding: 0; text-align: left; font-size: 10px;" %>'
                                                oncontextmenu="contextMenuUbicaciones()" ondblclick='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) == 1 ? "" : "dbClicUbicacion(this)" %>'><%# Eval("SiteRack.Descripcion")%></td>
                                            <td style="padding: 0; font-size: 10px;" oncontextmenu="contextMenuUbicaciones()" ondblclick="dbClicUbicacion(this)" id="colHabilitado"><%# (bool) Eval("Habilitado") ? "SI" : "NO"%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:Button runat="server" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" Style="visibility: hidden" />
                            </div>
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
                                            <asp:TextBox runat="server" ID="txtDescripcionCatalogo" placeholder="DESCRIPCION" class="form-control" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" autofocus="autofocus" />
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
                                Datos Generales
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <div class="form-group" runat="server" visible="False">
                                        <label class="col-sm-3 control-label">Tipo de Usuario</label>
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuarioCampus" CssClass="DropSelect" Enabled="False" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Nombre</label>
                                        <asp:TextBox runat="server" ID="txtDescripcionCampus" placeholder="Nombre" class="form-control" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox runat="server" ID="CheckBox1" Checked="True" Visible="False" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Código Postal</label>
                                        <asp:TextBox runat="server" ID="txtCp" placeholder="CODIGO POSTAL" AutoPostBack="True" OnTextChanged="txtCp_OnTextChanged" class="form-control" onkeypress="return ValidaCampo(this,2)" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Colonia</label>
                                        <asp:DropDownList runat="server" ID="ddlColonia" CssClass="DropSelect" OnSelectedIndexChanged="ddlColonia_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">Municipio</label>
                                            <asp:Label runat="server" class="col-sm-4 control-label" ID="lblMunicipio" />
                                            <label class="col-sm-2 control-label">Estado</label>
                                            <asp:Label runat="server" class="col-sm-4 control-label" ID="lblEstado" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Calle</label>
                                        <asp:TextBox runat="server" ID="txtCalle" placeholder="CALLE" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">Número Exterior</label>
                                        <asp:TextBox runat="server" ID="txtNoExt" placeholder="NUMERO EXTERIOR" class="form-control" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label">Número Interior</label>
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
</div>
