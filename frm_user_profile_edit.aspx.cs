using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Web.Configuration;
using System.Web.Services;
using System.ServiceModel.Web;


public partial class resources_javascript_frm_user_profile_edit : System.Web.UI.Page
{
    //For Logging
    private static String logger_name_as_defined_in_app_config = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);

    //webservice object 
    localhost.WebService service = new localhost.WebService();
    string website_url = WebConfigurationManager.AppSettings["WebsiteUrl"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["username"] != null)
            {
                log.Info("[" + Session["username"].ToString() + "] open Edit Profile and it is Loaded.");
                DataTable dt_get_all_themes = new DataTable();
                if (Page.IsPostBack == false)
                {
                    // get all themes in drop down list
                    dt_get_all_themes = service.f_get_all_themes();
                    ddl_table_themes.DataSource = dt_get_all_themes;
                    ddl_table_themes.DataTextField = "shortname";
                    ddl_table_themes.DataValueField = "theme_id";
                    // bind drop down list
                    ddl_table_themes.DataBind();
                    lbl_username.Text = Session["username"].ToString();
                    DataTable dt_password = new DataTable();
                    dt_password = service.f_get_password_from_db(lbl_username.Text);
                    // get details from database for current user
                    DataTable dt_user_details = service.my_user_details(lbl_username.Text, dt_password.Rows[0]["password"].ToString());
                    lbl_name.Text = dt_user_details.Rows[0]["fullname"].ToString();
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("resources/avitars"));
                    //  dll_available_avitars.DataSource = dir.GetFiles();
                    //  dll_available_avitars.DataBind();
                    rp_available_avitars.DataSource = dir.GetFiles();
                    rp_available_avitars.DataBind();
                }

            }
            else
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('Not authorised to view this page.');</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in Page Load of Edit Profile Page.", new Exception(ex.Message));
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            log.Info("[" + Session["username"].ToString() + "] clicked on Edit Profile Update button.");
            // method to save uploaded image

            if (file_profile_picture.HasFile)
            {
                save_profile_image();
            }
            else
            {
                //string imageurl = profile_image.ImageUrl;
                //save_image_from_avitars(imageurl);
                if (Session["img_url"] != null)
                {
                    save_image_from_avitars(Session["img_url"].ToString());
                }
            }
            // method for saving selected theme for that user
            int a_table_id = service.f_get_table_id_from_db(Session["username"].ToString());
            service.f_set_default_theme(a_table_id, Convert.ToInt16(ddl_table_themes.SelectedValue));
            Response.Redirect("frm_facebook_canvas.aspx", false);
        }
        catch (Exception ex)
        {
            log.Error("Error in btn_update_Click of Edit Profile Page.", new Exception(ex.Message));
        }
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

    // method to save profile image
    private void save_profile_image()
    {
        try
        {
            log.Info("[" + Session["username"].ToString() + "] trying to save profile image.");
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
                        //save image path to database
                        service.f_set_player_avitar(Session["username"].ToString(), filePath);
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
            log.Error("Error in save_profile_image.", new Exception(ex.Message));
        }
    }
    private String uploadImages_to_folder(HttpPostedFile fil)
    {
        String Name = "player_avatar_" + Session["username"].ToString() + ".jpg";
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
        return Name;
    }
    private Byte[] GetPictureBits(Stream fs, int size)
    {
        Byte[] img = new Byte[size];
        fs.Read(img, 0, size);
        return img;
    }
    //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    //{
    //    ImageButton newAvatar = (ImageButton)sender;
    //    string image_url = newAvatar.ImageUrl;
    //    string img_url = image_url.Substring(2);
    //    Session["img_url"] = img_url;
    //    file_profile_picture.Visible = false;
    //    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('profile_image').style.display = 'block';</script>");
    //    Page.RegisterStartupScript("OnLoad1", "<script>document.getElementById('fileupload').style.display = 'none'; </script>");
    //    profile_image.ImageUrl = image_url;
    //    file_profile_picture.Dispose();
    //    Page.RegisterStartupScript("OnLoad2", "<script>function stop_refresh() { return false; }</script>");
    //}

    private void save_image_from_avitars(string img_url)
    {
        var webClient = new WebClient();
        byte[] bytes = webClient.DownloadData(website_url + "/" + img_url);
        String Name = "player_avatar_" + Session["username"].ToString() + ".jpg";
        MemoryStream stream = new MemoryStream(bytes);
        Bitmap b = new Bitmap(stream);
        Size newsize = new Size();
        newsize.Height = 50;
        newsize.Width = 50;
        Bitmap newBitMap = new Bitmap(newsize.Width, newsize.Height);
        Graphics g = Graphics.FromImage(newBitMap);
        g.DrawImage(b, 0, 0, newsize.Width, newsize.Height);
        newBitMap.Save(Server.MapPath(@"~\resources\") + Name, ImageFormat.Jpeg);
        b.Dispose();
        //save image path to database
        service.f_set_player_avitar(Session["username"].ToString(), Name);
    }
    [WebMethod(EnableSession = true)]
    [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json")]
    public static void SetSession(string src)
    {
        HttpContext.Current.Session["img_url"] = src;
    }

}