﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductModel
    {
        public string ProductId { get; set; }
        public string title { get; set; }
        public int price { get; set; }
        public int discount { get; set; }
        public string description { get; set; }
        public string ChatLieu { get; set; }
        public string HuongDanSuDung { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public int ?Object_id { get; set; }
        public int? Bst_id { get; set; }
        public int? CateDetailId { get; set; }
        public DateTime Created { get; set; }



    }
}
