using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//Simons Imports


using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net;
using System.IO;
using log4net;
using log4net.Config;

public partial class frm_read_text_logs : System.Web.UI.Page
{
    string[] filePaths;

    //For Logging
    private static String logger_name_as_defined_in_app_config = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);

   

    protected void Page_Load(object sender, EventArgs e)
    {



        if (Page.IsPostBack == false)
        {
            filePaths = Directory.GetFiles(Server.MapPath("~/temp/"));
            ddl_files.DataSource = filePaths;
            ddl_files.DataBind();

            lbl_timestamp.Text = File.GetLastWriteTimeUtc(ddl_files.SelectedValue).ToString();
            lbl_msg.Text = "";
        }
      
       
    }


    //this function is used to bind the currently selected file with the textbox
    protected void f_bind_textbox_with_textfile(string file)
    {
        String line;

        log.Info("f_bind_textbox_with_textfile is called Reading File: " + file);

        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            try
            {

                lbl_timestamp.Text = File.GetLastWriteTimeUtc(file).ToString();


                using (StreamReader sr = new StreamReader(fs))
                {
                    line = sr.ReadToEnd();
                    txt_logs_data.Text = line;

                }

                lbl_msg.Text = "Data Fetched Successfully";
            }
            catch (Exception e1)
            {
                log.Error("Error in f_bind_textbox_with_textfile", new Exception(e1.Message));

                lbl_msg.Text = "Error Occurs:" + e1.Message;
            }
            finally
            {
                fs.Close();
            }
        
       

    }

    protected void ddl_files_SelectedIndexChanged(object sender, EventArgs e)
    {

        string path = ddl_files.SelectedValue;


        f_bind_textbox_with_textfile(path);

    }
    //this function is used to refresh the current file
    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        f_bind_textbox_with_textfile(ddl_files.SelectedValue);

    }
}