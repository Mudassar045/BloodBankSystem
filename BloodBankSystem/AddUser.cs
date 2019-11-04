using System;
using System.Data.SqlClient;
namespace BloodBankSystem
{
    public class AddUser
    {
        private string mFirstName;
        private string mLastName;
        private string mUsername;
        private string mGender;
        private string mContact;
        private string mAddress;
        private string mBloodGroup;
        private string mPassword;
        private string mDOB;
        private string mEmail;
        public AddUser(string firstname, string lastname,
            string username, string password,
            string bloodgroup,string gender, string email,
            string dob, string contact,
            string address)
        {
            mFirstName = firstname;
            mLastName = lastname;
            mUsername = username;
            mGender = gender;
            mContact = contact;
            mPassword = password;
            mBloodGroup = bloodgroup;
            mAddress = address;
            mEmail = email;
            mDOB = dob;
        }
        public void InsertIntoDatabase()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bbmsdatabase.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter adap = new SqlDataAdapter();
            adap.InsertCommand = new SqlCommand("Insert login values(@FName,@LName,@Username,@Password, @BloodGroup,@Gender,@Email,@DOB,@Contact,@Address)", con);
            adap.InsertCommand.Parameters.AddWithValue("@FName", mFirstName);
            adap.InsertCommand.Parameters.AddWithValue("@LName", mLastName);
            adap.InsertCommand.Parameters.AddWithValue("@Username", mUsername);
            adap.InsertCommand.Parameters.AddWithValue("@Password", Cryptor.Encrypt(mPassword, true, "lynxbbms"));
            adap.InsertCommand.Parameters.AddWithValue("@BloodGroup",mBloodGroup);
            adap.InsertCommand.Parameters.AddWithValue("@Gender", mGender);
            adap.InsertCommand.Parameters.AddWithValue("@Email", mEmail);
            adap.InsertCommand.Parameters.AddWithValue("@DOB",DateTime.Now.Date);
            adap.InsertCommand.Parameters.AddWithValue("@Contact",mContact);
            adap.InsertCommand.Parameters.AddWithValue("@Address",mAddress);
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
        public string GetDataInsertionException { get; set; }
        public bool IsInsertedData { get; set; }

    }
}
