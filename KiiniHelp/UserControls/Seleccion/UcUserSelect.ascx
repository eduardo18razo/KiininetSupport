<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcUserSelect.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcUserSelect" %>

<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="text-theme">
                    <asp:LinkButton runat="server" OnClick="OnClick" Text="text-theme">Home</asp:LinkButton></li>
                <li class="active">
                    <asp:Label runat="server" ID="lbltipoUsuario" /></li>
            </ol>

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module">
                        <div class="row">
                            <div class="module-inner">
                                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                                    <div class="module-heading no-border-bottom" runat="server" id="divCategoriaReference">
                                        <h3 class="text-center module-title">Haz una consulta rápida o selecciona una
                                        <asp:LinkButton ID="lbtnCategoria" runat="server" OnClick="lbtnCategoria_OnClick" CssClass="text-theme">categoría</asp:LinkButton></h3>
                                    </div>
                                    <div class="module-heading no-border-bottom" runat="server" id="divCategoriaTitle" visible="False">
                                        <h3 class="module-title">
                                            <asp:Label runat="server" ID="lblCategoria"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section class="module">
                        <div class="row module-inner">
                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_1">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-star icon iconoFontXg"></i>
                                                    <br />
                                                    10 más frecuentes
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse " id="faq5_1">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rpt10Frecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading ">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("DescripcionOpcion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </h4>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Repeater runat="server" ID="rptCatTodos">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("Tipificacion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("IdTipoArbolAcceso") %>' />
                                                                </h4>
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
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_2">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-notebook icon iconoFontXg"></i>
                                                    <br />
                                                    Consulta
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse" id="faq5_2">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptConsultasFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading ">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("DescripcionOpcion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </h4>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatConsultas">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("Tipificacion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("IdTipoArbolAcceso") %>' />
                                                                </h4>
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
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_3">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-config icon iconoFontXg"></i>
                                                    <br />
                                                    Servicio
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse" id="faq5_3">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptServiciosFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading ">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("DescripcionOpcion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </h4>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatServicios">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("Tipificacion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("IdTipoArbolAcceso") %>' />
                                                                </h4>
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
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_4">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-help2 icon iconoFontXg"></i>
                                                    <br />
                                                    Problema
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse" id="faq5_4">
                                        <div class="panel-body">

                                            <div class="panel-group panel-group-theme-1">
                                                <asp:Repeater runat="server" ID="rptProblemasFrecuentes">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading ">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("DescripcionOpcion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("IdArbol") %>' CommandName='<%# Eval("IdTipoArbol") %>' />
                                                                </h4>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                <asp:Repeater runat="server" ID="rptCatProblemas">
                                                    <ItemTemplate>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" Text='<%# Eval("Tipificacion") %>' CssClass="text-theme" OnClick="verOpcion_Click" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Eval("IdTipoArbolAcceso") %>' />
                                                                </h4>
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
