<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaHits.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaHits" %>
<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosConsulta.ascx" TagPrefix="uc1" TagName="UcFiltrosConsulta" %>
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
                    <uc1:UcFiltrosConsulta runat="server" ID="UcFiltrosConsulta" />
                </div>
                <div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_OnClick" />
                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-body">
                    <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" ></asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
