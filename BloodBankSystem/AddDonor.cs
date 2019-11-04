using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
namespace BloodBankSystem
{
    class AddDonor
    {
        private string mFirstName;
        private string mLastName;
        private string mGender;
        private string mContact;
        private string mAddress;
        private string mBloodGroup;
        private string mCity;
        private int mAge;
        private string mCNIC;
        private int mWeight;
        public AddDonor(string firstname, string lastname,
            string gender, string cnic,string contact,
            string bloodgroup, string address,
            string city, int age,
            int weight)
        {
            mFirstName = firstname;
            mLastName = lastname;
            mGender = gender;
            mCNIC = cnic;
            mContact = contact;
            mBloodGroup = bloodgroup;
            mAddress = address;
            mCity = city;
            mAge = age;
            mWeight = weight;
        }
        public void InsertIntoDatabase()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bbmsdatabase.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter adap = new SqlDataAdapter();
            adap.InsertCommand = new SqlCommand("Insert Donor values(@FName,@LName,@Gender,@CNIC,@BloodGroup,@Age,@Contact,@Address,@City,@Weight)", con);
            adap.InsertCommand.Parameters.AddWithValue("@FName", mFirstName);
            adap.InsertCommand.Parameters.AddWithValue("@LName", mLastName);
            adap.InsertCommand.Parameters.AddWithValue("@Gender", mGender);
            adap.InsertCommand.Parameters.AddWithValue("@CNIC", mCNIC);
            adap.InsertCommand.Parameters.AddWithValue("@BloodGroup", mBloodGroup);
            adap.InsertCommand.Parameters.AddWithValue("@Age", mAge);
            adap.InsertCommand.Parameters.AddWithValue("@Contact", mContact);
            adap.InsertCommand.Parameters.AddWithValue("@Address", mAddress);
            adap.InsertCommand.Parameters.AddWithValue("@City", mCity);
            adap.InsertCommand.Parameters.AddWithValue("@Weight", mWeight);
            try
            {
                con.Open();
                int pQueryResult = adap.InsertCommand.ExecuteNonQuery();
                con.Close();
                if (pQueryResult == 1)
                {
                    IsInsertedData = true;
                }
                else
                {
                    IsInsertedData = false;
                }
            }
            catch (SqlException ex)
            {
                con.Close();
                GetDataInsertionException = ex.Message;
            }
        }
        public string GetDataInsertionException{ get; set; }
        public bool IsInsertedData { get; set; }
    }
}
