using System;
using System.Data.SqlClient;
namespace BloodBankSystem
{
    class AddBloodBag
    {
        private int mDonorId;
        private string mDonorName;
        private string mDonorBloodGroup;
        private int mBagsQuantity;

        public AddBloodBag(int id, string name, string bloodgroup, int quantity)
        {
            mDonorId = id;
            mDonorName = name;
            mDonorBloodGroup = bloodgroup;
            mBagsQuantity = quantity;
        }
        public void InsertIntoDatabase()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bbmsdatabase.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter adap = new SqlDataAdapter();
            adap.InsertCommand = new SqlCommand("INSERT BloodStock VALUES(@DonorId,@DonorName,@BloodGroup,@Quantity, @DonationDate,@ExpiryDate)", con);
            adap.InsertCommand.Parameters.AddWithValue("DonorId", mDonorId);
            adap.InsertCommand.Parameters.AddWithValue("DonorName",mDonorName);
            adap.InsertCommand.Parameters.AddWithValue("BloodGroup",mDonorBloodGroup);
            adap.InsertCommand.Parameters.AddWithValue("Quantity", mBagsQuantity);
            adap.InsertCommand.Parameters.AddWithValue("DonationDate", DateTime.Now.Date);
            adap.InsertCommand.Parameters.AddWithValue("ExpiryDate", DateTime.Now.Date.AddDays(20));
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
