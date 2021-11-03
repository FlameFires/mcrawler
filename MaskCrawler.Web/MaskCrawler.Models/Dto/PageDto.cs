using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Text;

namespace MaskCrawler.Models.Dto
{
    public class PageDto
    {
        public int PageNumber { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;

        public string Conditions { get; set; }
        public string Orderby { get; set; }
        public IEnumerable<OrderByDto> Orderbys { get; set; }

        public string Parameters { get; set; }
    }
}
