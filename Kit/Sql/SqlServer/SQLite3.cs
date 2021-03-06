﻿using System;
using Kit.Sql.Exceptions;
using SQLitePCL;

namespace Kit.Sql.SqlServer
{
    public static class SQLite3
    {
        public enum Result : int
        {
            OK = 0,
            Error = 1,
            Internal = 2,
            Perm = 3,
            Abort = 4,
            Busy = 5,
            Locked = 6,
            NoMem = 7,
            ReadOnly = 8,
            Interrupt = 9,
            IOError = 10,
            Corrupt = 11,
            NotFound = 12,
            Full = 13,
            CannotOpen = 14,
            LockErr = 15,
            Empty = 16,
            SchemaChngd = 17,
            TooBig = 18,
            Constraint = 19,
            Mismatch = 20,
            Misuse = 21,
            NotImplementedLFS = 22,
            AccessDenied = 23,
            Format = 24,
            Range = 25,
            NonDBFile = 26,
            Notice = 27,
            Warning = 28,
            Row = 100,
            Done = 101
        }

        public enum ExtendedResult : int
        {
            IOErrorRead = (Result.IOError | (1 << 8)),
            IOErrorShortRead = (Result.IOError | (2 << 8)),
            IOErrorWrite = (Result.IOError | (3 << 8)),
            IOErrorFsync = (Result.IOError | (4 << 8)),
            IOErrorDirFSync = (Result.IOError | (5 << 8)),
            IOErrorTruncate = (Result.IOError | (6 << 8)),
            IOErrorFStat = (Result.IOError | (7 << 8)),
            IOErrorUnlock = (Result.IOError | (8 << 8)),
            IOErrorRdlock = (Result.IOError | (9 << 8)),
            IOErrorDelete = (Result.IOError | (10 << 8)),
            IOErrorBlocked = (Result.IOError | (11 << 8)),
            IOErrorNoMem = (Result.IOError | (12 << 8)),
            IOErrorAccess = (Result.IOError | (13 << 8)),
            IOErrorCheckReservedLock = (Result.IOError | (14 << 8)),
            IOErrorLock = (Result.IOError | (15 << 8)),
            IOErrorClose = (Result.IOError | (16 << 8)),
            IOErrorDirClose = (Result.IOError | (17 << 8)),
            IOErrorSHMOpen = (Result.IOError | (18 << 8)),
            IOErrorSHMSize = (Result.IOError | (19 << 8)),
            IOErrorSHMLock = (Result.IOError | (20 << 8)),
            IOErrorSHMMap = (Result.IOError | (21 << 8)),
            IOErrorSeek = (Result.IOError | (22 << 8)),
            IOErrorDeleteNoEnt = (Result.IOError | (23 << 8)),
            IOErrorMMap = (Result.IOError | (24 << 8)),
            LockedSharedcache = (Result.Locked | (1 << 8)),
            BusyRecovery = (Result.Busy | (1 << 8)),
            CannottOpenNoTempDir = (Result.CannotOpen | (1 << 8)),
            CannotOpenIsDir = (Result.CannotOpen | (2 << 8)),
            CannotOpenFullPath = (Result.CannotOpen | (3 << 8)),
            CorruptVTab = (Result.Corrupt | (1 << 8)),
            ReadonlyRecovery = (Result.ReadOnly | (1 << 8)),
            ReadonlyCannotLock = (Result.ReadOnly | (2 << 8)),
            ReadonlyRollback = (Result.ReadOnly | (3 << 8)),
            AbortRollback = (Result.Abort | (2 << 8)),
            ConstraintCheck = (Result.Constraint | (1 << 8)),
            ConstraintCommitHook = (Result.Constraint | (2 << 8)),
            ConstraintForeignKey = (Result.Constraint | (3 << 8)),
            ConstraintFunction = (Result.Constraint | (4 << 8)),
            ConstraintNotNull = (Result.Constraint | (5 << 8)),
            ConstraintPrimaryKey = (Result.Constraint | (6 << 8)),
            ConstraintTrigger = (Result.Constraint | (7 << 8)),
            ConstraintUnique = (Result.Constraint | (8 << 8)),
            ConstraintVTab = (Result.Constraint | (9 << 8)),
            NoticeRecoverWAL = (Result.Notice | (1 << 8)),
            NoticeRecoverRollback = (Result.Notice | (2 << 8))
        }


