// WpfFileUploadSimple/Models/UploadLog.cs

using System;

namespace WpfFileUploadSimple.Models
{
    public class UploadLog
    {
        public int LogId { get; set; }
        public string UploaderUserName { get; set; }
        public string OriginalFileName { get; set; }
        public string SavedPath { get; set; }
        public double FileSizeKB { get; set; }
        public DateTime UploadTimestamp { get; set; }
        public byte[] FileContent { get; set; }
    }
}