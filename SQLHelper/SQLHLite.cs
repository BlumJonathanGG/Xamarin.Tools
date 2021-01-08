﻿using System;
using System.Collections.Generic;
//Alias para poder trabajar con mas facilidad
using SqlConnection = SQLite.SQLiteConnection;
using SqlCommand = SQLite.SQLiteCommand;
using SqlDataReader = SQLHelper.SQLiteNetExtensions.ReaderItem;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using System.Text;
using System.Diagnostics;
using SQLHelper.Readers;
using SQLHelper.Interfaces;
using SQLHelper.Reflection;

namespace SQLHelper
{
    public class SQLHLite : BaseSQLHelper
    {
        private Type AssemblyType;
        private string ScriptResourceName;

        [Obsolete("Use ScriptResourceName")]
        public event EventHandler OnCreateDB;
        //private FileInfo file;
        public readonly string RutaDb;
        public readonly string DBVersion;
        public SQLHLite(string DBVersion, string DBName)
        {
            if (SQLHelper.Instance is null)
            {
                throw new Exception("Please call SQLHelper.Initi(LibraryPath,Debugging); before using it");
            }
            FileInfo db = new FileInfo($"{SQLHelper.Instance.LibraryPath}/{DBName}");
            this.RutaDb = db.FullName;
            this.DBVersion = DBVersion;
        }

        public SQLHLite SetDbScriptResource<T>(string ScriptResourceName)
        {
            this.AssemblyType = typeof(T);
            this.ScriptResourceName = ScriptResourceName;
            return this;
        }

        public SQLHLite(FileInfo file)
        {
            if (SQLHelper.Instance is null)
            {
                throw new Exception("Please call SQLHelper.Initi(LibraryPath,Debugging); before using it");
            }
            file.Refresh();
            this.RutaDb = file.FullName;
        }

        /// <summary>
        /// Comprueba que la base de datos exista y que sea la versión mas reciente
        /// de lo contrario crea una nueva base de datos
        /// </summary>
        public SQLHLite RevisarBaseDatos()
        {
            FileInfo db = new FileInfo(this.RutaDb);

            if (!db.Exists)
            {
                Crear(Conecction());
            }
            string dbVersion = GetDbVersion();
            if (dbVersion != DBVersion)
            {
                db.Delete();
                Crear(Conecction());
            }
            return this;
        }
        private void RevisarTablaDbVersion(SqlConnection connection)
        {
            if (TableExists("DB_VERSION"))
            {
                return;
            }
            connection.Execute(@"CREATE TABLE DB_VERSION ( VERSION VARCHAR NOT NULL )");
            connection.Execute($"INSERT INTO DB_VERSION(VERSION) VALUES('{DBVersion}')");
        }
        private void RevisarTablaConfiguracion()
        {
            if (!this.TableExists("CONFIGURACION"))
            {
                this.EXEC(@"CREATE TABLE CONFIGURACION (
ID INTEGER IDENTITY(1,1) PRIMARY KEY, 
ID_DISPOSITIVO TEXT NOT NULL,
NOMBREDB TEXT,
NOMBRE TEXT,
SERVIDOR TEXT,
PUERTO TEXT,
USUARIO TEXT,
PASSWORD TEXT,
CADENA_CON TEXT NOT NULL);");
            }
            return;
        }


        public string GetDbVersion()
        {
            string dbVersion = null;
            try
            {
                dbVersion = Single<string>("SELECT VERSION FROM DB_VERSION");
            }
            catch (Exception ex)
            {
                Log.LogMe(ex);
            }
            return dbVersion;
        }
        private void Crear(SqlConnection connection)
        {
            if (TableExists("DB_VERSION"))
            {
                connection.Execute("DROP TABLE DB_VERSION");
            }
            RevisarTablaDbVersion(connection);
            RevisarTablaConfiguracion();
            if (AssemblyType != null && !string.IsNullOrEmpty(this.ScriptResourceName))
            {
                CreateDbFromScript(connection);
            }
            else
            {
                OnCreateDB?.Invoke(this, EventArgs.Empty);
            }
            connection.Close();
        }
        private void CreateDbFromScript(SqlConnection connection)
        {
            string sql = string.Empty;
            using (ReflectionCaller reflection = new ReflectionCaller())
            {
                sql = ReflectionCaller.ToText(reflection
                    .GetAssembly(this.AssemblyType)
                    .GetResource(this.ScriptResourceName));
            }
            if (!string.IsNullOrEmpty(sql))
            {
                this.Batch(connection, sql);
            }
            this.AssemblyType = null;
            this.ScriptResourceName = null;

        }

