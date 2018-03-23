<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaTiempoEstimado.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaTiempoEstimado" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbol"/>
        <header id="panelAlert" runat="server" visible="False">
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
                <asp:Repeater runat="server" ID="rptHeaderError">
                    <ItemTemplate>
                        <%# Container.DataItem %>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>

        <div class="panel panel-primary">
            <div class="panel-heading">
                Tiempo de Notificación
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <table class="table table-responsive" id="tblHeader">
                            <thead>
                                <tr align="center">
                                    <td>
                                        <asp:Label runat="server">Notificar</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Grupo</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Dias</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Horas</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Minutos</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Segundos</asp:Label></td>
                                    <td>
                                        <asp:Label runat="server">Via</asp:Label></td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox runat="server" ID="chkDueno" AutoPostBack="True" OnCheckedChanged="chkDueno_OnCheckedChanged" CssClass="input" Text=" " />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="DUEÑO DEL SERVICIO" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDuenoDias" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDuenoHoras" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDuenoMinutos" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDuenoSegundos" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="DropSelect" ID="ddlDuenoVia" Enabled="False" /></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox runat="server" ID="chkMtto" AutoPostBack="True" OnCheckedChanged="chkMtto_OnCheckedChanged" Text=" " />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="RESPONSABLE DE MTTO" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtMttoDias" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtMttoHoras" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtMttoMinutos" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtMttoSegundos" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="DropSelect" ID="ddlMttoVia" Enabled="False" /></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <label for="chkDesarrollo"></label>
                                        <asp:CheckBox runat="server" ID="chkDesarrollo" AutoPostBack="True" OnCheckedChanged="chkDesarrollo_OnCheckedChanged" Text=" " />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="RESPONSABLE DESARROLLO" /></td>
                                    <%--onkeyup="sum();" --%>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDesarrolloDias" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtDesarrolloHoras" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" CssClass="form-control" Enabled="False" ID="txtDesarrolloMinutos" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" CssClass="form-control" Enabled="False" ID="txtDesarrolloSegundos" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="DropSelect" ID="ddlDesarrolloVia" Enabled="False" /></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox runat="server" ID="chkConsulta" AutoPostBack="True" OnCheckedChanged="chkConsulta_OnCheckedChanged" Text=" " /></td>
                                    <td>
                                        <asp:Label runat="server" Text="ESPECIAL DE CONSULTA" /></td>
                                    <%--onkeyup="sum();" --%>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtConsultaDias" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtConsultaHoras" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtConsultaMinutos" /></td>
                                    <td>
                                        <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtConsultaSegundos" /></td>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="DropSelect" ID="ddlConsultaVia" Enabled="False" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="panel-footer text-center">
                <asp:Button runat="server" CssClass="btn btn-success btn-sm" Text="Aceptar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger btn-sm" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger btn-sm" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
