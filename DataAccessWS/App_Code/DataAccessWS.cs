using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SimpleEncryption;
using System.IO;


/// <summary>
/// Summary description for DataAccessWS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
 
public class DataAccessWS : System.Web.Services.WebService {
    // **** SHA/MDE Data Transfer App: Screening
    //private string strEncryptionKey = "yv3477c2rerdg76"; //"FrankTheTank";

    // **** MESGIS.COM
    private string strEncryptionKey = "bb*Kasd%@(89asdflkj1lsdfn";

    // **** The Archives
    //private string strEncryptionKey = "cKwlwsd%@(63alkj1kjasdfo";

    public DataAccessWS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello MES GIS";
    }

    [WebMethod]
    public bool CheckConnectionWS(string strConnection)
    {
        bool bolCanConnect = false;

        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            bolCanConnect = true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }

        return bolCanConnect;
    }


    [WebMethod]
    public DataSet SelectCommandWS(string strConnection, string strCommand, List<SqlParameter> sqlParameters)
    {
        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                sqlCmd.Parameters.AddWithValue(sqlParameters[i].ParameterName, sqlParameters[i].Value);
            }

            DataSet ds = new DataSet();
            SqlDataAdapter sqlDA = new SqlDataAdapter(sqlCmd);
            sqlDA.Fill(ds);

            return ds;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message + "---  " + SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));
        }
        finally
        {
            if(sqlConn.State==ConnectionState.Open) sqlConn.Close();
        }
    }

    [WebMethod]
    public string InsertCommandWS(string strConnection, string strCommand, List<SqlParameter> sqlParameters)
    {
        bool bolInserted = false;

        SqlParameter sqlParamOutput = new SqlParameter();
    
        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                if (sqlParameters[i].Direction == ParameterDirection.Output)
                {
                    sqlParamOutput.ParameterName = sqlParameters[i].ParameterName;
                    sqlParamOutput.Direction = ParameterDirection.Output;
                    sqlParamOutput.Value = sqlParameters[i].Value;

                    sqlCmd.Parameters.Add(sqlParamOutput);                    
                }
                else
                    sqlCmd.Parameters.AddWithValue(sqlParameters[i].ParameterName, sqlParameters[i].Value);
            }

            bolInserted = sqlCmd.ExecuteNonQuery()>0? true:false;

            return sqlParamOutput.Value.ToString();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }
    }

    [WebMethod]
    public bool UpdateCommandWS(string strConnection, string strCommand, List<SqlParameter> sqlParameters)
    {
        bool bolUpdated = false;

        SqlParameter sqlParamOutput = new SqlParameter();

        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                if (sqlParameters[i].Direction == ParameterDirection.Output)
                {
                    sqlParamOutput.ParameterName = sqlParameters[i].ParameterName;
                    sqlParamOutput.Direction = ParameterDirection.Output;
                    sqlParamOutput.Value = sqlParameters[i].Value;

                    sqlCmd.Parameters.Add(sqlParamOutput);
                }
                else
                    sqlCmd.Parameters.AddWithValue(sqlParameters[i].ParameterName, sqlParameters[i].Value);
            }

            sqlCmd.ExecuteNonQuery();

            bolUpdated = Int32.Parse(sqlParamOutput.Value.ToString()) > 0 ? true : false;

            return bolUpdated;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }
    }

    [WebMethod]
    public int SaveCommandWS(string strConnection, string strCommand, List<SqlParameter> sqlParameters)
    {
        int intIdent;

        SqlParameter sqlParamOutput = null;

        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                if (sqlParameters[i].Direction == ParameterDirection.Output)
                {
                    sqlParamOutput = new SqlParameter();

                    sqlParamOutput.ParameterName = sqlParameters[i].ParameterName;
                    sqlParamOutput.Direction = sqlParameters[i].Direction;
                    sqlParamOutput.DbType = sqlParameters[i].DbType;
                    sqlParamOutput.Value = sqlParameters[i].Value;

                    sqlCmd.Parameters.Add(sqlParamOutput);
                }
                else
                    sqlCmd.Parameters.AddWithValue(sqlParameters[i].ParameterName, sqlParameters[i].Value);
            }

            sqlCmd.ExecuteNonQuery();

            intIdent = Int32.Parse(sqlParamOutput.Value.ToString());

            return intIdent;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }
    }
    
    [WebMethod]
    public bool DeleteCommandWS(string strConnection, string strCommand, List<SqlParameter> sqlParameters)
    {
        bool bolDeleted = false;

        SqlParameter sqlParamOutput = new SqlParameter();

        SqlConnection sqlConn;
        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));

        try
        {
            sqlConn.Open();
            SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < sqlParameters.Count; i++)
            {
               if (sqlParameters[i].Direction == ParameterDirection.Output)
                {
                    sqlParamOutput.ParameterName = sqlParameters[i].ParameterName;
                    sqlParamOutput.Direction = ParameterDirection.Output;
                    sqlParamOutput.Value = sqlParameters[i].Value;

                    sqlCmd.Parameters.Add(sqlParamOutput);
                }
                else
                    sqlCmd.Parameters.AddWithValue(sqlParameters[i].ParameterName, sqlParameters[i].Value);            }

            sqlCmd.ExecuteNonQuery();

            if (sqlParamOutput.Value != null)
                bolDeleted = Int32.Parse(sqlParamOutput.Value.ToString()) > 0 ? true : false;
            else
                bolDeleted = true;

            return bolDeleted;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }
    }

    [WebMethod]
    public void WebSyncTableDataUpload(int intWebSyncLogID, string strSourceSQLServer, string strSourceDatabase,
                                      string strMasterConnection, string strTableName, DataSet dsNewOrChanged)
    {
        const string STATUS_COMPLETED = "COMPLETED";
        DateTime DT_START = DateTime.Now;

        int iRowsInserted = 0, iRowsUpdated = 0;
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlCmd;
        SqlParameter sqlParamOut;

        try
        {
            sqlConn.ConnectionString = SimpleEncryption.Crypto.Decrypt(strMasterConnection, strEncryptionKey);
            sqlConn.Open();

            sqlCmd = new SqlCommand("InsertWebSyncLogDetail", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlParamOut = new SqlParameter();
            sqlParamOut.ParameterName = "@WebSyncLogDetailID"; sqlParamOut.Direction = ParameterDirection.Output; sqlParamOut.DbType = DbType.Int32; sqlParamOut.Value = 0;
            sqlCmd.Parameters.Add(sqlParamOut);

            sqlCmd.Parameters.AddWithValue("@WebSyncLogID", intWebSyncLogID);
            sqlCmd.Parameters.AddWithValue("@SourceSQLServer", strSourceSQLServer);
            sqlCmd.Parameters.AddWithValue("@SourceDatabase", strSourceDatabase);
            sqlCmd.Parameters.AddWithValue("@DestinationSQLServer", strMasterConnection);
            sqlCmd.Parameters.AddWithValue("@DestinationDataBase", strMasterConnection);
            sqlCmd.Parameters.AddWithValue("@ObjectType", "Table");
            sqlCmd.Parameters.AddWithValue("@ObjectName", strTableName);
            sqlCmd.Parameters.AddWithValue("@ActionType", "Upload");
            sqlCmd.Parameters.AddWithValue("@StartTime", DT_START);
            sqlCmd.Parameters.AddWithValue("@RowsTotal", dsNewOrChanged.Tables[0].Rows.Count);
            sqlCmd.Parameters.AddWithValue("@RowsInserted", 0);
            sqlCmd.Parameters.AddWithValue("@RowsUpdated", 0);
            sqlCmd.Parameters.AddWithValue("@RowsSkipped", 0);
            sqlCmd.Parameters.AddWithValue("@EndTime", "");
            sqlCmd.Parameters.AddWithValue("@Status", "");

            sqlCmd.ExecuteNonQuery();
            int intWebSyncLogDetailID = Int32.Parse(sqlParamOut.Value.ToString());

            foreach (DataRow dr in dsNewOrChanged.Tables[0].Rows)
            {
                sqlCmd = new SqlCommand("websyncMerge" + strTableName.Trim(), sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param;
                foreach (DataColumn dc in dsNewOrChanged.Tables[0].Columns)
                {
                    if(dc.DataType.Name.ToUpper()=="BYTE[]" && dc.ColumnName.ToUpper().IndexOf("SIGNATURE")>=0)
                    {
                        param = new SqlParameter("@" + dc.ColumnName, SqlDbType.Image);
                        param.Value = dr[dc.ColumnName];
                    }
                    else
                        param = new SqlParameter("@" + dc.ColumnName, dr[dc.ColumnName]);
                    sqlCmd.Parameters.Add(param);
                }
                string strIorU = (string)sqlCmd.ExecuteScalar();

                if (strIorU.IndexOf("I") >= 0) iRowsInserted++;
                if (strIorU.IndexOf("U") >= 0) iRowsUpdated++;
            }

            sqlCmd = new SqlCommand("UpdateWebSyncLogDetail", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@WebSyncLogDetailID", intWebSyncLogDetailID);
            sqlCmd.Parameters.AddWithValue("@WebSyncLogID", intWebSyncLogID);
            sqlCmd.Parameters.AddWithValue("@SourceSQLServer", strSourceSQLServer);
            sqlCmd.Parameters.AddWithValue("@SourceDatabase", strSourceDatabase);
            sqlCmd.Parameters.AddWithValue("@DestinationSQLServer", strMasterConnection);
            sqlCmd.Parameters.AddWithValue("@DestinationDataBase", strMasterConnection);
            sqlCmd.Parameters.AddWithValue("@ObjectType", "Table");
            sqlCmd.Parameters.AddWithValue("@ObjectName", strTableName);
            sqlCmd.Parameters.AddWithValue("@ActionType", "Upload");
            sqlCmd.Parameters.AddWithValue("@StartTime", DT_START);
            sqlCmd.Parameters.AddWithValue("@RowsTotal", dsNewOrChanged.Tables[0].Rows.Count);
            sqlCmd.Parameters.AddWithValue("@RowsInserted", iRowsInserted);
            sqlCmd.Parameters.AddWithValue("@RowsUpdated", iRowsUpdated);
            sqlCmd.Parameters.AddWithValue("@RowsSkipped", 0);
            sqlCmd.Parameters.AddWithValue("@EndTime", DateTime.Now);
            sqlCmd.Parameters.AddWithValue("@Status", STATUS_COMPLETED);

            sqlParamOut = new SqlParameter();
            sqlParamOut.ParameterName = "@RowsCount"; sqlParamOut.Direction = ParameterDirection.Output; sqlParamOut.DbType = DbType.Int32; sqlParamOut.Value = 0;
            sqlCmd.Parameters.Add(sqlParamOut);

            sqlCmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
        }

        return ;
    }


    [WebMethod]
    public string GetFileList(string strConnection, string strDir )
    {

        SqlConnection sqlConn;
        String strFileList;
        String s;

        sqlConn = new SqlConnection(SimpleEncryption.Crypto.Decrypt(strConnection, strEncryptionKey));
        strFileList = "";
        s = "";

        try
        {
    
            sqlConn.Open();     //We just need to make sure we have a valid connection for security, we're not actually going to do anything with it
            sqlConn.Close();


            if (Directory.Exists(strDir))
            {
                        // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(strDir);
                foreach (string fileName in fileEntries)
                {
                    s = fileName.Replace(strDir, "");
                    strFileList += s + ";";
                }

            }
            else
            {   throw new Exception("Directory does not exist: '" + strDir + "'"); }



            return strFileList;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
        }
    }
    
}

