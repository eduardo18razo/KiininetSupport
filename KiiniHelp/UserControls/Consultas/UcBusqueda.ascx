<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusqueda.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcBusqueda" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Inicio</asp:HyperLink></li>
                <li class="breadcrumb-item">Resultados de busqueda</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Resultados" /></h3>
                            </div>
                            <p>
                                <asp:Label runat="server" ID="lblNumeroResultados" Text="1,25769 resultados para campo busqueda"></asp:Label>
                                <br />
                                <div class="form col-lg-5">
                                    <div class="form-group">
                                        <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Por tipo:</label>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                            <asp:DropDownList runat="server" ID="ddlTipoArbol" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 separador-vertical-derecho">
                                    <div class="form-group">
                                        <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Por categoria</label>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                            <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" />
                                        </div>
                                    </div>
                                </div>
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            <section class="module module-headings">
                <div class="module-inner">

                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">
                                <asp:Repeater runat="server" ID="rptResults">
                                    <ItemTemplate>
                                        <div class="row">
                                            <asp:Label runat="server" ID="lblIdOpcion" Visible="False" Text='<%# Eval("Id") %>'></asp:Label>
                                            <asp:Label runat="server" ID="lblTitulo" Text='<%#Eval("Titulo") %>'></asp:Label>
                                            <asp:Image runat="server" ID="imgLike" ImageUrl="~/assets/images/like_S1.png"></asp:Image>
                                            <asp:Label runat="server" ID="lblLikes" Text='<%#Eval("TotalLikes") %>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblDescripcion" Text='<%#Eval("Descripcion") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Repeater runat="server" ID="rptPager">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkPage" CommandArgument='<%# Container.DataItem.ToString() %>' Text='<%# Container.DataItem.ToString() %>' OnClick="lnkPage_OnClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

