using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace EmployeeApp
{
    public partial class EmployeeInfoUI : Form
    {
        public EmployeeInfoUI()
        {
            InitializeComponent();
        }

        public int selectedEmnployeeId = 0;

        string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty())
            {
                MessageBox.Show("Please enter the value . ");
            }
            else
            {
                // check the email

                if (IsEmailExists(emailTextBox.Text))
                {
                    MessageBox.Show("Enter exists . Enter another email .");
                }
                else
                {
                    int rowsAffected = SaveEmployeInformation();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Inserted successfully . ");
                        nameTextBox.Text = "";
                        addressTextBox.Text = "";
                        emailTextBox.Text = "";
                        salaryTextBox.Text = "";

                        employeeListView.Items.Clear();
                        SelectStudent();
                    }
                    else
                    {
                        MessageBox.Show("Not inserted");
                    }
                }


                
              
            }
        }

        private int SaveEmployeInformation()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
            string query = "INSERT INTO tblEmployee values('"+nameTextBox.Text+"','"
                                                             +addressTextBox.Text+"','"
                                                             +emailTextBox.Text+"','"
                                                             +salaryTextBox.Text+"')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               SqlCommand cmd = new SqlCommand(query,connection);
                connection.Open();
                int rowsAffected =  cmd.ExecuteNonQuery();
                return rowsAffected;
            }

            
        }

        private bool IsEmailExists(string emailText)
        {
            bool isEmailExists = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                 SqlCommand cmd = new SqlCommand("SELECT * from tblEmployee where Email ='"+emailText+"'",connection);
                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

               // bool isEmailExists = false;
                while (rdr.Read())
                {
                    isEmailExists = true;
                    break;
                }
            }
            return isEmailExists;


        }
        private bool IsUpdateEmailExists(string emailText, int id)
        {
            bool isEmailExists = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * from tblEmployee where Email ='" + emailText + "' AND id !="+id+"", connection);
                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                // bool isEmailExists = false;
                while (rdr.Read())
                {
                    isEmailExists = true;
                    break;
                }
            }
            return isEmailExists;


        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
            employeeListView.Items.Clear();
            SelectStudent();
        }


        private void SelectStudent()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * from tblEmployee", connection);
                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                  //  ListViewItem listViewItem = new ListViewItem(int.Parse(rdr["ID"].ToString()));
                    ListViewItem listViewItem = new ListViewItem(rdr["ID"].ToString());
                   // listViewItem.SubItems.Add(rdr["ID"].ToString());
                    listViewItem.SubItems.Add(Convert.ToString(rdr["Name"]));
                    listViewItem.SubItems.Add((string)rdr["Address"]);
                    listViewItem.SubItems.Add((string)rdr["Email"]);
                    listViewItem.SubItems.Add((string)rdr["Salary"]);

                    employeeListView.Items.Add(listViewItem);
                }


               
                
            }
            
        }

        private void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem listViewItem = employeeListView.SelectedItems[0];

            selectedEmnployeeId = Convert.ToInt32(listViewItem.SubItems[0].Text);

            nameTextBox.Text = listViewItem.SubItems[1].Text;
            addressTextBox.Text = listViewItem.SubItems[2].Text;
            emailTextBox.Text = listViewItem.SubItems[3].Text;
            salaryTextBox.Text = listViewItem.SubItems[4].Text;
        }



        private void updateButton_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty())
            {
                MessageBox.Show("Enter the information .");
            }
            else
            {
                if (IsUpdateEmailExists(emailTextBox.Text, selectedEmnployeeId))
                {
                    MessageBox.Show("Email already exist .");
                }
                else
                {
                    int rowsAffected = UpdateInformation(selectedEmnployeeId);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Updated successfully .");

                        nameTextBox.Text = "";
                        addressTextBox.Text = "";
                        emailTextBox.Text = "";
                        salaryTextBox.Text = "";

                        employeeListView.Items.Clear();
                        SelectStudent();


                    }
                    else
                    {
                        MessageBox.Show("Not updated .");
                    }

                }
            }
            

        }

        private int UpdateInformation(int employeeId)
        {
            int rowsAffected = 0;
            string query = "UPDATE tblEmployee SET Name='"+nameTextBox.Text+"',Address='"
                                                         +addressTextBox.Text+"',Email='"
                                                         +emailTextBox.Text+"',Salary='"
                                                         + salaryTextBox.Text + "' where ID='" + employeeId + "'";

            //if (IsTextBoxEmpty())
            //{
            //    MessageBox.Show("Enter the information .");
            //}
          //  else
          //  {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }

               // return rowsAffected;
          //  }
            return rowsAffected;

            
        }




        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (employeeListView.SelectedItems.Count > 0)
            {
                int rowsAffected = DeleteInformationById(selectedEmnployeeId);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Deleted successfully .");

                    nameTextBox.Text = "";
                    addressTextBox.Text = "";
                    emailTextBox.Text = "";
                    salaryTextBox.Text = "";

                    employeeListView.Items.Clear();
                    SelectStudent();


                }
                else
                {
                    MessageBox.Show("Not delated .");
                }
            }
            else
            {
                MessageBox.Show("No item selected .");
            }
            
                

            

        }

        private int DeleteInformationById(int employeeId)
        {

            int rowsAffected = 0;
            string query = "DELETE from tblEmployee WHERE ID='" + employeeId + "'";

        //    if (employeeListView.SelectedItems.Count>0)
          //  {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
         //   }
          //  else
         //   {
            //    MessageBox.Show("No item selected.");
           // }
           
                

                // return rowsAffected;
            
            return rowsAffected;
        }



        private bool IsTextBoxEmpty()
        {
            bool isTextBoxEmpty = false;

            if (nameTextBox.Text=="" || addressTextBox.Text =="" || emailTextBox.Text =="" || salaryTextBox.Text =="")
            {
                isTextBoxEmpty = true;
            }

            return isTextBoxEmpty;

        }



    }
}
