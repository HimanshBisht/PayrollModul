using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class SalaryModule_GenerateSalarySlip : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    string Name = "";

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
                    BindEmpNature();
                    BindAppointment();
                    BindStaffType();
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
                BindEmpNature();
                BindAppointment();
                BindStaffType();
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

    protected void BindEmpNature()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageNatureofEmp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpNature", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlNatureOfEmp.DataSource = dt;
            ddlNatureOfEmp.DataTextField = "EmpNature";
            ddlNatureOfEmp.DataValueField = "NatureID";
            ddlNatureOfEmp.DataBind();
            ddlNatureOfEmp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Employee Nature", "0"));
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
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
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
            ddlemployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Employees", "0"));
            con.Close();
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
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblTotalRecords.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlNatureOfEmp.SelectedValue) > 0)
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

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblTotalRecords.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlNatureOfEmp.SelectedValue) > 0)
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

    protected void ddlNatureOfEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblTotalRecords.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlNatureOfEmp.SelectedValue) > 0)
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

    protected void ddlemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
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
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            grdrecord.DataSource = dt;
            grdrecord.DataBind();
            rptSalarySlip.DataSource = dt;
            rptSalarySlip.DataBind();
            pnlTotalRecords.Visible = true;
            lblTotalRecords.Text = dt.Rows.Count.ToString();

            if (dt.Rows.Count > 0)
            {
                pnlButtons.Visible = true;
            }
            else
            {
                pnlButtons.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnGetRecords_Click(object sender, EventArgs e)
    {
        try
        {           
            Bindgrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void grdrecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdrecord.PageIndex = e.NewPageIndex;
            Bindgrid();
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
            ddlNatureOfEmp.ClearSelection();
            ddlemployee.ClearSelection();
            ddlAppointment.ClearSelection();
            ddlStaffType.ClearSelection();
            pnlEmployees.Visible = false;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlButtons.Visible = false;
            Session["ctrl"] = null;
            pnlPrintSalarySlip.Visible = false;
            pnlTotalRecords.Visible = false;
            lblTotalRecords.Text = string.Empty;
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
            string ProfileID = "";
            foreach (GridViewRow row in grdrecord.Rows)
            {
                if (((CheckBox)row.FindControl("SelectChk")).Checked == true)
                {
                    if (ProfileID == "")
                    {
                        ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                    }
                    else
                    {
                        ProfileID = ProfileID + "," + ((Label)row.FindControl("lblProfileID")).Text;
                    }
                }
            }

            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowSalaryMaking", con);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            DataView dv = new DataView(dt);
            dv.RowFilter = "ProfileID in (" + ProfileID + ")";

            rptSalarySlip.DataSource = dv;
            rptSalarySlip.DataBind();

            if (dt.Rows.Count > 0)
            {
                pnlPrintSalarySlip.Visible = true;
                Session["ctrl"] = pnlPrintSalarySlip;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');$('#ContentPlaceHolder1_pnlPrintSalarySlip').hide();", true);
            }
            else
            {
                pnlPrintSalarySlip.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindStaffType()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageStaffType", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StaffType", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlStaffType.DataSource = dt;
            ddlStaffType.DataTextField = "StaffType";
            ddlStaffType.DataValueField = "StaffTypeID";
            ddlStaffType.DataBind();
            ddlStaffType.Items.Insert(0, new ListItem("Select Staff Type", "0"));

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }    

    protected void BindAppointment()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppointmentType", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlAppointment.DataSource = dt;
            ddlAppointment.DataTextField = "AppointmentType";
            ddlAppointment.DataValueField = "AppointmentID";
            ddlAppointment.DataBind();
            ddlAppointment.Items.Insert(0, new ListItem("Select Appointment", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlAppointment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblTotalRecords.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlNatureOfEmp.SelectedValue) > 0)
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

    protected void ddlStaffType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lblTotalRecords.Text = string.Empty;
            pnlTotalRecords.Visible = false;
            pnlPrintSalarySlip.Visible = false;
            pnlButtons.Visible = false;
            if (Convert.ToInt32(ddlMonth.SelectedValue) > 0 && Convert.ToInt32(ddlYear.SelectedValue) > 0 && Convert.ToInt32(ddlNatureOfEmp.SelectedValue) > 0)
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
}