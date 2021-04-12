using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;


public partial class SalaryModule_Home : System.Web.UI.Page
{

    Hashtable hash;

    protected void page_Init()
    {
        hash = new Hashtable();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["User"] != null)
            {
                hash = (Hashtable)Session["User"];
                if (!IsPostBack)
                {

                }
            }
            else
            {
                Response.Redirect("../Default.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }

    }
}