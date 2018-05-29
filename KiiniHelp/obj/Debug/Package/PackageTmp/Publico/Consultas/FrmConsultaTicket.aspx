<%@ Page Title="Consulta Ticket" Language="C#" MasterPageFile="~/Public.Master" ValidateRequest="false" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmConsultaTicket.aspx.cs" Inherits="KiiniHelp.Publico.Consultas.FrmConsultaTicket" %>

<%@ Register Src="~/UserControls/Detalles/UcTicketDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcTicketDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta" />
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                <li class="active">Consulta de tickets</li>
            </ol>
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12" runat="server" id="divTitle">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" Text="Consulta de tickets" /></h3>
                            </div>
                            <p>
                                Ingresa la siguiente información para consultar tu ticket. 
                            </p>
                            <hr />
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12" runat="server" id="divConsulta">
                            <div class="module-content collapse in" id="content-4">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtTicket">No. de ticket</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtTicket" onkeydown="return (event.keyCode!=13);" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="txtClave">Clave de registro</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox type="text" CssClass="form-control text-uppercase" runat="server" ID="txtClave" onkeydown="return (event.keyCode!=13);" />
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

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12" runat="server" id="divDetalle" visible="False">
                            <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div class="module-heading">
                                    <h3 class="module-title">Detalle del Ticket</h3>
                                </div>
                            </div>
                            <hr>
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12" runat="server" id="divDetalleTicket" visible="False">
                            <div class="module-content collapse in">
                                <div class="module-content-inner no-padding-bottom">
                                    <uc1:UcTicketDetalleUsuario runat="server" ID="ucTicketDetalleUsuario" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
