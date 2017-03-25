 <%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="kaldirirmi_polymer.user.profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="seo" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="style" runat="server">
    <link rel="import" href="../assets/components/kaldirirmi/vulcanized/kaldirirmi-profile-page-imports.vulcanized.html" async/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <kaldirirmi-profile-page></kaldirirmi-profile-page>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="script" runat="server">
</asp:Content>
    