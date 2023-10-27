﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer.Helper;
using System.Data;

namespace DataAccessLayer
{
    public class Order_DetailsDAL
    {
        DataHelper helper = new DataHelper();
        public List<Order_Details> GetListOrderByUsId(int usid)
        {
            try
            {
                DataTable tb = helper.ExcuteReader("GetOrderByUsId", "@usid", usid);
                if (tb != null)
                {
                        List<Order_Details> list = new List<Order_Details>();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        Order_Details order_Details = new Order_Details();
                        order_Details.ProductId = tb.Rows[i]["ProductId"].ToString();
                        order_Details.price = int.Parse(tb.Rows[i]["Price"].ToString());
                        order_Details.Amount = int.Parse(tb.Rows[i]["Amount"].ToString());
                        order_Details.Od_id = int.Parse(tb.Rows[i]["Od_id"].ToString());
                        list.Add(order_Details);

                    }
                    return list;
                }
                else return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
