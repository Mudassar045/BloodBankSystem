using System.Data.SqlClient;

namespace BloodBankSystem
{
    class AddPatient
    {
        private string mFirstName;
        private string mLastName;
        private string mGender;
        private string mContact;
        private string mAddress;
        private string mBloodGroup;
        private string mCity;
        private string mCNIC;
        private int mAge;
        private string mEmail;
        public AddPatient(string firstname, string lastname,
            string gender, string cnic, string contact,
            string bloodgroup, string address,
            string city, int age,
            string email)
        {
            mFirstName = firstname;
            mLastName = lastname;
            mGender = gender;
            mContact = contact;
            mCNIC = cnic;
            mBloodGroup = bloodgroup;
            mAddress = address;
            mCity = city;
            mAge = age;
            mEmail = email;
        }
        public void InsertIntoDatabase()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bbmsdatabase.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter adap = new SqlDataAdapter();
            adap.InsertCommand = new SqlCommand("Insert Patient values(@FName,@LName,@Gender,@CNIC,@BloodGroup,@Age,@Contact,@Address,@City,@Email)", con);
            adap.InsertCommand.Parameters.AddWithValue("@FName", mFirstName);
            adap.InsertCommand.Parameters.AddWithValue("@LName", mLastName);
            adap.InsertCommand.Parameters.AddWithValue("@Gender", mGender);
            adap.InsertCommand.Parameters.AddWithValue("@CNIC", mCNIC);
            adap.InsertCommand.Parameters.AddWithValue("@BloodGroup", mBloodGroup);
            adap.InsertCommand.Parameters.AddWithValue("@Age", mAge);
            adap.InsertCommand.Parameters.AddWithValue("@Contact", mContact);
            adap.InsertCommand.Parameters.AddWithValue("@Address", mAddress);
            adap.InsertCommand.Parameters.AddWithValue("@City", mCity);
            adap.InsertCommand.Parameters.AddWithValue("@Email", mEmail);
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
