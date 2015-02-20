namespace QuilmesAC.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Reflection;
    using StackExchange.Profiling.Data;

    public static class MiniProfilerHelper
    {
        /// <summary>
        /// Returns a MiniProfiler instance of the DataContext
        /// </summary>
        /// <typeparam name="T">DataContext class</typeparam>
        /// <param name="connectionStringName">Connection string name from the Web.Config</param>
        public static T Create<T>(string connectionStringName = null) where T : System.Data.Linq.DataContext, new()
        {
            var type = typeof(T);

            // if connectionStringName was not specified, attempt to pull the name from Constants class
            if (connectionStringName == null)
            {
                var constantType = typeof(Constants.ConnectionStrings);
                var field = constantType.GetField(type.Name, BindingFlags.Static | BindingFlags.NonPublic)
                    ?? constantType.GetField(type.Name, BindingFlags.Static | BindingFlags.Public);
                if (field != null)
                    connectionStringName = (string)field.GetRawConstantValue();
            }

            // try to get the connection string. throw error if not found
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName ?? ""].ConnectionString;
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new KeyNotFoundException();

            // create a mini profiler wrapper for the connection
            var connection = new ProfiledDbConnection(
                new SqlConnection(connectionString),
                StackExchange.Profiling.MiniProfiler.Current);

            // return new data context instance, passing the connection to constructor
            return (T)Activator.CreateInstance(type, connection);
        }
    }
}