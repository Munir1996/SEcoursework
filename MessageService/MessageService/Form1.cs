using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;//use file stream


namespace MessageService
{
    public partial class Form1 : Form
    {
        private OleDbConnection con = new OleDbConnection();
        public Form1()
        {
            InitializeComponent();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Messages.mdb; Persist Security Info=False;";//connection to the access database
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();//command for connecting
                MessageBox.Show("Connection Successful");
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            OleDbCommand com = new OleDbCommand();
            com.Connection = con;
            string query = "select * from tbRecords";//select the whole table
            com.CommandText = query;

            OleDbDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                cbDatabase.Items.Add(reader["Type"].ToString());
            }

        }

        //add new message and record
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbCommand com = new OleDbCommand();
                com.Connection = con;
                com.CommandText = "insert into tbRecords (Type, Subject, Contact, Message) values('" + txtID.Text + "','" + txtSubject.Text + "','" + txtContact.Text + "','" + txtMessage.Text + "')";//insert data in to the columns

                com.ExecuteNonQuery();
                MessageBox.Show("Message Saved");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex);//shows error message and what is wrong
            }

            //create a json file
            try
            {
                SMS text = new SMS();
                text._ID = txtID.Text;
                text._Sender = txtContact.Text;
                text._Body = txtMessage.Text;

                DataContractJsonSerializer data = new DataContractJsonSerializer(typeof(SMS));
                var FilePath = @"D:\Uni\Software Engineering\CourseWork\Work\MessageService\json" + txtID.Text + ".json";
                using (FileStream stream = File.Create(FilePath))
                {
                    data.WriteObject(stream, text);
                }
                using (FileStream fs = File.OpenRead(FilePath))
                {
                    SMS mS = (SMS)data.ReadObject(fs);
                    txtID.Text = mS._ID;
                    txtMessage.Text = mS._Body;
                    txtContact.Text = mS._Sender;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //clear textbox
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtContact.Clear();
            txtID.Clear();
            txtMessage.Clear();
            txtSubject.Clear();            
        }

        //shows the id for each record in database, when selected the rest of the values are displayed.
        private void cbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand com = new OleDbCommand();
            com.Connection = con;
            string query = "select * from tbRecords where Type='" + cbDatabase.Text + "'";
            com.CommandText = query;

            OleDbDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                txtID.Text = reader["Type"].ToString();
                txtMessage.Text = reader["Message"].ToString();
                txtSubject.Text = reader["Subject"].ToString();
                txtContact.Text = reader["Contact"].ToString();
            }
        }
    }
}
