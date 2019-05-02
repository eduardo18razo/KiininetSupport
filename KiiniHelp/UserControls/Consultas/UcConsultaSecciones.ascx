<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaSecciones.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaSecciones" %>
<%@ Register Src="~/UserControls/Altas/Organizaciones/UcAltaOrganizaciones.ascx" TagPrefix="uc1" TagName="UcAltaOrganizaciones" %>
<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcAltaSeccion.ascx" TagPrefix="uc1" TagName="UcAltaSeccion" %>


<div style="height: 100%;">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Centro de Soporte</li>
                <li class=" active">Secciones</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Menú de Navegación" /></h3>
                            </div>
                            <p>
                                Texto:
                                <br />
                                Categoría > Nivel 1 > Nivel 2 > Nivel 3 > Nivel 4 > Nivel 5 > Nivel 6 > Nivel 7

                            </p>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="col-lg-5">
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

                            <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4 separador-vertical-derecho">
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


                            <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3 text-center">
                                <div class="form-group margin-top-btn-consulta">

                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-success" ID="btnNew" OnClick="btnNew_OnClick" runat="server" Visible="False">
                                <i class="fa fa-plus"></i>Nuevo</asp:LinkButton>

                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </section>


            <section class="module">
                <div class="module-inner">

                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nivel 1" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel1.Descripcion")%>'><%# Eval("Nivel1.Descripcion")%> </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 2" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel2.Descripcion")%>'><%# Eval("Nivel2.Descripcion")%> </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 3" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto " title='<%# Eval("Nivel3.Descripcion")%>'><%# Eval("Nivel3.Descripcion")%> </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 4" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel4.Descripcion")%>'><%# Eval("Nivel4.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 5" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel5.Descripcion")%>'><%# Eval("Nivel5.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 6" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel6.Descripcion")%>'><%# Eval("Nivel6.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nivel 7" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <label class="ocultaTexto" title='<%# Eval("Nivel7.Descripcion")%>'><%# Eval("Nivel7.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                    <li>
                                                        <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" CommandArgument='<%# Eval("Id")%>' Visible='<%# (bool) Eval("Habilitado") %>' OnClick="btnEditar_OnClick" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="4%">
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
    <%--MODAL CATALOGOS--%>
    <div class="modal fade" id="editarSeccion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCatlogos" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaSeccion runat="server" id="ucAltaSeccion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>