using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wing1.Models;

namespace Wing1.Models
{
    public class ViewKintaiModel
    {
        public Users Users { get; set; }
        //public IEnumerable<Kintai> Kintai { get; set; }
        public Kintai Kintai {get; set;}
    }
}