        public enum ConfigOption : int
        {
            SingleThread = 1,
            MultiThread = 2,
            Serialized = 3
        }

        const string LibraryPath = "sqlite3";

#if !USE_CSHARP_SQLITE && !USE_WP8_NATIVE_SQLITE && !USE_SQLITEPCL_RAW
		[DllImport (LibraryPath, EntryPoint = "sqlite3_threadsafe", CallingConvention = CallingConvention.Cdecl)]
		public static extern int Threadsafe ();

		[DllImport (LibraryPath, EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Open ([MarshalAs (UnmanagedType.LPStr)] string filename, out IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Open ([MarshalAs (UnmanagedType.LPStr)] string filename, out IntPtr db, int flags, [MarshalAs (UnmanagedType.LPStr)] string zvfs);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Open (byte[] filename, out IntPtr db, int flags, [MarshalAs (UnmanagedType.LPStr)] string zvfs);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_open16", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Open16 ([MarshalAs (UnmanagedType.LPWStr)] string filename, out IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_enable_load_extension", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result EnableLoadExtension (IntPtr db, int onoff);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Close (IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_close_v2", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Close2 (IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_initialize", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Initialize ();

		[DllImport (LibraryPath, EntryPoint = "sqlite3_shutdown", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Shutdown ();

		[DllImport (LibraryPath, EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Config (ConfigOption option);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_win32_set_directory", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int SetDirectory (uint directoryType, string directoryPath);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result BusyTimeout (IntPtr db, int milliseconds);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_changes", CallingConvention = CallingConvention.Cdecl)]
		public static extern int Changes (IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Prepare2 (IntPtr db, [MarshalAs (UnmanagedType.LPStr)] string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

#if NETFX_CORE
		[DllImport (LibraryPath, EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Prepare2 (IntPtr db, byte[] queryBytes, int numBytes, out IntPtr stmt, IntPtr pzTail);
#endif

		public static IntPtr Prepare2 (IntPtr db, string query)
		{
			IntPtr stmt;
#if NETFX_CORE
            byte[] queryBytes = System.Text.UTF8Encoding.UTF8.GetBytes (query);
            var r = Prepare2 (db, queryBytes, queryBytes.Length, out stmt, IntPtr.Zero);
#else
			var r = Prepare2 (db, query, System.Text.UTF8Encoding.UTF8.GetByteCount (query), out stmt, IntPtr.Zero);
#endif
			if (r != Result.OK) {
				throw SqlServerException.New (r, GetErrmsg (db));
			}
			return stmt;
		}

		[DllImport (LibraryPath, EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Step (IntPtr stmt);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_reset", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Reset (IntPtr stmt);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result Finalize (IntPtr stmt);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_last_insert_rowid", CallingConvention = CallingConvention.Cdecl)]
		public static extern long LastInsertRowid (IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr Errmsg (IntPtr db);

		public static string GetErrmsg (IntPtr db)
		{
			return Marshal.PtrToStringUni (Errmsg (db));
		}

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_parameter_index", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindParameterIndex (IntPtr stmt, [MarshalAs (UnmanagedType.LPStr)] string name);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_null", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindNull (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindInt (IntPtr stmt, int index, int val);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindInt64 (IntPtr stmt, int index, long val);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_double", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindDouble (IntPtr stmt, int index, double val);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_text16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int BindText (IntPtr stmt, int index, [MarshalAs (UnmanagedType.LPWStr)] string val, int n, IntPtr free);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_bind_blob", CallingConvention = CallingConvention.Cdecl)]
		public static extern int BindBlob (IntPtr stmt, int index, byte[] val, int n, IntPtr free);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ColumnCount (IntPtr stmt);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_name", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ColumnName (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ColumnName16Internal (IntPtr stmt, int index);
		public static string ColumnName16 (IntPtr stmt, int index)
		{
			return Marshal.PtrToStringUni (ColumnName16Internal (stmt, index));
		}

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
		public static extern ColType ColumnType (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ColumnInt (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ColumnInt64 (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
		public static extern double ColumnDouble (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ColumnText (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ColumnText16 (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ColumnBlob (IntPtr stmt, int index);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ColumnBytes (IntPtr stmt, int index);

		public static string ColumnString (IntPtr stmt, int index)
		{
			return Marshal.PtrToStringUni (SQLite3.ColumnText16 (stmt, index));
		}

		public static byte[] ColumnByteArray (IntPtr stmt, int index)
		{
			int length = ColumnBytes (stmt, index);
			var result = new byte[length];
			if (length > 0)
				Marshal.Copy (ColumnBlob (stmt, index), result, 0, length);
			return result;
		}

		[DllImport (LibraryPath, EntryPoint = "sqlite3_errcode", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result GetResult (Sqlite3DatabaseHandle db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_extended_errcode", CallingConvention = CallingConvention.Cdecl)]
		public static extern ExtendedResult ExtendedErrCode (IntPtr db);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_libversion_number", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LibVersionNumber ();

		[DllImport (LibraryPath, EntryPoint = "sqlite3_backup_init", CallingConvention = CallingConvention.Cdecl)]
		public static extern Sqlite3BackupHandle BackupInit (Sqlite3DatabaseHandle destDb, [MarshalAs (UnmanagedType.LPStr)] string destName, Sqlite3DatabaseHandle sourceDb, [MarshalAs (UnmanagedType.LPStr)] string sourceName);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_backup_step", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result BackupStep (Sqlite3BackupHandle backup, int numPages);

		[DllImport (LibraryPath, EntryPoint = "sqlite3_backup_finish", CallingConvention = CallingConvention.Cdecl)]
		public static extern Result BackupFinish (Sqlite3BackupHandle backup);
#else
        public static Result Open(string filename, out sqlite3 db)
        {
            return (Result)raw.sqlite3_open(filename, out db);
        }

        public static Result Open(string filename, out sqlite3 db, int flags, string vfsName)
        {
#if USE_WP8_NATIVE_SQLITE
			return (Result)Sqlite3.sqlite3_open_v2(filename, out db, flags, vfsName ?? "");
#else
            return (Result)raw.sqlite3_open_v2(filename, out db, flags, vfsName);
#endif
        }

        public static Result Close(sqlite3 db)
        {
            return (Result)raw.sqlite3_close(db);
        }

        public static Result Close2(sqlite3 db)
        {
            return (Result)raw.sqlite3_close_v2(db);
        }

        public static Result BusyTimeout(sqlite3 db, int milliseconds)
        {
            return (Result)raw.sqlite3_busy_timeout(db, milliseconds);
        }

        public static int Changes(sqlite3 db)
        {
            return raw.sqlite3_changes(db);
        }

        public static sqlite3_stmt Prepare2(sqlite3 db, string query)
        {
            sqlite3_stmt stmt = default(sqlite3_stmt);
#if USE_WP8_NATIVE_SQLITE || USE_SQLITEPCL_RAW
            var r = raw.sqlite3_prepare_v2(db, query, out stmt);
#else
			stmt = new Sqlite3Statement();
			var r = Sqlite3.sqlite3_prepare_v2(db, query, -1, ref stmt, 0);
#endif
            if (r != 0)
            {
                throw SQLiteException.New((Kit.Sql.Sqlite.SQLite3.Result)r, GetErrmsg(db));
            }
            return stmt;
        }

        public static Result Step(sqlite3_stmt stmt)
        {
            return (Result)raw.sqlite3_step(stmt);
        }

        public static Result Reset(sqlite3_stmt stmt)
        {
            return (Result)raw.sqlite3_reset(stmt);
        }

        public static Result Finalize(sqlite3_stmt stmt)
        {
            return (Result)raw.sqlite3_finalize(stmt);
        }

        public static long LastInsertRowid(sqlite3 db)
        {
            return raw.sqlite3_last_insert_rowid(db);
        }

        public static string GetErrmsg(sqlite3 db)
        {
            return raw.sqlite3_errmsg(db).utf8_to_string();
        }

        public static int BindParameterIndex(sqlite3_stmt stmt, string name)
        {
            return raw.sqlite3_bind_parameter_index(stmt, name);
        }

        public static int BindNull(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_bind_null(stmt, index);
        }

        public static int BindInt(sqlite3_stmt stmt, int index, int val)
        {
            return raw.sqlite3_bind_int(stmt, index, val);
        }

        public static int BindInt64(sqlite3_stmt stmt, int index, long val)
        {
            return raw.sqlite3_bind_int64(stmt, index, val);
        }

        public static int BindDouble(sqlite3_stmt stmt, int index, double val)
        {
            return raw.sqlite3_bind_double(stmt, index, val);
        }

        public static int BindText(sqlite3_stmt stmt, int index, string val, int n, IntPtr free)
        {
#if USE_WP8_NATIVE_SQLITE
			return Sqlite3.sqlite3_bind_text(stmt, index, val, n);
#elif USE_SQLITEPCL_RAW
            return raw.sqlite3_bind_text(stmt, index, val);
#else
			return Sqlite3.sqlite3_bind_text(stmt, index, val, n, null);
#endif
        }

        public static int BindBlob(sqlite3_stmt stmt, int index, byte[] val, int n, IntPtr free)
        {
#if USE_WP8_NATIVE_SQLITE
			return Sqlite3.sqlite3_bind_blob(stmt, index, val, n);
#elif USE_SQLITEPCL_RAW
            return raw.sqlite3_bind_blob(stmt, index, val);
#else
			return Sqlite3.sqlite3_bind_blob(stmt, index, val, n, null);
#endif
        }

        public static int ColumnCount(sqlite3_stmt stmt)
        {
            return raw.sqlite3_column_count(stmt);
        }

        public static string ColumnName(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_name(stmt, index).utf8_to_string();
        }

        public static string ColumnName16(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_name(stmt, index).utf8_to_string();
        }

        public static ColType ColumnType(sqlite3_stmt stmt, int index)
        {
            return (ColType)raw.sqlite3_column_type(stmt, index);
        }

        public static int ColumnInt(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_int(stmt, index);
        }

        public static long ColumnInt64(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_int64(stmt, index);
        }

        public static double ColumnDouble(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_double(stmt, index);
        }

        public static string ColumnText(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_text(stmt, index).utf8_to_string();
        }

        public static string ColumnText16(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_text(stmt, index).utf8_to_string();
        }

        public static byte[] ColumnBlob(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_blob(stmt, index).ToArray();
        }

        public static int ColumnBytes(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_bytes(stmt, index);
        }

        public static string ColumnString(sqlite3_stmt stmt, int index)
        {
            return raw.sqlite3_column_text(stmt, index).utf8_to_string();
        }

        public static byte[] ColumnByteArray(sqlite3_stmt stmt, int index)
        {
            int length = ColumnBytes(stmt, index);
            if (length > 0)
            {
                return ColumnBlob(stmt, index);
            }
            return new byte[0];
        }

        public static Result EnableLoadExtension(sqlite3 db, int onoff)
        {
            return (Result)raw.sqlite3_enable_load_extension(db, onoff);
        }

        public static int LibVersionNumber()
        {
            return raw.sqlite3_libversion_number();
        }

        public static Result GetResult(sqlite3 db)
        {
            return (Result)raw.sqlite3_errcode(db);
        }

        public static ExtendedResult ExtendedErrCode(sqlite3 db)
        {
            return (ExtendedResult)raw.sqlite3_extended_errcode(db);
        }

        public static sqlite3_backup BackupInit(sqlite3 destDb, string destName, sqlite3 sourceDb, string sourceName)
        {
            return raw.sqlite3_backup_init(destDb, destName, sourceDb, sourceName);
        }

        public static Result BackupStep(sqlite3_backup backup, int numPages)
        {
            return (Result)raw.sqlite3_backup_step(backup, numPages);
        }

        public static Result BackupFinish(sqlite3_backup backup)
        {
            return (Result)raw.sqlite3_backup_finish(backup);
        }
#endif

        public enum ColType : int
        {
            Integer = 1,
            Float = 2,
            Text = 3,
            Blob = 4,
            Null = 5
        }
    }
}