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
        public ProductModel GetProductById(string productId)
        {
            var reuslt = prodDal.GetProductById(productId);
            return reuslt!=null ? reuslt : null;
        
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
        } 
        public List<ProductModel> Search(int pageIndex, int pageSize, out long total, string ProductName, string CateName)
        {
            List<ProductModel> ProductList = prodDal.Search(pageIndex,pageSize,out total,ProductName, CateName);
            return ProductList;
        }
       public List<ProductModel> GetPhanTrang(int?pageIndex, int ? pageSize, out int total)
        {
            List<ProductModel> ProductList = prodDal.GetPhanTrang(pageIndex, pageSize, out total);
            return ProductList;
        }
        public List<ProductModel> GetByBST(int pageSize,int pageIndex, out int total,string TenBST)
        {
            List<ProductModel> ProductList = prodDal.GetByBST(pageSize,pageIndex,out total,TenBST);
            return ProductList;
        }
        public List<ProductModel> GetRecommend(int pageIndex,int PageSize ,out int  total)
        {
            return prodDal.GetRecommend(pageIndex, PageSize, out total);
        }

    }
}
