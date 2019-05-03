<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReporteInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.InformacionConsulta.UcReporteInformacionConsulta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>

<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/FrmReportes.aspx">Analíticos</asp:HyperLink>
                </li>
                <li class="active">Valoración artículos</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Valoración artículos" /></h3>
                            </div>
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Artículos:</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-5 col-sm-10 col-xs-12">
                                <div class="form-group">
                                    <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-1 col-sm-2 col-xs-12 pull-bottom">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-xs-11 col-sm-12 col-md-12 col-lg-12 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary" OnClick="btnBuscar_OnClick">Aplicar</asp:LinkButton>
                                    </div>
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
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25" AllowSorting="True" OnSorting="tblResults_OnSorting"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Titulo" HeaderStyle-Width="30%" ItemStyle-CssClass="altoFijo" SortExpression="Titulo">
                                            <ItemTemplate>
                                                <div>
                                                    <label runat="server" class="ocultaTexto" title='<%# Eval("Titulo")%>'><%# Eval("Titulo")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="4%" ItemStyle-CssClass="altoFijo" SortExpression="Gusta">
                                            <ItemTemplate>
                                                <div>
                                                    <label runat="server" class="ocultaTexto" title='<%# int.Parse(Eval("MeGusta").ToString()) + int.Parse(Eval("NoMeGusta").ToString()) %>'><%# int.Parse(Eval("MeGusta").ToString()) + int.Parse(Eval("NoMeGusta").ToString())%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Me gusta" HeaderStyle-Width="4%" ItemStyle-CssClass="altoFijo" SortExpression="Gusta">
                                            <ItemTemplate>
                                                <div>
                                                    <label runat="server" class="ocultaTexto" title='<%# Eval("MeGusta")%>'><%# Eval("MeGusta")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="No me gusta" HeaderStyle-Width="5%" ItemStyle-CssClass="altoFijo" SortExpression="NoGusta">
                                            <ItemTemplate>
                                                <div>
                                                    <label runat="server" class="ocultaTexto" title='<%# Eval("NoMeGusta")%>'><%# Eval("NoMeGusta")%></label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Autor" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Autor")%>'><%# Eval("Autor")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Creación" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Creacion")%>'><%# Eval("Creacion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Últ. Edición" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("UltEdicion")%>'><%# Eval("UltEdicion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Enabled="False" CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Graficar" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' ID="btnDetalle" OnClick="btnDetalle_OnClick" Visible='<%# (int)Eval("NoMeGusta") > 0 || (int)Eval("MeGusta") > 0 %>' Text="Ver"></asp:LinkButton>
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
    </asp:UpdatePanel>
</div>
