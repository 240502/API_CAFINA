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
    public class GaleryDAL
    {

        DataHelper helper = new DataHelper();

        public GaleryModel GetByProductId(string productId)
        {
            try
            {
                DataTable tb = helper.ExcuteReader("Pro_Get_Galery_By_ProductId","@ProductId",productId);
                if (tb != null && tb.Rows.Count>0)
                {
                    GaleryModel galery = new GaleryModel();
                    galery.id = int.Parse(tb.Rows[0]["id"].ToString());
                    galery.ProductId = tb.Rows[0]["ProductId"].ToString();
                    galery.thumbnail = tb.Rows[0]["thumbnail"].ToString();
                    return galery;


                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
