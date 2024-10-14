using System;
using System.Collections.Generic;

namespace Manager.Model.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PageId { get; set; }
        public int CanRead { get; set; }
        public int CanWrite { get; set; }
        public int CanDelete { get; set; }
        public int CanExportExcel { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsModify { get; set; }
        public List<PermissonModifyLogs> permissonModifyLogs { get; set; }
    }

    public class PermissonModifyLogs
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }
        public string UserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
