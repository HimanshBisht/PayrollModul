using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;

public partial class SalaryModule_ProfileHistoryReport : System.Web.UI.Page
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
                    BindDesignation();
                    BindStaffType();
                    BindEmpNature();
                    BindAppointment();
                    BindSubjects();
                    Employee();
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
                BindDesignation();
                BindStaffType();
                BindEmpNature();
                BindAppointment();
                BindSubjects();
                Employee();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindDesignation()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageDesignation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Designation", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlDesignation.DataSource = dt;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "DesignationID";
            ddlDesignation.DataBind();
            ddlDesignation.Items.Insert(0, new ListItem("All Designations Types", "0"));
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
            ddlStaffType.Items.Insert(0, new ListItem("All Staff Types", "0"));
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
            ddlNatureOfEmp.Items.Insert(0, new ListItem("All Emp Nature Types", "0"));

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
            ddlAppointment.Items.Insert(0, new ListItem("All Appointment Types", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void BindSubjects()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageSubjects", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubjectName", null);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            ddlSubjects.DataSource = dt;
            ddlSubjects.DataTextField = "SubjectName";
            ddlSubjects.DataValueField = "SubjectID";
            ddlSubjects.DataBind();
            ddlSubjects.Items.Insert(0, new ListItem("All Subjects Types", "-1"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void Employee()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfileHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@Subject", ddlSubjects.SelectedValue);
            if (ddlPFDeduct.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFDeduct.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            else if (ddlPFDeduct.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            else if (ddlPFDeduct.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFDeduct.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@ESIDeduct", ddlESIDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@ModeofSalary", ddlModeOfSalary.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", ddlIsActive.SelectedValue);
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
            ddlemployee.Items.Insert(0, new ListItem("All Employees", "0"));
            con.Close();
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
            ddlMonth.Items.Insert(0, new ListItem("Select Month", "0"));
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
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
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
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ShowEmpSalaryProfileHistory", con);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@Subject", ddlSubjects.SelectedValue);
            if (ddlPFDeduct.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFDeduct.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            else if (ddlPFDeduct.SelectedValue == "1")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", 3);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            else if (ddlPFDeduct.SelectedValue == "2")
            {
                cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFDeduct.SelectedValue);
                cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFDeduct.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@ESIDeduct", ddlESIDeduct.SelectedValue);
            cmd.Parameters.AddWithValue("@ModeofSalary", ddlModeOfSalary.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", ddlIsActive.SelectedValue);
            cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
            if (ddlYear.SelectedValue == "0")
            {
                cmd.Parameters.AddWithValue("@YearID", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedItem.Text);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                pnlTotalRecords.Visible = true;
                pnlDetail.Visible = true;
                lnkExportToExcel.Visible = true;
            }
            else
            {
                pnlDetail.Visible = false;
                lnkExportToExcel.Visible = false;
            }
            lblTotalEmployees.Text = dt.Rows.Count.ToString();
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
            ddlNatureOfEmp.ClearSelection();
            ddlAppointment.ClearSelection();
            ddlStaffType.ClearSelection();
            ddlDesignation.ClearSelection();
            ddlSubjects.ClearSelection();
            ddlPFDeduct.ClearSelection();
            ddlESIDeduct.ClearSelection();
            ddlModeOfSalary.ClearSelection();
            ddlemployee.ClearSelection();
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lnkExportToExcel.Visible = false;
            pnlTotalRecords.Visible = false;
            pnlDetail.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
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

    private void ExportGridToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Profile_Modification_Report_FDB.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.AllowPaging = false;
                this.Bindgrid();
                grdrecord.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdrecord.HeaderRow.Cells)
                {
                    cell.BackColor = Color.White;
                }
                foreach (GridViewRow row in grdrecord.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.BackColor = Color.White;
                        cell.ForeColor = Color.Black;
                        cell.CssClass = "textmode";
                    }
                }
                grdrecord.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ExportGridToExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
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

    protected void ddlNatureOfEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
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
            Employee();
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
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlPFDeduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlESIDeduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void ddlModeOfSalary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}