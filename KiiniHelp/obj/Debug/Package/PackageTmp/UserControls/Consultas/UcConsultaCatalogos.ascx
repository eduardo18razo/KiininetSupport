﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaCatalogos" %>
<%@ Register Src="~/UserControls/Altas/UcCargaCatalgo.ascx" TagPrefix="uc1" TagName="UcCargaCatalgo" %>
<%@ Register Src="~/UserControls/Altas/Catalogo/UcAltaCatalogos.ascx" TagPrefix="uc1" TagName="UcAltaCatalogos" %>



<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Help Center</li>
                <li class="breadcrumb-item">Catálogos</li>
                <li class="breadcrumb-item active">Consulta</li>
            </ol>

            <br>

            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Catálogos" /></h3>
                            </div>
                            <p>
                                Texto para Catálogos
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-6 col-md-6 separador-vertical-derecho">
                            Consulta Catálogos:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-6">
                            <div class="module-inner text-center">
                                <div class="form-group">
                                    <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-download" Text="  Descargar reporte" ID="btnDownload" OnClick="btnDownload_OnClick" />
                                    <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Nuevo" OnClick="btnNew_OnClick" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </section>

            <section class="module module-headings">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false"
                                    CssClass="table table-striped display" Width="99%"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                    BorderStyle="None" PagerSettings-Mode="Numeric"
                                    PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="40%">
                                            <ItemTemplate>
                                                <div style="min-height: 30px;">
                                                    <label><%# Eval("Descripcion")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sistema" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <label><%# (bool)Eval("Sistema") ? "Si" : "No"%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Creación" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label><%# Eval("FechaAlta", "{0:d}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Últ. Edición" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label><%# Eval("FechaModificacion", "{0:d}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" Visible='<%# !(bool)Eval("Sistema") %>' data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                        <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" Visible='<%# !(bool)Eval("Sistema") %>' CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
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
    <%--MODAL ALTA--%>
    <div class="modal fade" id="modalAltaCatalogo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaCatalogo" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaCatalogos runat="server" ID="ucAltaCatalogos" />
                    </div>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--MODAL CARGAR--%>
    <div class="modal fade" id="modalCargaCatalogo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCargaCatalogo" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcCargaCatalgo runat="server" ID="ucCargaCatalgo" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>