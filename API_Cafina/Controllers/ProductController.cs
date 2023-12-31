﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Businesss;
using Model;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

namespace API_Cafina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class ProductController : ControllerBase
    {
        private IWebHostEnvironment _env;
       public ProductController(IWebHostEnvironment env) 
       {
            _env = env;
       }
        ProductBUS proBus = new ProductBUS();
        ProductTKBUS proTK = new ProductTKBUS();
        List<ProductModel> Product_List;
        string _path;

        [Route("ThongKeSLBanProduct")]

        [HttpPost]
        public IActionResult ThongKeSLBanProduct([FromBody] Dictionary<string,object> formData)
        {
            try
            {
                DateTime? to_date = null;
                DateTime? fr_date = null;
                if (formData.Keys.Contains("fr_date") && !string.IsNullOrEmpty(formData["fr_date"].ToString()))
                {
                    fr_date = DateTime.Parse(formData["fr_date"].ToString());
                }

                if (formData.Keys.Contains("to_date") && !string.IsNullOrEmpty(formData["to_date"].ToString()))
                {
                    to_date = DateTime.Parse(formData["to_date"].ToString());
                }

              
                List<ProductTKSLBanModel> model = proTK.ThongKeSLBanProduct(fr_date, to_date);

                return model != null ? Ok(model) : NotFound();

            }catch(Exception ex) {
                throw ex;
            }
        }
        [Route("PhanTrang_DSProduct")]
        [HttpPost]
        public IActionResult PhanTrang([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                int? page = null;
                if (formData.Keys.Contains("pageIndex") && !string.IsNullOrEmpty(formData["pageIndex"].ToString()))
                {
                    page = int.Parse(formData["pageIndex"].ToString());
                }
                int? pageSize = null;
                if (formData.Keys.Contains("pageSize") && !string.IsNullOrEmpty(formData["pageSize"].ToString()))
                {
                    pageSize = int.Parse(formData["pageSize"].ToString());
                }
                int total = 0;
                Product_List = proBus.GetPhanTrang(page, pageSize, out total);
                return Product_List != null ? Ok(new { TotalItems = total, Data = Product_List, Page = page, PageSize = pageSize }) : NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetProductById(string productId)
        {
            var result = proBus.GetProductById(productId);
            return result == null? NotFound():Ok(result);
        }
        [Route("Create_Product")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductModel product)
        {
            
            var result = proBus.CreateProduct(product);
            return result >= 1 ? Ok("Thêm thành công"):BadRequest("Thêm không thành công");
        }
        [Route("Update_Product")]
        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductModel product)
        {
            var result = proBus.UpdateProduct(product);
            return result >= 1 ? Ok("Sửa thành công") : BadRequest("Sửa không thành công");

        }
        [Route("Delete_Product")]
        [HttpDelete]
        public IActionResult DeleteProduct(string productId)
        {
            var result = proBus.DeleteProduct(productId);
            return result >= 1 ? Ok("Xóa thành công") : BadRequest("Xóa không thành công");

        }

        [AllowAnonymous]
        [Route("Search")]
        [HttpPost]
        public IActionResult Search([FromBody] Dictionary<string,object> formData)
        {
            try
            {

                int pageIndex = 0;
                int pageSize = 0;

                string value = "";
                if (formData.Keys.Contains("value") && !string.IsNullOrEmpty(Convert.ToString(formData["value"]))) {
                    value = Convert.ToString(formData["value"]); 
                }
                if (formData.Keys.Contains("pageIndex") && !string.IsNullOrEmpty(Convert.ToString(formData["pageIndex"])))
                {
                    pageIndex = int.Parse(formData["pageIndex"].ToString());
                }
                if (formData.Keys.Contains("pageSize") && !string.IsNullOrEmpty(Convert.ToString(formData["pageSize"])))
                {
                    pageSize = int.Parse(formData["pageSize"].ToString());
                }
                long total = 0;
                Product_List = proBus.Search(pageIndex, pageSize, out total, value);
                if (Product_List.Count > 0)
                {
                    return Ok(
                    new
                   {
                       TotalItems = total,
                       Data = Product_List,
                       Page = pageIndex,
                       PageSize = pageSize
                   }
                   );
                }
                else return NotFound();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Route("Top5SanPhamBanChay")]
        [HttpPost]
        public IActionResult ThongKeSanPhamBanChay([FromBody] Dictionary<string,object> formData)
        {
            try
            {
                DateTime? to_date = null;
                DateTime? fr_date = null;

                if (formData.Keys.Contains("fr_date") && !string.IsNullOrEmpty(Convert.ToString(formData["fr_date"])))
                {

                    fr_date = DateTime.Parse(formData["fr_date"].ToString());
                }
                if (formData.Keys.Contains("to_date") && !string.IsNullOrEmpty(Convert.ToString(formData["to_date"])))
                {

                    to_date = DateTime.Parse(formData["to_date"].ToString());
                }


             
                var result = proTK.ThongKeSanPhamBanChay(fr_date, to_date);
                return result != null ? Ok(result) : NotFound();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [NonAction]
        public string CreatePathFile(string RelativePathFileName)
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                builder.SetBasePath(Directory.GetCurrentDirectory());
                IConfigurationRoot configuration = builder.Build();
                IConfigurationSection configurationSection = configuration.GetSection("AppSettings").GetSection("PATH");
                _path = configurationSection.Value;
                string serverRootPathFolder = _path;
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = Path.GetDirectoryName(fullPathFile);
                if(!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                return fullPathFile;

            }catch(Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            //Lấy về thư mục hiện tại của project
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "upload\\upload", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }
        [Route("Upload")]
        [HttpPost , DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string filePath = $"upload/{file.FileName}";
                    var fullPath = CreatePathFile(filePath);
                    using(var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Ok(new { fullPath });
                }
                else { return BadRequest(); }
            }catch(Exception ex) 
            {
                return StatusCode(500, "Không tìm thấy");
            }
        }

       
    }
}
