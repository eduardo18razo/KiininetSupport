<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaTickets.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaTickets" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosTicket.ascx" TagPrefix="uc1" TagName="UcFiltrosTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="pnlResult">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Reportes</li>
                <li class="active">Tickets</li>
            </ol>
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="form-group">
                                <uc1:UcFiltrosTicket runat="server" ID="ucFiltrosTicket" />
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
                                <asp:GridView runat="server" ID="gvResult" AllowPaging="true" OnRowCreated="gvResult_OnRowCreated" Width="99%" AutoGenerateColumns="False"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-bordered table-hover table-responsive">
                                    <EmptyDataTemplate>
                                        ¡No hay resultados para mostrar!
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Canal" Width="40px" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Canal") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="TU" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + "green" + " !important" %>'>
                                                    <%# Eval("TipoUsuario.Abreviacion") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoEspecialConsulta" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoEspecialConsulta") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="40px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoAtendedor" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoAtendedor") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoMantenimiento" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoMantenimiento") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoDesarrollo" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoDesarrollo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="700px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Organización" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Organizacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="700px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Ubicación" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="ServicioIncidente" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("ServicioIncidente") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="120px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Tipificación" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="60px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Prioridad" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Prioridad") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="45px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Estatus" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Estatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Sla" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Sla") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="FechaHora" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("FechaHora") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
            </section>

            <%--<div class="panel panel-primary">
                <div class="panel-body">
                    <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="350px">
                        <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" Font-Size="11px"
                            OnRowCreated="gvResult_OnRowCreated" AutoGenerateColumns="false">
                            <EmptyDataTemplate>
                                ¡No hay resultados para mostrar!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Canal" Width="40px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Canal") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="TU" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + "green" + " !important" %>'>
                                            <%# Eval("TipoUsuario") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="100px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="GrupoEspecialConsulta" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("GrupoEspecialConsulta") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="40px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="GrupoAtendedor" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("GrupoAtendedor") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="GrupoMantenimiento" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("GrupoMantenimiento") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="GrupoDesarrollo" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("GrupoDesarrollo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="700px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Organización" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Organizacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="700px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Ubicación" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="100px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="ServicioIncidente" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("ServicioIncidente") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="120px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Tipificación" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="60px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Prioridad" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Prioridad") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="45px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Estatus" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Estatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Sla" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Sla") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="30px">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="FechaHora" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("FechaHora") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>--%>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

