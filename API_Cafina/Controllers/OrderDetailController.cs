﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Businesss;
using Microsoft.AspNetCore.Authorization;

namespace API_Cafina_Manage.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        Order_DetailsBUS odBUS = new Order_DetailsBUS();
        [Route("GetListOrderDetailByOrderId")]
        [HttpGet]
        public IActionResult GetListOrderDetailByOrderId(int orderId)
        {
            try
            {
                List<Order_Details> list = odBUS.GetListOrderDetailByOrderId(orderId);
                return list!=null ? Ok(list) : NotFound();

            }catch(Exception ex)
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

                List<Order_Details> list = odBUS.GetListOrderByUsId(usid);
                int total = list.Count;
                return list != null ? Ok(new { totalItems = total, data = list }) : NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
