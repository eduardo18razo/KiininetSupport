<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDashboardHelpCenter.ascx.cs" Inherits="KiiniHelp.UserControls.Dashboards.UcDashboardHelpCenter" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <section class="module">
                <div class="module-inner">
                    <div class="row">
                        <label class="TitulosAzul">Dashboard</label>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="module-inner">
                            <div class="row">
                                <div>
                                    <asp:RadioButton runat="server" GroupName="tipoArbol" AutoPostBack="True" ID="rbtnConConsultas" Text="Consultas" CssClass="selectConver" OnCheckedChanged="rbtnConConsultas_OnCheckedChanged" Checked="True" />
                                    <asp:RadioButton runat="server" GroupName="tipoArbol" AutoPostBack="True" ID="rbtnServicios" Text="Servicios" CssClass="selectConver" OnCheckedChanged="rbtnServicios_OnCheckedChanged" />
                                    <asp:RadioButton runat="server" GroupName="tipoArbol" AutoPostBack="True" ID="rbtnproblemas" Text="Problemas" CssClass="selectConver" OnCheckedChanged="rbtnproblemas_OnCheckedChanged" />
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-2 col-md-2 col-sm-2">
                                        <asp:Label runat="server" Text="Periodo" />
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCambiarAsignar" AutoPostBack="True"  />
                                        <asp:LinkButton runat="server" ID="lnkBtndeshacer" CssClass="pe-icon pe-7s-lock icon" />
                                    </div>
                                </div>
                                <div class="row ">

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Categorías activas (total)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblUsuariosActivos" CssClass="h4">98%</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Servicios activos (total)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados" CssClass="h4">1,520</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets creados por servicios (total)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblOperadoresAcumulados" CssClass="h4">10</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tiempo de resolución (promedio)</label>
                                            <br />
                                            <asp:Label runat="server" ID="Label1" CssClass="h4">10</asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <br />

                                <div class="row">

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">

                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados por Canal</label>
                                                </div>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsCanal">
                                                </tc:RadHtmlChart>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Usuarios Registrados</label>
                                                </div>
                                                <tc:RadHtmlChart runat="server" ID="rhcUsuarios">
                                                </tc:RadHtmlChart>
                                            </div>
                                        </section>
                                    </div>

                                </div>

                                <%--Top 10--%>
                                <div class="row">
                                </div>


                            </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
