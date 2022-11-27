using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FocusOPEN.Website.Controls
{
    public interface INestedDataSelectable<T>
    {
        NestedDataControlHelper<T> NestedDataHelper { get; }
        
//        IEnumerable<T> GetParentList();
//        IEnumerable<T> GetChildren(T entity);
//        
//        string GetEntityName(T entity);
//        int GetEntityId(T entity);
    }
}
