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
                                        <asp:LinkButton ID="lbtnCategoria" runat="server" OnClick="lbtnCategoria_OnClick">categoría</asp:LinkButton></h4>
                                    <div class="search-box form-inline text-center padding-10-bottom">
                                        <label class="sr-only" for="help_search_form">Buscar</label>
                                        <div class="form-group padding">
                                            <asp:TextBox ID="help_search_form" name="search-form" type="text" class="form-control help_search_form" placeholder="Busca con una palabra clave..." runat="server" />
                                            <asp:LinkButton CssClass="btn btn-primary btn-single-icon" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <!--TERMINA BUSCADOR-->
                                <div class="row">
                                    <h3>
                                        <asp:Label runat="server" ID="lblCategoria" Visible="False"></asp:Label>
                                    </h3>
                                </div>

                                <!--INICIAN TABS-->
                                <div class="module-content-inner">
                                    <div class="faq-section text-center margin-bottom-lg">
                                        <div class="faqs-tabbed tabpanel" role="tabpanel">
                                            <!-- Nav tabs -->
                                            <ul class="nav nav-tabs nav-tabs-theme-3 margin-bottom-lg" role="tablist">
                                                <li role="presentation" class="active"><a href="#cat1" aria-controls="cat1" role="tab" data-toggle="tab"><span class="pe-icon pe-7s-star icon"></span>
                                                    <br>
                                                    10 más frecuentes</a> </li>
                                                <li role="presentation"><a href="#cat2" aria-controls="cat2" role="tab" data-toggle="tab"><span class="pe-icon pe-7s-notebook icon"></span>
                                                    <br>
                                                    Consulta</a> </li>
                                                <li role="presentation"><a href="#cat3" aria-controls="cat3" role="tab" data-toggle="tab"><span class="pe-icon pe-7s-config icon"></span>
                                                    <br>
                                                    Servicio</a> </li>
                                                <li role="presentation" class="last"><a href="#cat4" aria-controls="cat4" role="tab" data-toggle="tab"><span class="pe-icon pe-7s-help2 icon"></span>
                                                    <br>
                                                    Problema</a> </li>
                                            </ul>

                                            <div class="tab-content text-left">
                                                <div role="tabpanel" class="tab-pane-1 tab-pane fade in active" id="cat1">
                                                    <h4 class="subtitle text-center">10 más frecuentes</h4>
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

                                                <div role="tabpanel" class="tab-pane-2 tab-pane fade" id="cat2">
                                                    <h4 class="subtitle text-center">Consultas más frecuentes</h4>
                                                    <div class="panel-group panel-group-theme-1">

                                                        <asp:Repeater runat="server" ID="rptConsultasFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title">
                                                                            <a data-toggle="collapse" class="panel-toggle" href='<%# "#faq2_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id='<%# "faq2_" + Eval("IdArbol") %>'>
                                                                        <div class="panel-body"><%# Eval("DescripcionOpcionLarga") %></div>
                                                                       <%-- <asp:LinkButton runat="server" ID="verOpcion" Text="Ver opción" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' />--%>
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
                                                <div role="tabpanel" class="tab-pane-3 tab-pane fade" id="cat3">
                                                    <h4 class="subtitle text-center">Servicios más frecuentes</h4>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rptServiciosFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href='<%# "#faq3_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id='<%# "faq3_" + Eval("IdArbol") %>'>
                                                                        <div class="panel-body"><%# Eval("DescripcionOpcionLarga") %></div>
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
                                                                            <asp:Label runat="server" ID="datos" Text='<%# Eval("TipoArbolAcceso.Descripcion") %>'></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </div>
                                                </div>

                                                <div role="tabpanel" class="tab-pane tab-pane fade" id="cat4">
                                                    <h4 class="subtitle text-center">Problemas más frecuentes</h4>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rptProblemasFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href='<%# "#faq4_" + Eval("IdArbol") %>'><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %>  </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id='<%# "faq4_" + Eval("IdArbol") %>'>
                                                                        <div class="panel-body"><%# Eval("DescripcionOpcionLarga") %></div>
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
                                    <div class="faq-lead text-center margin-bottom-lg">
                                        <h4 class="subtitle">¿Necesitas más ayuda?</h4>
                                        <a class="btn btn-primary" role="menuitem" data-toggle="modal" data-target="#modal-new-ticket"><i class="fa fa-play-circle"></i>Generar un ticket </a>
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
