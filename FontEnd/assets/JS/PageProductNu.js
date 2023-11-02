const categoryItemsSlide = $(".category-items .block-content .category-item-slide");
const productItemSlide = $(".block-product .product-items");

const urlApiGetProductByCateName = "https://localhost:7284/api-customer/Product/GetProductByCateName";
const urlApiGetProductByCateDetailName = "https://localhost:7284/api-customer/Product/GetProductByCateDetailName";
const urlApiGetRecommended = "https://localhost:7284/api-customer/Product/Recommend";
let thisPage =1;
let pageSize = 10
categoryItemsSlide.slick({
    slidesToShow: 7,
    slidesToScroll: 7,
    infinite: true,
    arrows: true,
    draggable: false,
    prevArrow: `<button type='button' class='slick-prev slick-arrow'><ion-icon name="arrow-back-outline"></ion-icon></button>`,
    nextArrow: `<button type='button' class='slick-next slick-arrow'><ion-icon name="arrow-forward-outline"></ion-icon></button>`,
    dots: true,
    responsive: [
      {
        breakpoint: 1025,
        settings: {
          slidesToShow: 3,
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          arrows: false,
          infinite: false,
        },
      },
    ],
    // autoplay: true,
    // autoplaySpeed: 1000,
});

var listCateName = ["Đồ mặc nhà","Váy"];
var listCateNameDetail = ["Áo nỉ","Áo len","Áo khoác gió"];

async function Start(){
   await GetProductByCateName();
   await GetProductByCateDetailName();
   await handleGetProductRecommended();
}
Start();
async function GetProductByCateName(){
  let i= 0;
  while(i<listCateName.length){
    var data = {cateName : listCateName[i]}
    const promise = new Promise((resolve, reject) => {
      httpGetAsync(urlApiGetProductByCateName,resolve,reject,data)
    });
    var res = await promise
    var galery= await handleGetGalery(res)
    renderBlockProduct(res,i);
    i++;
  }
};

async function GetProductByCateDetailName(){
  let i= 0;
  while(i<listCateNameDetail.length){
    var data = {cateDetailName : listCateNameDetail[i]}
    const promise = new Promise((resolve, reject) => {
      httpGetAsync(urlApiGetProductByCateDetailName,resolve,reject,data)
    });
    var res = await promise
    await handleGetGalery(res)
    await renderBlockProduct(res,i+listCateName.length);
    i++;
  }
};



function renderBlockProduct(products,index) {
  console.log(index)
    var html =products.map(product=>{
      return`
        <div class="product-item col-3">
          <div class="item__image">
              <a href="#">
                  <img src="${GetLinkImgBSTHome(product["productId"]) != null ? GetLinkImgBSTHome(product["productId"]):""}" alt="">
              </a>
              <div class="product-item-button-tocart">
                  <span>Thêm nhanh vào giỏ</span>
              </div>    
          </div>
          <div class="item__details">
              <div class="colors">
                  <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sp234.png);">
                  </div>

                  <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sa010.png)">
                  </div>

                  <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sk010.png)">
                  </div>
              </div>
              <h3 class="product-item-name">

                  <a href="#">

                       ${product["title"]}

                  </a>
              </h3>
              <div class="price-box">
                  <div class="normal-price"> ${
                    product["price"].toString().length>5 ? product["price"].toString().slice(0,3)+"."+product["price"].toString().slice(3,6): product["price"].toString().slice(0,2)+"."+product["price"].toString().slice(2,5)
                  }
                  đ</div>
              </div>
          </div>
        </div>
      `
    });
    productItemSlide[index].innerHTML = html.join("");
    $(`.${productItemSlide[index].classList[1]}`).slick({
      slidesToShow: 3,
      slidesToScroll: 1,
      infinite: true,
      arrows: true,
      draggable: false,
      prevArrow: `<button type='button' class='slick-prev slick-arrow'><ion-icon name="arrow-back-outline"></ion-icon></button>`,
      nextArrow: `<button type='button' class='slick-next slick-arrow'><ion-icon name="arrow-forward-outline"></ion-icon></button>`,
      dots: false,
      responsive: [
        {
          breakpoint: 1025,
          settings: {
            slidesToShow: 3,
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            arrows: false,
            infinite: false,
          },
        },
      ],
      // autoplay: true,
      // autoplaySpeed: 1000,
     });
};


function handleGetProductRecommended(){
  var data = {
    pageIndex: thisPage,
    gender:"Nữ"
  }
  GetProductRecommended(data);
};
function GetProductRecommended(data){
  $.post({
    url : urlApiGetRecommended,
    data : JSON.stringify(data),
    contentType : 'application/json'
  })
  .done(res=> {renderProductRecommended(res)
    console.log(res);
  });
};
function renderProductRecommended(Products){
  const countPage = Math.ceil(Products["totalItems"]/8)
  renderListPage(countPage);
  var html = Products["data"].map(product =>{
    return `
    <div class="product-item col-4">
                        <div class="item__image">
                            <a href="#">
                                <img src="${GetLinkImgBSTHome(product["productId"]) != null ? GetLinkImgBSTHome(product["productId"]):""}" alt="">
                            </a>
                            <div class="product-item-button-tocart">
                                <span>Thêm nhanh vào giỏ</span>
                            </div>    
                        </div>
                        <div class="item__details">
                            <div class="colors">
                                <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sp234.png);">
                                </div>

                                <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sa010.png)">
                                </div>

                                <div class="color__option selected" style="background-image: url(https://media.canifa.com/attribute/swatch/images/sk010.png)">
                                </div>
                            </div>
                            <h3 class="product-item-name">
        
                                <a href="#">
                                    ${product["title"]}
                                </a>
                            </h3>
                            <div class="price-box">
                                <div class="normal-price">${
                                  product["price"].toString().length>5 ? product["price"].toString().slice(0,3)+"."+product["price"].toString().slice(3,6): product["price"].toString().slice(0,2)+"."+product["price"].toString().slice(2,5)
                                } </div>
                            </div>
                        </div>
      </div>
    `
  })
  $(".block-new-product .products").html(html.join(''));
  btnPageNext.on("click",()=>{
    if(thisPage === countPage){

    }
    else{
        thisPage = thisPage + 1;

    }
    changePage(thisPage);
  });
};


 
