const urlApiGetTotalOrder = "https://localhost:7284/api-admin/Order/Get_Total_Order";
const urlApiGetTotalUser = "https://localhost:7284/api-admin/User/TotalUser";
const urlApiThongKeDoanhThu  = "https://localhost:7284/api-admin/ThongKeDoanhThu/Thong_Ke_Doanh_Thu";
const urrlApiThongKeSoLuongDonHangTheoThang = "https://localhost:7284/api-admin/Order/ThongKe_SLDonHangTheoThang";
const urlApiGetTotalProductViewInMonth = "https://localhost:7284/api-admin/ProductView/GetTotalProductView"
const urlApiThongKeSoTienTieuDungUs = "https://localhost:7284/api-admin/User/ThongKe_SoTienCuaUs";
async function Start(){
    //showSubNav();
    getTotalOrderByMonth();
    GetTotalProductViews();
    getTotalUser();
    getTotalOrder();
    handleThongKeDoanhThu();
    handleThongKeSoTienTieuDungUs();
}
Start();

function handleThongKeSoTienTieuDungUs(){
  const date = new Date();
  var data = {
    fr_date: date.getMonth()+1,
    to_date: date.getMonth()+1,
    year: date.getFullYear()
  };
  $(".block-user h3 span").html(date.getMonth()+1)
  ThongKeSoTienTieuDungUs(data);
};

async function ThongKeSoTienTieuDungUs(data){
  const promise = new Promise((resolve, reject) => {
    httpPostAsyncCate(urlApiThongKeSoTienTieuDungUs,resolve,reject,data);
  });
  try{
    const res = await promise;
    console.log(res);
    renderListUs(res);
  }catch(err){
    console.log(err);
  }
}

function renderListUs(data){
  var html = data.map(us=>{
      return `
      <tr class="tb-content" data-id = "${us["id"]}">
          <td class="us_id" >
              ${us["id"]}
          </td>
          <td class="fullName">
              ${us["fullName"]}
          </td>
          <td class="phone_number">
             ${us["phone_number"]}
          </td>
          <td class="email">
              ${us["email"]}
          </td>
          <td class="birthday">
              ${ us["birthday"].slice(0,10)}

          </td>
          <td class="gender">
              ${us["gender"]}
          </td>
          <td class="address">
              ${us["address"]}
          </td>
          <td>
            ${handlePrice(us["tongTienMua"])}
          </td>
         
      </tr>
      `
  })
  $(".block-user tbody").html(html.join(""));
  
};
async function GetTotalProductViews(){
  const date = new Date();
  var data = { 
      month: date.getMonth() +1,
      year:date.getFullYear() 
  }
  const promise = new Promise((resolve, reject) =>{
      httpPostAsyncCate(urlApiGetTotalProductViewInMonth,resolve,reject,data)
  })
  try{
      
      const res = await promise;
      renderProductView(res)
  }
  catch(err){
      console.log(err)
  }
}


function renderProductView(data){
  $(".productViews .data").html(data["total"])
}

async function getTotalOrderByMonth(){
  var month = [1,2,3,4,5,6,7,8,9,10,11,12];
  var listTotalOrder = [];
  const date = new Date();
    var data={};
    const promise = new Promise((resolve, reject) =>{
      httpPostAsyncCate(urrlApiThongKeSoLuongDonHangTheoThang,resolve,reject,data);
    })
    try{
      var response = await promise;
      console.log(response)
      handleListDataOrder(response)
    }catch(err){
      listTotalOrder.push(0);
      console.log(err);
    }

  
  
}


async function getTotalUser(){
    $.get({
      url:urlApiGetTotalUser,
      headers: { "Authorization": 'Bearer ' + token }
    })
    .done(res=>{
        $(".statistics-data .totalUser").html(res);
    })
    .fail(err=>{
        console.log(err);
    })
};

async function getTotalOrder(){
    $.get({
      url: urlApiGetTotalOrder,
      headers: { "Authorization": 'Bearer ' + token }})
    .done(res=>{
        
        $(".statistics-data .totalOrder").html(res);
    })
    .fail(err=>{
        console.log(err);
    })
};


