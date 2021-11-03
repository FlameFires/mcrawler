using System;
using System.Collections.Generic;
using System.Text;

namespace MaskCrawler.Models.Dto
{
    public class OrderByDto
    {
        public string Key {  get; set; }
        public OrderByTyepEnum type {  get; set; }
    }

    public enum OrderByTyepEnum
    {
        Asc,
        Desc,
    }
}
