<%@ Page Title="Consulta Ticket" Language="C#" MasterPageFile="~/Public.Master" ValidateRequest="false" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmConsultaTicket.aspx.cs" Inherits="KiiniHelp.Publico.Consultas.FrmConsultaTicket" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleMascaraCaptura.ascx" TagPrefix="uc1" TagName="UcDetalleMascaraCaptura" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<%@ Register TagPrefix="ctrlExterno" Namespace="Winthusiasm.HtmlEditor" Assembly="Winthusiasm.HtmlEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%--<script>
        function SetComment(parameters) {
            var txtEditor = document.getElementById('ContentPlaceHolderPublic_txtEditor_designEditor');
            if (txtEditor != undefined) {
                var hfComentario = document.getElementById('<%= hfComentario.ClientID%>');
                if (hfComentario != undefined) {
                    hfComentario.value = txtEditor.contentDocument.body.innerHTML;
                }
            }
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">

    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<asp:HiddenField runat="server" ID="hfComentario"/>--%>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta"/>
            <div runat="server" id="divConsulta">
                <div>                 
                    <ol class="breadcrumb">
                        <li><asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                        <li class="active">Consulta de tickets</li>
                    </ol>
                    
                </div>
                <div class="clearfix"></div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div>
                            <section class="module module-no-heading">
                                <div class="module-inner">
                                    <div class="module-heading no-border">
                                        <h4 class="title">Consulta de tickets</h4>
                                        <p class="text-primary">Ingresa la siguiente información para consultar tu ticket.</p>
                                        <hr>
                                    </div>
                                    <div class="module-content collapse in" id="content-4">
                                        <div class="module-content-inner no-padding-bottom">
                                            <div data-parsley-validate class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-sm-2 control-label" for="txtTicket">No. de ticket</label>
                                                    <div class="col-sm-10">
                                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtTicket" onkeydown="return (event.keyCode!=13);"/>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-2 control-label" for="txtClave">Clave de registro</label>
                                                    <div class="col-sm-10">
                                                        <asp:TextBox type="text" CssClass="form-control text-uppercase" runat="server" ID="txtClave"  onkeydown="return (event.keyCode!=13);"/>
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
                                </div>
                            </section>
                        </div>
                    </div>
                </div>

            </div>
            <div runat="server" id="divDetalle" visible="False">
                <div>
                    <br>
                     <ol class="breadcrumb">
                        <li><asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                        <li>Cliente</li>
                        <li class="active">Detalle del ticket</li>
                    </ol>
                </div>
                <asp:HiddenField runat="server" ID="hfEstatusActual" />
                <asp:HiddenField runat="server" ID="hfNivelAsignado" />
                <asp:HiddenField runat="server" ID="hfIdUsuarioTicket" />
                <div class="clearfix"></div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div >
                            <section class="module module-no-heading">
                                <div class="module-inner">
                                    <div class="module-heading">
                                        <h4 class="title">Detalle del ticket</h4>
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
                                                    <asp:Button runat="server" Text="Estatus" ID="btnCambiaEstatus" CssClass="btn btn-primary" OnClick="btnCambiaEstatus_OnClick" />
                                                    <hr>

                                                    <uc1:UcDetalleMascaraCaptura runat="server" id="ucDetalleMascaraCaptura" />

                                                    
                                                </div>
                                            </div>
                                            <hr>
                                            <div class="comment-list">
                                                <div class="reply-holder">
                                                    <h4 class="title">Agregar Comentario</h4>
                                                    <div class="reply-content">
                                                        <div class="author">
                                                            <asp:Image ImageUrl="~/assets/images/profiles/profile-1.png" alt="" runat="server" />
                                                        </div>
                                                        <div class="form-holder">
                                                            <div>
                                                                <asp:TextBox ID="txtConversacion" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control no-padding-top no-margin-top" MaxLength="999" />
                                                                <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <ctrlexterno:htmleditor runat="Server" id="txtEditor" height="350px" ToggleMode="ToggleButton" colorscheme="VisualStudio" onkeypress="SetComment(this)"/>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>--%>
                                                                <asp:Button CssClass="btn btn-primary" runat="server" Text="Comentar" ID="btnComentar"  OnClick="btnComentar_OnClick" /><%--OnClientClick="SetComment(this);"--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <hr />
                                                <h4 class="title">Comentarios</h4>
                                                <asp:Repeater runat="server" ID="rptComentrios" OnItemDataBound="rptComentrios_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="comment-item">
                                                            <div class="comment-item-box">
                                                                <div class="comment-author">
                                                                    <asp:Image class="img-responsive img-circle" ImageUrl="~/assets/images/profiles/profile.png" alt="" runat="server" />
                                                                </div>
                                                                <div class="comment-body">
                                                                    <cite class="name"><%# Eval("Nombre") %></cite>
                                                                    <p class="time"><%# Eval("FechaHora", "{0:MMM}").TrimEnd('.') + " " + Eval("FechaHora", "{0:dd}") + ", " +  Eval("FechaHora", "{0:yyyy}") + " " +  Eval("FechaHora", "{0:hh:mm tt}") %></p>
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
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" id="ucCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
