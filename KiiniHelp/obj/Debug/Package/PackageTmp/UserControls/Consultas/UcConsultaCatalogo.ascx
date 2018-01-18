<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaCatalogo" %>
<%@ Register Src="~/UserControls/Operacion/UcRegistroCatalogo.ascx" TagPrefix="uc1" TagName="UcRegistroCatalogo" %>

<div style="height: 100%;">

    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Help Center</li>
                <li class="breadcrumb-item">Catálogos</li>
                <li class="breadcrumb-item active">Registro</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="module-inner">
                                <div class="module-heading">
                                    <h3 class="module-title">
                                        <asp:Label runat="server" ID="lblSeccion" Text="Registro de Catálogos" /></h3>
                                </div>
                                <p>
                                    Texto para Registro de Catálogos
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-6 col-md-6 separador-vertical-derecho">
                            Consulta Registro de Catálogos:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Catálogo</label>
                                <div class="form-group ">
                                    <asp:DropDownList runat="server" ID="ddlCatalogos" Width="200px" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCatalogos_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-6 text-center">
                            <div class="module-inner">
                                <div class="form-group">
                                    <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-download" Text="  Descargar reporte" ID="btnDownload" OnClick="btnDownload_OnClick" />
                                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnNew" Text="Agregar Registro" OnClick="btnNew_OnClick" />
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
                                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <div style="min-height: 30px;">
                                                    <label><%# Eval("Descripcion")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                    <li>
                                                        <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" > 
                                                            <asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> 
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

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
    </asp:UpdatePanel>
    <%--MODAL ALTA--%>
    <div class="modal fade" id="modalAltaRegistro" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaAltaRegistro" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcRegistroCatalogo runat="server" ID="ucRegistroCatalogo" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
