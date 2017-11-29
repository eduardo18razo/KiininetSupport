<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaTickets.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaTickets" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosTicket.ascx" TagPrefix="uc1" TagName="UcFiltrosTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="pnlAlertaGral">
        <ContentTemplate>
            <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
                <div class="alert alert-danger">
                    <div>
                        <div style="float: left">
                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                        </div>
                        <div style="float: left">
                            <h3>Error</h3>
                        </div>
                        <div class="clearfix clear-fix" />
                    </div>
                    <hr />
                    <asp:Repeater runat="server" ID="rptErrorGeneral">
                        <ItemTemplate>
                            <ul>
                                <li><%# Container.DataItem %></li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </header>
            <br />
                    <uc1:UcFiltrosTicket runat="server" id="ucFiltrosTicket" />               
            <div class="panel panel-primary">
                <div class="panel-body">
                    <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="350px">
                        <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" Font-Size="11px"
                        OnRowCreated="gvResult_OnRowCreated" AutoGenerateColumns ="false">
                        <EmptyDataTemplate>
                                                ¡No hay resultados para mostrar!
                        </EmptyDataTemplate>
                        <Columns>                           
                            <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Canal" Width="40px"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Canal") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="TU"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + "green" + " !important" %>'>
                                            <%# Eval("TipoUsuario") %></span>                                    
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="100px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoEspecialConsulta"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoEspecialConsulta") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoAtendedor"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoAtendedor") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoMantenimiento"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoMantenimiento") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoDesarrollo"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoDesarrollo") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="700px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Organización"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Organizacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="700px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Ubicación"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="100px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="ServicioIncidente"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("ServicioIncidente") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="120px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Tipificación"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="60px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Prioridad"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Prioridad") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="45px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Estatus"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Estatus") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Sla"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Sla") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>                                                           
                             <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="FechaHora"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("FechaHora") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                                       
                        </Columns> 
                       </asp:GridView>
                     </asp:Panel>
                   <%-- <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="250px">
                    <div class="table-responsive">
                        <asp:Repeater runat="server" ID="rptResultados" >
                            <HeaderTemplate>
                                <table class="table table-striped display" id="tblResults"  style="font-size:11px">
                                    <thead>
                                        <tr>
                                            <th style="width: 30px">
                                                <asp:Label runat="server">Tipo Usuario</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Tipificación</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Canal</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">GrupoEspecialConsulta</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">GrupoAtendedor</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">GrupoMantenimiento</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">GrupoDesarrollo</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Organizacion</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Ubicacion</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">ServicioIncidente</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Tipificacion</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Prioridad</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Estatus</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">Sla</asp:Label></th>
                                            <th>
                                                <asp:Label runat="server">FechaHora</asp:Label></th>                                          
                                        </tr>
                                    </thead>
                                   
                            </HeaderTemplate>
                            <ItemTemplate>
                                 <tbody>
                                <tr id='<%# Eval("IdTicket")%>'>
                                    <td>--%>
                   <%--    <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                   <%# Eval("TipoUsuario.Abreviacion") %></button></td>--%>
                   <%--   <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + "green" + " !important" %>'>
                                            <%# Eval("TipoUsuario") %></button></td>
                                    <td><%# Eval("Tipificacion")%></td>
                                    <td><%# Eval("Canal")%></td>
                                    <td><%# Eval("GrupoEspecialConsulta")%></td>
                                    <td><%# Eval("GrupoAtendedor")%></td>
                                    <td><%# Eval("GrupoMantenimiento")%></td>
                                    <td><%# Eval("Organizacion")%></td>
                                    <td><%# Eval("Ubicacion")%></td>
                                    <td><%# Eval("ServicioIncidente")%></td>
                                    <td><%# Eval("Tipificacion")%></td>
                                    <td><%# Eval("Prioridad")%></td>
                                    <td><%# Eval("Estatus")%></td>
                                    <td><%# Eval("Sla")%></td>
                                    <td><%# Eval("FechaHora")%></td>                                   
                                </tr>
                                   </tbody>
                            </ItemTemplate>
                            <FooterTemplate>
                               
                             </table>
                                           
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    </asp:Panel>--%>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

