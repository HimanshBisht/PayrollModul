using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;

public partial class SalaryModule_MonthlyReimbursementReport : System.Web.UI.Page
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
                    Month();
                    Year();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Month();
                Year();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Month()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowMonth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlMonth.DataSource = dt;
            ddlMonth.DataTextField = "MonthName";
            ddlMonth.DataValueField = "MonthID";
            ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Month", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Year()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageYears", con);
            cmd.Parameters.AddWithValue("@Year", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlYear.DataSource = dt;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "YearID";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Year", "0"));
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Employee(int? EmployeeID)
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowReimbursementMaking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DataView dv = new DataView(dt);
            dv.Sort = "Name ASC";
            ddlemployee.DataSource = dv;
            ddlemployee.DataTextField = "DropText";
            ddlemployee.DataValueField = "ProfileID";
            ddlemployee.DataBind();
            ddlemployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Employee", "0"));
            con.Close();
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
            if (Convert.ToInt32(ddlemployee.SelectedValue) > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowReimbursementMaking", con);
                cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    pnlResult.Visible = true;
                    lblReimbursementFor1.Text = dt.Rows[0]["ReimbursementFor1"].ToString();
                    if (Convert.ToDecimal(dt.Rows[0]["ReimbursementValue1"]) > 0)
                    {
                        lblReimbursementValue1.Text = dt.Rows[0]["ReimbursementValue1"].ToString();
                    }
                    else
                    {
                        lblReimbursementValue1.Text = string.Empty;
                    }

                    lblReimbursementFor2.Text = dt.Rows[0]["ReimbursementFor2"].ToString();
                    if (Convert.ToDecimal(dt.Rows[0]["ReimbursementValue2"]) > 0)
                    {
                        lblReimbursementValue2.Text = dt.Rows[0]["ReimbursementValue2"].ToString();
                    }
                    else
                    {
                        lblReimbursementValue2.Text = string.Empty;
                    }

                    lblReimbursementFor3.Text = dt.Rows[0]["ReimbursementFor3"].ToString();
                    if (Convert.ToDecimal(dt.Rows[0]["ReimbursementValue3"]) > 0)
                    {
                        lblReimbursementValue3.Text = dt.Rows[0]["ReimbursementValue3"].ToString();
                    }
                    else
                    {
                        lblReimbursementValue3.Text = string.Empty;
                    }

                    lblReimbursementFor4.Text = dt.Rows[0]["ReimbursementFor4"].ToString();
                    if (Convert.ToDecimal(dt.Rows[0]["ReimbursementValue4"]) > 0)
                    {
                        lblReimbursementValue4.Text = dt.Rows[0]["ReimbursementValue4"].ToString();
                    }
                    else
                    {
                        lblReimbursementValue4.Text = string.Empty;
                    }

                    lblReimbursementFor5.Text = dt.Rows[0]["ReimbursementFor5"].ToString();
                    if (Convert.ToDecimal(dt.Rows[0]["ReimbursementValue5"]) > 0)
                    {
                        lblReimbursementValue5.Text = dt.Rows[0]["ReimbursementValue5"].ToString();
                    }
                    else
                    {
                        lblReimbursementValue5.Text = string.Empty;
                    }

                    lblTotalAmount.Text = dt.Rows[0]["TotalReimbursement"].ToString();

                    pnlDetail.Visible = true;
                    pnlPrint.Visible = true;
                    lblSTMT.Text = "Reimbursement Bill of " + ddlemployee.SelectedItem.Text + " For the Month of " + ddlMonth.SelectedItem.Text + " - " + ddlYear.SelectedItem.Text;
                }
                else
                {
                    lblSTMT.Text = string.Empty;
                    pnlDetail.Visible = false;
                    pnlPrint.Visible = false;
                    pnlResult.Visible = false;
                }
            }
            else
            {
                lblReimbursementFor1.Text = string.Empty;
                lblReimbursementValue1.Text = string.Empty;
                lblReimbursementFor2.Text = string.Empty;
                lblReimbursementValue2.Text = string.Empty;
                lblReimbursementFor3.Text = string.Empty;
                lblReimbursementValue3.Text = string.Empty;
                lblReimbursementFor4.Text = string.Empty;
                lblReimbursementValue4.Text = string.Empty;
                lblReimbursementFor5.Text = string.Empty;
                lblReimbursementValue5.Text = string.Empty;
                pnlDetail.Visible = false;
                pnlPrint.Visible = false;
                lblSTMT.Text = string.Empty;
            }
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
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            pnlEmployees.Visible = false;
            pnlDetail.Visible = false;
            pnlPrint.Visible = false;
            lblSTMT.Text = string.Empty;
            lblReimbursementFor1.Text = string.Empty;
            lblReimbursementValue1.Text = string.Empty;
            lblReimbursementFor2.Text = string.Empty;
            lblReimbursementValue2.Text = string.Empty;
            lblReimbursementFor3.Text = string.Empty;
            lblReimbursementValue3.Text = string.Empty;
            lblReimbursementFor4.Text = string.Empty;
            lblReimbursementValue4.Text = string.Empty;
            lblReimbursementFor5.Text = string.Empty;
            lblReimbursementValue5.Text = string.Empty;
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
            Empty();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                pnlEmployees.Visible = true;
                Employee(null);
            }
            else
            {
                pnlEmployees.Visible = false;
                ddlemployee.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                pnlEmployees.Visible = true;
                Employee(null);
            }
            else
            {
                pnlEmployees.Visible = false;
                ddlemployee.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ctrl"] = pnlDetail;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Empty()
    {
        try
        {
            lblReimbursementFor1.Text = string.Empty;
            lblReimbursementValue1.Text = string.Empty;

            lblReimbursementFor2.Text = string.Empty;
            lblReimbursementValue2.Text = string.Empty;

            lblReimbursementFor3.Text = string.Empty;
            lblReimbursementValue3.Text = string.Empty;

            lblReimbursementFor4.Text = string.Empty;
            lblReimbursementValue4.Text = string.Empty;

            lblReimbursementFor5.Text = string.Empty;
            lblReimbursementValue5.Text = string.Empty;

            lblTotalAmount.Text = string.Empty;

            pnlDetail.Visible = false;
            pnlPrint.Visible = false;
            pnlResult.Visible = false;
            lblSTMT.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Empty();
            Bindgrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

}