        public bool EliminarDB()
        {
            try
            {
                FileInfo file = new FileInfo(RutaDb);
                if (file.Exists)
                {
                    File.Delete(RutaDb);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogMe(ex);
                return false;
            }
        }
        public SqlConnection Conecction()
        {
            SqlConnection con;
            try
            {
                con = new SqlConnection(this.RutaDb);
                return con;
            }
            catch (Exception ex)
            {
                con = null;
                Log.LogMe(ex);
            }
            return con;
        }
        public SQLiteAsyncConnection ConecctionAsync()
        {
            SQLiteAsyncConnection con;
            try
            {
                con = new SQLiteAsyncConnection(this.RutaDb);
                return con;
            }
            catch (Exception ex)
            {
                con = null;
                Log.LogMe(ex);
            }
            return con;
        }
        public int Querry(string sql, params object[] args)
        {
            int afectadas = -1;
            using (SqlConnection con = this.Conecction())
            {
                afectadas = con.Execute(sql, args);
                con.Close();
            }

            return afectadas;
        }
        public T Single<T>(string sql)
        {
            T result = default;
            try
            {
                using (IReader reader = Leector(sql))
                {
                    if (reader.Read())
                    {
                        result = (
                            typeof(T).IsEnum ?
                                (T)Enum.Parse(typeof(T), reader[0].ToString(), true) :
                                (T)Convert.ChangeType(reader[0], typeof(T)));
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogMe(e, "Al ejecutar un single en SQLHLite");
            }
            return result;
        }
        public T Single<T>(SQLiteConnection con, string sql)
        {
            T result = default;
            try
            {
                using (IReader reader = Leector(sql, con))
                {
                    if (reader.Read())
                    {
                        result = (
                            typeof(T).IsEnum ?
                                (T)Enum.Parse(typeof(T), reader[0].ToString(), true) :
                                (T)Convert.ChangeType(reader[0], typeof(T)));
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogMe(e, "Al ejecutar un single en SQLHLite");
            }
            return result;
        }

        public int EXEC(string sql, params object[] parametros)
        {
            Log.DebugMe(sql);
            int afectadas = -1;
            using (SqlConnection con = Conecction())
            {
                afectadas = con.Execute(sql, parametros);
                con.Close();
            }
            return afectadas;
        }
        public int EXEC(SQLiteConnection con, string sql, params object[] parametros)
        {
            Log.DebugMe(sql);
            return con.Execute(sql, parametros);
        }
        public T ExecuteRead<T>(string sql, params object[] parametros)
        {
            Log.DebugMe(sql);
            T result = default(T);
            using (SqlConnection con = Conecction())
            {
                result = con.ExecuteScalar<T>(sql, parametros);
                con.Close();
            }
            return result;
        }
        public List<T> Lista<T>(string sql)
        {
            List<T> result = new List<T>();
            using (IReader reader = Leector(sql))
            {
                while (reader.Read())
                {
                    result.Add((T)Convert.ChangeType(reader[0], typeof(T)));
                }
            }
            return result;
        }
        //ListaTupla
        public List<Tuple<T, Q>> ListaTupla<T, Q>(string sql, params object[] parameters)
        {
            List<Tuple<T, Q>> result = new List<Tuple<T, Q>>();

            using (IReader reader = Leector(sql))
            {
                while (reader.Read())
                {
                    result.Add(new Tuple<T, Q>
                        ((T)Convert.ChangeType(reader[0], typeof(T)),
                        (Q)Convert.ChangeType(reader[1], typeof(Q))));
                }
            }

            return result;
        }
        public DataTable DataTable(string Querry, string TableName = null)
        {
            DataTable result = new DataTable(TableName);
            using (LiteReader reader = (LiteReader)Leector(Querry))
            {
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Columns.Add(reader.Fields[i]);
                    }
                }
                do
                {
                    List<object> row = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader[i]);
                    }

                    result.Rows.Add(row.ToArray());
                } while (reader.Read());
            }
            return result;
        }
        public IReader Leector(string sql)
        {
            try
            {
                return new LiteReader(Conecction(), sql);
            }
            catch (Exception ex)
            {
                Log.LogMe(ex, "Al compilar y ejecutar un leector");
                return new FakeReader();
            }
        }


        public LiteReader Leector(string sql, SqlConnection connection)
        {
            try
            {
                return new LiteReader(connection, sql);
            }
            catch (Exception ex)
            {
                Log.LogMe(ex, "Al compilar y ejecutar un leector");
                return null;
            }
        }
        public bool Exists(string sql)
        {
            using (IReader reader = Leector(sql))
            {
                return reader?.Read() ?? false;
            }
        }
        public override bool TableExists(string TableName)
        {
            using (IReader reader = Leector($"SELECT name FROM sqlite_master WHERE type='table' AND name='{TableName}';"))
            {
                return reader?.Read() ?? false;
            }
        }
        public void Batch(string sql)
        {
            Batch(Conecction(), sql);
        }
        public void Batch(SQLiteConnection con, string sql)
        {
            StringBuilder sqlBatch = new StringBuilder();
            try
            {
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "--GO" || (sqlBatch.Length > 0 && sqlBatch[sqlBatch.Length - 1] == ';'))
                    {
                        string batch = sqlBatch.ToString();

                        if (!string.IsNullOrEmpty(batch))
                            EXEC(batch);
                        sqlBatch.Clear();
                    }
                    if (!line.StartsWith("--"))
                    {
                        sqlBatch.Append(line.Trim());
                    }
                }
                if (sqlBatch.Length > 0)
                {
                    EXEC(sqlBatch.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.LogMeDemonio(ex.Message);
            }
        }



        public int LastScopeIdentity(SqlConnection con)
        {
            return Single<int>(con, "SELECT last_insert_rowid();");
        }
    }
}
