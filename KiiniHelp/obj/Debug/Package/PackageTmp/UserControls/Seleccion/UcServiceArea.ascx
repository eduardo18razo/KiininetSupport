<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcServiceArea.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcServiceArea" %>
<%@ Import Namespace="KiiniNet.Entities.Operacion.Usuarios" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>

<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>           

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">
                    <asp:LinkButton runat="server" ID="lbTipoUsuario" OnClick="lbTipoUsuario_Click">
                       <% Response.Write(string.Format("{0}", ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado ? "Empleado" : ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado ? "Cliente" : "Proveedor")); %>                        
                    </asp:LinkButton>
                </li>
                <li class="breadcrumb-item">Categorías</li>
                <li class="breadcrumb-item active">
                    <label>Área</label>
                    <%--<asp:LinkButton runat="server" ID="LinkButton1" OnClick="lbTipoUsuario_Click"> 
                        
                       <% Response.Write(string.Format("{0}", ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado ? "Empleado" : ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado ? "Cliente" : "Proveedor")); %>                        
                    </asp:LinkButton>--%>
                </li>
            </ol>

            <%--<br>
            <h3 class="h6"><a href="index.html">Home</a> / <a href="user_select.html">Empleado</a> / Recursos Humanos</h3>--%>
            <%--<hr>--%>
            <div class="row">
                <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content">
                                <h2 class="title text-left">Recursos humanos</h2>
                                <hr>
                                <div class="module-content-inner">
                                    <div class="help-section">
                                        <div class="help-search">
                                            <h3 class="text-center title">¿Podemos ayudarte?</h3>
                                            <div class="search-box form-inline text-center margin-bottom-lg">
                                                <label class="sr-only" for="help-search-form">Buscar</label>
                                                <div class="form-group">
                                                    <input id="help-search-form" name="search-form" type="text" class="form-control help-search-form" placeholder="Busca con una palabra clave..." style="width:250px">
                                                    <button type="submit" class="btn btn-primary btn-single-icon"><i class="fa fa-search"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                        <h3 class="text-center title">Revisa nuestras consultas rápidas</h3>
                                       <%-- <br>--%>

                                        <!--INICIA LISTA-->

                                        <div class="help-category-wrapper margin-bottom-lg">
                                            <div class="row text-center">
                                                <div class="cat-item col-md-4 col-sm-6 col-xs-12  margin-bottom-lg">
                                                    <h4 class="cat-title">Consultar Información</h4>
                                                    <asp:Repeater runat="server" ID="rptConsultas">
                                                        <HeaderTemplate>
                                                            <ul class="list list-unstyled margin-bottom-sm">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <li><a href="#"><%#Eval("Descripcion") %></a></li>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </ul>
                                                            <a href="faqs.html" class="btn btn-default">Ver más</a>
                                                        </FooterTemplate>
                                                    </asp:Repeater>

                                                </div>
                                                <div class="row text-center">
                                                    <div class="cat-item col-md-4 col-sm-6 col-xs-12  margin-bottom-lg">
                                                        <h4 class="cat-title">Acerca de un Servicio</h4>
                                                        <asp:Repeater runat="server" ID="rptServicios">
                                                            <HeaderTemplate>
                                                                <ul class="list list-unstyled margin-bottom-sm">
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li><a href="#"><%#Eval("Descripcion") %></a></li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ul>
                                                        <a href="faqs.html" class="btn btn-default">Ver más</a>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class="row text-center">
                                                        <div class="cat-item col-md-4 col-sm-6 col-xs-12  margin-bottom-lg">
                                                            <h4 class="cat-title">Reportar un Problema</h4>
                                                            <asp:Repeater runat="server" ID="rptIncidentes">
                                                                <HeaderTemplate>
                                                                    <ul class="list list-unstyled margin-bottom-sm">
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <li><a href="#"><%#Eval("Descripcion") %></a></li>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </ul>
                                                            <a href="faqs.html" class="btn btn-default">Ver más</a>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                    <!--TERMINA LISTA-->
                                                    <br>
                                                    <br>
                                                    <div class="help-lead text-center margin-bottom-lg">
                                                        <h4 class="subtitle">¿Aún necesitas ayuda?</h4>
                                                        <a class="btn btn-primary" role="menuitem" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>Generar un ticket </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
