<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="firstinit.aspx.cs" Inherits="duyarliol.firstinit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
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
    </style>
    <script src="assets/js/masterpage.js"></script>
    <link rel="import" href="assets/components/kaldirirmi/vulcanized/do-all-imports.html" />
    <link rel="import" href="assets/duyarliol/do-firstinit.html" async/>
     <link rel="import" href="assets/components/money-input/money-input.html" async/>

</head>
<body>
        <do-firstinit uid=<%= uid %>></do-firstinit>
</body>
</html>
