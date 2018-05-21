using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ContentLibrary
{
    public enum InsertResult
    {
        OK,
        WRONG_AGE,
        DRAW_LIMIT_REACHED
    }
    public class DataAccess
    {
        private static DataAccess instance = null;
        private static readonly object padLock = new object();

        static List<int> serialNumbersList = new List<int>();
        static Dictionary<string, Customer> customersList = new Dictionary<string, Customer>();
        static List<Submission> submissionsList = new List<Submission>();

        private static string connectionString = @"Server=DESKTOP-LJVHKR0\SQLEXPRESS;Database=AcmeLanderDB;Trusted_Connection=true";

        public DataAccess Instance
        {
            get
            {
                lock (padLock)
                {
                    if (instance == null)
                    {
                        instance = new DataAccess();
                    }
                    return instance;
                }
            }
        }


        public static List<int> GetSerialNumbers()
        {
            serialNumbersList.Clear();
            FileInfo fileInfo = new FileInfo(@"serialNumbers.txt");
            if (fileInfo.Exists)
            {
                FileStream fs = fileInfo.OpenRead();
                StreamReader sr = new StreamReader(fs);
                while (sr.Peek() != -1)
                {
                    serialNumbersList.Add(Convert.ToInt32(sr.ReadLine()));
                }

                sr.Close();
                fs.Close();
            }

            return serialNumbersList;
        }

        public static void GetStarted()
        {
            submissionsList.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmdInitial = new SqlCommand("SP_Initial", connection);
                    cmdInitial.CommandType = CommandType.StoredProcedure;
                    cmdInitial.ExecuteNonQuery();
                }
                catch (SqlException e)
                {

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public static Dictionary<string, Customer> GetCustomers()
        {
            customersList.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmdGetCustomers = new SqlCommand("SP_GetCustomers", connection);
                    cmdGetCustomers.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmdGetCustomers.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                reader["Email"].ToString(),
                                Convert.ToInt32(reader["Age"]));
                            customersList.Add(reader["Email"].ToString(), customer);
                        }
                    }
                }
                catch (SqlException e)
                {

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return customersList;
        }

        public static bool CheckAge(string email)
        {
            bool isAllowed = true;

            if (customersList.Count == 0)
            {
                GetCustomers();
            }

            if (customersList.ContainsKey(email))
            {
                if (customersList[email].Age < 18)
                {
                    isAllowed = false;
                }
            }

            return isAllowed;
        }

        public static int GetCountFromMatch(string email, int productNr)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmdGetCount = new SqlCommand("SP_GetCountMatched", connection);
                    cmdGetCount.CommandType = CommandType.StoredProcedure;
                    cmdGetCount.Parameters.Add(new SqlParameter("@email", email));
                    cmdGetCount.Parameters.Add(new SqlParameter("@productNr", productNr));

                    SqlDataReader reader = cmdGetCount.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        count = reader.GetInt32(0);
                    }

                }
                catch (SqlException e)
                {

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return count;
        }

        public static InsertResult InsertSubmission(Submission submission)
        {
            if (CheckAge(submission.Email) == false)
                return InsertResult.WRONG_AGE;

            if (GetCountFromMatch(submission.Email, submission.ProductSerialNr) >= 2)
                return InsertResult.DRAW_LIMIT_REACHED;

            submissionsList.Add(submission);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmdInsertSubm = new SqlCommand("SP_InsertSubmission", connection);
                    cmdInsertSubm.CommandType = CommandType.StoredProcedure;
                    cmdInsertSubm.Parameters.Add(new SqlParameter("@FirstName", submission.FirstName));
                    cmdInsertSubm.Parameters.Add(new SqlParameter("@LastName", submission.LastName));
                    cmdInsertSubm.Parameters.Add(new SqlParameter("@Email", submission.Email));
                    cmdInsertSubm.Parameters.Add(new SqlParameter("@ProductSerialNr", submission.ProductSerialNr));
                    cmdInsertSubm.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    if (e.Number == 2627)
                    {
                        Console.WriteLine("Someone has already taken this prize!");
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return InsertResult.OK;
        }

        public static List<Submission> GetSubmissions()
        {
            submissionsList.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmdGetSubm = new SqlCommand("SP_GetSubmissions", connection);
                    cmdGetSubm.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmdGetSubm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            Submission submission = new Submission(
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                reader["Email"].ToString(),
                                Convert.ToInt32(reader["ProductSerialNr"]));
                            submissionsList.Add(submission);
                        }
                    }
                }
                catch (SqlException e)
                {

                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return submissionsList;
        }
    }
}
