<%@ Page Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="FrmAgenteNuevoTicket.aspx.cs" Inherits="KiiniHelp.Agente.FrmAgenteNuevoTicket" %>

<%@ Register Src="~/UserControls/Altas/Formularios/UcFormulario.ascx" TagPrefix="uc1" TagName="UcFormulario" %>
<%@ Register Src="~/UserControls/Agentes/UcBusquedaFormulario.ascx" TagPrefix="uc1" TagName="UcBusquedaFormulario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfIdUsuarioSolicito" />
                <%--<div class="module">--%>
                <div class="row">

                    <div class="col-lg-9 col-md-9 col-sm-9">
                        <section class="module" style="height: 400px;">
                            <div class="row">
                                <div class="module-inner">
                                    <uc1:UcBusquedaFormulario runat="server" ID="ucBusquedaFormulario" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="module-inner" runat="server" id="divFormulario">
                                    <uc1:UcFormulario runat="server" ID="ucFormulario" />
                                </div>
                            </div>
                        </section>
                    </div>

                    <div class="col-lg-3 col-md-3 col-sm-3 no-padding-left">
                        <section class="module">
                            <div class="module-inner">
                                <div class="row">
                                    <div class="col-lg-2 col-md-12 col-sm-12">
                                        <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-square-1.png"/>
                                    </div>
                                    <div class="col-lg-10 col-md-12 col-sm-12">
                                        <asp:Label runat="server" ID="lblNombreDetalle" Text="gabriela Vega" CssClass="TitulosAzul"/><br />
                                        <span class="btn btn-square-usuario empleado btn-circle" style="padding-top: 6px;width: 23px;height: 23px;">
                                            <asp:Label runat="server" ID="lblTipoUsuarioDetalle"/></span>
                                        <asp:Image runat="server" ImageUrl="~/assets/images/icons/vip.png" Width="25px" ID="imgVip" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:Image runat="server" ImageUrl="~/assets/images/icons/ojo.png" Width="25px" CssClass="margin-right-15px"/>
                                        <asp:Label runat="server" Text="6 de abril 2017" ID="lblFechaUltimaconexion"/>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <span class="col-lg-2 col-md-2 col-sm-2 pe-icon pe-7s-ticket icon" style="font-size: 24px"></span>
                                        <div class="col-lg-10 col-md-10 col-sm-10">
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTicketUsuario" />
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Puesto" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblPuesto" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="correo electornico principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblCorreoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="telefono principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblTelefonoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row" style="display: none">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Facebook" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblFacebook" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>

                                <div class="row" style="display: none">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="twitter" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lbltwitter" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Organización" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblOrganizacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Ubicación" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Label runat="server" ID="lblUbicacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Etiquetas" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                        <asp:Repeater runat="server" ID="rptEtiquetas">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEtiquetas" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Creado" CssClass="col-lg-12 col-md-12 col-sm-12" /></strong>
                                    </div>
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:Label runat="server" ID="lblFechaAltaDetalle" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <strong>
                                            <asp:Label runat="server" Text="Ultima Actualizacion" CssClass="col-lg-12 col-md-12 col-sm-12" /></strong>
                                    </div>
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:Label runat="server" ID="lblfechaUltimaActualizacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                    </div>
                                </div>
                                <hr />
                            </div>
                        </section>
                    </div>


                </div>
                <%-- </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
