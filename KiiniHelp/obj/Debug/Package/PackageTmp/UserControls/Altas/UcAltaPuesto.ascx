<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaPuesto.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaPuesto" %>
<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdPuesto" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server" Text='&times;'/>
            <%--<h2 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBranding" /></h2>--%>
            <h6 class="text-center">
                <asp:Label runat="server" ID="lblOperacion" Font-Bold="true"/>          
                <hr />                  
            </h6>
        </div>
        <div class="modal-body">
            
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    Tipo de usuario
                   
                            <br />
                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" />
                    </div>
                </div>
            </div>
           <%-- <hr />--%>

            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="bg-grey">
                            <h3 class="text-left">Editar Puesto</h3>
                            <hr />
                            <div class="form-group margin-top">
                                Escribe el nombre del puesto*<br />
                                <asp:TextBox runat="server" ID="txtDescripcionPuesto" CssClass="form-control" onkeydown="return (event.keyCode!=13);" MaxLength="50"/>
                            </div>
                            <p class="margin-top-40">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                            </p>
                        </div>
                        <p class="text-right margin-top-40">
                            <asp:Button runat="server" CssClass="btn btn-success" Text="Terminar" ID="btnTerminar" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
