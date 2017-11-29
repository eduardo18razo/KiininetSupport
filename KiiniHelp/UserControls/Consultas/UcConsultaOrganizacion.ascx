<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaOrganizacion.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaOrganizacion" %>
<%@ Register Src="~/UserControls/Altas/Organizaciones/UcAltaOrganizaciones.ascx" TagPrefix="uc1" TagName="UcAltaOrganizaciones" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfModalName" />
            <asp:HiddenField runat="server" ID="hfIdSeleccion" />

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Administración</li>
                <li class="breadcrumb-item active">Organizaciones</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblOrganización" Text="Organización" /></h3>
                            </div>
                            <p>
                                Las organizaciones representan la estructura organizacional a la que pertenece un usuario. Estas se construyen en forma de árbol y pueden contener desde uno hasta siete niveles de detalle. Puedes crear organizaciones para facilitar la administración de los usuarios y sus interacciones. 
                            </p>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-5 col-md-5">

                            <div class="form-group">
                                <div class="form-inline">
                                    <asp:Label runat="server" Text="Consulta Organizaciones:" />
                                </div>
                                <div class="search-box form-inline margin-bottom-lg">
                                    <label class="sr-only" for="txtFiltro">Buscar</label>
                                    <div class="form-group ">
                                        <asp:TextBox runat="server" ID="txtFiltroDecripcion" CssClass="form-control help_search_form col-lg-9 col-md-9" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." style="margin-right: 10px;" />
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon col-lg-2 col-md-2" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
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
                            <asp:LinkButton CssClass="btn btn-success" Style="margin-top: 10px;" ID="btnNew" OnClick="btnNew_OnClick" runat="server"><i class="fa fa-plus">
                                     </i>Crear Nueva Organización</asp:LinkButton>
                        </div>

                    </div>
                </div>
            </section>
            <!--/MÓDULO-->
            <%-- <asp:Label runat="server" Text="Organizaciones" ID="lblTitleOrganizacion" /></h3>--%>
            <div id="masonry" class="row">
                <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <%--<div class="module-heading">--%>
                            <%-- <ul class="actions list-inline">
                                    <li><a class="collapse-module" data-toggle="collapse" href="#content-1" aria-expanded="false" aria-controls="content-1">
                                        <span aria-hidden="true" class="icon arrow_carrot-up"></span></a></li>
                                </ul>--%>
                            <%-- </div>--%>
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">
                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" HeaderStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <%--<asp:Label runat="server" Text='<%# Eval("TipoUsuario.Abreviacion") %>' CssClass="btn btn-default-alt btn-square-usuario" BorderStyle=""/>--%>
                                                        <div style="min-height: 30px;" >
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 1" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Holding.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 2" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis; word-wrap: break-word;"><%# Eval("Compania.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 3" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Direccion.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 4" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("SubDireccion.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 5" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Gerencia.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 6" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("SubGerencia.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nivel 7" HeaderStyle-Width="13%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Jefatura.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# int.Parse(Eval("IdNivelOrganizacion").ToString()) != 1 %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                            <li>
                                                                <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" CommandArgument='<%# Eval("Id")%>' Visible='<%# (bool) Eval("Habilitado") %>' OnClick="btnEditar_OnClick" />
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
            <div id="contextMenuOrganizacion" class="panel-heading contextMenu">
                <asp:HiddenField runat="server" ClientIDMode="Inherit" ID="hfModal" />
            </div>
            <asp:Repeater ID="rptPager" runat="server">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                        CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
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
    <div class="modal fade" id="editCatalogoOrganizacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCatlogos" runat="server">
            <ContentTemplate>
                <uc1:UcAltaOrganizaciones runat="server" ID="ucAltaOrganizaciones" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
