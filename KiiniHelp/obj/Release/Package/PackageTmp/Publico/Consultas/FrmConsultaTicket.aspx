<%@ Page Title="Consulta Ticket" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmConsultaTicket.aspx.cs" Inherits="KiiniHelp.Publico.Consultas.FrmConsultaTicket" %>

<%@ Import Namespace="KinniNet.Business.Utils" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server" id="divConsulta">
                    <div class="project-heading">
                        <br>
                        <h3 class="h6"><asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink> / <asp:HyperLink runat="server" NavigateUrl="~/DefaultUserSelect.aspx">Cliente</asp:HyperLink> / Consulta de tickets</h3>
                        <hr>
                    </div>
                    <div class="clearfix"></div>
                    <!--INICIA COMENTARIOS-->
                    <div class="row">
                        <div class="col-wrapper col-md-12 col-sm-12 col-xs-12">
                            <div class="module-wrapper">
                                <section class="module member-module module-no-heading module-no-footer">
                                    <div class="module-inner">
                                        <div class="module-heading">
                                            <h3 class="title">Consulta de tickets</h3>
                                            <p class="text-primary">Ingresa la siguiente información para consultar tu ticket.</p>
                                            <hr>
                                        </div>

                                        <!--INICIA input text-->
                                        <div class="module-content collapse in" id="content-4">
                                            <div class="module-content-inner no-padding-bottom">
                                                <div data-parsley-validate id="" class="form-horizontal">
                                                    <div class="form-group">
                                                        <label class="col-sm-2 control-label" for="">No. de ticket</label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox required="required" runat="server" CssClass="form-control" ID="txtTicket" placeholder="Ingresa aquí" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-2 control-label" for="">Clave de registro</label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox required="required" type="text" class="form-control" placeholder="Ingresa aquí" runat="server" ID="txtClave" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-offset-2 col-sm-10">
                                                            <asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnConsultar" OnClick="btnConsultar_OnClick">Consultar</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--TERMINA input text-->

                                    </div>
                                </section>
                            </div>
                        </div>
                    </div>
                    <!--TERMINA COMENTARIOS-->

                </div>
                <div runat="server" id="divDetalle" visible="False">
                    <div class="project-heading">
                        <br>
                        <br>
                        <br>
                        <h3 class="h6"><a href="index.html">Home</a> / <a href="user_select.html">Cliente</a> / Detalle del ticket</h3>
                        <hr>
                    </div>
                    <div class="clearfix"></div>
                    <!--INICIA COMENTARIOS-->
                    <div class="row">
                        <div class="col-wrapper col-md-12 col-sm-12 col-xs-12">
                            <div class="module-wrapper">
                                <section class="module member-module module-no-heading module-no-footer">
                                    <div class="module-inner">
                                        <div class="module-heading">
                                            <h3 class="title">Detalle del ticket</h3>
                                        </div>
                                        <div class="module-content">
                                            <div class="module-content-inner no-padding-bottom">
                                                <div class="topic-holder">
                                                    <div class="topic-author">
                                                        <asp:Image class="img-responsive img-circle" ImageUrl="~/assets/images/profiles/profile-1.png" alt="" runat="server" />
                                                    </div>
                                                    <div class="topic-content-wrapper">
                                                        <a class="name"><strong>No. de ticket:</strong>
                                                            <asp:Label runat="server" ID="lblticket" /></a><br>
                                                        <a class="name"><strong>Clave de registro:</strong><asp:Label runat="server" ID="lblCveRegistro" /></a>
                                                        <hr>
                                                        <a class="name"><strong>Fecha:</strong>
                                                            <asp:Label runat="server" ID="lblfecha" /></a>
                                                        <p class="name">
                                                            <strong>Última actualización:</strong>
                                                            <asp:Label runat="server" ID="lblFechaActualiza" />
                                                        </p>
                                                        <p class="name">
                                                            <strong>Estatus Actual:</strong>
                                                            <asp:Label runat="server" ID="lblestatus" />
                                                        </p>
                                                        <hr>
                                                        <p class="name"><strong>Solicitante:</strong> Javier Gómez</p>
                                                        <p class="name"><strong>Correo:</strong> igomez@bancremex.com.mx</p>
                                                        <p class="name"><strong>Asunto:</strong> Apertura de cuenta</p>
                                                        <div class="topic-content">
                                                            <p><strong>Comentario:</strong> Quiero información para abrir una cuenta de Débito. </p>
                                                        </div>
                                                    </div>
                                                </div>
                                                <hr>
                                                <!--INICIAN COMETARIOS-->
                                                <div class="comment-list">
                                                    <h3 class="title">Comentarios</h3>
                                                    <asp:Repeater runat="server" ID="rptComentrios" OnItemDataBound="rptComentrios_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="comment-item depth-1 parent">
                                                                <div class="comment-item-box">
                                                                    <div class="comment-author">
                                                                        <asp:Image class="img-responsive img-circle" ImageUrl="~/assets/images/profiles/profile.png" alt="" runat="server" />
                                                                    </div>
                                                                    <div class="comment-body">
                                                                        <cite class="name"><%# Eval("Nombre") %></cite>
                                                                        <p class="time"><%# Eval("FechaHora") %></p>
                                                                        <div class="content">
                                                                            <p><%# Eval("Comentario") %></p>
                                                                        </div>
                                                                        <asp:Repeater runat="server" ID="rptDownloads">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" Text="" CssClass="col-sm-2 control-label" />
                                                                                <div class="col-sm-10">
                                                                                    <asp:HyperLink runat="server" Text='<%# Eval("Archivo") %>' NavigateUrl='<%# ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioCorreo + "~" + Eval("Archivo"))) %>'></asp:HyperLink>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                        <a class="comment-reply-link btn btn-default" href="#">Responder</a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                    <%--<div class="comment-item depth-1 parent">
                                                    <div class="comment-item-box">
                                                        <div class="comment-author">
                                                            <img class="img-responsive" src="assets/images/profiles/profile-1.png" alt="">
                                                        </div>
                                                        <div class="comment-body">
                                                            <cite class="name">Javier Gómez:</cite>
                                                            <p class="time">Mar 13, 2016 / 9:25am</p>
                                                            <div class="content">
                                                                <p>Sed condimentum vel quam ut sagittis. Aliquam erat enim, lobortis sit amet tellus accumsan, scelerisque porttitor sem. Suspendisse dictum risus id iaculis sodales phasellus id ex aliquam.</p>
                                                                <p>
                                                                    <img class="img-responsive" src="assets/images/demo/image-example-2.jpg" alt="" />
                                                                </p>
                                                            </div>
                                                            <a class="comment-reply-link btn btn-default" href="#">Responder</a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="comment-item depth-1 parent">
                                                    <div class="comment-item-box">
                                                        <div class="comment-author">
                                                            <img class="img-responsive" src="assets/images/profiles/profile.png" alt="">
                                                        </div>
                                                        <div class="comment-body">
                                                            <cite class="name">Agente de servicio:</cite>
                                                            <p class="time">Feb 14, 2016 / 10:35am</p>
                                                            <div class="content">
                                                                <p>Nunc in urna eu lorem accumsan placerat vel eu turpis. Etiam laoreet posuere mauris, id pharetra orci molestie sit amet. Duis pretium diam ex, vitae eleifend diam ornare sit amet. </p>
                                                            </div>
                                                            <a class="comment-reply-link btn btn-default" href="#">Responder</a>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                    <nav class="text-center pagination-wrapper">
                                                        <p class="pagination-info">1-3 de 1</p>
                                                        <ul class="pagination pagination-sm">
                                                            <li class="disabled"><a href="#" aria-label="Previous"><span aria-hidden="true">«</span></a></li>
                                                            <li class="active"><a href="#">1 <span class="sr-only">(current)</span></a></li>
                                                            <li><a href="#">2</a></li>
                                                            <li><a href="#">3</a></li>
                                                            <li><a href="#">4</a></li>
                                                            <li><a href="#">5</a></li>
                                                            <li><a href="#" aria-label="Next"><span aria-hidden="true">»</span></a></li>
                                                        </ul>
                                                    </nav>
                                                </div>
                                                <hr>
                                                <div class="reply-holder">
                                                    <h4 class="title">Agregar Comentario</h4>
                                                    <div class="reply-content">
                                                        <div class="author">
                                                            <asp:Image ImageUrl="~/assets/images/profiles/profile-1.png" alt="" runat="server" />
                                                        </div>
                                                        <div class="form-holder">
                                                            <div class="margin-bottom-lg">
                                                                <asp:TextBox ID="targetEditor" onclick="CargaEditor('#targetEditor')" data-provide="markdown" Rows="10" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                <asp:Button CssClass="btn btn-primary" runat="server" Text="Comentar" />
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
                    </div>
                    <!--TERMINA COMENTARIOS-->
                </div>
                <%--<script>
                CargaEditor("#targetEditor");
            </script>--%>
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

            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
