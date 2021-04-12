using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class SalaryModule_TransportSetup : System.Web.UI.Page
{
    string constr = "";
    SqlConnection con;
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;

    protected void page_Init()
    {
        constr = ConfigurationManager.ConnectionStrings["myconnectionstring"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        da = new SqlDataAdapter();
        cmd = new SqlCommand();
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
                    CheckUserRights();
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

    public enum MenuType
    {
        All = 0,
        SetUp = 1,
        ImportData = 2,
        Actions = 3,
        Reports = 4
    }

    public void CheckUserRights()
    {
        try
        {
            int HasMatch = 0;
            string RequestURL = Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(RequestURL);
            string PageName = oInfo.Name;
            string CheckPageName = "";

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetLoginDetails", con);
            cmd.Parameters.AddWithValue("@UserName", null);
            cmd.Parameters.AddWithValue("@Password", null);
            cmd.Parameters.AddWithValue("@LoginID", Session["LoginID"]);
            cmd.Parameters.AddWithValue("@MenuID", MenuType.All);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            if (ds.Tables[1].Rows.Count > 0)
            {
                int i = 0;

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    CheckPageName = ds.Tables[1].Rows[i]["PageName"].ToString();
                    if (PageName == CheckPageName)
                    {
                        HasMatch++;
                        break;
                    }

                    i++;
                }

                if (HasMatch > 0)
                {
                    Bindgrid();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Bindgrid();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Bindgrid()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowTransportSetUp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            grdrecord.DataSource = dt;
            grdrecord.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Clear()
    {
        try
        {
            txtMinRange.Text = string.Empty;
            txtMaxRange.Text = string.Empty;
            txtTransportValue.Text = string.Empty;
            ViewState["TransportID"] = null;
            btnSave.Text = "Save";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMinRange.Text.Length > 0 && txtTransportValue.Text.Length > 0)
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("SaveTransportSetUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MinRange", txtMinRange.Text);
                if (txtMaxRange.Text == "")
                {
                    cmd.Parameters.AddWithValue("@MaxRange", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MaxRange", txtMaxRange.Text);
                }

                cmd.Parameters.AddWithValue("@TransportValue", txtTransportValue.Text);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                if (ViewState["TransportID"] == null && btnSave.Text == "Save")
                {
                    cmd.Parameters.AddWithValue("@TransportID", 0);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TransportID", Convert.ToInt32(ViewState["TransportID"]));
                    cmd.Parameters.AddWithValue("@Type", "Update");
                }
                con.Open();

                int Count = (int)cmd.ExecuteScalar();
                con.Close();
                if (Count == 0)
                {
                    Bindgrid();
                    Clear();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                }

                else if (Count == 1)
                {

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Already Exist for this Range, Please Try Again.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Min Range and Transport Value can not be blank.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "lnkEdit")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowTransportSetUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransportID", e.CommandArgument);
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    txtMinRange.Text = dt.Rows[0]["MinRange"].ToString();
                    txtMaxRange.Text = dt.Rows[0]["MaxRange"].ToString();
                    txtTransportValue.Text = dt.Rows[0]["TransportValue"].ToString();
                    ViewState["TransportID"] = e.CommandArgument;
                    btnSave.Text = "Update";
                }
            }

            else if (e.CommandName == "lnkDeactivate")
            {
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("SaveTransportSetUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MinRange", 0);
                if (txtMaxRange.Text == "")
                {
                    cmd.Parameters.AddWithValue("@MaxRange", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@MaxRange", txtMaxRange.Text);
                }
                cmd.Parameters.AddWithValue("@TransportValue", 0);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@TransportID", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Type", "Deactive");
                con.Open();
                int Count = (int)cmd.ExecuteScalar();
                con.Close();
                if (Count == 0)
                {
                    Bindgrid();
                    Clear();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            Bindgrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}