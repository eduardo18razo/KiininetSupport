<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaArboles.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaArboles" %>

<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcAltaAbrolAcceso.ascx" TagPrefix="uc1" TagName="UcAltaAbrolAcceso" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleArbolAcceso.ascx" TagPrefix="uc1" TagName="UcDetalleArbolAcceso" %>


<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Help Center</li>
                <li class="breadcrumb-item active">Configuración de Menú</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Configuración de Menú" /></h3>
                            </div>
                            <p>
                                Texto para Configuración de Menú
                            </p>
                        </div>


                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="form col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Opciones de Menú:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-4 col-sm-4 col-md-4 col-lg-4 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o Consulta por Categoría y Tipo de Usuario</label>
                                    <div class="col-xs-7 col-sm-7 col-md-7 col-lg-7  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control no-padding-left no-margin-left" Width="220px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" />
                                    </div>
                                    <div class="col-xs-5 col-sm-5 col-md-5 col-lg-5  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" Style="width: 120px;" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>


                            <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 text-center">
                                <div class="form-group margin-top-btn-consulta">

                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-success" ID="btnNew" OnClick="btnNew_OnClick" runat="server">
                                <i class="fa fa-plus"></i>Nuevo</asp:LinkButton>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </section>

            <div id="masonry" class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                <div class="module-wrapper masonry-item col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                    <section class="module module-headings">
                        <div class="module-inner">

                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">

                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <div class="altoFijo">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Título" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title=''><%# Eval("Tipificacion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Categoría" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title=''><%# Eval("Area.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tipificación" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title=''><%# Eval("TipoArbolAcceso.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title=''><%# (bool) Eval("EsTerminal") ? "OPCIÓN" : "SECCIÓN" %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto" title=''><%# Eval("Nivel") %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Detalle" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text="Ir" CommandArgument='<%# Eval("Id")%>' ID="lnkBtnDetalleOpciones" OnClick="lnkBtnDetalleOpciones_OnClick"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# !(bool) Eval("Sistema") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
            <script type="text/javascript">
                $(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });

            </script>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>

    </asp:UpdatePanel>

    <div class="modal fade" id="modalAtaOpcion" tabindex="-1" role="dialog" aria-labelledby="basicModal">
        <asp:UpdatePanel ID="upAltaArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <<div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc1:UcAltaAbrolAcceso runat="server" ID="UcAltaAbrolAcceso" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL DETALLE GRUPO OPCIONES--%>
    <div class="modal fade" id="modalDetalleOpciones" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" style="height: 250px;">
                    <div class="modal-content">
                        <uc1:UcDetalleArbolAcceso runat="server" ID="ucDetalleArbolAcceso" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
</div>
