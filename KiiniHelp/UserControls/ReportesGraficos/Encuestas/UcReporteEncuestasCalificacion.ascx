<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReporteEncuestasCalificacion.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.Encuestas.UcReporteEncuestasCalificacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/FrmReportes.aspx">Analíticos</asp:HyperLink>
                </li>
                <li class="active">Escala 0-10</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Escala 0-10" /></h3>
                            </div>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="col-lg-4 col-md-3 col-sm-5 col-xs-12">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left no-margin-left">Opción donde está la encuesta:</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-1 col-md-2 col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Categoría:</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control no-padding-left no-margin-left" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Tipo de Usuario:</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-4 col-sm-10 col-xs-12">
                                <div class="form-group">
                                    <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-1 col-sm-2 col-xs-12 pull-bottom">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
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
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                    <Columns>
                                        <asp:TemplateField HeaderText="TU" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <div class="altoFijo">
                                                    <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                        <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Título" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Tipificacion")%>'><%# Eval("Tipificacion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Total encuestas" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("NumeroEncuestas")%>'><%# Eval("NumeroEncuestas")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. Preguntas" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("NumeroPreguntasEncuesta")%>'><%# Eval("NumeroPreguntasEncuesta")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="0 - 5" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("CeroCinco")%>'><%# Eval("CeroCinco")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="6 - 7" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("SeisSiete")%>'><%# Eval("SeisSiete")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="8 - 9" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("OchoNueve")%>'><%# Eval("OchoNueve")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="10" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Diez")%>'><%# Eval("Diez")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Promedio ponderado" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# String.Format("{0:f1}", Eval("PromedioPonderado")) %>'><%# String.Format("{0:f1}", Eval("PromedioPonderado"))%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Categoría" HeaderStyle-Width="21%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Area.Descripcion")%>'><%# Eval("Area.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tipificación" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("TipoArbolAcceso.Descripcion")%>'><%# Eval("TipoArbolAcceso.Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="12%" Visible="False">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# (bool) Eval("EsTerminal") ? "Opción" : "Sección" %>'><%# (bool) Eval("EsTerminal") ? "Opción" : "Sección" %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="5%" Visible="False">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("Nivel") %>'><%# Eval("Nivel") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Ult. Movimiento" HeaderStyle-Width="15%" Visible="False">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("FechaAlta")%>'><%# Eval("FechaAlta")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Gráfico" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' ID="btnGraficar" Text="Ver" OnClick="btnGraficar_OnClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
