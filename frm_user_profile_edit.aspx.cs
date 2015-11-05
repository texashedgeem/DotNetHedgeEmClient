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
using HedgeEmClient;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


public partial class resources_javascript_frm_user_profile_edit : System.Web.UI.Page
{
    //For Logging
    private static String logger_name_as_defined_in_app_config = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);
    private static string p_current_json_webservice_url_base
    {
        get
        {
            return WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"].ToString();
        }
    }
    private int p_session_personal_table_id
    {
        get
        {
            int my_table_id = -1;
            try
            {
                my_table_id = Convert.ToInt32(Session["p_session_personal_table_id"]);
            }
            catch (Exception e)
            {
                my_table_id = -1;
            }

            return my_table_id;
        }

        set { Session["p_session_personal_table_id"] = value; }
    }

    // method to convert a list into datatable
    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        try
        {
            if (Session["p_session_username"] != null)
            {
                log.Info("[" + Session["p_session_username"].ToString() + "] open Edit Profile and it is Loaded.");
                DataTable dt_get_all_themes = new DataTable();
                if (Page.IsPostBack == false)
                {
                    //  string my_endpoint = String.Format("{0}ws_get_list_of_hedgeem_table_themes/10/", p_current_json_webservice_url_base);
                    List<HedgeemThemeInfo> theme_info_list = (List<HedgeemThemeInfo>)f_get_object_from_json_call_to_server("ws_get_list_of_hedgeem_table_themes/10/", typeof(List<HedgeemThemeInfo>));
                    // get all themes in drop down list
                    //   dt_get_all_themes = service.f_get_all_themes();
                    dt_get_all_themes = ToDataTable(theme_info_list);
                    ddl_table_themes.DataSource = dt_get_all_themes;
                    ddl_table_themes.DataTextField = "short_name";
                    ddl_table_themes.DataValueField = "theme_id";
                    // bind drop down list
                    ddl_table_themes.DataBind();
                    lbl_username.Text = Session["p_session_username"].ToString();
                    DataTable dt_password = new DataTable();
                    string my_endpoint = String.Format("ws_get_password_from_db/{0}/",

                                                                                 lbl_username.Text);


                    DataTable get_password_from_db = (DataTable)f_get_object_from_json_call_to_server(my_endpoint, typeof(DataTable));
                    dt_password = get_password_from_db;

                    //  dt_password = service.f_get_password_from_db(lbl_username.Text);
                    // get details from database for current user

                    string my_endpoint1 = String.Format("ws_my_user_details/{0},{1}/",

                                                                                 lbl_username.Text,
                                                                                 dt_password.Rows[0]["password"].ToString());

                    DataTable dt_user_details = (DataTable)f_get_object_from_json_call_to_server(my_endpoint1, typeof(DataTable));


                    //    DataTable dt_user_details = service.my_user_details(lbl_username.Text, dt_password.Rows[0]["password"].ToString());
                    lbl_name.Text = dt_user_details.Rows[0]["fullname"].ToString();
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("resources/avitars"));
                    //  dll_available_avitars.DataSource = dir.GetFiles();
                    //  dll_available_avitars.DataBind();
                    rp_available_avitars.DataSource = dir.GetFiles();
                    rp_available_avitars.DataBind();
                    //get current theme selected 
                    HedgeemThemeInfo my_hedgeem_theme_info = new HedgeemThemeInfo();
                    my_endpoint = String.Format("ws_get_theme_details_for_hedgeem_table/{1}/", p_current_json_webservice_url_base, Session["p_session_personal_table_id"]);
                    my_hedgeem_theme_info = (HedgeemThemeInfo)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeemThemeInfo));

                    if (my_hedgeem_theme_info == null)
                    {
                        throw new Exception(String.Format("Unable to determine theme for table [{0}]", Session["p_session_personal_table_id"]));
                    }

                    if (my_hedgeem_theme_info._error_message != "")
                    {
                        throw new Exception(my_hedgeem_theme_info._error_message);
                    }
                    string theme_name = my_hedgeem_theme_info.short_name;
                    Int64 theme_id = my_hedgeem_theme_info.theme_id;
                    if (theme_id != null)
                    {
                        ddl_table_themes.SelectedValue = theme_id.ToString();
                    }
                    if (!Page.IsPostBack)
                    {
                        if (Session["p_session_username"] != null)
                        {
                            if (File.Exists(Server.MapPath("../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg")))
                            {
                                profile_image.ImageUrl = "../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
                            }
                            else
                            {
                                profile_image.ImageUrl = "../resources/avitars/user_square.png";
                            }
                        }
                    }
                    else
                    {
                        if (Session["img_url"] != null)
                        {
                            profile_image.ImageUrl = Session["img_url"].ToString();
                        }
                    }
                }

            }
            else
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('Not authorised to view this page.');</script>");
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in Page Load of Edit Profile Page." + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            log.Error("Error in Page Load of Edit Profile Page.", new Exception(ex.Message));
        }
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        try
        {
            log.Info("[" + Session["p_session_username"].ToString() + "] clicked on Edit Profile Update button.");
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

            string my_endpoint = String.Format("ws_get_table_id_from_db/{0}/",
                                                                                 Session["p_session_username"].ToString());

            int a_table_id = (int)f_get_object_from_json_call_to_server(my_endpoint, typeof(int));
            // int a_table_id = service.f_get_table_id_from_db(Session["p_session_username"].ToString());

            string table_id = a_table_id.ToString();
            string dd1_table_theme = ddl_table_themes.SelectedValue;


            string my_endpoint1 = String.Format("ws_set_default_theme/{0},{1}/",
                                                                               table_id,
                                                                               dd1_table_theme);

            string default_theme = (string)f_get_object_from_json_call_to_server(my_endpoint1, typeof(string));


            // service.f_set_default_theme(a_table_id, Convert.ToInt16(ddl_table_themes.SelectedValue));
            Response.Redirect("frm_facebook_canvas.aspx?signout=true", false);
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in btn_update_Click of Edit Profile Page." + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            log.Error("Error in btn_update_Click of Edit Profile Page."+ex.Message,ex);
            //throw new Exception(ex.Message);
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

    private object f_get_object_from_json_call_to_server(string endpoint, Type typeIn)
    {

        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = String.Format("Method called with endpoint [{0}]", endpoint); ;
        my_log.p_method_name = "f_get_object_from_json_call_to_server";
        my_log.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
        my_log.p_table_id = p_session_personal_table_id;
        log.Debug(my_log.ToString());
        object obj = null;
        HttpWebRequest request;
        String my_service_url = "not set";


        my_log.p_message = String.Format("WARNING! If a exception happens in this function is does not appear to user.");
        log.Warn(my_log.ToString());
        //throw new Exception("Test is this still used");


        my_service_url = WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"];
        my_log.p_message = String.Format("Hedgeem Webservice URI = [{0}{1}]", my_service_url, endpoint);

        log.Debug(my_log.ToString());
        request = WebRequest.Create(my_service_url + endpoint) as HttpWebRequest;


        // Get response
        if (request != null)
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();

                    if (stream != null)
                    {


                        var reader = new StreamReader(stream);
                        try
                        {
                            string json = reader.ReadToEnd();
                            // Check if stream is empty, if it is throw and exception
                            if (json.Length == 0)
                            {
                                String my_error_msg = String.Format("JSON string has been returned empty for URI endpoint [{0}]", request.Address);
                                my_log.p_message = my_error_msg;
                                log.Error(my_log.ToString());
                                throw new Exception(my_error_msg);
                            }
                            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                            if (typeIn != null)
                            {
                                var ser = new DataContractJsonSerializer(typeIn);
                                obj = ser.ReadObject(ms);
                            }
                            else
                            {
                                obj = json;
                            }
                        }

                        finally
                        {
                            reader.Close();
                        }
                    }
                }
            }

        return obj;
    }

    // method to save profile image
    private void save_profile_image()
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        try
        {
            log.Info("[" + Session["p_session_username"].ToString() + "] trying to save profile image.");
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


                        string my_endpoint1 = String.Format("ws_set_player_avitar/{0},{1}/",
                                                                               Session["p_session_username"].ToString(),
                                                                               filePath);

                        string set_player_avitar = (string)f_get_object_from_json_call_to_server(my_endpoint1, typeof(string));


                        //      service.f_set_player_avitar(Session["p_session_username"].ToString(), filePath);
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
            string my_error_popup = "Error in save_profile_image." + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            log.Error("Error in save_profile_image.", new Exception(ex.Message));
        }
    }
    private String uploadImages_to_folder(HttpPostedFile fil)
    {
        String Name = "player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
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
    public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
    {
        var ratioX = (double)maxWidth / image.Width;
        var ratioY = (double)maxHeight / image.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);

        var newImage = new Bitmap(newWidth, newHeight);

        using (var graphics = Graphics.FromImage(newImage))
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

        return newImage;
    }
    private void save_image_from_avitars(string img_url)
    {
        try
        {

           // System.Windows.Forms.MessageBox.Show("12345");
           // var webClient = new WebClient();
           // byte[] bytes = webClient.DownloadData(img_url);
        //    String Name = "player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
            //MemoryStream ms = new MemoryStream(bytes);
            //System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            //returnImage.Save(Server.MapPath(@"~\resources\") + Name, ImageFormat.Jpeg);

       //      System.Drawing.Image img=  System.Drawing.Image.FromFile(img_url);

         //   MemoryStream stream = new MemoryStream(bytes);
            
            //Size newsize = new Size();
            //newsize.Height = 50;
            //newsize.Width = 50;
            //System.Drawing.Image resizedImage = (System.Drawing.Image) new Bitmap(img, newsize);
            //resizedImage.Save(Server.MapPath(@"~\resources\") + Name, ImageFormat.Jpeg);
            //Bitmap newBitMap = new Bitmap(newsize.Width, newsize.Height);
            //Graphics g = Graphics.FromImage(newBitMap);
            //g.DrawImage(b, 0, 0, newsize.Width, newsize.Height);
            //newBitMap.Save(Server.MapPath(@"~\resources\") + Name, ImageFormat.Jpeg);         

            //b.Dispose();                

           // string path1 = Server.MapPath(img_url);
            //string path = Server.MapPath("/webguruz.dev.hedgeem.com/resources/avitars/player_avatar_anneka.thomason@gmail.com.jpg");
            //using (var image = System.Drawing.Image.FromFile(path))
            //{
            //    using (var newImage = ScaleImage(image, 50, 50))
            //    {
            //        newImage.Save(@"~\resources\" + Name, ImageFormat.Jpeg);
            //    }
            //}


            String Name = "player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
            byte[] data;
            using (WebClient client = new WebClient())
            {
                data = client.DownloadData(img_url);
            }
            string hh = Server.MapPath("~/resources/" + Name);

            File.WriteAllBytes(hh, data);


            //save image path to databases
            string my_endpoint = String.Format("ws_set_player_avitar/{0},{1}/",
                                                                               Session["p_session_username"].ToString(),
                                                                               Name);

            string set_player_avitar = (string)f_get_object_from_json_call_to_server(my_endpoint, typeof(string));

            //  service.f_set_player_avitar(Session["p_session_username"].ToString(), Name);

        }
        catch (Exception ex)
        {
            log.Error("Error in save_image_from_avitars.", ex);
            throw new Exception(ex.Message);

        }
    }
    //[WebMethod(EnableSession = true)]
    //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json")]
    //[WebMethod(EnableSession = true)]

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SetSession()
    {
        string src = HttpContext.Current.Request.QueryString["src"].ToString();
        HttpContext.Current.Session["img_url"] = src;
        string path = HttpContext.Current.Session["img_url"].ToString();
        return path;

    }

}