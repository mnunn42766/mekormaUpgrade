using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Crm
{
    public class Entity
    {
        public LinkEntity link = new LinkEntity();
        public List<String> columns = new List<string>();
        public String entityAlias = String.Empty;
    }
}
