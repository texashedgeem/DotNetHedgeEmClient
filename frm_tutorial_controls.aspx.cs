using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Data;
using System.Text;
using HedgeEmClient;
using System.Drawing;
using System.Drawing.Imaging;



public partial class frm_tutorial_controls : System.Web.UI.Page
{

    #region Controls
    hedgeem_control_card cc_flop_card1;
    hedgeem_control_card cc_flop_card2;
    hedgeem_control_card cc_flop_card3;
    hedgeem_control_card cc_turn_card;
    hedgeem_control_card cc_river_card;
    hedgeem_control_seats ss_seat;
    hedgeem_hand_panel[] _hedgeem_hand_panels;
    BETTING_PANEL[,] _hedgeem_betting_panels;
    #endregion


    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.Page.Theme = "TUTORIAL";
        if (Session["ohyes"] == "yes")
        {
            this.Page.Theme = "TEMP";
        }
        //chk_show_chip_icon.Text = "45";
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack == false)
        {
            StreamReader StreamReader1 = new StreamReader(Server.MapPath("~/App_Themes/TUTORIAL/hedgeem_seat.css"));
            txt_css_input.Text = StreamReader1.ReadToEnd();
            StreamReader1.Close();
            Session.Abandon();
            f_redraw_page("Simon", "", "100",false);
        }
    }

    public void f_redraw_page(string my_player_name, string my_photo_image, string my_player_balance, bool show_bets)
    {
        ss_seat = new hedgeem_control_seats();
        ss_seat.p_player_name = my_player_name;

        if (Session["photo_uploaded"] != null)
        {
            ss_seat.p_photo_image = "resources/" + Session["photo_uploaded"].ToString() + ".jpg";
        }
        else
        {
            ss_seat.p_photo_image = "resources/player_avitar_sit_here.jpg";
        }
        ss_seat.p_balance = "£ " + my_player_balance;
        ss_seat.p_player_id = 123;
        ss_seat.p_seat_index = 0;
        ss_seat.p_show_bets_info = show_bets;

        for (int hand_index = 0; hand_index < 4; hand_index++)
        {
            HedgeEmHandPayout my_hand_payout = new HedgeEmHandPayout();
            my_hand_payout.hand_index = hand_index;
            my_hand_payout.payout_value = 444;
            ss_seat.f_add_hedgeem_hand_payout_to_list(my_hand_payout);
        }


        string chip_icon_resource_name = "chip_icon_0.png";
        ss_seat.p_chip_icon = "resources/chips/" + chip_icon_resource_name;

        //if (chk_show_chip_icon.Checked == true)
        //{
        //    ss_seat.p_balance = "puke";
        //    ss_seat.p_show_bets_info = true;
        //}

        bool xxx_show_payout = true;
        if (xxx_show_payout)
        {
            ss_seat.p_payout += 10;
            ss_seat.p_payout = "£12.50";
        }

        Place_Holder_Player_Info.Controls.Add(ss_seat);
    }


    protected void chk_show_chip_icon_TextChanged(object sender, EventArgs e)
    {
        //ss_seat.p_balance = "Need new functionality here";
        //txt_player_balance.Text = "Need new functionality here. xxx";
        //   f_redraw_page();
        if (chk_show_chip_icon.Checked == true)
        {
            Session["show_bets"] = true;
            f_redraw_page("Simon", "", "100", true);
        }
        else
        {
            Session["show_bets"] = false;
            f_redraw_page("Simon", "", "100", false);
        }
    }

    //protected void txt_player_balance_TextChanged(object sender, EventArgs e)
    //{
    //    ss_seat.p_balance = txt_player_balance.Text;
    //}
    protected void btn_Attributes_Output_Click(object sender, EventArgs e)
    {
        save_uploaded_image();
        f_redraw_page(txt_player_name.Text, "", txt_player_balance.Text,Convert.ToBoolean(Session[""].ToString()));
    }



    // this function is used to check the extension of the file user is trying to upload for an image
    public Boolean check_file_extension(String ext)
    {
        String[] exts = { ".bmp", ".jpg", ".jpeg", ".gif", ".tif", ".png" };
        if (exts.Contains(ext.ToLower()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // this function is used to check the extension of the file user is trying to upload for an image
    public Boolean check_file_extension_css(String ext)
    {
        String[] exts = { ".css" };
        if (exts.Contains(ext.ToLower()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // method to save profile image
    private void save_uploaded_image()
    {
        try
        {

            Boolean flag = true;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile PostedFile = Request.Files[i];
                if (PostedFile.ContentLength > 0)
                {
                    String extension = Path.GetExtension(PostedFile.FileName);
                    flag = check_file_extension(extension);

                    if (flag == true)
                    {
                        String filePath = uploadImages_to_folder(PostedFile);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select an image file.');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    private String uploadImages_to_folder(HttpPostedFile fil)
    {
        String Name = "temp_tutorial.jpg";
        Byte[] fileBinary = GetPictureBits(fil.InputStream, fil.ContentLength);
        MemoryStream stream = new MemoryStream(fileBinary);
        Bitmap b = new Bitmap(stream);
        Size newsize = new Size();
        newsize.Height = 50;
        newsize.Width = 50;
        Bitmap newBitMap = new Bitmap(newsize.Width, newsize.Height);
        Graphics g = Graphics.FromImage(newBitMap);
        g.DrawImage(b, 0, 0, newsize.Width, newsize.Height);
        newBitMap.Save(Server.MapPath(@"~\resources\") + Name, ImageFormat.Jpeg);
        b.Dispose();
        Session["photo_uploaded"] = "temp_tutorial";
        return Name;
    }
    private Byte[] GetPictureBits(Stream fs, int size)
    {
        Byte[] img = new Byte[size];
        fs.Read(img, 0, size);
        return img;
    }
    protected void lnk_edit_css_Click(object sender, EventArgs e)
    {
        txt_css_input.ReadOnly = false;
        txt_css_input.BackColor = Color.White;
        f_redraw_page("Simon", "", "100",false);
    }
    protected void txt_css_input_TextChanged(object sender, EventArgs e)
    {
        StreamWriter sw = new StreamWriter(Server.MapPath(@"~\App_Themes\TEMP\hedgeem_seat.css"));
        //Use write method to write the text
        sw.Write(txt_css_input.Text);
        //always close your stream
        sw.Close();
        Session["ohyes"] = "yes";
        f_redraw_page("Simon", "", "100",false);
    }
    protected void btn_CSS_Output_Click(object sender, EventArgs e)
    {
        //   txt_css_input.ReadOnly = false;
        //Use StreamWriter class.
        StreamWriter sw = new StreamWriter(Server.MapPath(@"~\App_Themes\TEMP\hedgeem_seat.css"));
        //Use write method to write the text
        sw.Write(txt_css_input.Text);
        //always close your stream
        sw.Close();
        Session["ohyes"] = "yes";
        f_redraw_page("Simon", "", "100",false);
    }
    protected void btn_upload_css_Click(object sender, EventArgs e)
    {
        Boolean flag = true;
        for (int i = 0; i < Request.Files.Count; i++)
        {
            HttpPostedFile PostedFile = Request.Files[i];
            if (PostedFile.ContentLength > 0)
            {
                String extension = Path.GetExtension(PostedFile.FileName);
                flag = check_file_extension_css(extension);

                if (flag == true)
                {
                   upload_own_css_file.SaveAs(Server.MapPath(@"~\App_Themes\TEMP\hedgeem_seat.css"));
                    Session["ohyes"] = "yes";
                    f_redraw_page("Simon", "", "100",false);
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('File Uploaded Successfully. Please check Output window for details.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select a css file.');", true);
                    f_redraw_page("Simon", "", "100",false);
                }
            }
        }
    }
}