<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmTicket.aspx.cs" Inherits="KiiniHelp.Users.Ticket.FrmTicket" %>
<%@ Register Src="~/UserControls/Temporal/UcMascaraCaptura.ascx" TagPrefix="uc1" TagName="UcMascaraCaptura" %>
<%@ Register Src="~/UserControls/Genericos/UcInformacionConsulta.ascx" TagPrefix="uc1" TagName="UcInformacionConsulta" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <header>
                <div class="alert alert-danger" id="panelAlert" runat="server" visible="False">
                    <div>
                        <div style="float: left">
                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                        </div>
                        <div style="float: left">
                            <h3>Error</h3>
                        </div>
                        <div class="clearfix clear-fix" />
                    </div>
                    <hr />
                    <asp:Repeater runat="server" ID="rptHeaderError">
                        <ItemTemplate>
                            <%# Container.DataItem %>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </header>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:HiddenField runat="server" ID="hfIdMascara" />
                    <asp:HiddenField runat="server" ID="hfIdConsulta" />
                    <asp:HiddenField runat="server" ID="hfIdEncuesta" />
                    <asp:HiddenField runat="server" ID="hfIdSla" />
                    <asp:HiddenField runat="server" ID="hfIdCanal" />
                    <asp:HiddenField runat="server" ID="hfIdUsuarioSolicita" />
                    <asp:Label runat="server" ID="lblTicketDescripcion"></asp:Label>
                </div>

                <div class="panel-body">
                    <uc1:UcInformacionConsulta runat="server" ID="UcInformacionConsulta" />
                    <uc1:UcMascaraCaptura runat="server" ID="UcMascaraCaptura" />

                </div>
                <div class="panel-footer">
                    <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" Text="Guardar" CssClass="btn btn-lg btn-success" />
                    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-lg btn-danger" OnClick="btnCancelar_OnClick"/>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalExito" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upConfirmacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h1>Generación de Ticket</h1>
                            </div>
                            <div class="panel.panel-body">
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Se ha generado correctamente el ticket No.:" />
                                    <asp:TextBox runat="server" ID="lblNoTicket" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" Text="Con clave:" ID="lblDescRandom" />
                                    <asp:TextBox runat="server" ID="lblRandom" CssClass="form-control" ReadOnly="True" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrar" Text="Cerrar" OnClick="btnCerrar_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
