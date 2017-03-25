<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="moderator.aspx.cs" Inherits="kaldirirmi_polymer.user.moderator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="seo" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="style" runat="server">
  <style>
    #mod-contents {
      display: flex;
    }

    .partition {
      width: 50%;
    }

    .user-ban {
      border: 1px solid white;
    }
    header{
      padding:15px;
      text-align:left;
      color:white;
      font-family:inherit;
      font-size:22px;
    }
  </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
  <link rel="import" href="../assets/components/kaldirirmi/moderator/kaldirirmi-mod-threads-approval.html" />
  <link rel="import" href="../assets/components/kaldirirmi/moderator/kaldirirmi-mod-user-ban.html" />

  <header>
    Moderator Sayfası
  </header>

  <div id="mod-contents">
    <div class="partition user-ban">
      <kaldirirmi-mod-threads-approval></kaldirirmi-mod-threads-approval>
    </div>
    <div class="partition user-ban">
      <kaldirirmi-mod-user-ban></kaldirirmi-mod-user-ban>
    </div>
    <%-- <div class="partition green">
        
    </div>--%>
  </div>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="script" runat="server">
</asp:Content>
