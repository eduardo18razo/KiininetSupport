<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusqueda.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcBusqueda" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" class="heigth100">
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
                                <asp:Label runat="server" ID="lblNumeroResultados" CssClass="totalResultados" Text="0 resultados para campo búsqueda" />
                                <br />
                                <div class="form col-xs-12 col-sm-12 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Por tipo:</label>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                            <asp:DropDownList runat="server" ID="ddlTipoArbol" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoArbol_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form col-xs-12 col-sm-12 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Por categoria</label>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                            <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoria_OnSelectedIndexChanged"/>
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
                                            <asp:Label runat="server" CssClass="text-theme" ID="lblIdOpcion" Visible="False" Text='<%# Eval("Id") %>' />
                                            <asp:LinkButton runat="server" CssClass="text-theme" Text='<%#Eval("Titulo") %>' CommandArgument='<%# Eval("Id") %>' ID="lnkBtnResult" OnClick="lnkBtnResult_OnClick"/>
                                            <i class="text-theme fa fa-thumbs-up"></i>
                                            <asp:Label runat="server" CssClass="text-theme" ID="lblLikes" Text='<%#Eval("TotalLikes") %>' />
                                            <br />
                                            <asp:Label runat="server" ID="lblDescripcion" Text='<%#Eval("Descripcion") %>' />
                                            <br />
                                            <br />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div class="col-lg-12 bg-grisMedio margin-top-30 text-right">
                                    <asp:Repeater runat="server" ID="rptPager">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CssClass="margin-right-4" ID="lnkPage" CommandArgument='<%# Container.DataItem.ToString() %>' Text='<%# Container.DataItem.ToString() %>' OnClick="lnkPage_OnClick"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

