﻿using Businesss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;

namespace API_Cafina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        OrderBUS orderBus = new OrderBUS();
        Order_ThongKeBUS odTK = new Order_ThongKeBUS();
        List<OrderModel> listOrder;

        Order_DetailsBUS odDetail = new Order_DetailsBUS();

        [Route("Get_Total_Order")]
        [HttpGet]
        public IActionResult GetTotalOrder()
        {
            try
            {
                int result = odTK.GetTotalOrder();
                return result != 0 ? Ok(result): NotFound();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Route("Get_OrderDetail_ByUsId")]
        [HttpGet]
        public IActionResult GetListOrderByUsId(int usid)
        {
            try
            {

                List<Order_Details> list = odDetail.GetListOrderByUsId(usid);
                int total = list.Count;
                return list != null ? Ok(new { totalItems = total, data = list }) : NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("Get_ListOrder_Manage")]
        [HttpPost]
        public IActionResult GetListOrderManage([FromBody] Dictionary<string,object> formData)
        {
            int pageIndex = 0;
            if(formData.Keys.Contains("pageIndex") && !string.IsNullOrEmpty(formData["pageIndex"].ToString()))
            {
                pageIndex = int.Parse(formData["pageIndex"].ToString());
            }
            int pageSize = 0;
            if (formData.Keys.Contains("pageSize") && !string.IsNullOrEmpty(formData["pageSize"].ToString()))
            {
                pageSize = int.Parse(formData["pageSize"].ToString());
            }
            int status = 0;
            if (formData.Keys.Contains("status") && !string.IsNullOrEmpty(formData["status"].ToString()))
            {
                status = int.Parse(formData["status"].ToString());
            }

            int total = 0;
            listOrder = orderBus.GetListOrderManage(pageIndex, pageSize,out total, status);
            return listOrder != null ? Ok(new { totalItems = total, data = listOrder }) : NotFound();
        }


        [Route("Get_Order_ById")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            OrderModel order = orderBus.GetById(id);
            if(order == null ) return NotFound();
            return Ok(order);
        }
        [Route("Create_Oder")]
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderModel order)
        {
            var result = orderBus.CreateOrder(order);
            return result >= 1 ? Ok("Thêm thành công") : BadRequest("Thêm không thành công");

        }
        [Route("Delete_Order")]
        [HttpDelete]
        public IActionResult DeleteOrder(int orderId)
        {
            var result = orderBus.DeleteOrder(orderId);
            return result >= 1 ? Ok("Xóa thành công") : BadRequest("Xóa không thành công");
        }
        [Route("Update_Order")]
        [HttpPut]
        public IActionResult UpdateOrder(OrderModel order)
        {
            var result = orderBus.UpdateOrder(order);
            return result >= 1 ? Ok("Sửa thành công") : BadRequest("Sửa không thành công");

        }

        [AllowAnonymous]
        [Route("Search_Order")]
        [HttpPost]
        public IActionResult Search_Order([FromBody] Dictionary <string,object> formData)
        {
            long total = 0;
            try
            {
                int page = int.Parse(formData["page"].ToString());
                int pageSize = int.Parse(formData["pageSize"].ToString());
                string name = "";
                if (formData.Keys.Contains("name") && !string.IsNullOrEmpty(formData["name"].ToString()))
                {
                    name = formData["name"].ToString();
                }
                listOrder = orderBus.SearchOrder(page,pageSize, out total, name);

                return Ok(new
                {
                    TotalItem = total,
                    Data = listOrder,
                    Page = page,
                    PageSize = pageSize
                   
                }) ;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("ThongKe_SoDonHang_TheoUser")]
        [HttpPost]
        public IActionResult ThongKe([FromBody] Dictionary<string,object> formData)
        {
            try
            {
                int  page = int.Parse (formData["page"].ToString());
                int  pageSize = int.Parse(formData["pageSize"].ToString());
                string fullname = "";
                if (formData.Keys.Contains("fullname") && !string.IsNullOrEmpty(Convert.ToString(formData["fullname"])))
                    fullname = Convert.ToString(formData["fullname"]);
                DateTime ? ngaybd = null;
                if(formData.Keys.Contains("ngaybd") && !string.IsNullOrEmpty(Convert.ToString(formData["ngaybd"])))
                {
                    var dt = DateTime.Parse(formData["ngaybd"].ToString());
                    ngaybd = new DateTime(dt.Year,dt.Month,dt.Day,0,0,0,0);
                }
                DateTime? ngaykt = null;
                if (formData.Keys.Contains("ngaykt") && !string.IsNullOrEmpty(Convert.ToString(formData["ngaykt"])))
                {
                    var dt = DateTime.Parse(formData["ngaykt"].ToString());
                    ngaykt = new DateTime(dt.Year, dt.Month, dt.Day);
                }
              
                int total = 0;
                var result = odTK.ThongKeSoLuongDonHangTheoKH(page,pageSize,out total,fullname,ngaybd,ngaykt);
                long abc = total;
                return Ok(new
                {
                    TotalItem = total,
                    Data = result,
                    Page = page,
                    PageSize = pageSize
                });
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("ThongKe_TongSoLuongDonHang")]
        [HttpPost]
        public IActionResult ThongKeTongSlDonHang([FromBody] Dictionary<string,object> formData)
        {
            try
            {
                DateTime? fr_date = null;
                DateTime? to_date = null;
                if (formData.Keys.Contains("fr_date") && !string.IsNullOrEmpty(Convert.ToString(formData["fr_date"])))
                {
                    var dt = DateTime.Parse(formData["fr_date"].ToString());
                    fr_date = new DateTime(dt.Year,dt.Month,dt.Day);

                }
                if (formData.Keys.Contains("to_date") && !string.IsNullOrEmpty(formData["to_date"].ToString()))
                {
                    var dt= DateTime.Parse((formData["to_date"]).ToString());
                    to_date = new DateTime(dt.Year,dt.Month,dt.Day);
                }
               
                ThongKe_SLOrderModel tk = odTK.ThongKeSoLuongDonHangTheoTgian(fr_date, to_date);
                if (tk != null) return Ok(tk);
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("ThongKe_SLDonHangTheoThang")]
        [HttpPost]
        public IActionResult ThongKeSLOrderTheoThang([FromBody] Dictionary<string,object> formData)
        {
            try
            {
                int fr_month = 0;
                int to_month = 0;

                int year = 0;
                if(formData.Keys.Contains("fr_month" )&& !string.IsNullOrEmpty(formData["fr_month"].ToString()))
                {
                    fr_month = int.Parse(formData["fr_month"].ToString());
                }
                if (formData.Keys.Contains("to_month") && !string.IsNullOrEmpty(formData["to_month"].ToString()))
                {
                    to_month = int.Parse(formData["to_month"].ToString());
                }

                if (formData.Keys.Contains("year") && !string.IsNullOrEmpty(formData["year"].ToString()))
                {
                    year = int.Parse(formData["year"].ToString());
                }
                List<ThongKeOrderByMonthModel> model = odTK.ThongKeSLOrderTheoThang(fr_month,to_month, year);
                return model !=null? Ok(model) : NotFound();

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}
