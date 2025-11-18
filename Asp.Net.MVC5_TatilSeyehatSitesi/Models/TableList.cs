using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Net.MVC5_TatilSeyehatSitesi.Models
{
    public class TableList
    {
        public IPagedList<TBLBLOG> Blogs { get; set; }
        public List<TBLBLOGCOMMENTS> BlogComment { get; set; }
    }
}