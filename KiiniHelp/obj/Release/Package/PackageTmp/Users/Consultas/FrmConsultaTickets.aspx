<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaTickets.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaTickets" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosTicket.ascx" TagPrefix="uc1" TagName="UcFiltrosTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
                <div class="alert alert-danger">
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
                    <asp:Repeater runat="server" ID="rptErrorGeneral">
                        <ItemTemplate>
                            <ul>
                                <li><%# Container.DataItem %></li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </header>
            <div class="panel panel-primary">
                <div class="panel-body">
                    <uc1:UcFiltrosTicket runat="server" id="ucFiltrosTicket" />
                </div>
                <div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_OnClick" />
                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-body">
                    <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" OnRowCreated="gvResult_OnRowCreated"></asp:GridView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

