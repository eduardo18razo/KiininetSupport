<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusquedaFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Agentes.UcBusquedaFormulario" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .RadSearchBox {
        width: 100% !important;
    }

        .RadSearchBox .rsbSearchContext {
            width: 200px;
            height: 30px;
        }

    .RadAutoCompleteBoxPopup {
        width: 100% !important;
    }

    .racSlide {
        width: 100% !important;
    }

    .contextDropDown {
        background: #ecf0f1 !important;
        width: 200px !important;
    }

    .RadSearchBox .rsbInput{
        margin: 4px 3px;
    }

    .RadSearchBox .rsbSCFakeInput {
        height: 26px;
        padding-top: 4px;
    }

    .RadSearchBox .rsbButtonSearch {
        margin: 0 -2px;
        width: 22px;
        height: 26px;
    }

    .RadSearchBox_Default .rsbLoadingIcon{
        margin-top: 4px;
    }

    .RadSearchBox_Default .rsbInner.rsbFocused {
        border-color: #676767;
        color: #333;
        background-color: #fff;
    }

    .rsbPopup_Default .rsbListItemSelected {
        color: #333;
        background-color: #ecf0f1;
    }

    .rsbPopup_Default .rsbListItemHovered {
        color: #fff;
        background-color: #676767;
    }
</style>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdUsuarioSolicito" />
            <asp:HiddenField runat="server" ID="hfAreaSeleccionada" Value="0" />
            <%-- <section class="module">--%>
            <div class="row">
                <div class="module-inner">
                    <div class="col-lg-12 col-md-9 col-sm-9">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <tc:RadSearchBox runat="server" DataTextField="DescripcionTipificacion" OnSearch="txtBusquedaFormulario_OnSearch" DataValueField="Id" EnableAutoComplete="True" ShowSearchButton="True" ID="txtBusquedaFormulario" DataSourceID="SourceOpciones"
                                    OnDataSourceSelect="txtBusquedaFormulario_OnDataSourceSelect" Width="90%" DropDownSettings-Height="41px">
                                    <SearchContext DataSourceID="SourceAreasSearch" DataTextField="Descripcion" DataKeyField="Id" DropDownCssClass="contextDropDown" ShowDefaultItem="False" />
                                </tc:RadSearchBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%-- </section>--%>

            <asp:ObjectDataSource runat="server" ID="SourceOpciones" TypeName="KiiniHelp.UserControls.Agentes.BusquedaOpciones" SelectMethod="GetOptions">
                <SelectParameters>
                    <asp:QueryStringParameter Name="idUsuarioSolicita" QueryStringField="idUsuarioSolicita" />
                    <asp:QueryStringParameter Name="idUsuarioLevanta" QueryStringField="idUsuarioLevanta" />
                    <asp:Parameter Name="idArea" Type="Int32" ConvertEmptyStringToNull="True" />
                    <asp:Parameter Name="insertarSeleccion" Type="Boolean" DefaultValue="false" />
                    <asp:Parameter Name="keys" Type="String" DefaultValue="" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource runat="server" ID="SourceAreasSearch" TypeName="KiiniHelp.UserControls.Agentes.BusquedaOpciones"
                SelectMethod="GetAreas">
                <SelectParameters>
                    <asp:QueryStringParameter Name="idUsuarioSolicito" QueryStringField="idUsuarioSolicitante" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
