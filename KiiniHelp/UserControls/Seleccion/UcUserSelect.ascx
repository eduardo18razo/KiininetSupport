<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcUserSelect.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcUserSelect" %>

<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item active">
                    <asp:Label runat="server" ID="lbltipoUsuario" /></li>
            </ol>

            <div class="row">
                <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content">
                                <!--INICIA BUSCADOR-->
                                <div class="help-search">
                                    <h4 class="text-center title">Haz una consulta rápida o selecciona una
                                        <asp:LinkButton ID="lbtnCategoria" runat="server" OnClick="lbtnCategoria_OnClick" CssClass="text-theme">categoría</asp:LinkButton></h4>
                                    <div class="row">
                                        <div class="search-box form-inline text-center padding-10-bottom">
                                            <label class="sr-only" for="help_search_form">Buscar</label>
                                            <div class="form-group col-lg-6 col-md-6 col-sm-10 col-xs-10 col-lg-offset-3 col-md-offset-3 col-sm-offset-1 col-xs-offset-1">
                                                <div class="col-lg-8 col-md-8 col-sm-11 col-xs-11 text-right no-padding-top">
                                                    <asp:TextBox ID="help_search_form" name="search-form" type="text" class="form-control help_search_form" placeholder="Busca con una palabra clave..." runat="server" />
                                                </div>

                                                <div class="col-lg-4 col-md-4 col-sm-1 col-xs-1 text-left">
                                                    <asp:LinkButton CssClass="btn btn-primary btn-single-icon" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!--TERMINA BUSCADOR-->
                                <!--INICIAN TABS-->
                            </div>
                        </div>
                    </section>

                    <section class="module">
                        <div class="row module-inner">
                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1 ">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_1">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-star icon iconoFontXg"></i>
                                                    <br />
                                                    10 más frecuentes
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse  collapse" id="faq5_1">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rpt10Frecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %>  </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq_" + Eval("IdArbol") %>'>
                                                                <div class="panel-body">
                                                                    <%# Eval("DescripcionOpcionLarga") %>
                                                                    <asp:LinkButton runat="server" ID="verOpcion" Text="Ver más" CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Repeater runat="server" ID="rptCatTodos">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq5_" + Eval("Id") %>'><i class="fa fa-plus-square"></i><%# Eval("Tipificacion") %> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq5_" + Eval("Id") %>'>
                                                                <div class="panel-body">
                                                                    <asp:Label runat="server" ID="datos" Text='<%# Eval("TipoArbolAcceso.Descripcion") %>'></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_2">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-notebook icon iconoFontXg"></i>
                                                    <br />
                                                    Consulta
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse  collapse" id="faq5_2">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptConsultasFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq2_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq2_" + Eval("IdArbol") %>'>
                                                                <div class="panel-body">
                                                                    <%# Eval("DescripcionOpcionLarga") %>
                                                                    <asp:LinkButton runat="server" ID="verConsulta" Text="Ver más" CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatConsultas">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq6_" + Eval("Id") %>'><i class="fa fa-plus-square"></i><%# Eval("Tipificacion") %> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq6_" + Eval("Id") %>'>
                                                                <div class="panel-body">
                                                                    <asp:Label runat="server" ID="datos" Text='<%# Eval("TipoArbolAcceso.Descripcion") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_3">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-config icon iconoFontXg"></i>
                                                    <br />
                                                    Servicio
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse  collapse" id="faq5_3">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptServiciosFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq3_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq3_" + Eval("IdArbol") %>'>
                                                                <div class="panel-body">
                                                                    <%# Eval("DescripcionOpcionLarga") %>
                                                                    <asp:LinkButton runat="server" ID="verServicio" Text="Ver más" CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatServicios">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq6_" + Eval("Id") %>'><i class="fa fa-plus-square"></i><%# Eval("Tipificacion") %> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq6_" + Eval("Id") %>'>
                                                                <div class="panel-body">
                                                                    <asp:Label runat="server" ID="datos" Text='<%# Eval("TipoArbolAcceso.Descripcion") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading panel-heading-theme-1">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="panel-toggle no-underline" href="#faq5_4">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-help2 icon iconoFontXg"></i>
                                                    <br />
                                                    Problema
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse  collapse" id="faq5_4">
                                        <div class="panel-body">

                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptProblemasFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href='<%# "#faq4_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %>  </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq4_" + Eval("IdArbol") %>'>
                                                                <div class="panel-body">
                                                                    <%# Eval("DescripcionOpcionLarga") %>
                                                                    <asp:LinkButton runat="server" ID="verServicio" Text="Ver más" CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatProblemas">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href='<%# "#faq4_" + Eval("Id") %>'><i class="fa fa-plus-square"></i><%# Eval("Tipificacion") %>  </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id='<%# "faq4_" + Eval("Id") %>'>
                                                                <div class="panel-body">
                                                                    <asp:Label runat="server" ID="datos" Text='<%# Eval("TipoArbolAcceso.Descripcion") %>' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </section>

                    <section class="module">
                        <div class="row">
                            <div class="faq-lead text-center margin-bottom-lg">
                                <h4 class="subtitle">¿Necesitas más ayuda?</h4>
                                <a class="btn btn-primary margin-bottom-15" role="menuitem" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>Generar un ticket </a>
                            </div>
                        </div>
                    </section>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</div>
