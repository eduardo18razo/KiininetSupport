<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaEncuestasUnitarias.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaEncuestasUnitarias" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosEncuestas.ascx" TagPrefix="uc1" TagName="UcFiltrosEncuestas" %>

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
                    <uc1:UcFiltrosEncuestas runat="server" ID="ucFiltrosEncuestas" />
                </div>
                <div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_OnClick" />
                </div>
            </div>
            <div class="panel panel-primary">
                <div class="panel-body">
                    <div style="overflow-x: auto">
                        <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" OnRowCreated="gvResult_OnRowCreated"></asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
