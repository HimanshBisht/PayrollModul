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

public partial class SalaryModule_FNFReport : System.Web.UI.Page
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
                    BindEmpNature();
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
            ddlMonth.Items.Insert(0, new ListItem("All Months", "0"));
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
            ddlYear.Items.Insert(0, new ListItem("All Years", "0"));
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
            ddlNatureOfEmp.Items.Insert(0, new ListItem("All Emp Nature Types", "0"));

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

    public void Employee()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageFNFMaking", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FNFMonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@FNFYearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@NatureID", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", ddlIsActive.SelectedValue);
            cmd.Parameters.AddWithValue("@Type", "GetRecord");
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ddlemployee.DataSource = dt;
            ddlemployee.DataTextField = "Name";
            ddlemployee.DataValueField = "ProfileID";
            ddlemployee.DataBind();
            ddlemployee.Items.Insert(0, new ListItem("All Employees", "0"));
            con.Close();
            ddlemployee.Visible = true;
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
            SqlCommand cmd = new SqlCommand("ManageFNFMaking", con);
            cmd.Parameters.AddWithValue("@FNFMonthID", ddlMonth.SelectedValue);
            cmd.Parameters.AddWithValue("@FNFYearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@NatureID", ddlNatureOfEmp.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", ddlIsActive.SelectedValue);
            cmd.Parameters.AddWithValue("@Type", "GetRecord");
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                lnkExportToExcel.Visible = true;
            }
            else
            {
                lnkExportToExcel.Visible = false;
            }

            lblTotalFNFEmployees.Text = dt.Rows.Count.ToString();
            pnlTotalRecords.Visible = true;
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
            ddlMonth.ClearSelection();
            ddlYear.ClearSelection();
            ddlemployee.ClearSelection();
            ddlNatureOfEmp.ClearSelection();
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            lnkExportToExcel.Visible = false;
            pnlTotalRecords.Visible = false;
            lblTotalFNFEmployees.Text = string.Empty;
            ddlemployee.Visible = false;
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
            Response.AddHeader("content-disposition", "attachment;filename=FNF_Report_FDB.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.AllowPaging = false;
                this.Bindgrid();                
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

    protected void ddlIsActive_SelectedIndexChanged(object sender, EventArgs e)
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