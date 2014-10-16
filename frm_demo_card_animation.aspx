<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_demo_card_animation.aspx.cs"
    Inherits="demo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Card Animation</title>
    <link href="../resources/css/online/card_animation.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="stage">
        <!-- -->
        <div id="buttons">
            <input type="button" value="Deal" id="deal" />
            <input type="button" value="Reset" id="reset" />
        </div>
    </div>
    <script type="text/javascript" src="../resources/javascript/card_animation.js"></script>
    <script type="text/javascript">        var game = new CardGame("stage"); </script>
    </form>
</body>
</html>
