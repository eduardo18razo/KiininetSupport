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
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Ubicación" /></h3>
                            </div>
                            <p>
                                Las ubicaciones representan el lugar físico donde comúnmente se localiza un usuario. Estas se construyen en forma de árbol, se incluye la dirección y pueden contener desde uno hasta siete niveles de detalle. Puedes crear ubicaciones para facilitar la localización y administración de los usuarios y sus eventos. 
                           
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-5 col-md-5">
                            <div class="form-group">
                                <div class="form-inline">
                                    <asp:Label runat="server" Text="Consulta Ubicaciones:" />
                                </div>
                                <div class="search-box form-inline margin-bottom-lg">
                                    <label class="sr-only" for="txtFiltroDecripcion">Buscar</label>
                                    <div class="form-group">
                                        <asp:TextBox runat="server" ID="txtFiltroDecripcion" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                        <asp:LinkButton runat="server" ID="btnBuscar" CssClass="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-md-3">
                            <div class="form-group separador-vertical-derecho">
                                <div class="form-inline">
                                    <asp:Label runat="server" Text="... o Consulta por Tipo de Usuario" />
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" Width="190px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 text-center">
                            <asp:LinkButton CssClass="btn btn-success" Style="margin-top: 10px;" ID="btnNuevo" OnClick="btnNew_OnClick" runat="server">
                                <i class="fa fa-plus"></i>Nuevo</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </section>
            <br>
            <div id="masonry" class="row">
                <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <%--<div class="module-heading">
                                <ul class="actions list-inline">
                                    <li><a class="collapse-module" data-toggle="collapse" href="#content-1" aria-expanded="false" aria-controls="content-1"><span aria-hidden="true" class="icon arrow_carrot-up"></span></a></li>
                                </ul>
                            </div>--%>
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">

                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" ControlStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <div style="min-height: 30px;">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 1" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Pais.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 2" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Campus.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 3" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Torre.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 4" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Piso.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 5" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Zona.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 6" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("SubZona.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 7" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("SiteRack.Descripcion")%></label>
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
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" Visible='<%# int.Parse(Eval("IdNivelUbicacion").ToString()) != 1 && (bool) Eval("Habilitado") %>' ><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
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
    </asp:UpdatePanel>

    <%--MODAL CATALOGOS--%>
    <div class="modal fade" id="editCatalogoUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCatlogos" runat="server">
            <ContentTemplate>
                <uc1:UcAltaUbicaciones runat="server" ID="ucAltaUbicaciones" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
