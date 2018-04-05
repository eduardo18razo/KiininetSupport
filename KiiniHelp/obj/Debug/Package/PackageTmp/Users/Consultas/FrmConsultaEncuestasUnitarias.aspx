<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaEncuestasUnitarias.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaEncuestasUnitarias" %>
<%@ Page MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosEncuestas.ascx" TagPrefix="uc1" TagName="UcFiltrosEncuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
                <div class="alert alert-danger">
                    <div>
                        <div class="float-left">
                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                        </div>
                        <div class="float-left">
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
            <uc1:UcFiltrosEncuestas runat="server" ID="ucFiltrosEncuestas" />                
                <%--<div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_OnClick" />
                </div>--%>           
            <div class="panel panel-primary">
                <div class="panel-body">
                    <asp:Panel runat="server" ScrollBars="Both" Width="100%" Height="350px">
                    <div style="overflow-x: auto">
                        <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" OnRowCreated="gvResult_OnRowCreated">
                        </asp:GridView>
                    </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
