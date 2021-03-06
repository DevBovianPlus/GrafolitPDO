﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="SearchProduct_popup.aspx.cs" Inherits="GrafolitPDO.Pages.Order.SearchProduct_popup" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        
        var postbackInitiated = false;

        function ActionButton_Click(s, e) {
            var process = true

            if (process) {
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
            }
            else
                e.processOnServer = false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <div class="row m-0 pb-3">
        <div class="col-lg-12 mb-2 p-0">
            <div class="row m-0 align-items-center">
                <div class="col p-0">

                    <dx:ASPxGridView ID="ASPxGridViewProductSearchResult" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridProductSearchResult"
                        OnDataBinding="ASPxGridViewProductSearchResult_DataBinding" Width="100%" OnDataBound="ASPxGridViewProductSearchResult_DataBound"
                        KeyFieldName="TempID" CssClass="gridview-no-header-padding">
                        <Paddings Padding="0" />
                        <Settings ShowVerticalScrollBar="True" AutoFilterCondition="Contains" HorizontalScrollBarMode="Auto" 
                            ShowFilterBar="Auto" ShowFilterRow="true" VerticalScrollableHeight="380" UseFixedTableLayout="true"
                            ShowFilterRowMenu="true" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                        <SettingsPager PageSize="50" ShowNumericButtons="true">
                            <PageSizeItemSettings Visible="false" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                            </PageSizeItemSettings>
                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                        </SettingsPager>    
                        <SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="true" />
                        <Styles Header-Wrap="True">
                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                        </Styles>
                        <SettingsText EmptyDataRow="Ni zadetkov za iskane dobavitelje." />
                        <SettingsBehavior AllowEllipsisInText="true" AllowSelectSingleRowOnly="true" />
                        <Columns>

                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />

                            <dx:GridViewDataTextColumn Caption="ID" FieldName="TempID" Visible="false" SortOrder="Descending"  />

                            <dx:GridViewDataTextColumn Caption="Stevilka" FieldName="StevilkaArtikel" Width="130px" />
                            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="300" />
                            <dx:GridViewDataTextColumn Caption="Kategorija" FieldName="Kategorija" Width="100" />
                            <dx:GridViewDataTextColumn Caption="Gloss" FieldName="Gloss" Width="130" />
                            <dx:GridViewDataTextColumn Caption="Gramatura" FieldName="Gramatura" Width="50" />
                            <dx:GridViewDataTextColumn Caption="Velikost" FieldName="Velikost" Width="50" />
                            <dx:GridViewDataTextColumn Caption="Tek" FieldName="Tek" Width="50" />
                            <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="Dobavitelj" Width="250" />

                        </Columns>
                    </dx:ASPxGridView>

                </div>
            </div>
        </div>
    </div>

    <div class="row m-0 pb-3 ">
        <div class="col-lg-12 mb-2 mb-lg-0">
            <div class="row m-0 align-items-center justify-content-end">
                <div class="col-0 p-0 pr-2">
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                        Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                        <Image Url="../../Images/cancelPopup.png" />
                    </dx:ASPxButton>
                </div>
                <div class="col-0 p-0">
                    <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                        Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnConfirm_Click" ClientInstanceName="btnConfirm">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                        <Image Url="../../Images/addPopup.png" />
                        <ClientSideEvents Click="ActionButton_Click" />
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