function handleThongKeDoanhThu(){
    const date = new Date();
    var data={};
    ThongKe(data)
}
function ThongKe(data){
    $.post({
        url:urlApiThongKeDoanhThu,
        headers: { "Authorization": 'Bearer ' + token },
        data:JSON.stringify(data),
        contentType:"application/json"
    })
    .done(res=>{
      handleListDataDoanhThu(res);
        renderTotalPrice(res);
        console.log(res)
    })
    .fail(err=>{
        console.log(err)
    })
};
function renderTotalPrice(res) {
    var total = 0 ;
    res.forEach(element => {
        total +=element["doanhThu"] 
    });
    $(".totalMoney .data").html(handlePrice(total))
}


function handleListDataDoanhThu(data){
    var listdata = [];
    if(data.length>=2){
      var fr_month = data[0]["thang"];
      var to_month = data[data.length-1]["thang"];
        var mid_month = to_month - fr_month;
      for(var i=1; i<=12; i ++){
           if(i<fr_month || i>to_month){
             listdata[i-1] = 0 
           }
           if(i>fr_month && i<to_month){
            if(mid_month ===2){
                for(j=0;j<data.length;j++){

                    if(data[j]["thang"] == to_month -1){
                        listdata.push(data[j]["doanhThu"]);
                    }
                    else{
                        console.log(data.length)
                        listdata.push(0);
                    }
                }
            }
            if(mid_month > 2){
                for(j=1;j<data.length-1;j++){
                    for(var z = fr_month+1; z<to_month; z++){
                        if(data[j]["thang"] == z){
                          listdata[z-1] = data[j]["doanhThu"];
                        
                        }
                        if(data[j]["thang"] != z)
                        {
                           listdata.push(0);
                        }
                   }
                }
            }
          }
          if(i == fr_month){
            listdata[fr_month-1] = data[0]["doanhThu"];
          }
           if ( i == to_month){

            listdata[to_month-1] = data[data.length-1]["doanhThu"];
           }
      }
    }
    else if(data.length == 1){
        for(var i = 1; i <=12; i ++){
          listdata[data[0]["thang"]-1]=data[0]["doanhThu"]
          if(i<=fr_month || i>=to_month){
            listdata.push(0)
          }
        }
    };

    renderLineDoanhThu(listdata.slice(0,12));
};



function handleListDataOrder(data){
  var listdata = [];
  if(data.length>=2){
    var fr_month = data[0]["month"];
    var to_month = data[data.length-1]["month"];
      var mid_month = to_month - fr_month;
    for(var i=1; i<=12; i ++){
         if(i<fr_month || i>to_month){
           listdata[i-1] = 0 
         }
         if(i>fr_month && i<to_month){
          if(mid_month ===2){
              for(j=0;j<data.length;j++){

                  if(data[j]["month"] == to_month -1){
                      listdata.push(data[j]["totalOrder"]);
                  }
                  else{
                      listdata.push(0);
                  }
              }
          }
          if(mid_month > 2){
              for(j=1;j<data.length-1;j++){
                  for(var z = fr_month+1; z<to_month; z++){
                      if(data[j]["month"] == z){
                        listdata[z-1] = data[j]["totalOrder"];
                      
                      }
                      if(data[j]["month"] != z)
                      {
                         listdata.push(0);
                      }
                 }
              }
          }
        }
        if(i == fr_month){
          listdata[fr_month-1] = data[0]["totalOrder"];
        }
         if ( i == to_month){

          listdata[to_month-1] = data[data.length-1]["totalOrder"];
         }
    }
  }
  else if(data.length == 1){
      for(var i = 1; i <=12; i ++){
        listdata[data[0]["month"]-1]=data[0]["totalOrder"]
        if(i<=fr_month || i>=to_month){
          listdata.push(0)
        }
      }
  };
  var maxTotalOrder = 0 ;
  for (var j = 0; j < listdata.slice(0,12).length-1; j++){
    for (var z = j; z < listdata.slice(0,12).length; z++){
      if(listdata[j]>listdata[z]){
        maxTotalOrder = listdata[j]
      }
    }
  }
  renderLineOrder(listdata.slice(0,12),maxTotalOrder);
};

