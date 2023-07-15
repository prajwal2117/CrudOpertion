using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Contact : System.Web.UI.Page
    {

        SqlConnection SqlCon = new SqlConnection(@"Data Source =DESKTOP-VOE8H6R; Initial Catalog = ASPCRUD; Integrated Security = true"); 



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();


        }

        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            lblSuccessMessage.Text = lblError.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SqlCon.State == ConnectionState.Closed)
                SqlCon.Open();
            SqlCommand SqlCmd = new SqlCommand("ContactCreateOrUpdate", SqlCon);
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlCmd.Parameters.AddWithValue("@ContactID", (hfContactID.Value == "" ? 0 : Convert.ToInt32(hfContactID.Value)));
            SqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            SqlCmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
            SqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            SqlCmd.ExecuteNonQuery();
            SqlCon.Close();
            string ContactID = hfContactID.Value;
            Clear();
            if (ContactID == "")
                lblSuccessMessage.Text = "Saved Successfully";
            else
                lblSuccessMessage.Text = "Updated Successfully";
            FillGridView();


        }

        void FillGridView()
        {
            if (SqlCon.State == ConnectionState.Closed)
                SqlCon.Open();
            SqlDataAdapter SqlDa = new SqlDataAdapter("ContactViewall", SqlCon);
            SqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            SqlDa.Fill(dtbl);
            SqlCon.Close();
            gvContact.DataSource = dtbl;
            gvContact.DataBind();

        }

        protected void lnk_onClick(Object sender, EventArgs e)
        {
          int ContactID=Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (SqlCon.State == ConnectionState.Closed)
                SqlCon.Open();
            SqlDataAdapter SqlDa = new SqlDataAdapter("ContactViewByID", SqlCon);
            SqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlDa.SelectCommand.Parameters.AddWithValue("@ContactID", ContactID);
            DataTable dtbl = new DataTable();
            SqlDa.Fill(dtbl);
            SqlCon.Close();
            hfContactID.Value = ContactID.ToString();
            txtName.Text = dtbl.Rows[0]["Name"].ToString();
            txtMobile.Text = dtbl.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
            btnSave.Text = "Update";
            btnDelete.Enabled = true;



        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (SqlCon.State == ConnectionState.Closed)
                SqlCon.Open();
            SqlCommand SqlCmd = new SqlCommand("ContactDeleteByID",SqlCon);
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlCmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
            SqlCmd.ExecuteNonQuery();
            SqlCon.Close();
            Clear();
            FillGridView();
            lblSuccessMessage.Text = "Delete Successfully";
        }
    }
}