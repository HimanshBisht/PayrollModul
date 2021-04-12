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
using System.Text;

public partial class SalaryModule_AdvanceReport : System.Web.UI.Page
{
    string constr = "";
    SqlCommand cmd;
    SqlDataAdapter da;
    Hashtable hash;
    decimal PayDrawnBasic = 0;
    decimal Da = 0;
    decimal Hra = 0;
    decimal Transport = 0;
    decimal Medical = 0;
    decimal Washing = 0;
    decimal GrossRevisedSalary = 0;
    decimal ExGratia = 0;
    decimal ArearAdjust = 0;
    decimal GrossTotal = 0;
    decimal PF = 0;
    decimal Deduction = 0;
    decimal TdsOnBasic = 0;
    decimal Advance = 0;
    decimal TPTREC = 0;
    decimal GIS = 0;
    decimal ESI = 0;
    decimal TotalDeduction = 0;
    decimal GrossTotalSalary = 0;
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
        if (!Page.IsPostBack)
        {
            Employee();
        }
    }

    public enum Status
    {
        Active = 1,
        Deactive = 0
    }
    //public void Employee()
    //{
    //    try
    //    {
    //        SqlConnection con = new SqlConnection(constr);
    //        cmd = new SqlCommand("ShowSalaryMaking", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@MonthID", ddlMonth.SelectedValue);
    //        cmd.Parameters.AddWithValue("@YearID", ddlYear.SelectedValue);
    //        cmd.Parameters.AddWithValue("@EmpNature", ddlNatureOfEmp.SelectedValue);
    //        cmd.Parameters.AddWithValue("@StaffType", ddlStaffType.SelectedValue);
    //        cmd.Parameters.AddWithValue("@Designation", ddlDesignation.SelectedValue);
    //        cmd.Parameters.AddWithValue("@SalaryMakeStatus", ddlSalaryStatus.SelectedValue);
    //        cmd.Parameters.AddWithValue("@IsApprove", IsApprove.Approve);

    //        if (ddlPFApply.SelectedValue == "0")
    //        {
    //            cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
    //            cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
    //        }
    //        else if (ddlPFApply.SelectedValue == "1")
    //        {
    //            cmd.Parameters.AddWithValue("@PFDeductOld", 3);
    //            cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
    //        }
    //        else if (ddlPFApply.SelectedValue == "2")
    //        {
    //            cmd.Parameters.AddWithValue("@PFDeductOld", ddlPFApply.SelectedValue);
    //            cmd.Parameters.AddWithValue("@PFDeductNew", ddlPFApply.SelectedValue);
    //        }
    //        con.Open();
    //        DataTable dt = new DataTable();
    //        da = new SqlDataAdapter(cmd);
    //        da.Fill(dt);
    //        DataView dv = new DataView(dt);
    //        dv.Sort = "Name ASC";
    //        ddlemployee.DataSource = dv;
    //        ddlemployee.DataTextField = "DropText";
    //        ddlemployee.DataValueField = "ProfileID";
    //        ddlemployee.DataBind();
    //        ddlemployee.Items.Insert(0, new ListItem("All Employees", "0"));
    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
    //    }
    //}

    protected void AdvanceReport()
    {
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("SP_AdvanceReport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        if (ddlemployee.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@ProfileId", ddlemployee.SelectedValue);
        }
        else
        {
            cmd.Parameters.AddWithValue("@ProfileId", 0);
        }
       
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            grdrecord.DataSource = dt;
        }
        else
        {
            grdrecord.DataSource = null;
        }
        grdrecord.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        AdvanceReport();
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
}