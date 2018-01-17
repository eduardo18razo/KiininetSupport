<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewOpcionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Preview.UcPreviewOpcionConsulta" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>

<style>
    .preview {
        width: 98%;
        padding: 30PX;
    }
</style>
<asp:UpdatePanel runat="server" ID="upInfo">
    <ContentTemplate>
        <br>
        <h3 class="h6">
            <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
            / Editor de contenido / Nuevo artículo </h3>
        <hr />
        <section class="module">

            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <asp:Label runat="server" ID="lblTitle" class="col-lg-12 col-md-12 col-sm-12" />
                        <br />
                        <hr />
                        <div id="TextPreview" runat="server" class="preview"></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <hr />
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <asp:Repeater runat="server" ID="rptArchivos">
                            <ItemTemplate>
                                <div class="row">
                                    <span class=" col-lg-12 col-md-12 col-sm-12 fa fa-paperclip">
                                        <asp:Label runat="server" ID="lblFile" Text='<%# Eval("NombreArchivo")%>' />
                                    </span>
                                    <br />
                                    <asp:Label runat="server" ID="Label1" CssClass="col-lg-1 col-md-1 col-sm-1 " Text='<%# Eval("Tamaño")%>' />
                                    <asp:HyperLink runat="server" Text="Download" NavigateUrl='<%# ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioInformacionConsulta + "~" + Eval("NombreArchivo"))) %>'></asp:HyperLink>
                                    
                                    <asp:LinkButton runat="server" Text="Preview" CssClass="margin-left-5" OnClick="OnClick" CommandArgument='<%# Eval("NombreArchivo")%>'></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="row text-center" runat="server" id="divEvaluacion">
                <hr />
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        Calificar este Artículo<br />
                        <asp:LinkButton runat="server" ID="lbtnLike"> <asp:Image runat="server" ImageUrl="~/assets/images/like_S1.png"/></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lbtnNotLike"> <asp:Image runat="server" ImageUrl="~/assets/images/dontlike_S1.png"/></asp:LinkButton><br />

                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
