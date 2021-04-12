using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;

public partial class SalaryModule_SurchargeRules : System.Web.UI.Page
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
                    Year();
                }
                else
                {
                    Response.Redirect("../NotAuthorized/NotAuthorized.aspx");
                }
            }
            else
            {
                Year();
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

    public void Bindgrid()
    {
        try
        {
            SqlConnection con = new SqlConnection(constr);
            cmd = new SqlCommand("ManageSurchargeRules", con);
            cmd.Parameters.AddWithValue("@Type", "GetRecords");
            cmd.Parameters.AddWithValue("@FromYearID", ddlYear.SelectedValue);
            cmd.Parameters.AddWithValue("@User", null);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            grdrecord.DataSource = dt;
            grdrecord.DataBind();
            if (dt.Rows.Count > 0)
            {
                grdrecord.Columns[0].Visible = true;
                grdrecord.Caption = "<h3>" + "Surcharge Rules " + ddlYear.SelectedItem.Text + " - " + (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1) + "<h3 />";
                pnlButtons.Visible = true;
            }
            else
            {
                grdrecord.Caption = string.Empty;
                pnlButtons.Visible = false;
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
            ddlYear.ClearSelection();
            txtAgeFrom.Text = string.Empty;
            txtAgeTo.Text = string.Empty;
            txtNetIncomeFrom.Text = string.Empty;
            txtNetIncomeTo.Text = string.Empty;
            txtSurchargeRates.Text = string.Empty;
            grdrecord.DataSource = null;
            grdrecord.DataBind();
            grdrecord.Caption = string.Empty;
            btnSave.Text = "Save";
            ViewState["SurchargeRuleID"] = null;
            pnlButtons.Visible = false;
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
            int AgeFrom = Convert.ToInt32(txtAgeFrom.Text);
            int AgeTo = Convert.ToInt32(txtAgeTo.Text);
            decimal NetIncomeFrom = Convert.ToDecimal(txtNetIncomeFrom.Text);
            decimal NetIncomeTo = Convert.ToDecimal(txtNetIncomeTo.Text);

            //if (AgeFrom > AgeTo || NetIncomeFrom > NetIncomeTo)
            //{
            //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed : Age From can not be more than Age To and as well as Net Income From also can not be more than Net Income To, Please Enter Valid Details.');", true);
            //}
            //else
            //{
                SqlConnection con = new SqlConnection(constr);
                hash = new Hashtable();
                hash = (Hashtable)Session["User"];

                cmd = new SqlCommand("ManageSurchargeRules", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromYearID", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@FromYear", ddlYear.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ToYear", Convert.ToInt32(ddlYear.SelectedItem.Text) + 1);
                cmd.Parameters.AddWithValue("@AgeFrom", AgeFrom);
                cmd.Parameters.AddWithValue("@AgeTo", AgeTo);
                cmd.Parameters.AddWithValue("@NetIncomeFrom", NetIncomeFrom);
                cmd.Parameters.AddWithValue("@NetIncomeTo", NetIncomeTo);
                cmd.Parameters.AddWithValue("@SurchargeRates", txtSurchargeRates.Text);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                if (ViewState["SurchargeRuleID"] == null && btnSave.Text == "Save")
                {
                    cmd.Parameters.AddWithValue("@SurchargeRuleID", 0);
                    cmd.Parameters.AddWithValue("@Type", "Save");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SurchargeRuleID", ViewState["SurchargeRuleID"]);
                    cmd.Parameters.AddWithValue("@Type", "Update");
                }
                con.Open();
                int HasRow = (int)cmd.ExecuteScalar();
                con.Close();
                if (HasRow == 1)
                {
                    Bindgrid();
                    Clear();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Saved Sucessfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Already Exists for this Criteria.');", true);
                }
            //}
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
                cmd = new SqlCommand("ManageSurchargeRules", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SurchargeRuleID", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Type", "GetRecords");
                cmd.Parameters.AddWithValue("@User", null);
                con.Open();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                ddlYear.SelectedValue = dt.Rows[0]["FromYearID"].ToString();
                txtAgeFrom.Text = dt.Rows[0]["AgeFrom"].ToString();
                txtAgeTo.Text = dt.Rows[0]["AgeTo"].ToString();
                txtNetIncomeFrom.Text = dt.Rows[0]["NetIncomeFrom"].ToString();
                txtNetIncomeTo.Text = dt.Rows[0]["NetIncomeTo"].ToString();
                txtSurchargeRates.Text = dt.Rows[0]["SurchargeRates"].ToString();
                btnSave.Text = "Update";
                ViewState["SurchargeRuleID"] = e.CommandArgument;
            }
            else if (e.CommandName == "lnkDeactivate")
            {
                SqlConnection con = new SqlConnection(constr);
                cmd = new SqlCommand("ManageSurchargeRules", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SurchargeRuleID", e.CommandArgument);
                cmd.Parameters.AddWithValue("@User", Convert.ToString(hash["Name"].ToString()));
                cmd.Parameters.AddWithValue("@Type", "Deactivate");
                con.Open();
                int HasRow = (int)cmd.ExecuteScalar();
                con.Close();

                if (HasRow == 2)
                {
                    Bindgrid();
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Record Deactivate Sucessfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('Failed in Deactivating the Record.');", true);
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
            if (Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                Bindgrid();
            }
            else
            {
                Clear();
            }
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
            Response.AddHeader("content-disposition", "attachment;filename=Surcharge_Rules_" + ddlYear.SelectedItem.Text + "-" + (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1) + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdrecord.Columns[0].Visible = false;
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

    protected void btnPrintSelected_Click(object sender, EventArgs e)
    {
        try
        {
            grdrecord.UseAccessibleHeader = true;
            grdrecord.HeaderRow.TableSection = TableRowSection.TableHeader;
            grdrecord.Attributes["style"] = "border-collapse:separate";
            grdrecord.Columns[0].Visible = false;
            Session["ctrl"] = grdrecord;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "validate", "javascript: window.open('../Print.aspx','PrintMe');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "validate", "javascript: alert('" + ex.Message + "');", true);
        }
    }
}