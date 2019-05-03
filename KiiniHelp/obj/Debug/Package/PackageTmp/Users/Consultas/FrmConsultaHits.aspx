<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaHits.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaHits" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosConsulta.ascx" TagPrefix="uc1" TagName="UcFiltrosConsulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="pnlResult">
            <ContentTemplate>
                <ol class="breadcrumb">
                    <li>
                        <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                    <li>Reportes</li>
                    <li class="active">Encuestas</li>
                </ol>
                <section class="module">
                    <div class="row">
                        <div class="module-inner">
                            <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <uc1:UcFiltrosConsulta runat="server" ID="UcFiltrosConsulta" />
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
                                    <asp:GridView runat="server" ID="gvResult" AllowPaging="true" Width="99%"
                                        OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                        BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                        PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-bordered table-hover table-responsive">
                                        <EmptyDataTemplate>
                                            ¡No hay resultados para mostrar!
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Hit" Width="40px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("IdHit") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="TU" Width="30px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuarioColor") + " !important; width: 25px !important" %>'>
                                                        <%# Eval("TipoUsuarioAbreviacion") %></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Fecha" Width="30px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("FechaHora") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Hora" Width="30px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Hora") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Organización" Width="700px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Organizacion") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Ubicación" Width="700px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Tipificación" Width="350px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Vip" Width="30px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Vip") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Total" Width="40px" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Eval("Total") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

