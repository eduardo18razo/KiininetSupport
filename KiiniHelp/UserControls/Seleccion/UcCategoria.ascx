<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCategoria.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcCategoria" %>
<%@ Import Namespace="KiiniNet.Entities.Operacion.Usuarios" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
           <%-- <br>--%>
             <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">
                    <asp:LinkButton runat="server" ID="lbTipoUsuario" OnClick="lbTipoUsuario_Click">
                                            
                    </asp:LinkButton>
                    <%--<a href="service-area.html"><% Response.Write(string.Format("{0}", ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado ? "Empleado" : ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado ? "Cliente" : "Proveedor")); %></a>--%>
                </li>
                <li class="breadcrumb-item active">Categorías</li>
            </ol>

            <%--<h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink>
                / <a href="service-area.html"><% Response.Write(string.Format("{0}", ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado ? "Empleado" : ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado ? "Cliente" : "Proveedor")); %></a> / Categorìas</h3>--%>
            <%--<hr />--%>
            <div class="row">
                <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content">
                                <!--INICIA CATEGORÍAS-->
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <h3>Categorias</h3>
                                        <hr>
                                       
                                        <%--<div class="module-inner">--%>
                                           <%-- <div class="module-heading">
                                                <h3 class="module-title">
                                                    <asp:Label runat="server" ID="lblSeccion" Text="Categorias" /></h3>
                                            </div>--%>
                                           <%-- <p>
                                                Texto para Categorias
                                            </p>--%>
                                        <%--</div>--%>
                                        
                                         <dl>
                                            <asp:Repeater runat="server" ID="rptAreas">
                                                <ItemTemplate>
                                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                                        <dt class="h4 text-theme"><%# Eval("Descripcion") %></dt>
                                                        <dd>Areas por Repeater.</dd>
                                                        <ul class="list list-unstyled">
                                                            <li>
                                                                <a href="service-area.html">
                                                                    <asp:LinkButton class="btn btn-default btn-sm" runat="server" CommandArgument='<%#Eval("Id") %>' ID="btnAreaSelect" OnClick="btnAreaSelect_OnClick">Ir a <%# Eval("Descripcion") %></asp:LinkButton>
                                                                </a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <%--<div class="col-md-6 col-sm-6 col-xs-12">
                                                <dt class="h4 text-theme">Recuersos Humanos</dt>
                                                <dd>Descripción del área de servicio y lo que podrá encontrar el usuario.</dd>
                                                <ul class="list list-unstyled">
                                                    <li><a href="service-area.html">
                                                        <button type="button" class="btn btn-default btn-sm">Ir a Recuersos Humanos</button></a></li>
                                                </ul>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <dt class="h4 text-theme">Sistemas</dt>
                                                <dd>Descripción del área de servicio y lo que podrá encontrar el usuario.</dd>
                                                <ul class="list list-unstyled">
                                                    <li><a href="service-area.html">
                                                        <button type="button" class="btn btn-default btn-sm">Ir a Sistemas</button></a></li>
                                                </ul>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <dt class="h4 text-theme">Compras</dt>
                                                <dd>Descripción del área de servicio y lo que podrá encontrar el usuario.</dd>
                                                <ul class="list list-unstyled">
                                                    <li><a href="service-area.html">
                                                        <button type="button" class="btn btn-default btn-sm">Ir a Compras</button></a></li>
                                                </ul>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <dt class="h4 text-theme">Mantenimiento a sucursales</dt>
                                                <dd>Descripción del área de servicio y lo que podrá encontrar el usuario.</dd>
                                                <ul class="list list-unstyled">
                                                    <li><a href="service-area.html">
                                                        <button type="button" class="btn btn-default btn-sm">Ir a Mantenimiento a sucursales</button></a></li>
                                                </ul>
                                            </div>
                                            <hr>
                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                <dt class="h4 text-theme">Empleados</dt>
                                                <dd>Descripción del área de servicio y lo que podrá encontrar el usuario.</dd>
                                                <ul class="list list-unstyled">
                                                    <li><a href="service-area.html">
                                                        <button type="button" class="btn btn-default btn-sm">Ir a Empleados</button></a></li>
                                                </ul>
                                            </div>--%>
                                            <hr>
                                        </dl>
                                    </div>
                                </div>
                                <!--TERMINA BUSCADOR-->
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
