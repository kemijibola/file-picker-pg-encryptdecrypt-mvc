using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Data
{
    public class DBManager
    {
        public DBManager(string connectionString)
        {
            _dbFactory = new DatabaseHandlerFactory(connectionString);
            _database = _dbFactory.CreateDatabase();
            _providerName = _dbFactory.GetProviderName();
        }

        private DatabaseHandlerFactory _dbFactory;
        private IDatabaseHandler _database;
        private string _providerName;

        public DbConnection GetDatabaseConnection()
        {
            return _database.CreateConnection();
        }

        public void CloseConnection(DbConnection connection)
        {
            _database.CloseConnection(connection);
        }

        
        public IDbDataParameter CreateParameter(string name, OracleDbType dbType, object value)
        {
            return DataParameterManager.CreateParameter(_providerName, name, dbType, value);
        }
        public IDbDataParameter CreateParameter(string name, object value, OracleDbType dbType,ParameterDirection direction)
        {
            return DataParameterManager.CreateParameterArray(_providerName, name, dbType, value,direction);
        }
        public IDbDataParameter CreateParameter(string name, int size,object value, OracleDbType dbType, ParameterDirection direction)
        {
            return DataParameterManager.CreateParameterDbTypeWithSize(_providerName, name, size, dbType, value, direction);
        }
        public IDbDataParameter CreateParameter(string name, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(_providerName, name, value, dbType, ParameterDirection.Input);
        }
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return DataParameterManager.CreateParameter(_providerName, name, size, value, dbType, ParameterDirection.Input);
        }
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return DataParameterManager.CreateParameter(_providerName, name, size, value, dbType, direction);
        }
        
        public DataTable GetDataTable(string commandText, CommandType commandType, int commandTimeout, List<IDbDataParameter> parameters = null)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    var dataset = new DataSet();
                    var dataAdaper = _database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);   
                    return dataset.Tables[0];
                }
            }
        }



        public DataSet GetDataSet(string commandText, CommandType commandType, int commandTimeout, IDbDataParameter[] parameters = null)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    var dataset = new DataSet();
                    var dataAdaper = _database.CreateAdapter(command);
                    dataAdaper.Fill(dataset);
                    return dataset;
                }
            }
        }

        public void BulkCopyWriteToServer(DataTable data, string tableName)
        {

                using (var connection = _database.CreateConnectionForBulkCopy())
                {
                    try
                    {   
                        connection.Open();
                        using (var bulkCopy = new OracleBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            bulkCopy.WriteToServer(data);
                        }
                    }
                    catch(Exception ex)
                    {
                         throw ex;
                    }
                    finally
                    {
                         connection.Close();
                    }

                }

        }


        public DbCommand GetExecuteNonQueryArray(string commandText, CommandType commandType, List<IDbDataParameter> parameters,int arraySize, int commandTimeout, out DbConnection connection)
        {
            connection = _database.CreateConnection();
            connection.Open();
            var command = _database.CreateCommandArray(commandText, commandType, connection, arraySize, commandTimeout);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            command.ExecuteReader();

            return command;
        }

        
        public DbCommand GetExecuteNonQuery(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout, out DbConnection connection)
        {
            connection = _database.CreateConnection();
            connection.Open();
            var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            command.ExecuteReader();

            return command;
        }

        public DbDataReader GetDataReader(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout, out DbConnection connection)
        {
            DbDataReader reader = null;
            connection = _database.CreateConnection();
            connection.Open();
            var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            reader = command.ExecuteReader();

            return reader;
        }

        public void Delete(string commandText, CommandType commandType, int commandTimeout, IDbDataParameter[] parameters = null)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }
        public int Insert(string commandText, CommandType commandType, List<IDbDataParameter> parameters,int commandTimeout, out int lastId)
        {
            lastId = 0;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt32(newId);
                }
            }
            return lastId;
        }
        public long Insert(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout, out long lastId)
        {
            lastId = 0;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }
            return lastId;
        }

        public void InsertWithTransaction(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout)
        {
            DbTransaction transactionScope = null;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, List<IDbDataParameter> parameters, int commandTimeout)
        {
            IDbTransaction transactionScope = null;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public void Update(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }
        public DbCommand UpdateWithTransaction(string commandText, CommandType commandType, List<IDbDataParameter> parameters, int commandTimeout)
        {
            IDbTransaction transactionScope = null;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteReader();
                        transactionScope.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        command.Parameters["P_ERROR_MSG"].Value = ex.Message;
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return command;
                }
            }
        }
        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, List<IDbDataParameter> parameters, int commandTimeout)
        {
            IDbTransaction transactionScope = null;
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public object GetScalarValue(string commandText, CommandType commandType, int commandTimeout, List<IDbDataParameter> parameters = null)
        {
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var command = _database.CreateCommand(commandText, commandType, connection, commandTimeout))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    return command.ExecuteScalar();
                }
            }
        }
    }
}
