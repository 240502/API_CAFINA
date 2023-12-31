﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Helper;
using Model;
namespace DataAccessLayer
{
    public class CategoryDAL
    {
        DataHelper helper = new DataHelper();

        public List<CategoryModel> GetCateByObId(int ob_id)
        {
            try
            {
                DataTable tb = helper.ExcuteReader("Pro_GetCate_ByObId","@ob_id",ob_id);
                if (tb != null)
                {
                    List<CategoryModel> list = new List<CategoryModel>();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        CategoryModel model = new CategoryModel();
                        model.id = int.Parse(tb.Rows[i]["id"].ToString());
                        model.CateName = tb.Rows[i]["CateName"].ToString();
                        model.Object_Id = int.Parse(tb.Rows[i]["Object_id"].ToString());
                        list.Add(model);
                    }
                    return list;
                }
                else return null;

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryModel> GetList()
        {
            try
            {
                DataTable tb = helper.ExcuteReader("GetCateAll");
                if (tb != null)
                {
                    List<CategoryModel> list = new List<CategoryModel>();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        CategoryModel model = new CategoryModel();
                        model.id = int.Parse(tb.Rows[i]["id"].ToString());
                        model.CateName = tb.Rows[i]["CateName"].ToString();
                        list.Add(model);
                    }
                    return list;
                }
                else return null;

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public CategoryModel GetCateById(int id)
        {
            try 
            {
                var result = helper.ExcuteReader("Pro_Get_Cate_By_Id","@id",id);
                if (result != null)
                {
                    CategoryModel cate = new CategoryModel();
                    cate.id = int.Parse(result.Rows[0]["id"].ToString());
                    cate.CateName = result.Rows[0]["CateName"].ToString();
                    cate.Object_Id = int.Parse(result.Rows[0]["Object_id"].ToString());

                    return cate;
                }
                else return null;

            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public CategoryModel Search(string CateName)
        {
            try
            {
                var result = helper.ExcuteReader("Pro_Search_Cate_By_Name", "@CateName", CateName);
                if (result != null)
                {
                    CategoryModel cate = new CategoryModel();
                    cate.id = int.Parse(result.Rows[0]["id"].ToString());
                    cate.CateName = result.Rows[0]["CateName"].ToString();
                    cate.Object_Id = int.Parse(result.Rows[0]["Object_id"].ToString());
                    return cate;
                }
                else return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Create(string cateName,int obId)
        {
            try
            {
                var result = helper.ExcuteNonQuery("Pro_Create_Cate","@CateName", "@Ob_id", cateName,obId);
                return result;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete( int cateId)
        {
            try
            {
                var result = helper.ExcuteNonQuery("Pro_Delete_Cate", "@id", cateId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(CategoryModel cate)
        {
            try
            {
                var result = helper.ExcuteNonQuery("Pro_Update_Cate", "@id", "@CateName", "@ob_id", cate.id, cate.CateName,cate.Object_Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
