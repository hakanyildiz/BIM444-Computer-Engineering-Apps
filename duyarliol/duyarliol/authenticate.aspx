<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="authenticate.aspx.cs" Inherits="duyarliol.authenticate" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="tr" xml:lang="tr" dir="ltr">
<head runat="server">
  <meta charset="utf-8" />
  <meta class="viewport" name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
  <meta name="mobile-web-app-capable" content="yes">
  <meta name="apple-mobile-web-app-capable" content="yes">
  <meta name="robots" content="noindex,nofollow" />

  <title>duyarli.ol</title>

  <style is="custom-style">
    * {
      @apply(--paper-font-common-base);
    }

    body {
      -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
    }

    paper-header-panel {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: #222;
    }

    paper-toolbar[first] {
     background-color: #171059;
                  color: #fff;
                  box-shadow: 0 16px 24px 2px rgba(0, 0, 0, 0.14), 0 6px 30px 5px rgba(0, 0, 0, 0.12), 0 8px 10px -5px rgba(0, 0, 0, 0.4);
    }

      paper-toolbar[first] .logo {
        flex-grow: 2;
        @apply(--layout-horizontal);
        @apply(--layout-center);
        height: 50px;
      }

        paper-toolbar[first] .logo iron-image#mainlogo {
          width: 160px;
          height: 100%;
          cursor: pointer;
        }

      paper-toolbar[first] paper-icon-button {
        --paper-icon-button-ink-color: #ff7800;
        color: #ff7800;
      }

    paper-material {
      position: relative;
      display: block;
      max-width: 450px;
      margin: 30px auto 0 auto;
    }

      paper-material paper-tabs {
        color: white;
        background: #ff7800;
        --paper-tabs-selection-bar-color: white;
        font-size: 17px;
      }

      paper-material iron-pages {
        background: #fff;
      }

        paper-material iron-pages form {
          @apply(--layout-vertical);
          padding: 24px;
        }

          paper-material iron-pages form p {
            font-size: 14px;
          }

            paper-material iron-pages form p a {
              color: #ff7800;
              text-decoration: none;
            }

          paper-material iron-pages form .buttons {
            margin-top: 20px;
            @apply(--layout-horizontal);
            @apply(--layout-end-justified);
          }

            paper-material iron-pages form .buttons paper-button:last-child {
              background: #ff7800;
              color: white;
            }
  </style>

   <script src="assets/js/masterpage.js"></script>
    <link rel="import" href="assets/components/kaldirirmi/vulcanized/do-all-imports.html"/>
</head>
<body class="fullbleed layout vertical">
  <paper-header-panel class="flex">
    <paper-toolbar first>
    <%--  <div class="logo">
        <iron-image id="mainlogo" src="/assets/img/logo.png" sizing="cover"></iron-image>
      </div>--%>
        <div>Duyarlı.Ol</div>
    </paper-toolbar>
    <div id="app" class="fit">
      <paper-material id="authform" elevation="1">
        <paper-tabs id="methods" selected="0">
          <paper-tab>Yeni Hesap Oluştur</paper-tab>
          <paper-tab>Kayıtlı Hesabıma Bağla</paper-tab>
        </paper-tabs>
        <iron-pages id="method" selected="0">
          <div id="new">
            <form id="newform" action="/auth/<%= method %>/confirm" method="post">
              <input type="hidden" name="method" value="new" />
              <input type="hidden" name="email" value="<%= email %>" />
               <input type="hidden" name="accesstoken" value="<%= accesstoken %>" />
               <paper-input id="newusername" label="Kullanıcı Adı" name="username" type="text" required auto-validate char-counter minlength="6" maxlength="16" error-message="minimum 6 karakter olmalı"></paper-input>
              <paper-input id="newpassword" label="Şifre" name="password" type="password" required auto-validate char-counter minlength="8" maxlength="12" error-message="minimum 8 karakter olmalı"></paper-input>
              <div class="buttons">
                <paper-button onclick="location.href='/'">Vazgeç</paper-button>
                <paper-button id="newsubmit" raised>Kayıt</paper-button>
              </div>
            </form>
          </div>
          <div id="link">
            <form id="linkform" action="/auth/<%= method %>/confirm" method="post">
              <input type="hidden" name="method" value="link" />
              <input type="hidden" name="email" value="<%= email %>" />
              <input type="hidden" name="accesstoken" value="<%= accesstoken %>" />
              <paper-input id="linkusername" label="Kullanıcı Adı" name="username" type="text" required auto-validate error-message="bu alanın doldurulması zorunludur"></paper-input>
              <paper-input id="linkpassword" label="Şifre" name="password" type="password" required auto-validate error-message="bu alanın doldurulması zorunludur"></paper-input>
              <div class="buttons">
                <paper-button onclick="location.href='/'">Vazgeç</paper-button>
                <paper-button id="linksubmit" raised>Giriş</paper-button>
              </div>
            </form>
          </div>
        </iron-pages>
      </paper-material>
    </div>
  </paper-header-panel>
  <script>
    document.getElementById('methods').addEventListener('iron-select', function () {
      document.getElementById('method').select(this.selected);
    });

    if (location.hash == '#new') document.getElementById('methods').select(0);
    else if (location.hash == '#link') document.getElementById('methods').select(1);

    document.getElementById('newsubmit').addEventListener('click', function (e) {
      if (document.getElementById('newusername').validate() && document.getElementById('newpassword').validate()) {
        document.getElementById('newsubmit').disabled = true;
        document.getElementById('newform').submit();
      }
    });
    document.getElementById('linksubmit').addEventListener('click', function (e) {
      if (document.getElementById('linkusername').validate() && document.getElementById('linkpassword').validate()) {
        document.getElementById('linksubmit').disabled = true;
        document.getElementById('linkform').submit();
      }
    });
  </script>
  <%
    if (Session["auth-message"] != null)
    {
  %>
  <paper-toast id="alert" text="<%= Session["auth-message"].ToString() %>" opened duration="5000"></paper-toast>
  <%
      Session["auth-message"] = null;
    }
  %>
</body>
</html>
