<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmConsultaEncuestasUnitarias.aspx.cs" Inherits="KiiniHelp.Users.Consultas.FrmConsultaEncuestasUnitarias" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/Filtros/Consultas/UcFiltrosEncuestas.ascx" TagPrefix="uc1" TagName="UcFiltrosEncuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="pnlResult">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Reportes</li>
                <li class="active">Encuestas</li>
            </ol>
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="form-group">
                                <uc1:UcFiltrosEncuestas runat="server" ID="ucFiltrosEncuestas" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section class="module module-headings">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">
                                <asp:GridView runat="server" ID="gvResult" AllowPaging="true" Width="99%" OnRowCreated="gvResult_OnRowCreated"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-bordered table-hover table-responsive">
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
