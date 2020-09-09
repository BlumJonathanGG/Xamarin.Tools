﻿using SQLHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SQLHelper.Abstractions
{
    public class Select : IQuery
    {
        private readonly List<string> SelectionFields;
        private Select(BaseSQLHelper SQLH, string TableName) : base(SQLH, TableName)
        {
            this.SelectionFields = new List<string>();
        }
        private Select(BaseSQLHelper SQLH) : base(SQLH, null)
        {
            this.SelectionFields = null;
        }
        public static Select BulidFrom(BaseSQLHelper SQLH)
        {
            return new Select(SQLH);
        }
        public IReader ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            if (this.SQLH is SQLHLite lite)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    Regex regex = new Regex(@$"\@{parameter.ParameterName}");
                    query = regex.Replace(query, $"'{parameter.Value}'");
                }
                return lite.Leector(query);
            }
            else if (this.SQLH is SQLH sql)
            {
                return sql.Leector(query, CommandType.Text, false, parameters);
            }
            throw new NotSupportedException("No sql connection set");
        }
        public static Select BulidFrom(BaseSQLHelper SQLH, string TableName)
        {
            return new Select(SQLH, TableName);
        }
        public Select AddFields(params string[] Fields)
        {
            this.SelectionFields.AddRange(Fields);
            return this;
        }
        public Select Where(string Field, object value)
        {
            return (Select)this.AddParameter(Field, value);
        }
        protected override string BuildQuery()
        {
            StringBuilder builder = new StringBuilder()
                .Append("SELECT ")
                  .Append(string.Join(",", this.SelectionFields))
                  .Append(" FROM ")
                  .Append(TableName);
            if (this.Parameters.Any())
            {
                builder.Append(" WHERE");
                foreach (var pair in this.Parameters)
                {
                    builder.Append(" ")
                        .Append(pair.Key)
                        .Append(" =@")
                        .Append(pair.Key);
                }
            }
            return builder.ToString();
        }
        protected override string BuildLiteQuery()
        {
            StringBuilder builder = new StringBuilder()
                .Append("SELECT ")
                  .Append(string.Join(",", this.SelectionFields))
                  .Append(" FROM ")
                  .Append(TableName);
            if (this.Parameters.Any())
            {
                builder.Append(" WHERE");
                foreach (var pair in this.Parameters)
                {
                    builder.Append(" ")
                        .Append(pair.Key)
                        .Append(" ='")
                        .Append(pair.Value)
                        .Append('\'');
                }
            }
            return builder.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.SelectionFields.Clear();
            return;
        }

        public override int Execute()
        {
            return ExecuteReader().Read() ? 1 : 0;
        }
        public IReader ExecuteReader()
        {
            if (this.SQLH is SQLH sql)
            {
                IEnumerable<SqlParameter> parameters =
                    this.Parameters.Select(x => new SqlParameter(x.Key, x.Value)).ToArray();

                return sql.Leector(BuildQuery(), System.Data.CommandType.Text, false, parameters.ToArray());
            }
            else if (this.SQLH is SQLHLite sqlite)
            {
                return sqlite.Leector(BuildLiteQuery());
            }
            throw new NotSupportedException("No sql connection set");
        }


    }
}