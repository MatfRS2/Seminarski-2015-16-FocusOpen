using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FocusOPEN.Website.Controls
{
    /// <summary>
    /// Manages the nestedness and datasource of a list control. Because the methods used in the nestedness logic are delegates this
    /// allows for them to be overriden at the control level to allow for custom behaviour without affecting the way nestedness is
    /// introduced in here. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NestedDataControlHelper<T>
    {
        protected GetParentList getParentList;
        protected GetChildren getChildren;
        protected GetEntityName getName;
        protected GetEntityId getId;
        protected string prefix;
        public string Prefix
        {
            get { return prefix ?? " "; }
            set { prefix = value; }
        }

        public delegate IEnumerable<T> GetParentList();
        public delegate IEnumerable<T> GetChildren(T entity);
        public delegate string GetEntityName(T entity);
        public delegate int GetEntityId(T entity); 

        public NestedDataControlHelper()
        {
//            getParentList = getParentListArg;
//            getChildren = getChildrenArg;
//            getName = getNameArg;
//            getId = getIdArg;
        }

        /// <summary>
        /// Constructor allowing for passing delegates to the methods that do the job - this way we are passing pointers
        /// to these methods that could be implemented in the control rather than here and override the default logic with
        /// custom one
        /// </summary>
        public NestedDataControlHelper(GetParentList getParentListArg, GetChildren getChildrenArg, GetEntityName getNameArg, GetEntityId getIdArg, string prefixArg)
        {
            getParentList = getParentListArg;
            getChildren = getChildrenArg;
            getName = getNameArg;
            getId = getIdArg;
            prefix = prefixArg;
        }

        public object GetDataSource()
        {
            var parentList = getParentList();
            var lookupList = new List<LookupItem>();

            foreach (var entity in parentList)
                AddEntity(entity, 0, lookupList);

            return lookupList;
        }

        private void AddEntity(IEnumerable<T> entities, int level, List<LookupItem> data)
        {
            foreach (T entity in entities)
                AddEntity(entity, level, data);
        }

        public void AddEntity(T entity, int level, List<LookupItem> data)
        {
            AddSingleEntity(entity, level, data);
            AddEntity(getChildren(entity), level + 1, data);
        }

        private void AddSingleEntity(T entity, int level, ICollection<LookupItem> data)
        {
            string prefix = GetPrefixString(Prefix, level * 3);
            string name = prefix + getName(entity);
            int id = getId(entity);

            var li = new LookupItem(id, name);
            data.Add(li);
        }

        private static string GetPrefixString(string s, int count)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
                sb.Append(s);

            return sb.ToString();
        }

        public string GetDataTextField()
        {
            return "Name";
        }

        public string GetDataValueField()
        {
            return "Id";
        }

    }
}
