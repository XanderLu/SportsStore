using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.Controllers;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.Models;
using SportsStore.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID=1 , Name = "P1"},
                new Product{ProductID=2 , Name = "P2"},
                new Product{ProductID=3 , Name = "P3"},
                new Product{ProductID=4 , Name = "P4"},
                new Product{ProductID=5 , Name = "P5"} ,
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-defalut"" href=""Page1"">1</a>", result.ToString());
        }


        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1,Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2,Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3,Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4,Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5,Name = "P5", Category = "Cat3" } ,
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();


            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");    
            Assert.IsTrue(result[1].Name=="P4" && result[1].Category == "Cat2");    
                
                
                
        }


    }
}
