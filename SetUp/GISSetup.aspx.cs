using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class SalaryModule_GISSetup : System.Web.UI.Page
{
    string constr = "";
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
            cmd = new SqlCommand("ShowGISSetup", con);
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
            txtGISName.Text = string.Empty;
            txtGISValue.Text = string.Empty;
            btnSave.Text = "Save";
            ViewState["GISID"] = null;
            ViewState["OldGISValue"] = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public enum Status
    {
        Active = 1,
        Deactive = 2,
        All = 0
    }

    public enum GISDeduct
    {
        Yes = 1,
        No = 2,
        All = 0
    }

    public decimal Converter(decimal value)
    {
        return Math.Round(value, MidpointRounding.AwayFromZero);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            int i = 0;

            SqlConnection con = new SqlConnection(constr);
            con.Open();

            if (ViewState["GISID"] == null && btnSave.Text == "Save")
            {
                cmd = new SqlCommand("SaveGISSetUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GISName", txtGISName.Text);
                cmd.Parameters.AddWithValue("@GISValue", txtGISValue.Text);
                cmd.Parameters.AddWithValue("@ProfileID", 0);
                cmd.Parameters.AddWithValue("@GrossDeduction", 0);
                cmd.Parameters.AddWithValue("@NetSalary", 0);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@GISID", 0);
                cmd.Parameters.AddWithValue("@Type", "Save");
                int Count = (int)cmd.ExecuteScalar();

                if (Count == 0)
                {
                    Bindgrid();
                    Clear();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                }

                else if (Count == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : GIS Value is already exixt , Please Try Again.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                }
            }

            if (ViewState["GISID"] != null && btnSave.Text == "Update")
            {
                decimal OldGisValue = Convert.ToDecimal(ViewState["OldGISValue"]);

                SqlCommand cmdProfile = new SqlCommand("ShowEmpSalaryProfile", con);
                cmdProfile.Parameters.AddWithValue("@IsActive", Status.Active);
                cmdProfile.Parameters.AddWithValue("@GISDeduct", GISDeduct.Yes);
                cmdProfile.Parameters.AddWithValue("@GisValue", OldGisValue);
                cmdProfile.CommandType = CommandType.StoredProcedure;
                DataTable dtProfile = new DataTable();
                SqlDataAdapter daProfile = new SqlDataAdapter(cmdProfile);
                daProfile.Fill(dtProfile);

                if (dtProfile.Rows.Count > 0)
                {
                    int Count = 0;

                    foreach (DataRow row in dtProfile.Rows)
                    {
                        decimal BasicScale = 0;
                        decimal DaValue = 0;
                        decimal TransportValue = 0;
                        decimal PFValue = 0;
                        decimal ESIValue = 0;
                        decimal TransportRecovery = 0;
                        decimal GrossAllowance = 0;
                        decimal GrossDeduction = 0;
                        decimal GrossTotal = 0;
                        decimal NetSalary = 0;
                        string ProfileID = "0";
                        decimal HRAValue = 0;
                        decimal MedicalValue = 0;
                        decimal ExGratiaValue = 0;
                        decimal WashingValue = 0;
                        decimal GISValue = 0;

                        ProfileID = dtProfile.Rows[i]["ProfileID"].ToString();
                        BasicScale = Convert.ToDecimal(dtProfile.Rows[i]["BasicScale"].ToString());
                        DaValue = Convert.ToDecimal(dtProfile.Rows[i]["DaValue"].ToString());
                        HRAValue = Convert.ToDecimal(dtProfile.Rows[i]["HRAValue"].ToString());
                        TransportValue = Convert.ToDecimal(dtProfile.Rows[i]["TransportValue"].ToString());
                        MedicalValue = Convert.ToDecimal(dtProfile.Rows[i]["MedicalValue"].ToString());
                        WashingValue = Convert.ToDecimal(dtProfile.Rows[i]["WashingValue"].ToString());
                        ExGratiaValue = Convert.ToDecimal(dtProfile.Rows[i]["ExGratiaValue"].ToString());

                        GrossAllowance = Converter(DaValue + HRAValue + MedicalValue + TransportValue + WashingValue + ExGratiaValue);
                        GrossTotal = Converter(BasicScale + DaValue + HRAValue + MedicalValue + TransportValue + WashingValue + ExGratiaValue);

                        PFValue = Convert.ToDecimal(dtProfile.Rows[i]["PFValue"].ToString());
                        ESIValue = Convert.ToDecimal(dtProfile.Rows[i]["EsiValue"].ToString());
                        GISValue = Convert.ToDecimal(txtGISValue.Text);
                        TransportRecovery = Convert.ToDecimal(dtProfile.Rows[i]["TransportRecovery"].ToString());

                        GrossDeduction = Converter(PFValue + ESIValue + GISValue + TransportRecovery);
                        NetSalary = Converter(GrossTotal - GrossDeduction);

                        cmd = new SqlCommand("SaveGISSetUp", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@GISName", txtGISName.Text);
                        cmd.Parameters.AddWithValue("@GISValue", txtGISValue.Text);
                        cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                        cmd.Parameters.AddWithValue("@GrossDeduction", GrossDeduction);
                        cmd.Parameters.AddWithValue("@NetSalary", NetSalary);
                        cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                        cmd.Parameters.AddWithValue("@GISID", Convert.ToInt32(ViewState["GISID"]));
                        cmd.Parameters.AddWithValue("@Type", "Update");

                        Count = (int)cmd.ExecuteScalar();                        
                        i++;
                    }

                    if (Count == 0)
                    {
                        Bindgrid();
                        Clear();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                    }

                    else if (Count == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : GIS Value is already exixt , Please Try Again.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                    }
                }
                else
                {
                    cmd = new SqlCommand("SaveGISSetUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GISName", txtGISName.Text);
                    cmd.Parameters.AddWithValue("@GISValue", txtGISValue.Text);
                    cmd.Parameters.AddWithValue("@ProfileID", 0);
                    cmd.Parameters.AddWithValue("@GrossDeduction", 0);
                    cmd.Parameters.AddWithValue("@NetSalary", 0);
                    cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                    cmd.Parameters.AddWithValue("@GISID", Convert.ToInt32(ViewState["GISID"]));
                    cmd.Parameters.AddWithValue("@Type", "Update");
                    int Count = (int)cmd.ExecuteScalar();

                    if (Count == 0)
                    {
                        Bindgrid();
                        Clear();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                    }

                    else if (Count == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : GIS Value is already exixt , Please Try Again.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Saving Details.');", true);
                    }
                }
            }

            con.Close();
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
                cmd = new SqlCommand("ShowGISSetUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GISID", e.CommandArgument);
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                txtGISName.Text = dt.Rows[0]["GISName"].ToString();
                txtGISValue.Text = dt.Rows[0]["GISValue"].ToString();
                ViewState["GISID"] = e.CommandArgument;
                ViewState["OldGISValue"] = dt.Rows[0]["GISValue"].ToString();
                btnSave.Text = "Update";
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