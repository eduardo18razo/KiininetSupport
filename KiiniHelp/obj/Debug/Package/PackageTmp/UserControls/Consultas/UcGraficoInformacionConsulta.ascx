<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcGraficoInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcGraficoInformacionConsulta" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Help Center</li>
                <li class="active">Artículos</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Grafico Like Dont Like" /></h3>
                            </div>
                        </div>

                    </div>
                </div>
            </section>


            <section class="module module-headings">
                <div class="module-inner">

                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <tc:RadHtmlChart runat="server" ID="rhcLikeBarra"></tc:RadHtmlChart>
                            <tc:RadHtmlChart runat="server" ID="rhcLikePie"></tc:RadHtmlChart>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
