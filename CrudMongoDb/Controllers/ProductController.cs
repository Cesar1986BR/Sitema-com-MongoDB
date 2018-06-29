using CrudMongoDb.App_Start;
using CrudMongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudMongoDb.Controllers
{
    public class ProductController : Controller
    {
        private MongoDBContext dbContext;
        private IMongoCollection<ProductModel> productColletion;

        public ProductController()
        {
            dbContext = new MongoDBContext();
            productColletion = dbContext.database.GetCollection<ProductModel>("product");
        }


        // GET: Product
        public ActionResult Index()
        {

            List<ProductModel> products = productColletion.AsQueryable<ProductModel>().ToList();
            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(string id)
        {
            var productId = new ObjectId(id);
            var product = productColletion.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel product)
        {
            try
            {
                productColletion.InsertOne(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string id)
        {
            var productId = new ObjectId(id);
            var product = productColletion.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ProductModel product)
        {
            try
            {
                var filter = Builders<ProductModel>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
                var update = Builders<ProductModel>.Update
                    .Set(x => x.ProductName, product.ProductName)
                    .Set(x => x.ProductDescription, product.ProductDescription)
                    .Set(x => x.Quantity, product.Quantity);

                var result = productColletion.UpdateOne(filter,update);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            var productId = new ObjectId(id);
            var product = productColletion.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, ProductModel product)
        {
            try
            {
                productColletion.DeleteOne(Builders<ProductModel>.Filter.Eq(x => x.Id, ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
