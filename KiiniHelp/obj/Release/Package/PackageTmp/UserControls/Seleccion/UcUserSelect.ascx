<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcUserSelect.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcUserSelect" %>
<%@ Import Namespace="KiiniNet.Entities.Operacion.Usuarios" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <br>
            <br>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink>
                / <% Response.Write(string.Format("{0}", ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado ? "Empleado" : ((Usuario)Session["UserData"]).IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado ? "Cliente" : "Proveedor")); %></h3>
            <hr />
            <div class="row">
                <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content">
                                <!--INICIA BUSCADOR-->
                                <div class="help-search">
                                    <h3 class="text-center title">Utiliza nuestro buscador para una consulta rápida o busca por
                                        <asp:LinkButton ID="lbtnCategoria" runat="server" OnClick="lbtnCategoria_OnClick">categoría</asp:LinkButton></h3>
                                    <br>
                                    <div class="search-box form-inline text-center margin-bottom-lg">
                                        <label class="sr-only" for="help_search_form">Buscar</label>
                                        <div class="form-group">
                                            <asp:TextBox ID="help_search_form" name="search-form" type="text" class="form-control help_search_form" placeholder="Busca con una palabra clave..." runat="server" />
                                            <asp:LinkButton CssClass="btn btn-primary btn-single-icon" runat="server"><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <hr>
                                <!--TERMINA BUSCADOR-->
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
                                            <!--INICIAN TABS-->
                                            <!--INICIA TAB-1 10 MAS-->
                                            <div class="tab-content text-left">
                                                <div role="tabpanel" class="tab-pane-1 tab-pane fade in active" id="cat1">
                                                    <h3 class="subtitle text-center">10 más frecuentes</h3>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rpt10Frecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href="#faq1-1"><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %>  </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id="faq1-1">
                                                                        <div class="panel-body">este es item de repetidor</div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <%--<div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-1"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 1? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq1-1">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-2"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 2? <span class="label label-success">Actualizado</span> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-2">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-3"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 3? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-3">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-4"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 4? <span class="label label-success">Actualizado</span> </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-4">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-5"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 5? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-5">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-6"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 6? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-6">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-7"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 7? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-7">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-8"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 8? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-8">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-9"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 9? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-9">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq1-10"><i class="fa fa-plus-square"></i>¿En que sección encuentro los documentos de RH 10? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq1-10">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                                <!--TERMINA TAB-1 10 MAS-->
                                                <!--INICIA TAB-2 CONSULTA-->
                                                <div role="tabpanel" class="tab-pane-2 tab-pane fade" id="cat2">
                                                    <h3 class="subtitle text-center">FAQ´s Consulta</h3>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rptConsultasFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href="#faq2-1"><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id="faq2-1">
                                                                        <div class="panel-body">Consulta repetidor </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <%--<div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq2-1"><i class="fa fa-plus-square"></i>Información sobre el funcionamiento administrativo 1 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq2-1">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq2-2"><i class="fa fa-plus-square"></i>Información sobre el funcionamiento administrativo 2 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq2-2">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq2-3"><i class="fa fa-plus-square"></i>Información sobre el funcionamiento administrativo 3 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq2-3">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq2-4"><i class="fa fa-plus-square"></i>Información sobre el funcionamiento administrativo 4 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq2-4">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq2-5"><i class="fa fa-plus-square"></i>Información sobre el funcionamiento administrativo 5 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse collapse" id="faq2-5">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. Suspendisse turpis orci, tempus ac sollicitudin a, ullamcorper quis orci. Nunc vel ullamcorper lorem, quis rutrum leo. Quisque facilisis turpis ac purus aliquam, a blandit augue vehicula. </div>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                                <!--TERMINA TAB-2 CONSULTA-->
                                                <!--INICIA TAB-3 SERVICIO-->
                                                <div role="tabpanel" class="tab-pane-3 tab-pane fade" id="cat3">
                                                    <h3 class="subtitle text-center">FAQ´s SERVICIO</h3>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rptServiciosFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href="#faq3-1"><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %> </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id="faq3-1">
                                                                        <div class="panel-body">Servicio Repetidor</div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <%--<div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq3-1"><i class="fa fa-plus-square"></i>¿Cómo puedo solicitar mantenimiento de mi PC-1? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq3-1">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq3-2"><i class="fa fa-plus-square"></i>¿Cómo puedo solicitar mantenimiento de mi PC-2? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq3-2">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq3-3"><i class="fa fa-plus-square"></i>¿Cómo puedo solicitar mantenimiento de mi PC-3? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq3-3">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq3-4"><i class="fa fa-plus-square"></i>¿Cómo puedo solicitar mantenimiento de mi PC-4? </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq3-4">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                                <!--TERMINA TAB-3 SERVICIO-->
                                                <!--INICIA TAB-4 PROBLEMA-->
                                                <div role="tabpanel" class="tab-pane tab-pane fade" id="cat4">
                                                    <h3 class="subtitle text-center">FAQ´s Problema</h3>
                                                    <div class="panel-group panel-group-theme-1">
                                                        <asp:Repeater runat="server" ID="rptProblemasFrecuentes">
                                                            <ItemTemplate>
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading panel-heading-theme-1">
                                                                        <h4 class="panel-title"><a
                                                                            data-toggle="collapse" class="panel-toggle" href="#faq4-1"><i class="fa fa-plus-square"></i><%# Eval("DescripcionOpcion") %>  </a></h4>
                                                                    </div>
                                                                    <div class="panel-collapse  collapse" id="faq4-1">
                                                                        <div class="panel-body">ProblemaRepetidor </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <%--<div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq4-1"><i class="fa fa-plus-square"></i>Mi tarjeta de acceso no funciona 1 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq4-1">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq4-2"><i class="fa fa-plus-square"></i>Mi tarjeta de acceso no funciona 2 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq4-2">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq4-3"><i class="fa fa-plus-square"></i>Mi tarjeta de acceso no funciona 3 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq4-3">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading panel-heading-theme-1">
                                                                <h4 class="panel-title"><a
                                                                    data-toggle="collapse" class="panel-toggle" href="#faq4-4"><i class="fa fa-plus-square"></i>Mi tarjeta de acceso no funciona 4 </a></h4>
                                                            </div>
                                                            <div class="panel-collapse  collapse" id="faq4-4">
                                                                <div class="panel-body">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum et sodales dolor, eu hendrerit lorem. Maecenas vestibulum, nulla eu fringilla rhoncus, sapien metus mollis lorem, ac pulvinar ligula velit quis eros. </div>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--TERMINA TAB-4 PROBLEMA-->
                                    <!--TERMINAN TABS-->
                                    <div class="faq-lead text-center margin-bottom-lg">
                                        <h4 class="subtitle">¿No pudiste solucionar tu duda?</h4>
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
