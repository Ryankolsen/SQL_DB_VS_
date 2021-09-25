using EmployeeManagement;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmoloyeeManagement
{
    public partial class employeeForm : Form
    {
        public employeeForm()
        {
            InitializeComponent();
        }

        private string dbCommand = "";  //INSERT OR UPDATE

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void employeeForm_Load(object sender, EventArgs e)
        {
            Db.OpenConnection();

            updateDataBinding();

            
        }

        private void updateDataBinding(SqlCommand command = null, Button btn = null)
        {
            try
            {


                TextBox txt;
                ComboBox cmb;
                RadioButton rdb;

                foreach(Control c in groupBox1.Controls)
                {
                    if(c.GetType() == typeof(TextBox))
                    {
                        txt = (TextBox)c;
                        txt.DataBindings.Clear();
                        txt.Text = "";
                    }
                    else if (c.GetType() == typeof(ComboBox))
                    {
                        cmb = (ComboBox)c;
                        cmb.DataBindings.Clear();
                        cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }

                foreach (Control c in groupBox3.Controls)
                {
                    if (c.GetType() == typeof(ComboBox))
                    {
                        cmb = (ComboBox)c;
                        cmb.DataBindings.Clear();
                        cmb.DropDownStyle = ComboBoxStyle.DropDownList;

                        if(btn == null)
                        {
                            cmb.Enabled = false;
                        }

                    }
                    else if (c.GetType() == typeof(RadioButton))
                    {
                        rdb = (RadioButton)c;
                        if(btn == null)
                        {
                            rdb.Checked = false;
                        }
                        
                    }
                }

                if(command == null)
                {
                    //Db.sql = "";
                    Db.cmd.CommandText = "SELECT Employees.*, Departments.DepartmentName " +
                        "FROM employees INNER JOIN Departments ON Employees.DepartmentID = " +
                        "Departments.DepartmentID ORDER BY Employees.AutoID ASC;";
                }
                else
                {
                    Db.cmd = command;
                }

                Db.da = new SqlDataAdapter(Db.cmd);
                Db.ds = new DataSet();
                Db.da.Fill(Db.ds, "EmployeeList");

                //Import Data
                Db.bs = new BindingSource(Db.ds, "EmployeeList");

                bindingNavigator1.BindingSource = Db.bs;

                //Simple Data Bindings
                autoIDTextBox.DataBindings.Add("Text", Db.bs, "AutoID");
                FirstNameTextBox.DataBindings.Add("Text", Db.bs, "FirstName");
                LastNameTextBox.DataBindings.Add("Text", Db.bs, "LastName");
                EmailTextBox.DataBindings.Add("Text", Db.bs, "Email");
                JobTitleTextBox.DataBindings.Add("Text", Db.bs, "JobTitle");
                PhoneTextBox.DataBindings.Add("Text", Db.bs, "Phone");

                //DataGrid
                dataGridView1.Enabled = true;
                dataGridView1.DataSource = Db.bs;

                //Select Full Rows
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                //Complex Data Binding
                string mySql = "SELECT DepartmentID, DepartmentName FROM Departments ORDER BY DepartmentID ASC";
                Db.cmd.CommandText = mySql;
                Db.da.SelectCommand = Db.cmd;
                Db.da.Fill(Db.ds, "Division");

                //Insert Row to DataSet to select Dept ID
                DataRow row1 = Db.ds.Tables["Division"].NewRow();
                row1["DepartmentID"] = 0;
                row1["DepartmentName"] = "-- Please Select --";
                Db.ds.Tables["Division"].Rows.InsertAt(row1, 0);

                DivisionComboBox.DataSource = Db.ds.Tables["Division"];
                DivisionComboBox.DisplayMember = "DepartmentName";
                DivisionComboBox.ValueMember = "DepartmentID";
                DivisionComboBox.DataBindings.Add("SelectedValue", Db.bs, "DepartmentID");

                if(btn == null)
                {
                    Db.cmd.CommandText = mySql;
                    Db.da.SelectCommand = Db.cmd;
                    DataSet DataSt = new DataSet();
                    Db.da.Fill(DataSt, "SearchDivision");

                    //Insert Row to DataSet to select Dept ID
                    DataRow row2 = DataSt.Tables["SearchDivision"].NewRow();
                    row2["DepartmentID"] = 0;
                    row2["DepartmentName"] = "-- Please Select --";
                    DataSt.Tables["SearchDivision"].Rows.InsertAt(row2, 0);

                    FindByDivisionComboBox.DataSource = DataSt.Tables["SearchDivision"];
                    FindByDivisionComboBox.DisplayMember = "DepartmentName";
                    FindByDivisionComboBox.ValueMember = "DepartmentID";

                    //FindByDivisionComboBox.Enabled = false;


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Update Data Binding Error: " + ex.Message);
            }
            finally
            {
                if (KeywordTextBox.CanSelect)
                {
                    KeywordTextBox.Select();
                }
                
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        

        private void employeeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Db.CloseConnection();
        }

        private void FindByDivisionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //if user clicks on Add new:
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingNavigatorAddNewItem.Text == "Add New")
                {
                    //User must complete add new function before allowing access to these functions:
                    bindingNavigatorAddNewItem.Text = "Cancel";
                    bindingNavigatorAddNewItem.ToolTipText = "Cancel";

                    bindingNavigatorMoveFirstItem.Enabled = false;
                    bindingNavigatorMovePreviousItem.Enabled = false;
                    bindingNavigatorPositionItem.Enabled = false;
                    bindingNavigatorMoveNextItem.Enabled = false;
                    bindingNavigatorMoveLastItem.Enabled = false;

                    dataGridView1.ClearSelection();
                    dataGridView1.Enabled = false;

                }
                else //press cancel button
                {
                    bindingNavigatorAddNewItem.Text = "Add New";
                    bindingNavigatorAddNewItem.ToolTipText = "Add New";

                    bindingNavigatorMoveFirstItem.Enabled = true;
                    bindingNavigatorMovePreviousItem.Enabled = true;
                    bindingNavigatorPositionItem.Enabled = true;
                    bindingNavigatorMoveNextItem.Enabled = true;
                    bindingNavigatorMoveLastItem.Enabled = true;

                    updateDataBinding();

                    return;

                }

                TextBox txt;
                ComboBox cmb;

                foreach(Control c in groupBox1.Controls)
                {
                    if(c.GetType() == typeof(TextBox))
                    {
                        txt = (TextBox)c;
                        txt.Text = "";
                        if (txt.Name.Equals("FirstNameTextBox"))
                        {
                            if (txt.CanSelect)
                            {
                                txt.Select();
                            }
                        }
                    }else if (c.GetType() == typeof(ComboBox))
                    {
                        cmb = (ComboBox)c;
                        cmb.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {

                
            }
            finally
            {
                // Error: 
            }

            
        }

        private void addCommandParameters()
        {
            Db.cmd.Parameters.Clear();
            Db.cmd.CommandText = Db.sql;

            Db.cmd.Parameters.AddWithValue("FirstName", FirstNameTextBox.Text.Trim());
            Db.cmd.Parameters.AddWithValue("LastName", LastNameTextBox.Text.Trim());
            Db.cmd.Parameters.AddWithValue("Email", EmailTextBox.Text.Trim());
            Db.cmd.Parameters.AddWithValue("JobTitle", JobTitleTextBox.Text.Trim());
            Db.cmd.Parameters.AddWithValue("Phone", PhoneTextBox.Text.Trim());

            Db.cmd.Parameters.AddWithValue("Division", DivisionComboBox.SelectedValue);

            if (dbCommand.ToUpper() == "UPDATE")
            {
                Db.cmd.Parameters.AddWithValue("ID", autoIDTextBox.Text.Trim());
            }
        }

        private void bindingNavigatorUpdateItem_Click(object sender, EventArgs e)
        {
            //check to see if all text boxes are filled:
            TextBox txt;
            foreach(Control c in groupBox1.Controls)
            {
                if(c.GetType() == typeof(TextBox))
                {
                    txt = (TextBox)c;
                    if (!txt.Name.Equals("autoIDTextBox"))
                    {
                        if (string.IsNullOrEmpty(txt.Text.Trim()))
                        {
                            MessageBox.Show("Please fill in the required fields.");
                            return;
                        }
                    }
                }
            }

            if (DivisionComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Please select Division from dropdown menu.");
                return;

            }
                Db.OpenConnection();
            
            try
            {

                    if (bindingNavigatorAddNewItem.Text.Equals("Add New"))
                    {
                        //UPDATE SET WHERE

                        //check for null
                        if (autoIDTextBox.Text.Trim() == "" || string.IsNullOrEmpty(autoIDTextBox.Text.Trim()))
                        {
                            MessageBox.Show("Please select an item from datagridview");
                            return;
                        }

                        //confirm update with user
                        if(MessageBox.Show("ID: " + autoIDTextBox.Text.Trim() + " Do you want to update the selected record?", 
                            "C# and SQL SERVER: UPDATE", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }

                    //Process Update
                    dbCommand = "UPDATE";
                    //WHERE Clause:
                    Db.sql = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
                        "JobTitle = @JobTitle, Phone = @Phone, DepartmentID = @Division WHERE AutoID = @ID";

                    addCommandParameters();


                    }
                        else if (bindingNavigatorAddNewItem.Text == "Cancel")
                        {
                            //INSERT INTO
                            DialogResult result;
                            result = MessageBox.Show("Do you want to add a new employee record? Y/N)",
                                "C# and SQL Server (INSERT INTO) : Employee Database", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                //INSERT INTO
                                dbCommand = "INSERT INTO";
                                Db.sql = "INSERT INTO Employees(FirstName, LastName, Email, JobTitle, Phone, DepartmentID) " +
                                        "VALUES(@FirstName, @LastName, @Email, @JobTitle, @Phone, @Division)";
                  
                                addCommandParameters();
                            }
                            else //if no
                            {
                                return;
                            }
                        }
                    //Excecute Query
                    int execute = Db.cmd.ExecuteNonQuery();

                    if (execute != -1)
                    {
                        MessageBox.Show("The data has been saved " + dbCommand);
                        updateDataBinding();
                        bindingNavigatorAddNewItem.Text = "Add New";
                    }

            }
                catch (Exception ex)
                {
                    MessageBox.Show("Save Data Failed: " + ex.Message);

                    
                }
                finally
                {
                    dbCommand = "";
                    Db.CloseConnection();
                }


        }

        //testing function to detect add new process
        private bool IsAddingNewRecord()
        {
            if(bindingNavigatorAddNewItem.Text == "Cancel")
            {
                MessageBox.Show("Please cancel or finish adding new record first");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if(IsAddingNewRecord() == true)
            {
                return; //exit
            }
            Db.OpenConnection();

            //check for null
            if (autoIDTextBox.Text.Trim() == "" || string.IsNullOrEmpty(autoIDTextBox.Text.Trim()))
            {
                MessageBox.Show("Please select an item from datagridview");
                return;
            }


            try
            {
                //confirm update with user
                if (MessageBox.Show("ID: " + autoIDTextBox.Text.Trim() + " Do you want to DELETE the selected record?",
                    "C# and SQL SERVER: DELETE", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                //If Yes
                dbCommand = "DELETE";

                Db.sql = "DELETE FROM Employees WHERE AutoId = @ID";

                Db.cmd.Parameters.Clear();
                Db.cmd.CommandText = Db.sql;

                Db.cmd.Parameters.AddWithValue("ID", autoIDTextBox.Text.Trim());

                //Excecute Query
                int execute = Db.cmd.ExecuteNonQuery();

                if (execute != -1)
                {
                    MessageBox.Show("The data has been deleted " + dbCommand);
                    updateDataBinding();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Delete Data Error: " + ex.Message);
            }
            finally
            {
                dbCommand = "";
                Db.CloseConnection();
            }

        }

        private void bindingNavigatorRefreshData_Click(object sender, EventArgs e)
        {
            if(IsAddingNewRecord() == true)
            {
                return;
            }

            updateDataBinding();

            KeywordTextBox.Clear();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //change font color when checked
            if (findByIDRradioButton.Checked == true)
            {
                findByIDRradioButton.ForeColor = Color.Red;
            }
            else
            {
                findByIDRradioButton.ForeColor = Color.Black;
            }
        }

        private void FindByDivisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (FindByDivisionRadioButton.Checked == true)
            {
                FindByDivisionRadioButton.ForeColor = Color.Red;
                FindByDivisionComboBox.Enabled = true;
            }
            else
            {
                FindByDivisionRadioButton.ForeColor = Color.Black;
                FindByDivisionComboBox.Enabled = false;
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {

            if(IsAddingNewRecord() == true)
            {
                return;
            }

            Db.OpenConnection();

            //Search 
            try
            {
                if(findByIDRradioButton.Checked == false && FindByDivisionRadioButton.Checked == false)
                {
                    if (string.IsNullOrEmpty(KeywordTextBox.Text.Trim()))
                    {
                        updateDataBinding(null, searchButton);
                        return;
                    }
                }

                Db.sql = "SELECT Employees.*, Departments.DepartmentName " +
                        "FROM employees INNER JOIN Departments ON Employees.DepartmentID = " +
                        "Departments.DepartmentID ";

                //Find By Employee ID
                if(findByIDRradioButton.Checked == true)
                {
                    if (string.IsNullOrEmpty(KeywordTextBox.Text.Trim()))
                    {
                        MessageBox.Show("Please input an employee id");
                        return;

                    }
                    Db.sql += "WHERE Employees.AutoID = @Keyword1 ";
                    Db.sql += "ORDER BY Employees.AutoID ASC";
                }//"SELECT Employees.*, Departments.DepartmentName FROM employees INNER JOIN Departments ON Employees.DepartmentID = Departments.DepartmentID WHERE Employees.AutoID = @Keyword1 "

                //Find By Department
                else if(FindByDivisionRadioButton.Checked == true)
                {
                    if(FindByDivisionComboBox.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select the Department");
                        return;
                    }

                    Db.sql += "WHERE (Employees.FirstName LIKE @Keyword2 ";
                    Db.sql += "OR Employees.LastName LIKE @Keyword2 ";
                    Db.sql += "OR Employees.JobTitle LIKE @Keyword2) ";
                    Db.sql += "AND (Employees.DepartmentId = @Keyword1x) ";
                    Db.sql += "ORDER BY Employees.AutoID ASC";
                }
                //Search by Keyword Only
                else
                {
                    Db.sql += "WHERE (Employees.FirstName LIKE @Keyword2 ";
                    Db.sql += "OR Employees.LastName LIKE @Keyword2) ";
                    Db.sql += "OR Employees.Email = @Keyword1 ";
                    Db.sql += "OR Employees.JobTitle LIKE @Keyword2 ";
                    Db.sql += "OR Employees.Phone = @Keyword1 ";
                    Db.sql += "ORDER BY Employees.AutoID ASC";
                }

                Db.cmd.CommandType = CommandType.Text;
                Db.cmd.CommandText = Db.sql;

                Db.cmd.Parameters.Clear(); 

                Db.cmd.Parameters.AddWithValue("Keyword1", KeywordTextBox.Text.Trim());
                Db.cmd.Parameters.AddWithValue("Keyword1x", FindByDivisionComboBox.SelectedValue.ToString());
                //WildCard
                string keywordString = string.Format("%{0}%", KeywordTextBox.Text);
                Db.cmd.Parameters.AddWithValue("Keyword2", keywordString);

                updateDataBinding(Db.cmd, searchButton);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Search Error: " + ex.Message);
            }
            finally
            {
                Db.CloseConnection();
                if (KeywordTextBox.CanFocus)
                {
                    KeywordTextBox.Focus();
                }
                
            }
        }


    }
}
