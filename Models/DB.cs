using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using Night_Shadow.Models;
using System.Linq;
using System.Reflection;

namespace Night_Shadow.Models
{
	public class DB
	{
		private static SqlConnection _conn;
		private static SqlCommand _cmd;
		private static DB _instance = null;
		private DB(string constr)
		{
			_conn = new SqlConnection(constr);
			_cmd = new SqlCommand(" ", _conn);
		}

		/// <summary>
		/// Gets an insance of the DB connetion
		/// </summary>
		/// <returns>instance of DB class</returns>
		public static DB GetInstance()
		{
			return _instance;
		}

		/// <summary>
		/// Initialises the DB connection
		/// </summary>
		/// <param name="constr"></param>
		/// <param name="env"></param>
		public static void Init(string constr, bool env = false)
		{
			if (_instance == null)
			{
				if (env == true)
					constr = Environment.GetEnvironmentVariable(constr);
				_instance = new DB(constr);
				Console.WriteLine($"Connected to {_conn.Database}");
			}
		}

		private SqlDataReader Query(string sql, Dictionary<string, object> fields = null, bool storedProc = false)
		{
			try
			{
				_cmd.CommandText = sql;
				if (storedProc)
					_cmd.CommandType = CommandType.StoredProcedure;
				else
					_cmd.CommandType = CommandType.Text;
				if (fields != null && fields.Count != 0)
				{

					foreach (var item in fields)
					{
						SqlParameter param = new SqlParameter();
						param.ParameterName = "@" + item.Key;
						param.Value = item.Value;
						_cmd.Parameters.Add(param);
					}
				}
				if (_conn.State.ToString() == "Closed")
					_conn.Open();
				return _cmd.ExecuteReader();
			}
			finally
			{
				_cmd.Parameters.Clear();

			}
		}
		/// <summary>
		/// This is a simply INSERT statment that takes in a table name and a dictionary of fields and values
		/// </summary>
		/// <param name="table"></param>
		/// <param name="fields"></param>
		/// <returns>Number of records affected</returns>
		public int Insert(string table, Dictionary<string, object> fields = null)
		{

			if (fields != null && fields.Count != 0)
			{
				try
				{
					string[] keys = fields.Keys.Select(x => x.ToString()).ToArray();
					string value = "";
					int x = 1;
					foreach (var key in keys)
					{
						value += "@" + key;
						if (x < keys.Length)
						{
							value += ",";
						}
						x++;
					}
					string sql = $"INSERT INTO {table} ({String.Join(',', keys)}) VALUES ({value})";
					SqlDataReader rows = Query(sql, fields);
					return rows.RecordsAffected;
				}
				finally
				{
					_conn.Close();
				}
			}
			return 0;
		}

		/// <summary>
		/// This is simple SELECT statment that takes a table name, a operator and a dictionary for the where clause
		/// </summary>
		/// <param name="table"></param>
		/// <param name="Operator"></param>
		/// <param name="where"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>A list of objects specified by the user</returns>
		public List<T> Select<T>(string table, string Operator = null, Dictionary<string, object> where = null) where T : new()
		{
			try
			{
				string sql = $"SELECT * FROM {table}";
				if (Operator != null && where != null && where.Count != 0)
				{
					sql += $" WHERE {where.First().Key} {Operator} @{where.First().Key}";
				}
				SqlDataReader reader = Query(sql, where);
				return ExtractRecords<T>(reader);
			}
			finally
			{
				_conn.Close();
			}

		}

		/// <summary>
		/// This is a simple UPDATE statment that takes a table, a dictionary of the fields and values, a operator and a dictonary for the where clause
		/// </summary>
		/// <param name="table"></param>
		/// <param name="fields"></param>
		/// <param name="Operator"></param>
		/// <param name="where"></param>
		/// <returns>Number of records affected</returns>
		public int Update(string table, Dictionary<string, object> fields, string Operator, Dictionary<string, object> where)
		{
			try
			{
				string[] keys = fields.Keys.Select(x => x.ToString()).ToArray();
				string value = "";
				int x = 1;
				foreach (var key in keys)
				{
					value += key + " = @" + key;
					if (x < keys.Length)
					{
						value += ",";
					}
					x++;
				}
				string sql = $"UPDATE {table} SET {value} WHERE {where.First().Key} {Operator} @WhereKey_{where.First().Key}";
				fields.Add($"WhereKey_{where.First().Key}", where.First().Value);
				SqlDataReader rows = Query(sql, fields);
				return rows.RecordsAffected;
			}
			finally
			{
				_conn.Close();
			}
		}

		/// <summary>
		/// This is a simple DELETE statment that takes a table name, a operator and a dictionary for the where clause
		/// </summary>
		/// <param name="table"></param>
		/// <param name="Operator"></param>
		/// <param name="where"></param>
		/// <returns>Number of records affected</returns>
		public int Delete(string table, string Operator, Dictionary<string, object> where)
		{
			try
			{
				string sql = $"DELETE FROM {table} WHERE {where.First().Key} {Operator} @{where.First().Key}";
				SqlDataReader rows = Query(sql, where);
				return rows.RecordsAffected;
			}
			finally
			{
				_conn.Close();
			}
		}

		/// <summary>
		/// This is a simple StoredProc statment that takes in a storedProc name and a dictionary of fields and values
		/// </summary>
		/// <param name="storedProc"></param>
		/// <param name="fields"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>A list of objects specified by the user or a list with one item which is the number of records affected</returns>
		public List<T> StoredProc<T>(string storedProc, Dictionary<string, object> fields) where T : new()
		{
			try
			{
				SqlDataReader rows = Query(storedProc, fields, true);
				if (rows.HasRows)
					return ExtractRecords<T>(rows);
				return new List<T>() { (T)Convert.ChangeType(rows.RecordsAffected, typeof(T)) };
			}
			finally
			{
				_conn.Close();
			}
		}

		private List<T> ExtractRecords<T>(SqlDataReader reader) where T : new()
		{
			List<T> res = new List<T>();
			while (reader.Read())
			{
				T rec = new T();
				for (int inc = 0; inc < reader.FieldCount; inc++)
				{
					Type type = rec.GetType();
					PropertyInfo prop = type.GetProperty(reader.GetName(inc));
					if (prop == null)
						continue;
					prop.SetValue(rec, Convert.ChangeType(reader.GetValue(inc), prop.PropertyType), null);
				}
				res.Add(rec);
			}
			reader.Close();
			return res;
		}

	}
}