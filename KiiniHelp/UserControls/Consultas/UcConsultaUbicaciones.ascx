<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaUbicaciones.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaUbicaciones" %>
<%@ Register Src="~/UserControls/Altas/Ubicaciones/UcAltaUbicaciones.ascx" TagPrefix="uc1" TagName="UcAltaUbicaciones" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%" ID="upGeneral">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfModalName" />
            <asp:HiddenField runat="server" ID="hfIdSeleccion" />
            <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfModal" />
            <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfId" />

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Administración</li>
                <li class="breadcrumb-item active">Ubicaciones</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Ubicación" /></h3>
                            </div>
                            <p>
                                Las ubicaciones representan el lugar físico donde comúnmente se localiza un usuario. Estas se construyen en forma de árbol, se incluye la dirección y pueden contener desde uno hasta siete niveles de detalle. Puedes crear ubicaciones para facilitar la localización y administración de los usuarios y sus eventos. 
                            </p>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="form col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Ubicaciones:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltroDecripcion" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" ID="btnBuscar" CssClass="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o Consulta por Tipo de Usuario</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-4 col-sm-4 col-md-4 col-lg-4 text-center">
                                <div class="form-group margin-top-btn-consulta">
                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-success" ID="btnNuevo" OnClick="btnNew_OnClick" runat="server">
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
                                                <asp:TemplateField HeaderText="Nivel 1" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Pais.Descripcion")%>'><%# Eval("Pais.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 2" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Campus.Descripcion")%>'><%# Eval("Campus.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 3" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Torre.Descripcion")%>'><%# Eval("Torre.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 4" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Piso.Descripcion")%>'><%# Eval("Piso.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 5" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("Zona.Descripcion")%>'><%# Eval("Zona.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 6" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("SubZona.Descripcion")%>'><%# Eval("SubZona.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 7" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label runat="server" class="ocultaTexto " title='<%# Eval("SiteRack.Descripcion")%>'><%# Eval("SiteRack.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) != 1 %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                            <li>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" Visible='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) != 1 && (bool) Eval("Habilitado") %>'><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
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

    <%--MODAL CATALOGOS--%>
    <div class="modal fade" id="editCatalogoUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCatlogos" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaUbicaciones runat="server" ID="ucAltaUbicaciones" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
