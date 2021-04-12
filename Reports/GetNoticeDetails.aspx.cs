using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class SalaryModule_GetNoticeDetails : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal ExtraPaidDaysSalary = 0;

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

    public enum Status
    {
        Active = 1,
        Deactive = 0
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
                    GetProfileDetails();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                GetProfileDetails();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetProfileDetails()
    {
        try
        {
            string ProfileID = Request.QueryString["item"];

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageNoticePeriod", con);
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            cmd.Parameters.AddWithValue("@Type", "GetData");
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                lblTitle.Text = "K. R. Mangalam World School, Faridabad " + "<br />" + dt.Rows[0]["NoticeType"].ToString() + " Report of " + dt.Rows[0]["DropText"].ToString();
                lblEmpName.Text = dt.Rows[0]["Name"].ToString();
                lblEmpCode.Text = dt.Rows[0]["Emp_Code"].ToString();
                lblDOJ.Text = dt.Rows[0]["DOJ"].ToString();
                lblDesignation.Text = dt.Rows[0]["Designation"].ToString();
                lblStaffType.Text = dt.Rows[0]["StaffType"].ToString();
                lblNature.Text = dt.Rows[0]["NatureOfEmp"].ToString();
                lblResignDate.Text = dt.Rows[0]["ResignationDate"].ToString();
                lblActualLWD.Text = dt.Rows[0]["LWD"].ToString();
                lblTotalWorking.Text = dt.Rows[0]["TotalWorking"].ToString();
                lblNoticePeriodDays.Text = dt.Rows[0]["NoticePeriodDays"].ToString();
                lblAsPerNormsLWD.Text = dt.Rows[0]["AsPerNormsLWD"].ToString();
                lblNoticeAmount.Text = dt.Rows[0]["NoticeAmount"].ToString();
                lblOtherAdjustment.Text = dt.Rows[0]["OtherAdjustment"].ToString();
                lblRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                lblNetNoticePeriodAdjustmentAmount.Text = dt.Rows[0]["FinalNoticeAmount"].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('No Record Found.');", true);
            }

            grdrecord.DataSource = dt;
            grdrecord.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Cancel()
    {
        try
        {
            lblEmpName.Text = string.Empty;
            lblEmpCode.Text = string.Empty;
            lblDOJ.Text = string.Empty;
            lblDesignation.Text = string.Empty;
            lblStaffType.Text = string.Empty;
            lblNature.Text = string.Empty;
            lblResignDate.Text = string.Empty;
            lblActualLWD.Text = string.Empty;
            lblAsPerNormsLWD.Text = string.Empty;
            lblTotalWorking.Text = string.Empty;
            lblNoticePeriodDays.Text = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblNetNoticePeriodAdjustmentAmount.Text = string.Empty;
            lblOtherAdjustment.Text = string.Empty;
            lblRemarks.Text = string.Empty;
            lblNetNoticePeriodAdjustmentAmount.Text = string.Empty;
            pnlShowData.Visible = false;            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void GetPrint()
    {
        try
        {
            Session["ctrl"] = pnlPrint;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnPrintSelected_Click(object sender, EventArgs e)
    {
        try
        {
            GetPrint();
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
            Cancel();           
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: window.close();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}