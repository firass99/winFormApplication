using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using CreateReadUpdateDelete;

namespace CreateReadUpdateDelete
{
    public partial class Form1 : Form
    {
        SqlConnection connect
            = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mega-PC\Documents\winapp.mdf;Integrated Security=True;Connect Timeout=30");
        public Form1()
        {
            InitializeComponent();

            // afficher infos grid view
            displayData();
        }

        public void displayData()
        {
            UserListData uld = new UserListData();
            //add list to gridview
            List<UserListData> listData = uld.getListData();
            dataGridView1.DataSource = listData;
        }

        //btn exit
        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if(full_name.Text == "" || sexe.Text == "" || dep.Text == "" || contact.Text == "" || email.Text == "")
            {
                MessageBox.Show("Champs Vide", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(connect.State != ConnectionState.Open)
                {
                    try
                    {
                        // TO GET THE DATE TODAY
                        DateTime today = DateTime.Today;
                        connect.Open();

                        string insertData = "INSERT INTO users " +
                            "(full_Name, sexe,dep, contact, email, age, date_insert) " +
                            "VALUES(@fullName, @sexe,@dep, @contact, @email, @age, @dateInsert)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@fullName", full_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@sexe", sexe.Text.Trim());
                            cmd.Parameters.AddWithValue("@contact", contact.Text.Trim());
                            cmd.Parameters.AddWithValue("@dep", dep.Text.Trim());
                            cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@age", age.Value);
                            cmd.Parameters.AddWithValue("@dateInsert", today);

                            cmd.ExecuteNonQuery();

                            // affichage gridview   
                            displayData();

                            MessageBox.Show("Etudiant Ajouter Avec Succes!", "Information Message"
                                , MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // rest fields
                            clearFields();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Erreur: " + ex, "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }


        
        public void clearFields()
        {
            full_name.Text = "";
            sexe.SelectedIndex = -1;
            dep.SelectedIndex = -1;
            contact.Text = "";
            email.Text = "";
        }

        private int tempID = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                tempID = (int)row.Cells[0].Value;
                full_name.Text = row.Cells[1].Value.ToString();
                sexe.Text = row.Cells[2].Value.ToString();
                dep.Text = row.Cells[3].Value.ToString();
                contact.Text = row.Cells[4].Value.ToString();
                email.Text = row.Cells[5].Value.ToString();
                age.Text = row.Cells[6].Value.ToString();
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (full_name.Text == ""
                || sexe.Text == ""
                || dep.Text == ""
                || contact.Text == ""
                || email.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Confirmer la modification de : "
                    + tempID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            // TO GET THE DATE TODAY
                            DateTime today = DateTime.Today;
                            connect.Open();

                            string updateData = "UPDATE users SET " +
                                "full_name = @fullName, sexe = @sexe,  dep = @dep, " +
                                "contact = @contact, email = @email, " +
                                "age = @age, date_update = @dateUpdate " +
                                "WHERE id = @id";

                            using(SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@fullName", full_name.Text.Trim());
                                cmd.Parameters.AddWithValue("@sexe", sexe.Text.Trim());
                                cmd.Parameters.AddWithValue("@dep", dep.Text.Trim());
                                cmd.Parameters.AddWithValue("@contact", contact.Text.Trim());
                                cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                                cmd.Parameters.AddWithValue("@age", age.Value);
                                cmd.Parameters.AddWithValue("@dateUpdate", today);
                                cmd.Parameters.AddWithValue("@id", tempID);

                                cmd.ExecuteNonQuery();

                                // TO DISPLAY THE DATA  
                                displayData();

                                MessageBox.Show("Mise à jour avec succes!", "Information Message"
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // TO CLEAR ALL FIELDS
                                clearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Annulé.", "Inforamtion Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (full_name.Text == ""
                || sexe.Text == ""
                || dep.Text == ""
                || contact.Text == ""
                || email.Text == "")
            {
                MessageBox.Show("Verifier les champs", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Confirmer la suppression de: "
                    + tempID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            // datetime
                            DateTime today = DateTime.Today;
                            connect.Open();

                            string updateData = "DELETE FROM users WHERE id = @id";

                            using (SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@id", tempID);

                                cmd.ExecuteNonQuery();

                                // afficher info 
                                displayData();

                                MessageBox.Show("Supprimer avec succes!", "Information Message"
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // reset champs
                                clearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Annulé.", "Inforamtion Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }





        //labels
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click_1(object sender, EventArgs e)
        {

        }
    }
}
