using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.Interface.IDeleteModel
{
    public class DeletedItemModel<T> : IDeletedItemModel<T> where T : class
    {
        public List<T> DeletedItems { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
