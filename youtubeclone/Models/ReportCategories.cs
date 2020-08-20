using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class ReportCategories
    {
        public int ReportCategoryid { get; set; }
        public string ReportCategoryName { get; set; }

        public List<ReportCategories> AllreportCategories { get; set; }
    }
}