<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCategoria.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcCategoria" %>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li class="text-theme">
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx" Text="text-theme">Home</asp:HyperLink></li>
                <li class="text-theme">
                    <asp:LinkButton runat="server" ID="lnkbtnTipoUsuario" OnClick="lbTipoUsuario_Click" CssClass="text-theme" />
                </li>
                <li class="active">Categorías</li>
            </ol>
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading ">
                                <h3 class="module-title">Categorias</h3>
                            </div>
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <dl>
                                <asp:Repeater runat="server" ID="rptAreas">
                                    <ItemTemplate>
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                            <dt class="h4 text-theme">
                                                <asp:LinkButton runat="server" CommandArgument='<%#Eval("Id") %>' ID="btnAreaSelect" OnClick="btnAreaSelect_OnClick"><%# Eval("Descripcion") %></asp:LinkButton>
                                            </dt>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <hr>
                            </dl>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