function renderLineOrder(data,maxTotalOrder){
  let myChart = document.getElementById('lineSoLuongDonHang').getContext('2d');
  
  Chart.defaults.global.defaultFontFamily = 'Lato';
  Chart.defaults.global.defaultFontSize = 14;
  Chart.defaults.global.defaultFontColor = '#777';
  new Chart("lineSoLuongDonHang", 
  {
    type: "line",
    data: {
      labels: xLabel(),
      xAxisID:"Tháng",
      datasets: [{
        label:'Đơn hàng',
        data: data,
        borderColor: "#0766AD",
        borderWidth:1,
        fill: true,
        hoverBorderWidth:3,
        hoverBorderColor:'#0766AD'
      }]
    },
    options: {
      scales: {
        yAxes: [{
            ticks: {
              min: 0,
              max: maxTotalOrder +1,
            }
        }] 
      },      
      title:{
        display:true,
        text:'Số lượng đơn hàng theo tháng',
        fontSize:25
      },
      legend: {display: true},
      tooltips:{
        enabled:true
      }
    }
  });
};


function renderLineDoanhThu(data){
    let myChart = document.getElementById('barSoNguoiDung').getContext('2d');
    Chart.defaults.global.defaultFontFamily = 'Lato';
    Chart.defaults.global.defaultFontSize = 14;
    Chart.defaults.global.defaultFontColor = '#777';
    
    new Chart("barSoNguoiDung", 
    {
      type: "line",
      data: {
        labels: xLabel(),
        xAxisID:"Tháng",
        datasets: [{
          label:'Doanh thu',
          data: data,
          borderColor: "#0766AD",
          borderWidth:1,
          fill: true,
          hoverBorderWidth:3,
          hoverBorderColor:'#0766AD'
        }]
      },
      options: {
        scales: {
          yAxes: [{
              ticks: {
                  beginAtZero: true,
                  callback: function(value, index, values) {
                      return float2vnd(value);
                  }
              }
          }] 
        },             
        title:{
          display:true,
          text:'Doanh thu theo tháng',
          fontSize:25
        },
        legend: {display: true},
        tooltips:{
          enabled:true
        }
      }
    });
};

function float2vnd(value){
    return value.toLocaleString('it-IT', {style : 'currency', currency : 'VND'});
  };
  
  function xLabel(){
    var month = []
    for(var i = 1; i <= 12; i++)
      month.push("T."+i)
    return month;
  };
  



// const ctx2 = document.getElementById('luotXemSanPham').getContext('2d');
// const myChart2 = new Chart(ctx2, {
//     type: 'line',
//     data: {
//         labels: ['Red', 'Blue', 'Yellow'],
//         datasets: [{
//             label: '# of Votes',
//             data: [12, 19, 3],
//             backgroundColor: [
//                 'rgba(255, 99, 132, 0.2)',
//                 'rgba(54, 162, 235, 0.2)',
//                 'rgba(255, 206, 86, 0.2)',
//             ],
//             borderColor: [
//                 'rgba(255, 99, 132, 1)',
//                 'rgba(54, 162, 235, 1)',
//                 'rgba(255, 206, 86, 1)',
//             ],
//             borderWidth: 1
//         }]
//     },
// });
// let myChart = document.getElementById('barSoNguoiDung').getContext('2d');
//     Chart.defaults.global.defaultFontFamily = 'Lato';
//     Chart.defaults.global.defaultFontSize = 16;
//     Chart.defaults.global.defaultFontColor = '#777';
    
//     new Chart("barSoNguoiDung", 
//     {
//       type: "bar",
//       data: {
//         labels: [1,2,3,4,5,6,7,8,9,10,11,12],
//         xAxisID:"Tháng",
//         datasets: [{
//           label:'Doanh thu',
//           data: [10,10,20,30,32,34,35,36,37,38,12,12],
//           borderColor: "#0766AD",
//           backgroundColor:"#0766AD",
//           borderWidth:1,
//           fill: false,
//           hoverBorderWidth:3,
//           hoverBorderColor:'#0766AD'
//         }]
//       },
//       options: {
//         // scales: {
//         //   yAxes: [{
//         //       ticks: {
//         //           beginAtZero: true,
//         //           callback: function(value, index, values) {
//         //               return float2vnd(value);
//         //           }
//         //       }
//         //   }] 
//         // },             
//         title:{
//           display:true,
//           text:'Doanh thu theo tháng',
//           fontSize:16
//         },
//         legend: {display: true},
//         tooltips:{
//           enabled:true
//         }
//       }
//     });