﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcVisorConsultainformacion.ascx.cs" Inherits="KiiniHelp.UserControls.Preview.UcVisorConsultainformacion" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>

<style>
    .preview {
        width: 98%;
        padding: 30PX;
    }
    .dontlike {
        transform: scale(0.5);
    }
</style>
<asp:UpdatePanel runat="server" ID="upInfo">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbolAcceso" />
        <asp:HiddenField runat="server" ID="hfIdInformacinConsulta" />
        <asp:HiddenField runat="server" ID="hfIdTipoUsuario" />
        <asp:HiddenField runat="server" ID="hfEvaluacion" />
        <asp:HiddenField runat="server" ID="hfMeGusta" />
        <section class="module">
            <div class="row">
                <div class="module-inner">
                    <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                        <div class="module-heading">
                            <h3 class="module-title18px">
                                <asp:Label runat="server" ID="lblTitle" />
                            </h3>
                        </div>
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
                                <div class="row margin-top-10" runat="server">
                                    <span class=" col-lg-12 col-md-12 col-sm-12 fa fa-paperclip">
                                        <asp:Label runat="server" ID="lblFile" Text='<%# Eval("Archivo")%>' />
                                    </span>
                                    <br />
                                    <asp:HyperLink runat="server" Enabled='<%# BusinessFile.ExisteArchivo(BusinessVariables.Directorios.RepositorioInformacionConsulta + Eval("Archivo")) %>' Text="Download" NavigateUrl='<%# ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioInformacionConsulta + "~" + Eval("Archivo"))) %>'></asp:HyperLink>
                                    <asp:LinkButton runat="server" Enabled='<%# BusinessFile.ExisteArchivo(BusinessVariables.Directorios.RepositorioInformacionConsulta + Eval("Archivo")) %>' Text="Preview" ID="btnPreviewDocument" CssClass="margin-left-5" OnClick="btnPreviewDocument_OnClick" CommandArgument='<%# Eval("Archivo") %>'></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="row text-center" runat="server" id="divEvaluacion">
                    <hr />
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            Calificar este Artículo<br />
                            <asp:LinkButton runat="server" ID="lbtnLike" OnClick="lbtnLike_OnClick">
                                <asp:Image runat="server" ID="imgLike" ImageUrl="~/assets/images/like_S1.png" />
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lbtnNotLike" OnClick="lbtnNotLike_OnClick">
                                <asp:Image runat="server" ID="imgDontLike" ImageUrl="~/assets/images/dontlike_S1.png" />
                            </asp:LinkButton><br />

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
