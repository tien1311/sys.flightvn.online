using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Model.Models.Interface.IDeleteModel
{
    public interface IDeletedItemModel<T>
    {
        List<T> DeletedItems { get; set; }
        DateTime DeletionTime { get; set; }
    }
}
