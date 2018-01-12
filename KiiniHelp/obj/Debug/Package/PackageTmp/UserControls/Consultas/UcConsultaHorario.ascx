<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaHorario.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaHorario" %>
<%@ Register Src="~/UserControls/Altas/UcAltaHorario.ascx" TagPrefix="uc1" TagName="UcAltaHorario" %>


<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfId" />

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Atención</li>
                <li class="breadcrumb-item active">Horarios</li>
            </ol>


            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Horarios" Font-Bold="true" /></h3>
                            </div>
                            <p>
                                Los horarios son las horas laborales específicas para cada día de la semana, se asocian a un usuario o un grupo y se crean reglas de negocio en torno a éstos. 
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner ">
                        <div class="col-lg-6 col-md-6 separador-vertical-derecho">
                            Consulta Horarios:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" CssClass="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-6 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-download" Text="  Descargar reporte" ID="btnDownload" OnClick="btnDownload_OnClick" />
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Nuevo" OnClick="btnNew_OnClick" />
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
                                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <div style="min-height: 30px;">
                                                    <label><%# Eval("Descripcion")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Creación" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label><%# Eval("FechaAlta")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Últ. Edición" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <label><%# Eval("FechaModificacion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Visible='<%# !(bool) Eval("Sistema") %>' Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                        <asp:LinkButton runat="server" Visible='<%# !(bool) Eval("Sistema") %>' CommandArgument='<%# Eval("Id")%>'  OnClick="btnEditar_OnClick"><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>

                                                       <%-- <asp:ImageButton runat="server" Visible='<%# !(bool) Eval("Sistema") %>' CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />--%>
                                                       <%-- <asp:Label runat="server" Text="|" Visible='<%# !(bool) Eval("Sistema") %>' />
                                                        <asp:LinkButton runat="server" Text="Clonar" CommandArgument='<%# Eval("Id")%>' OnClick="btnClonar_OnClick"></asp:LinkButton>--%>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Clonar" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                      <%--  <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" Visible='<%# !(bool) Eval("Sistema") %>' CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                                        <asp:Label runat="server" Text="|" Visible='<%# !(bool) Eval("Sistema") %>' />--%>
                                                        <asp:LinkButton runat="server" Text="Clonar" CommandArgument='<%# Eval("Id")%>' OnClick="btnClonar_OnClick"></asp:LinkButton>
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
                                                        <asp:Label runat="server" Width="25%">Nombre</asp:Label></th>
                                                    <th>
                                                        <asp:Label runat="server" Width="15%">Creación</asp:Label></th>
                                                    <th>
                                                        <asp:Label runat="server" Width="20%" Style="display: inline;">Últ. Edición</asp:Label></th>
                                                    <th>
                                                        <asp:Label runat="server" Width="15%">Activo</asp:Label></th>
                                                    <th>
                                                        <asp:Label runat="server" Width="25%" Text="Editar | Clonar" Style="display: inline;"></asp:Label></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="height: 46px;"><%# Eval("Descripcion")%></td>
                                            <td><%# Eval("FechaAlta")%></td>
                                            <td><%# Eval("FechaModificacion")%></td>
                                            <td id="colHabilitado">
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Visible='<%# !(bool) Eval("Sistema") %>' Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </td>
                                            <td id="colEditar">
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                        <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" Visible='<%# !(bool) Eval("Sistema") %>' CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                                        <asp:Label runat="server" Text="|" Visible='<%# !(bool) Eval("Sistema") %>' />
                                                        <asp:LinkButton runat="server" Text="Clonar" CommandArgument='<%# Eval("Id")%>' OnClick="btnClonar_OnClick"></asp:LinkButton>
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
    <div class="modal fade" id="modalAltaHorario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upAltaHorario" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaHorario runat="server" ID="ucAltaHorario" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
