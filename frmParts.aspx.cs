﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using Transport;
using System.Configuration.Assemblies;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;

namespace MiCar
{
    public partial class frmParts : System.Web.UI.Page
    {
        public string varPageName;
        public string connectionString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = WebConfigurationManager.ConnectionStrings["FleetConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                HttpCookie cookie = Request.Cookies["UserName"];

            string varName = "";
            if (cookie != null)
            {
                varName = cookie["varUserName"];
                txtUserLabel.Text = varName;
            }
            else
            {
                txtUserLabel.Text = "No cookies found";
            }


            varPageName = "Parts";
            //**********************************************************
            // Create the Command.
            AXcontrol DB = new AXcontrol();

            //**********************************************************
            // Create the Command.
            //string insertSQL = "select * from tb_RoleUser where userid=@userid";

            //SqlCommand cmd = new SqlCommand(insertSQL, con);
            //cmd.Parameters.AddWithValue("@userid", txtUserLabel.Text);
            //SqlDataReader reader;
            //con.Open();
            //reader = cmd.ExecuteReader();
            //reader.Read();
            string varUserPresence = DB.FindUserPresence(txtUserLabel.Text);

            if (varUserPresence.Length > 0)
            {
                string varRole = varUserPresence.ToString();  // (string)reader["role"];

                //string query = "SELECT * FROM operator_permission where role=@role and menus=@menus";
                //SqlConnection con1 = new SqlConnection(connectionString1);
                //SqlCommand cmd1 = new SqlCommand(query, con1);
                //cmd1.Parameters.AddWithValue("@role", varRole);
                //cmd1.Parameters.AddWithValue("@menus", varPageName1);
                //SqlDataReader readerPermissions;
                //con1.Open();
                //readerPermissions = cmd1.ExecuteReader();
                //readerPermissions.Read();


                DataSet ds = new DataSet();
                ds = DB.FindRoleMenus(varRole, varPageName);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblResults.Text = "No permissions found for role " + varRole;
                }
                else
                {
                    string varMenu = DB.FindRoleMenusCreate(varRole, varPageName);
                    if (varMenu == "false")
                    {
                        cmdAdd.Visible = false;
                    }
                    else
                    {
                        cmdAdd.Visible = true;
                    }
                    varMenu = DB.FindRoleMenusRead(varRole, varPageName);
                    if (varMenu == "false")
                    {
                        GridView1.Visible = false;
                    }
                    else
                    {
                        GridView1.Visible = true;
                    }
                    varMenu = DB.FindRoleMenusUpdate(varRole, varPageName);
                    if (varMenu == "false")
                    {
                        cmdEdit.Visible = false;
                    }
                    else
                    {
                        cmdEdit.Visible = true;
                    }
                    varMenu = DB.FindRoleMenusDelete(varRole, varPageName);
                    if (varMenu == "false")
                    {
                        cmdDelete.Visible = false;
                    }
                    else
                    {
                        cmdDelete.Visible = true;
                    }
                }
            }
            }
            catch (Exception err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
                return;
            }
            finally
            {
                con.Close();
            }   
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            clsParts DB = new clsParts();
            try
            {
                // txtCode, txtDesc,  txtUnitPrice,   cboSupplierID,  txtManuDate,  txtExpireDate
                if (txtCode.Text == null)
                {
                    lblResults.Text = "Please enter the part code";
                    return;
                }

                if (txtDesc.Text == null)
                {
                    lblResults.Text = "Enter the part name";
                    return;
                }
                if (txtUnitPrice.Text == null)
                {
                    lblResults.Text = "Enter the unit price";
                    return;
                }
                if (txtUnitPrice.GetType() != typeof(Decimal))
                {
                    lblResults.Text = "Enter the monetary unit price";
                    return;
                }
                if (cboSupplierID.Text == null)
                {
                    lblResults.Text = "Select the supplier";
                    return;
                }
                if (txtManuDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid license manufacture date";
                    return;
                }
                if (txtExpireDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid license expiry date";
                    return;
                }

                DB.Add_rec(txtCode.Text, txtDesc.Text, Decimal.Parse(txtUnitPrice.Text), cboSupplierID.Text, DateTime.Parse(txtManuDate.SelectedDate.ToString()), DateTime.Parse(txtExpireDate.SelectedDate.ToString()));


                // Fill the DataSet.
                DataSet ds = new DataSet();
                ds = DB.FindTable();
                //adapter.Fill(ds, "tb_Customer");
                // Perform the binding.
                GridView1.DataSource = ds;
                GridView1.DataBind();

                lblResults.Text = "Operation successful";

                txtCode.Text = "";
                txtDesc.Text = "";
                txtUnitPrice.Text = "0";
                cboSupplierID.Text = "";

                return;
            }
            catch (FormatException err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Transport Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
            }

        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            clsParts DB = new clsParts();
            try
            {
                // txtCode, txtDesc,  txtUnitPrice,   cboSupplierID,  txtManuDate,  txtExpireDate
                if (txtCode.Text == null)
                {
                    lblResults.Text = "Please enter the part code";
                    return;
                }

                if (txtDesc.Text == null)
                {
                    lblResults.Text = "Enter the part name";
                    return;
                }
                if (txtUnitPrice.Text == null)
                {
                    lblResults.Text = "Enter the unit price";
                    return;
                }
                if (txtUnitPrice.GetType() != typeof(Decimal))
                {
                    lblResults.Text = "Enter the monetary unit price";
                    return;
                }
                if (cboSupplierID.Text == null)
                {
                    lblResults.Text = "Select the supplier";
                    return;
                }
                if (txtManuDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid manufacture date";
                    return;
                }
                if (txtExpireDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid expiry date";
                    return;
                }

                DB.Edit_rec(txtCode.Text, txtDesc.Text, Decimal.Parse(txtUnitPrice.Text), cboSupplierID.Text, DateTime.Parse(txtManuDate.SelectedDate.ToString()), DateTime.Parse(txtExpireDate.SelectedDate.ToString()));


                // Fill the DataSet.
                DataSet ds = new DataSet();
                ds = DB.FindTable();
                //adapter.Fill(ds, "tb_Customer");
                // Perform the binding.
                GridView1.DataSource = ds;
                GridView1.DataBind();

                lblResults.Text = "Operation successful";

                txtCode.Text = "";
                txtDesc.Text = "";
                txtUnitPrice.Text = "0";
                cboSupplierID.Text = "";

                return;
            }
            catch (FormatException err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Transport Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
            }
        }
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            clsParts DB = new clsParts();
            try
            {
                // txtCode, txtDesc,  txtUnitPrice,   cboSupplierID,  txtManuDate,  txtExpireDate
                if (txtCode.Text == null)
                {
                    lblResults.Text = "Please enter the part code";
                    return;
                }

                if (txtDesc.Text == null)
                {
                    lblResults.Text = "Enter the part name";
                    return;
                }
                if (txtUnitPrice.Text == null)
                {
                    lblResults.Text = "Enter the unit price";
                    return;
                }
                if (txtUnitPrice.GetType() != typeof(Decimal))
                {
                    lblResults.Text = "Enter the monetary unit price";
                    return;
                }
                if (cboSupplierID.Text == null)
                {
                    lblResults.Text = "Select the supplier";
                    return;
                }
                if (txtManuDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid license manufacture date";
                    return;
                }
                if (txtExpireDate.GetType() != typeof(DateTime))
                {
                    lblResults.Text = "Enter a valid license expiry date";
                    return;
                }

                DB.Delete_rec(txtCode.Text, txtDesc.Text, Decimal.Parse(txtUnitPrice.Text), cboSupplierID.Text, DateTime.Parse(txtManuDate.SelectedDate.ToString()), DateTime.Parse(txtExpireDate.SelectedDate.ToString()));


                // Fill the DataSet.
                DataSet ds = new DataSet();
                ds = DB.FindTable();
                //adapter.Fill(ds, "tb_Customer");
                // Perform the binding.
                GridView1.DataSource = ds;
                GridView1.DataBind();

                lblResults.Text = "Operation successful";

                txtCode.Text = "";
                txtDesc.Text = "";
                txtUnitPrice.Text = "0";
                cboSupplierID.Text = "";

                return;
            }
            catch (FormatException err)
            {
                EventLog log = new EventLog();
                log.Source = "Milorry Transport Frontend";
                log.WriteEntry(err.Message, EventLogEntryType.Error);
            }

        }

    }
}