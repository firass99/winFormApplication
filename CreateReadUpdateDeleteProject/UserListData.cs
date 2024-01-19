using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CreateReadUpdateDelete
{
    class UserListData
    {
        SqlConnection connect
            = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mega-PC\Documents\winapp.mdf;Integrated Security=True;Connect Timeout=30");
        public int ID { set; get; } // 0
        public string Name { set; get; } // 1
        public string sexe { set; get; } // 2
        public string dep { set; get; } // 3
        public string Contact { set; get; } // 4
        public string Email { set; get; } // 5
        public string age { set; get; } // 6

        public List<UserListData> getListData()
        {
            List<UserListData> listData = new List<UserListData>();

            if(connect.State == ConnectionState.Closed)
            {
                try
                {
                    connect.Open();
                    string selectData = "SELECT * FROM users";

                    using(SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            UserListData uld = new UserListData();
                            uld.ID = (int)reader["id"];
                            uld.Name = reader["full_Name"].ToString();
                            uld.sexe = reader["sexe"].ToString();
                            uld.dep = reader["dep"].ToString();
                            uld.Contact = reader["contact"].ToString();
                            uld.Email = reader["email"].ToString();
                            uld.age = reader["age"].ToString();

                            listData.Add(uld);
                        }

                        reader.Close();
                    }

                }catch(Exception ex)
                {
                    Console.WriteLine("Erreur: " + ex);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;
        }
    }
}
