<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCategoria.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcCategoria" %>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <%-- <br>--%>
            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">
                    <asp:LinkButton runat="server" ID="lbTipoUsuario" OnClick="lbTipoUsuario_Click">
                    </asp:LinkButton>
                </li>
                <li class="breadcrumb-item active">Categorías</li>
            </ol>
            <div class="row">
                <div class="module-wrapper col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <h3>Categorias</h3>
                                        <hr>

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
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
