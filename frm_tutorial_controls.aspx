<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_tutorial_controls.aspx.cs"
    Inherits="frm_tutorial_controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="shortcut icon" href="http://designhuntr.com/wp-content/themes/DesignHuntR/images/favicon.ico" />
    <link rel="stylesheet" type="text/css" href="http://designhuntr.com/wp-content/themes/DesignHuntR/demoStyle.css" />
    <link rel="stylesheet" type="text/css" href="style.css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <style type="text/css">
        #popup_window
        {
        }
        .popup-overlay
        {
            z-index: 99;
            width: 100%;
            height: 100%;
            position: fixed;
            background: rgba(196, 196, 196, .85);
            top: 0;
            left: 100%;
            opacity: 0;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
        }
        .overlay .popup-overlay
        {
            opacity: 1;
            left: 0;
        }
        .popup
        {
            position: fixed;
            top: 25%;
            left: 50%;
            z-index: -9999;
        }
        .popup .popup-body
        {
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #f7f7f7 100%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #ffffff), color-stop(100%, #f7f7f7));
            background: -webkit-linear-gradient(top, #ffffff 0%, #f7f7f7 100%);
            background: -o-linear-gradient(top, #ffffff 0%, #f7f7f7 100%);
            background: -ms-linear-gradient(top, #ffffff 0%, #f7f7f7 100%);
            background: linear-gradient(to bottom, #ffffff 0%, #f7f7f7 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f7f7f7', GradientType=0);
            opacity: 0;
            min-height: 150px;
            width: 400px;
            margin-left: -200px;
            padding: 20px;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
            position: relative;
            -moz-box-shadow: 1px 2px 3px 1px rgb(185, 185, 185);
            -webkit-box-shadow: 1px 2px 3px 1px rgb(185, 185, 185);
            box-shadow: 1px 2px 3px 1px rgb(185, 185, 185);
            text-align: center;
            border: 1px solid #e9e9e9;
        }
        .popup.visible, .popup.transitioning
        {
            z-index: 9999;
        }
        .popup.visible .popup-body
        {
            opacity: 1;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
        }
        .popup .popup-exit
        {
            cursor: pointer;
            display: block;
            width: 24px;
            height: 24px;
            position: absolute;
            top: 150px;
            right: 195px;
            background: url("images/quit.png") no-repeat;
        }
        .popup .popup-content
        {
            overflow-y: auto;
        }
        .popup-content .popup-title
        {
            font-size: 24px;
            border-bottom: 1px solid #e9e9e9;
            padding-bottom: 10px;
        }
        .popup-content p
        {
            font-size: 13px;
            text-align: justify;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!-- This is a container DIV that all other divs will be positioned relative to it. -->
    <div id="hedgeem_container">
        <div class="popup-overlay">
        </div>
        <div id="hedgeem_sub_content_container_control_output" class="hedgeem_sub_content_container">
            <p>
                Output :</p>
            <!-- This is the DIV that shows a HedgeEm Seat -->
            <div id="Player_Info">
                <asp:PlaceHolder ID="Place_Holder_Player_Info" runat="server"></asp:PlaceHolder>
            </div>
        </div>
        <div id="hedgeem_sub_content_container_user_input" class="user_input_container">
            <div id="hedgeem_seat_css_container">
                <p>
                    <asp:LinkButton ID="lnk_edit_css" runat="server" OnClick="lnk_edit_css_Click">Edit Seat CSS here :</asp:LinkButton>
                    or <a id="popup_window" data-popup-target="#example-popup" href="#">Upload you own CSS
                        file</a></p>
                <asp:Button ID="btn_CSS_Output" CssClass="buttons" runat="server" Text="View CSS Output"
                    OnClick="btn_CSS_Output_Click" />
                <asp:TextBox ID="txt_css_input" runat="server" Height="300px" Width="99%" TextMode="MultiLine"
                    ReadOnly="true" BackColor="WhiteSmoke" OnTextChanged="txt_css_input_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </div>
            <div id="example-popup" class="popup">
                <div class="popup-body">
                    <span class="popup-exit"></span>
                    <div class="popup-content">
                        Select File :
                        <asp:FileUpload ID="upload_own_css_file" runat="server" />
                        <asp:Button ID="btn_upload_css" runat="server" Text="Upload" OnClick="btn_upload_css_Click" />
                    </div>
                </div>
            </div>
            <!-- This is the DIV for user to 'experiement' with control -->
            <div id="user_input">
                <p>
                    Edit Seat Attributes Here :</p>
                <asp:Button ID="btn_Attributes_Output" CssClass="buttons" runat="server" Text="View Attributes Output"
                    OnClick="btn_Attributes_Output_Click" />
                <div id="user_input_attributes">
                    <table>
                        <tr>
                            <td>Show Bet Info :
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_show_chip_icon" runat="server" OnCheckedChanged="chk_show_chip_icon_TextChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Player Name :
                            </td>
                            <td>
                                <asp:TextBox ID="txt_player_name" placeholder="Simon" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Player Balance :
                            </td>
                            <td>
                                <asp:TextBox ID="txt_player_balance" placeholder="100" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Player Image :
                            </td>
                            <td>
                                <asp:FileUpload ID="file_player_image" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type='text/javascript'>//<![CDATA[
        $(window).load(function () {
            jQuery(document).ready(function ($) {

                $('[data-popup-target]').click(function () {
                    $('html').addClass('overlay');
                    var activePopup = $(this).attr('data-popup-target');
                    $(activePopup).addClass('visible');

                });

                $(document).keyup(function (e) {
                    if (e.keyCode == 27 && $('html').hasClass('overlay')) {
                        clearPopup();
                    }
                });

                $('.popup-exit').click(function () {
                    clearPopup();

                });

                $('.popup-overlay').click(function () {
                    clearPopup();
                });

                function clearPopup() {
                    $('.popup.visible').addClass('transitioning').removeClass('visible');
                    $('html').removeClass('overlay');

                    setTimeout(function () {
                        $('.popup').removeClass('transitioning');
                    }, 200);
                }

            });
        });//]]>  

</script>
</body>
</html>
