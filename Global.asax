<%@ Application Language="C#" %>

<script runat="server">
 

    void Application_Start(object sender, EventArgs e) 
    {
        log4net.Config.XmlConfigurator.Configure();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        // Unfinished (Aug 2014) error handling code see ... http://odetocode.com/articles/69.aspx
        Exception exception = Server.GetLastError().GetBaseException();

        // NEED TO WRITE TO exception log here.
        
        /*
        Response.Write("We're Sorry...");
        Response.Write("An error has occured on the page you were requesting.  Your System Administrator has been notified <BR>");
        Response.Write("Url: " + Request.Url.ToString() + "<BR>");
        Response.Write("Err: " + exception.Message  + "<BR>");
         * */
        // Code that runs when an unhandled error occurs
        

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
