// WpfFileUploadSimple/Services/DatabaseService.cs

using Npgsql;
using System;
using WpfFileUploadSimple.Models;

namespace WpfFileUploadSimple.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=9120;Database=postgres;";

        public void SaveLog(UploadLog log)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO upload_logs (uploader_user_name, original_file_name, saved_path, file_size_kb, upload_timestamp, file_content)
                    VALUES (@p_user_name, @p_file_name, @p_saved_path, @p_file_size, @p_timestamp, @p_file_content)";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("p_user_name", log.UploaderUserName);
                    cmd.Parameters.AddWithValue("p_file_name", log.OriginalFileName);
                    cmd.Parameters.AddWithValue("p_saved_path", log.SavedPath);
                    cmd.Parameters.AddWithValue("p_file_size", log.FileSizeKB);
                    cmd.Parameters.AddWithValue("p_timestamp", log.UploadTimestamp);
                    cmd.Parameters.AddWithValue("p_file_content", log.FileContent ?? (object)DBNull.Value); // null 값 처리
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}