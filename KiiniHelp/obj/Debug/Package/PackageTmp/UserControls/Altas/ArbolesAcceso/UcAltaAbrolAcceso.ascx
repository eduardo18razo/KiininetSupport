﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaAbrolAcceso.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcAltaAbrolAcceso" %>
<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcAltaConsulta.ascx" TagPrefix="uc1" TagName="UcAltaConsulta" %>
<%@ Register Src="~/UserControls/Altas/ArbolesAcceso/UcAltaServicio.ascx" TagPrefix="uc1" TagName="UcAltaServicio" %>


<asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbol" />
        <asp:HiddenField runat="server" ID="hfIdTipoArbol" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h2 class="modal-title" id="modal-new-ticket-label">
                <br />
                ALTA DE OPCIÓN</h2>
        </div>
        <div class="modal-body">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row" style="margin-top: -30px">
                        <div class="module-content-inner">
                            <div class="faq-section text-center margin-bottom-lg">
                                <div class="faqs-tabbed tabpanel" role="tabpanel">
                                    <ul class="nav nav-tabs nav-tabs-theme-3 margin-bottom-lg" role="tablist">
                                        <li role="presentation" class="active">
                                            <a href="#tabAltaConsulta" aria-controls="tabAltaConsulta" role="tab" data-toggle="tab"><span class="pe-icon fa fa-group icon"></span>
                                                <br>
                                                Consulta</a>
                                        </li>
                                        <li role="presentation">
                                            <a href="#tabAltaForma" aria-controls="tabAltaForma" role="tab" data-toggle="tab"><span class="pe-icon fa fa-clock-o icon"></span>
                                                <br>
                                                Servicio/Problema</a>
                                        </li>
                                    </ul>

                                    <div class="tab-content text-left">
                                        <div role="tabpanel" class="tab-pane tab-pane fade in active" id="tabAltaConsulta">
                                            <div class="row">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <uc1:UcAltaConsulta runat="server" id="UcAltaConsulta" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <div role="tabpanel" class="tab-pane tab-pane fade in" id="tabAltaForma">
                                            <div class="row">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <uc1:UcAltaServicio runat="server" id="UcAltaServicio" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
