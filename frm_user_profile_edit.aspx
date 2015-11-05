<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_user_profile_edit.aspx.cs"
    Inherits="resources_javascript_frm_user_profile_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../resources/css/online/profile_edit.css" rel="stylesheet" type="text/css" />
    <link href="/resources/css/online/facebook_canvas.css" rel="stylesheet" type="text/css" />
    <link href="/resources/css/online/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/resources/css/online/animate.css" rel="stylesheet" type="text/css" />
    <link href="/resources/css/online/hedgeem_popup_message.css" rel="stylesheet" type="text/css" />
    <script src="//code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showpreview(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#profile_image').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }

        }

    </script>
    <script type="text/javascript">
        function SetSession1(src) {

            document.getElementById('profile_image').src = src;
            document.getElementById('profile_image').style.display = 'block';
            document.getElementById('fileupload').style.display = 'block';
            //Code to Set selected image URL in session and display selected image in Image tag. 
            $.ajax({

                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "frm_user_profile_edit.aspx/SetSession?src=" + src,
                dataType: "json",
                data: "{}",
                async: true,
                cache: true,

                success: function (data) {

                    debugger
                    var a = data.d;
                    //                    document.getElementById('profile_image').src = "";
                    document.getElementById('profile_image').src = a;

                    //                    var x = document.createElement("IMG");
                    //                    x.setAttribute("src", "resources/avitars/player_avatar_Patrick.jpg");
                    //                    x.setAttribute("width", "50");
                    //                    x.setAttribute("Id", "GGN");
                    //                    x.setAttribute("Height", "50");
                    //                    x.setAttribute("alt", "No Image");
                    //                    debugger;
                    //                    $("#edit_profile").find("form").append(x);
                    //                    var html = $("#edit_profile").find("form");
                    //                    var article = $("#edit_profile").find("article");
                    //                    $("#form44").remove();
                    //                   // $("#edit_profile").append(article);
                    //                    $("#edit_profile").append(html);

                    // document.getElementById('profile_image').src = src;

                    //       debugger
                    // var a = data.d;
                    //       var a = data.d + "?" + Math.random();
                    //                    console.log(a);
                    //                    $("#profile_image").attr("src", "");
                    //                    $("#profile_image").attr("src", a.toString());
                    //  $("#profile_image").attr("src", "");

                    //  document.getElementById('profile_image').src = "";

                    //                    var x = document.getElementById("profile_image");

                    //                    alert("555" + x);
                    //                    x.setAttribute("src", a);


                    //                    document.getElementById('profile_image').src = a;

                    //                    document.getElementById('profile_image').style.display = 'block';
                    //                    document.getElementById('fileupload').style.display = 'block';

                },
                error: function (er) {
                    // alert(er.toString());

                }
            });
        }
        function OnSuccess(response) {

            alert(response.d);
        }
        function onclose() {

            document.getElementById('popup_message').style.display = 'none'; document.getElementById('hide_login').style.display = 'none';
        }
    </script>
</head>
<body>
    <div class="editprofile">
        <article>
 <div class="modal-header">
                        <div id="hide_login" class="close" onclick=" document.getElementById('edit_profile').style.display = 'none';document.getElementById('hide_login').style.display = 'none';">
                            <%--<span aria-hidden="true">×</span>--%>
                             <a href="frm_facebook_canvas.aspx?signout=true">×</a>

                        </div>
                        <h4 id="myModalLabel" class="modal-title">
                            Edit Profile</h4>
                    </div>
    <form id="form1" runat="server">
    <ul style="padding-left:0;">
        <li>
            <label for="email">
                Username:</label>
            <asp:Label ID="lbl_username" runat="server"></asp:Label></li>
        <li>
            <label for="name">
                Display Name:</label>
            <asp:Label ID="lbl_name" runat="server"></asp:Label></li>
        <li>
            <label for="file">
                Profile Picture:</label>
            <div class="uploader" id="fileupload">
                <input type="file" style="opacity: 0;display:none;">
                <!--<span class="filename" style="-moz-user-select: none;">No file selected</span> <span
                    class="action" style="-moz-user-select: none;">Choose File</span>-->
                <asp:FileUpload ID="file_profile_picture" runat="server" onchange="document.getElementById('avitar_dl').style.display = 'none'; document.getElementById('profile_image').style.display = 'block'; showpreview(this);" /></div>
            <asp:Image ID="profile_image" runat="server" Height="50px" Width="50px" /></li>
        <li id="avitar_dl">
            <label style="display:block" for="avitar">
                Or Select any avitar:</label>
            <asp:Repeater ID="rp_available_avitars" runat="server">
                <ItemTemplate>
                    <img src="<%# Eval("Name","resources/avitars/{0}") %>" alt="img" width="50px" height="50px"
                        onclick="SetSession1(this.src);" />
                </ItemTemplate>
            </asp:Repeater>
        </li>
        <li>
            <label for="theme">
                Select Table Theme:</label>
            <asp:DropDownList ID="ddl_table_themes" runat="server">
            </asp:DropDownList>
        </li>
        <li id="update_button">
            <asp:Button ID="btn_update" CssClass="btn btn-success" runat="server" Text="Update" OnClick="btn_update_Click" />
        </li>
    </ul>
    </form>
    </div>
    <div id="popup_message">
        <asp:PlaceHolder ID="Place_Holder_Popup_Message" runat="server"></asp:PlaceHolder>
    </div>
</body>
<script type="text/javascript">
    function hide_userprofile_edit() {
        document.getElementById('edit_profile').style.display = 'none';
        document.getElementById('hide_login').style.display = 'none';
        return;
    }
</script>
</html>
