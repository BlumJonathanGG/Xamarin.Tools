﻿using System.Collections.Generic;
using System.Linq;
using Kit.Sql.Base;
using Kit.Sql.Sqlite;

namespace Kit.Sql.Exceptions
{
    public class NotNullConstraintViolationException : SQLiteException
    {
        public IEnumerable<Kit.Sql.Sqlite.TableMapping.Column> Columns { get; protected set; }

        protected NotNullConstraintViolationException(SQLite3.Result r, string message)
            : this(r, message, null, null)
        {

        }

        protected NotNullConstraintViolationException(SQLite3.Result r, string message, Kit.Sql.Base.TableMapping mapping, object obj)
            : base(r, message)
        {
            if (mapping != null && obj != null)
            {
                this.Columns = from c in mapping.Columns
                    where c.IsNullable == false && c.GetValue(obj) == null
                    select c;
            }
        }

        public static new NotNullConstraintViolationException New(SQLite3.Result r, string message)
        {
            return new NotNullConstraintViolationException(r, message);
        }

        public static NotNullConstraintViolationException New(SQLite3.Result r, string message, Kit.Sql.Base.TableMapping mapping, object obj)
        {
            return new NotNullConstraintViolationException(r, message, mapping, obj);
        }

        public static NotNullConstraintViolationException New(SQLiteException exception, Kit.Sql.Base.TableMapping mapping, object obj)
        {
            return new NotNullConstraintViolationException(exception.Result, exception.Message, mapping, obj);
        }
    }
}