using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleMDB.Tool
{
    // MDB操作核心工具类（封装所有MDB读取逻辑）
    public class MdbReader
    {
        private readonly string _connectionString;

        public MdbReader(string mdbPath)
        {
            if (string.IsNullOrEmpty(mdbPath))
                throw new ArgumentNullException(nameof(mdbPath), "MDB文件路径不能为空");


            _connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath};";
            OleDbConnection conn = new OleDbConnection(_connectionString);
            conn.Open();

            _connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath}";
            // 初始化连接字符串（适配不同Access版本）
            /*  _connectionString = mdbPath.EndsWith(".accdb", StringComparison.OrdinalIgnoreCase)
                  ? $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath};"
                  : $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};";
            */
        }

        /// <summary>
        /// 获取MDB中所有用户表名称
        /// </summary>
        public List<string> GetAllTableNames()
        {
            var tableNames = new List<string>();

            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                // 筛选用户表（排除系统表）
                var schema = connection.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables,
                    new object[] { null, null, null, "TABLE" }
                );

                if (schema?.Rows != null)
                {
                    tableNames.AddRange(schema.Rows.Cast<System.Data.DataRow>()
                        .Select(row => row["TABLE_NAME"].ToString()));
                }
            }

            return tableNames;
        }

        /// <summary>
        /// 读取指定表的数据并转换为JObject列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表数据（无数据时返回空列表，表不存在时返回null）</returns>
        public List<JObject> ReadTableData(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");

            // 先检查表是否存在
            if (!GetAllTableNames().Contains(tableName, StringComparer.OrdinalIgnoreCase))
                return null;

            var result = new List<JObject>();

            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();

                // 处理表名包含特殊字符的情况
                var query = $"SELECT * FROM [{tableName}]";
                using (var command = new OleDbCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    var columns = Enumerable.Range(0, reader.FieldCount)
                                           .Select(reader.GetName)
                                           .ToList();

                    while (reader.Read())
                    {
                        var jObject = new JObject();
                        foreach (var column in columns)
                        {
                            var value = reader[column] == DBNull.Value ? null : reader[column];
                            jObject[column] = value != null ? JToken.FromObject(value) : JValue.CreateNull();
                        }
                        result.Add(jObject);
                    }
                }
            }

            return result;
        }
    }

}
