﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;
namespace Businesss
{
    public class ProductBUS
    {
        ProductDAL prodDal = new ProductDAL();
        public List<ProductModel> GetAll()
        {

            List<ProductModel> ProductList = prodDal.GetAll();
            return ProductList != null ? ProductList : null;
        }
        public List<ProductModel> PhanTrangDSProduct(int ? page,int ? pageSize,out int total)
        {

            if(page == null ) {
                page = 1;
            }
            if(pageSize == null) pageSize = 10;
            var ProductList = prodDal.PhanTrangDSProduct(page, pageSize,out total);

            return ProductList !=null ? ProductList:null;
        }
        public ProductModel GetProductById(string productId)
        {
            var reuslt = prodDal.GetProductById(productId);
            return reuslt!=null ? reuslt : null;
        
        }
        public List<ProductModel> SearchProduct(string value)
        {
            List<ProductModel> ProductList = prodDal.SearchProduct(value);
            return ProductList!=null ? ProductList:null;
        }
        public int CreateProduct(ProductModel product)
        {
            var result = prodDal.CreateProduct(product);
            return result;
        }
        public int UpdateProduct(ProductModel product)
        {
           
            var result = prodDal.UpdateProduct(product);
            return result;
        }
        public int DeleteProduct(string productId) { 
            var result = prodDal.DeleteProduct(productId);
            return result;
        } public List<ProductModel> Search2(int pageIndex, int pageSize, out long total, string ProductName, string CateName)
        {
            List<ProductModel> ProductList = prodDal.Search2(pageIndex,pageSize,out total,ProductName, CateName);
            return ProductList;
        }
        public List<ThongKeSoLuongBanProductModel> ThongKeSanPhamBanChay(DateTime ? fr_date,DateTime ? td_date)
        {
            var result = prodDal.ThongKeSanPhamBanChay(fr_date, td_date);
            return result != null ? result:null;
        }


    }
}
