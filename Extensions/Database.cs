using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Configuration;

namespace Extensions
{
    public class Database
    {
        public static readonly string CONNECTIONSTRING = ConfigurationManager.AppSettings["DatabaseConnectionString"];

        public static List<int> GetListInt(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }

                        var oReader = cmd.ExecuteReader();
                        List<int> oReturn = new List<int>();
                        while (oReader.Read())
                            oReturn.Add(oReader.GetInt32(0));


                        return oReturn;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T Find<T>(String query, List<OracleParameter> parameters = null, params Object[] values) where T : class
        {
            try
            {
                var result = Database.GetList<T>(query, parameters, values);
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T Find<T>(String query, List<OracleParameter> parameters = null, CultureInfo cultureGlobal = null) where T : class
        {
            try
            {
                Object[] values = new Object[] { cultureGlobal };
                var result = Database.GetList<T>(query, parameters, values);
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> GetList<T>(String query) where T : class
        {
            try
            {
                List<OracleParameter> parameters = null;
                return Database.GetList<T>(query, parameters);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> GetList<T>(String query, List<OracleParameter> parameters) where T : class
        {
            try
            {
                Object[] values = new Object[] { null };
                return Database.GetList<T>(query, parameters, values);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> GetList<T>(String query, List<OracleParameter> parameters = null, params Object[] values) where T : class
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }

                        var oReader = cmd.ExecuteReader();
                        var lReturn = oReader.Select(reader => (T)Activator.CreateInstance(typeof(T), new Object[] { reader, values }));
                        return lReturn.ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int Execute(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }

                        //Returns the numbers of rows affected
                        var result = cmd.ExecuteNonQuery();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int ExecuteInsert(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }
                        cmd.Parameters.Add(":ID", OracleDbType.Decimal, ParameterDirection.Output);

                        cmd.ExecuteNonQuery();

                        var result = cmd.Parameters[":ID"].Value.TryParseAsInt();

                        return result;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int ExecuteScalar(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }

                        var result = cmd.ExecuteScalar();
                        if (result == DBNull.Value)
                            return 0;
                        else
                        {
                            result = (result ?? "0");
                            return int.Parse(result.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ExecuteScalarString(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }

                        var result = cmd.ExecuteScalar();
                        if (result == DBNull.Value)
                            return string.Empty;
                        else
                        {
                            result = (result ?? string.Empty);
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool ExecuteNonQuery(String query, List<OracleParameter> parameters = null)
        {
            try
            {
                using (var conn = new OracleConnection(CONNECTIONSTRING))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        cmd.CommandText = query;
                        cmd.Parameters.Clear();
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                                cmd.Parameters.Add(param);
                        }
                        //Returns the numbers of rows affected.
                        var result = cmd.ExecuteNonQuery();
                        return (result != 0);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
