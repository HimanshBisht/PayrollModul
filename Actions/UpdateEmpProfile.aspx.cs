using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class UpdateEmpProfile : System.Web.UI.Page
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

    public enum Status
    {
        Active = 1,
        Deactive = 0
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
                    CalDOB.EndDate = DateTime.Now.Date;
                    CalDOJ.EndDate = DateTime.Now.Date;
                    Employee();
                    BindDesignation();
                    BindStaffType();
                    BindEmpNature();
                    BindAppointment();
                    BindSubjects();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                CalDOB.EndDate = DateTime.Now.Date;
                CalDOJ.EndDate = DateTime.Now.Date;
                Employee();
                BindDesignation();
                BindStaffType();
                BindEmpNature();
                BindAppointment();
                BindSubjects();
            }
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
            cmd = new SqlCommand("ShowEmpSalaryProfile", con);
            cmd.Parameters.AddWithValue("@IsActive", Status.Active);
            cmd.CommandType = CommandType.StoredProcedure;
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
            ddlemployee.Items.Insert(0, new ListItem("Select Employee", "0"));
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
            ddlDesignation.Items.Insert(0, new ListItem("Select Designation", "0"));

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
            ddlNatureOfEmp.Items.Insert(0, new ListItem("Select Nature", "0"));
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
            ddlSubjects.Items.Insert(0, new ListItem("Select Subject", "0"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    public void ClearPnlData()
    {
        try
        {
            pnlInfo.Visible = false;
            ddlemployee.ClearSelection();
            txtName.Text = string.Empty;
            txtFatherORHusbandName.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtMobileNo.Text = string.Empty;
            ddlGender.ClearSelection();
            txtDOB.Text = string.Empty;
            txtEmpAge.Text = string.Empty;
            txtDOJ.Text = string.Empty;
            ddlDesignation.ClearSelection();
            txtEmp_Code.Text = string.Empty;            
            ddlAppointment.ClearSelection();
            ddlNatureOfEmp.ClearSelection();
            ddlStaffType.ClearSelection();
            ddlSubjects.ClearSelection();
            pnlSubjectFor.Visible = false;
            txtPfNo.Text = string.Empty;
            txtEsiNo.Text = string.Empty;
            txtPANCardNo.Text = string.Empty;
            txtAadharCardNo.Text = string.Empty;
            txtUANNo.Text = string.Empty;
            txtHomeAddress.Text = string.Empty;
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
            ClearPnlData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void txtDOB_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDOB.Text.Length > 0)
            {
                DateTime dob = Convert.ToDateTime(txtDOB.Text);
                string text = CalculateYourAge(dob);
                txtEmpAge.Text = text;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }

    }

    static string CalculateYourAge(DateTime Dob)
    {
        DateTime Now = DateTime.Now;
        int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
        DateTime PastYearDate = Dob.AddYears(Years);
        int Months = 0;
        for (int i = 1; i <= 12; i++)
        {
            if (PastYearDate.AddMonths(i) == Now)
            {
                Months = i;
                break;
            }
            else if (PastYearDate.AddMonths(i) >= Now)
            {
                Months = i - 1;
                break;
            }
        }

        int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
        return String.Format("{0} Year's {1} Month's {2} Day's",
        Years, Months, Days);
    }

    protected void ddlemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlemployee.SelectedValue) > 0)
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ShowEmpSalaryProfile", con);
                cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
                cmd.Parameters.AddWithValue("@IsActive", Status.Active);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtFatherORHusbandName.Text = dt.Rows[0]["Father/Husband"].ToString();
                    txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                    if (dt.Rows[0]["Gender"].ToString().Length > 0)
                    {
                        ddlGender.SelectedValue = dt.Rows[0]["Gender"].ToString();
                    }
                    else
                    {
                        ddlGender.SelectedValue = "0";
                    }
                    txtDOB.Text = dt.Rows[0]["DOB"].ToString();
                    txtDOB_TextChanged(sender, e);
                    txtDOJ.Text = dt.Rows[0]["DOJ"].ToString();
                    ddlDesignation.SelectedValue = dt.Rows[0]["Designation"].ToString();
                    txtEmp_Code.Text = dt.Rows[0]["Emp_Code"].ToString();                    
                    ddlAppointment.SelectedValue = dt.Rows[0]["Appointment"].ToString();
                    ddlNatureOfEmp.SelectedValue = dt.Rows[0]["NatureOfEmp"].ToString();
                    ddlStaffType.SelectedValue = dt.Rows[0]["StaffType"].ToString();
                    if (ddlStaffType.SelectedValue == "1")
                    {
                        pnlSubjectFor.Visible = true;
                    }
                    else
                    {
                        pnlSubjectFor.Visible = false;
                    }
                    ddlSubjects.SelectedValue = dt.Rows[0]["SubjectID"].ToString();
                    txtPfNo.Text = dt.Rows[0]["PFNo"].ToString();
                    txtEsiNo.Text = dt.Rows[0]["ESINo"].ToString();
                    txtPANCardNo.Text = dt.Rows[0]["PanCardNo"].ToString();
                    txtAadharCardNo.Text = dt.Rows[0]["AadharCardNo"].ToString();
                    txtUANNo.Text = dt.Rows[0]["UANNo"].ToString();
                    txtHomeAddress.Text = dt.Rows[0]["Address"].ToString();
                    pnlInfo.Visible = true;
                }
                else
                {
                    ClearPnlData();
                }
            }
            else
            {
                ClearPnlData();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            hash = new Hashtable();
            hash = (Hashtable)Session["User"];
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("SaveEmpSalaryProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FatherORHusband", txtFatherORHusbandName.Text);
            cmd.Parameters.AddWithValue("@EmailID", txtEmailID.Text);
            cmd.Parameters.AddWithValue("@MobileNo", txtMobileNo.Text);
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
            if (txtDOB.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@DOB", txtDOB.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DOB", null);
            }
            cmd.Parameters.AddWithValue("@SubjectID", ddlSubjects.SelectedValue);
            if (txtPfNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@PFNo", txtPfNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PFNo", null);
            }

            if (txtEsiNo.Text.Length > 0)
            {
                cmd.Parameters.AddWithValue("@ESINo", txtEsiNo.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ESINo", null);
            }
            cmd.Parameters.AddWithValue("@PanCardNo", txtPANCardNo.Text);
            cmd.Parameters.AddWithValue("@AadharCardNo", txtAadharCardNo.Text);
            cmd.Parameters.AddWithValue("@UANNo", txtUANNo.Text);
            cmd.Parameters.AddWithValue("@Address", txtHomeAddress.Text);
            cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
            cmd.Parameters.AddWithValue("@ProfileID", ddlemployee.SelectedValue);
            cmd.Parameters.AddWithValue("@Type", "UpdateBasicInfo");

            con.Open();
            int Count = cmd.ExecuteNonQuery();
            con.Close();
            if (Count > 0)
            {
                ClearPnlData();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Update Sucessfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Error in Updating Details.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}