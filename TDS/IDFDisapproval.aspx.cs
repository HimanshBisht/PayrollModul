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

public partial class SalaryModule_IDFDisapproval : System.Web.UI.Page
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

    public enum Type
    {
        All = -1,
        Approved = 1,
        NotApproved = 0
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
                    BindDesignation();
                    BindStaffType();
                    BindEmpNature();
                    BindAppointment();
                    Year();
                    Employee();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                BindDesignation();
                BindStaffType();
                BindEmpNature();
                BindAppointment();
                Year();
                Employee();
            }
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
            ddlYear.Items.Insert(0, new ListItem("Select From Year", "0"));
            con.Close();
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

    public void Employee()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetIDFRecords", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", Type.Approved);
            if (ddlYear.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@FromYear", ddlYear.SelectedItem.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FromYear", 0);
            }
            if (ddlYear.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@ToYear", Convert.ToInt32(ddlYear.SelectedItem.Text) + 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToYear", 0);
            }

            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlemployee.DataSource = dt;
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

    public void Bindgrid()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("GetIDFRecords", con);
            cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@Appointment", ddlAppointment.SelectedValue);
            cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
            cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@IsApprove", Type.Approved);
            if (ddlYear.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@FromYear", ddlYear.SelectedItem.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FromYear", 0);
            }
            if (ddlYear.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@ToYear", Convert.ToInt32(ddlYear.SelectedItem.Text) + 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ToYear", 0);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                pnlDetail.Visible = true;
                pnlButtons.Visible = true;
                lblSTMT.Text = "K.R.Mangalam World School, Faridabad <br /> List of Dis-Approval IDF for " + ddlYear.SelectedItem.Text + "-" +
                    (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
                pnlStmt.Visible = true;
                pnlApproval.Visible = true;
            }
            else
            {
                pnlButtons.Visible = false;
                pnlDetail.Visible = false;
                pnlStmt.Visible = false;
                pnlApproval.Visible = false;
            }

            lblTotalEmployees.Text = dt.Rows.Count.ToString();
            pnlTotalRecords.Visible = true;
            grdrecord.Caption = string.Empty;
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
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            ddlemployee.Items.Clear();
            grdrecord.Caption = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlTotalRecords.Visible = false;
            pnlDetail.Visible = false;
            pnlStmt.Visible = false;
            pnlButtons.Visible = false;
            pnlEmployee.Visible = false;
            pnlApproval.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearOnChange()
    {
        try
        {
            grdrecord.Caption = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            pnlTotalRecords.Visible = false;
            pnlDetail.Visible = false;
            pnlStmt.Visible = false;
            pnlButtons.Visible = false;
            pnlEmployee.Visible = false;
            pnlApproval.Visible = false;
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
            Response.AddHeader("content-disposition", "attachment;filename=IDF_List_For_Approval_FDB.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.Columns[0].Visible = false;
                grdrecord.Caption = "<b>" + lblSTMT.Text + "</b>";
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

    protected void ddlNatureOfEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Employee();
            ClearOnChange();
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
            ClearOnChange();
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
            ClearOnChange();
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
            ClearOnChange();
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
            if (ddlYear.SelectedValue == "0")
            {
                Clear();
            }
            else
            {
                pnlEmployee.Visible = true;
                Employee();
                ClearOnChange();
            }
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
            grdrecord.UseAccessibleHeader = true;
            grdrecord.HeaderRow.TableSection = TableRowSection.TableHeader;
            grdrecord.Attributes["style"] = "border-collapse:separate";
            grdrecord.Caption = "<h3>" + lblSTMT.Text + "<h3 />";
            Session["ctrl"] = grdrecord;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
            lblSTMT.Text = string.Empty;
            pnlStmt.Visible = false;
            pnlButtons.Visible = false;
            pnlDetail.Visible = false;
            pnlApproval.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnDisapproved_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            string User = Convert.ToString(hash["Name"].ToString());
            string FromYear = ddlYear.SelectedItem.Text;
            string ToYear = (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString();
            int Count = 0;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            foreach (GridViewRow row in grdrecord.Rows)
            {
                if (((CheckBox)row.FindControl("SelectChk")).Checked == true)
                {
                    string ProfileID = ((Label)row.FindControl("lblProfileID")).Text;
                    cmd = new SqlCommand("ManageTDSIDF", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
                    cmd.Parameters.AddWithValue("@FromYear", FromYear);
                    cmd.Parameters.AddWithValue("@ToYear", ToYear);
                    cmd.Parameters.AddWithValue("@SeniorCitizen", null);
                    cmd.Parameters.AddWithValue("@AgeCriteria", null);
                    cmd.Parameters.AddWithValue("@OthersDetails", null);
                    cmd.Parameters.AddWithValue("@Place", null);
                    cmd.Parameters.AddWithValue("@Signature", null);
                    cmd.Parameters.AddWithValue("@Date", null);
                    cmd.Parameters.AddWithValue("@User", User);
                    cmd.Parameters.AddWithValue("@NameAndAddress", null);
                    cmd.Parameters.AddWithValue("@AddressOfAccommodation", null);
                    cmd.Parameters.AddWithValue("@CityName", null);
                    cmd.Parameters.AddWithValue("@PanNoOfOwner", null);
                    cmd.Parameters.AddWithValue("@Type", "DisApprovedIDF");
                    Count = cmd.ExecuteNonQuery();
                }
            }

            con.Close();

            if (Count > 0)
            {
                Clear();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Dis-Approved Sucessfully.');", true);
            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Approval.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}