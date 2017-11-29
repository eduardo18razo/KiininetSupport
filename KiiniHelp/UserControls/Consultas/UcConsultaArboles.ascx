<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaArboles.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaArboles" %>

<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcAltaAbrolAcceso.ascx" TagPrefix="uc1" TagName="UcAltaAbrolAcceso" %>

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
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Configuración de Menú" /></h3>
                            </div>
                            <p>
                                Texto para Configuración de Menú
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner separador-vertical-derecho">
                        <div class="col-lg-5 col-md-5">
                            Consulta Opciones de Menú:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"> <i class="fa fa-search"></i> </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 separador-vertical-derecho">
                            ... o Consulta por Categoría y Tipo de Usuario<br />
                            <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control" Style="width: 200px; display: inline-block" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true" autocompletemode="SuggestAppend" casesensitive="false" />
                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" Style="width: 120px; display: inline-block; margin-left: 20px" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                        </div>

                        <div class="col-lg-3 col-md-3 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Crear Nueva Opción" OnClick="btnNew_OnClick" />
                            </div>
                        </div>
                    </div>
            </section>

            <div id="masonry" class="row">
                <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-heading">
                                <ul class="actions list-inline">
                                    <li><a class="collapse-module" data-toggle="collapse" href="#content-1" aria-expanded="false" aria-controls="content-1"><span aria-hidden="true" class="icon arrow_carrot-up"></span></a></li>
                                </ul>
                            </div>
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">

                                         <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" 
                                            CssClass="table table-striped display" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" 
                                            PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" ControlStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <div style="min-height: 30px;">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Titulo" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Tipificacion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Categoría" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Area.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tipificación" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("TipoArbolAcceso.Descripcion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <label><%# (bool) Eval("EsTerminal") ? "OPCIÓN" : "SECCIÓN" %></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Nivel") %></label>
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




                                       <%-- <asp:Repeater runat="server" ID="rptResultados">
                                            <HeaderTemplate>
                                                <table class="table table-striped display" id="tblResults">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                <asp:Label runat="server">Tipo Usuario</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Titulo</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Categoría</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Tipificación</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Tipo</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Nivel</asp:Label></th>
                                                            <th>
                                                                <asp:Label runat="server">Activo</asp:Label></th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr id='<%# Eval("Id")%>'>
                                                    <td>
                                                        <div style="min-height: 30px;">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </td>
                                                    <td><%# Eval("Tipificacion")%></td>
                                                    <td><%# Eval("Area.Descripcion")%></td>
                                                    <td><%# Eval("TipoArbolAcceso.Descripcion")%></td>
                                                    <td><%# (bool)Eval("EsTerminal") ? "OPCIÓN" : "SECCIÓN"%></td>
                                                    <td><%# Eval("Nivel")%></td>
                                                    <td id="colHabilitado">
                                                        <ul class="list list-unstyled " id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# !(bool) Eval("Sistema") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                            </li>
                                                        </ul>
                                                    </td>
                                                    <td id="colEditar">
                                                        <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                            <li>
                                                            </li>
                                                        </ul>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                            </table>
                                            </FooterTemplate>
                                        </asp:Repeater>--%>
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
    <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
</div>
