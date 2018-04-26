<%@ Page Title="Consulta Ticket" Language="C#" MasterPageFile="~/Public.Master" ValidateRequest="false" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmConsultaTicket.aspx.cs" Inherits="KiiniHelp.Publico.Consultas.FrmConsultaTicket" %>

<%@ Register Src="~/UserControls/Detalles/UcTicketDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcTicketDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderPublic" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta" />
            <div>
                <ol class="breadcrumb">
                    <li>
                        <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></li>
                    <li class="active">Consulta de tickets</li>
                </ol>
            </div>
            <div runat="server" id="divConsulta">
                <div class="clearfix"></div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div>
                            <section class="module module-no-heading">
                                <div class="module-inner">
                                    <div class="module-heading no-border">
                                        <h4 class="title">Consulta de tickets</h4>
                                        <hr>
                                        <p class="text-primary">Ingresa la siguiente información para consultar tu ticket.</p>
                                        <hr>
                                    </div>
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
                            </section>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row" runat="server" id="divDetalle" Visible="False">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div>
                        <section class="module module-no-heading">
                            <div class="module-inner">
                                <div class="module-heading no-border">
                                    <h4 class="title">Detalle del Ticket</h4>
                                    <hr>
                                </div>
                                <div class="module-content collapse in">
                                    <div class="module-content-inner no-padding-bottom">
                                        <uc1:UcTicketDetalleUsuario runat="server" ID="ucTicketDetalleUsuario" />
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
