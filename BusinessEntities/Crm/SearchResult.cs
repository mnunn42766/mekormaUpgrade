using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Crm
{
    public class SearchResult
    {
        public List<Entity> Entities { get; set; }
        public bool MoreRecords { get; set; }
        public string PagingCookie { get; set; }
        public string MinActiveRowVersion { get; set; }
        public int TotalRecordCount { get; set; }
        public bool TotalRecordCountLimitExceeded { get; set; }
        public string EntityName { get; set; }
        public object ExtensionData { get; set; }

    }
}
