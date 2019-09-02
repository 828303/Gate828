using GateBoys.Models;
using GateBoys.ViewModels;
//using GateBoys.Models.InventoryProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;
using GateBoys.Helpers;

namespace GateBoys.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult ShowAll()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Sales()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Paym()
        {
            return RedirectToAction("Payment", "Checkout");
        }
        public ActionResult Sales2()
        {
            return RedirectToAction("Payment", "Checkout");
        }
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartVIEWModel_
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }


        public ActionResult AddToCart(int id)
        {

            // Retrieve the product from the database
            var addedProduct = db.Products
                .Single(album => album.productId == id);



            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);


            cart.AddToCart(addedProduct);
            Session["cout1"] = cart.GetCount();
            

            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "Main");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpGet]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id);
            ViewData["CartCount"] = cart.GetCount();
            Session["cout1"] = cart.GetCount();
            return RedirectToAction("Index");

        }


        [HttpGet]
        public ActionResult Minus_1(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.Minus_1(id);
            ViewData["CartCount"] = cart.GetCount();
            Session["cout1"] = cart.GetCount();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Plus_1(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.Plus_1(id);
            ViewData["CartCount"] = cart.GetCount();
            Session["cout1"] = cart.GetCount();

            return RedirectToAction("Index");
        }


        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            Session["cout1"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        [Authorize]
        public ActionResult CheckOut()
        {
            var get = ShoppingCart.GetCart(this.HttpContext);
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var name = currentUser.UserName;
            //var name = User.Identity.GetUserName();
            var currentUserId1 = User.Identity.GetUserId();
            var manager1 = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser1 = manager.FindById(User.Identity.GetUserId());
            //addr = currentUser.Address;
            
            //
            var sname = currentUser.Email;
            var cell = db.addInfoes.FirstOrDefault(a=>a.addInfoOf==sname).phone;

            Order neworder = new Order();
            var myCart = ShoppingCart.GetCart(this.HttpContext);
           var CartItems = myCart.GetCartItems();
            if(CartItems.Count > 0)
            {
                neworder.orderedQty = myCart.GetCount();
                for (int p = 0; p < CartItems.Count; p++)
                {
                    int prodId = CartItems[p].productID;
                    neworder.orderedItems += $"{ db.Products.ToList().Where(a => a.productId == prodId).FirstOrDefault().productName }" +
                        $" - ({CartItems[p].Count}), ";

                }
            }

            //neworder.orderedItems;
            //neworder.orderedQty = qtyItems;
            string email = User.Identity.GetUserName().ToString();
            var user=db.addInfoes.FirstOrDefault(a=>a.addInfoOf== email);

            neworder.OrderId = neworder.OrderId + 1;
            neworder.OrderNumber = "#gateboys" + user.addInfoOf.Substring(0, 2) + Convert.ToString(neworder.OrderId) + neworder.OrderDate.Day.ToString() + neworder.OrderDate.Minute.ToString();
            neworder.OrderQuantity = get.GetCount();
            neworder.TotalOrderCost = get.GetTotal();
            neworder.Status = "Awaiting Payment";
            neworder.username = user.addInfoOf;
            TempData["naaam"] = (user.midName==null)?$"{user.name} {user.surname}": $"{user.name} {user.midName} {user.surname}";
            neworder.Cell = cell;
            if (TempData["Address"]!=null)
            {
                neworder.DeliveryAddress = TempData["Address"] as string;
            }
           
            string d = neworder.DeliveryAddress;
            db.Orders.Add(neworder);
            neworder.OrderId = neworder.OrderId + 1;
            db.SaveChanges();
            string subject = $"Order {neworder.OrderNumber} received, Awaitng Payment";
            string emailbody= $"Hi {user.name} {user.surname}. We have received your order of order number{neworder.OrderNumber}, Please continue and pay for your order. Contact us on gateboys@mail.com for enquiries";

            emailhelper.sendMail(user.addInfoOf,subject,emailbody);

            TempData["Order"] = neworder;
            InventoryProduct st = new InventoryProduct();
            db.SaveChanges();
            get.CreateOrder(neworder);

            //return View();
            return RedirectToAction("Payment");
        }

        public ActionResult DetailedOrder(int OrderId)
        {
            return View(db.OrderItems.Where(x => x.OrderId == OrderId).ToList());
        }

        public ActionResult Complete()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            InventoryProduct st = new InventoryProduct();
            foreach (var prod in cart.GetCartItems())
            {
                st = db.Products.ToList().Find(a => a.productId == prod.productID);
                var inv = db.Products.ToList().Find(x => x.productId == st.productId);
                inv.quantityOnHand = inv.quantityOnHand - prod.Count;

                //pa.save(prod.product.productName, prod.Count);

            }

            db.SaveChanges();
            cart.EmptyCart();
            return RedirectToAction("MyOrders");
        }
        public ActionResult Payment()
        {
            Order neworder = TempData["Order"] as Order;

            if (TempData["Address"] != null)
            {
                string yho = TempData["Address"] as string;
                neworder.DeliveryAddress = yho;
            }
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            Session["cout1"] = cart.GetCount();

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Payment(Order oo)
        {
            return View(oo);
        }
        public ActionResult Location()
        {
            return View("Location");
        }

        public ActionResult Pickup()
        {
            return View("Pickup");
        }
        
        //Delivery
        [Authorize]
        public ActionResult Delivery()
        {
            return View();
        }

        // POST: DeliveryAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delivery([Bind(Include = "DeliveryAddressId,Address,street_number,route,administrative_area_level_1,locality,country,postal_code,adress")] DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress.adress == null || deliveryAddress.adress == "")
            {
                if (deliveryAddress.country != null && deliveryAddress.locality != null && deliveryAddress.postal_code != null && deliveryAddress.administrative_area_level_1 != null && deliveryAddress.route != null && deliveryAddress.adress == null)
                {
                    deliveryAddress.Address = deliveryAddress.addressCMBN();
                    deliveryAddress.adress = deliveryAddress.addressCMBN();
                }
                else if (deliveryAddress.country == null && deliveryAddress.locality == null && deliveryAddress.postal_code == null && deliveryAddress.administrative_area_level_1 == null && deliveryAddress.route == null && deliveryAddress.adress != null)
                {
                    deliveryAddress.adress = deliveryAddress.adress;
                }
                else if (deliveryAddress.country == null && deliveryAddress.locality == null && deliveryAddress.postal_code == null && deliveryAddress.administrative_area_level_1 == null && deliveryAddress.route == null && deliveryAddress.adress == null)
                {
                    deliveryAddress.adress = null;
                }


            }
            string da = deliveryAddress.adress;

            if (ModelState.IsValid)
            {
                TempData["Address"] = deliveryAddress.adress;

                db.DeliveryAddresses.Add(deliveryAddress);
                db.SaveChanges();


                return RedirectToAction("CheckOut", "ShoppingCart");
            }

            return View(deliveryAddress);
        }



        public ActionResult PayFast(decimal Amount, string delcl, string oid)
        {
            ViewData["CartCount"] = 0;

            var get = ShoppingCart.GetCart(this.HttpContext);
            Order neworder = new Order();
            string dddd = delcl;
            string orderIdvalue = oid;
            string email= User.Identity.GetUserName().ToString();

            TrackOrder trackOrd = new TrackOrder();
            trackOrd.TrackId = trackOrd.TrackId + 1;
            trackOrd.OrderNumber = orderIdvalue;
            trackOrd.OrderPlaced = true;
            trackOrd.OrderPlacedDate = DateTime.Now.ToString();
            trackOrd.UserMail = email;
            trackOrd.colIdNum = db.addInfoes.FirstOrDefault(a=>a.addInfoOf==email).idNum;

            if (dddd == "collect")
            {
                trackOrd.delColect = "Collect";
            }
            else if (dddd == "deliver")
            {
                trackOrd.delColect = "Deliver";
            }
            db.TrackOrders.Add(trackOrd);
            db.SaveChanges();

            List<Item> cart = (List<Item>)Session["ShoppingCart"];
            if (cart != null)
            {
                foreach (Item cad in cart)
                {
                    neworder.TotalOrderCost += Convert.ToDecimal(cad.Amount());
                }
            }

            var updSt = db.Orders.ToList().Where(st => st.OrderNumber == orderIdvalue && st.Status== "Awaiting Payment" && st.username== User.Identity.GetUserName().ToString()).ToList();
            if (updSt.Count > 0)
            {
                foreach(var u in updSt)
                {
                    u.Status = "Paid";
                }
                var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrd.UserMail);
                string subject = $"Payment for order {orderIdvalue} is Successful";
                string emailbody = $"Hi {user.name} {user.surname}, We have received payment for order number {orderIdvalue}, Please use your order number({orderIdvalue}) to get updated on status order. Contact us on gateboys@mail.com for enquiries";
                emailhelper.sendMail(user.addInfoOf, subject, emailbody);
                db.SaveChanges();
            }
            neworder.OrderQuantity = get.GetCount();
            neworder.TotalOrderCost = get.GetTotal();


            var id = User.Identity.GetUserId();
            string amount = neworder.TotalOrderCost.ToString();

            // string orderId = new Random().Next(1, 9999).ToString();
            string name = "gateboys " + orderIdvalue;
            string description = "Gateboys and TV";

            string site = "";
            string merchant_id = "";
            string merchant_key = "";

            // Check if we are using the test or live system
            string paymentMode =ConfigurationManager.AppSettings["PaymentMode"];

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchant_id = "10000100";
                merchant_key = "46f0cd694581a";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchant_id = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchant_key = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
            }
            // Build the query string for payment site

            StringBuilder str = new StringBuilder();
            str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
            str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
            str.Append("&return_url=" + HttpUtility.UrlEncode("http://gateboys.azurewebsites.net/ShoppingCart/paySuccess/" + orderIdvalue));
            str.Append("&cancel_url=" + HttpUtility.UrlEncode(ConfigurationManager.AppSettings["PF_CancelURL"]));
            //str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

            //str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderIdvalue));
            str.Append("&amount=" + HttpUtility.UrlEncode(amount));
            str.Append("&item_name=" + HttpUtility.UrlEncode(name));
            str.Append("&item_description=" + HttpUtility.UrlEncode(description));

            // Redirect to PayFast
            Response.Redirect(site + str.ToString());

            return View();
        }
        public ActionResult paySuccess(string orderIdvalue)
        {
            var paid = db.Orders.FirstOrDefault(a => a.OrderNumber == orderIdvalue);
            if (paid !=null)
            {
                ViewBag.OrderN = paid.OrderNumber;
                ViewBag.price = paid.OrderQuantity;
                ViewBag.Date = DateTime.Now;
                ViewBag.Total = paid.TotalOrderCost;
            }
            return View();
        }

        public ActionResult Create(InventoryProduct cartClass, int id)
        {
            var user = User.Identity.GetUserId();
            InventoryProduct p = db.Products.Find(id);
            int count = db.Products.Count(m => m.productId == id);
            if (count > 0)
            {
                var find = db.Products.Where(m => m.productId == id);
                foreach (var item in find)
                {
                    InventoryProduct cart = db.Products.Find(item.productId);
                    cart.quantityOnHand += 1;
                    cart.unitPrice += p.unitPrice;
                    db.Entry(cart).State = EntityState.Modified;
                }
                p.quantityOnHand -= 1;
                if (p.quantityOnHand < p.minimumStock)
                {
                    p.status = "Almost Gone!";
                }
                else if (p.quantityOnHand == 0)
                {
                    p.status = "Out of Stock!";
                }
                else
                {
                    p.status = "In Stock";
                }
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                cartClass.productName = p.productName;
                cartClass.unitPrice = p.unitPrice;
                cartClass.quantityOnHand = 1;
                cartClass.productId = id;
                //cartClass.UserId = user;

                if (cartClass.quantityOnHand < cartClass.minimumStock)
                {
                    cartClass.status = "Almost Gone!";
                }
                if (cartClass.quantityOnHand == 0)
                {
                    cartClass.status = "Out of Stock!";
                }
                else
                {
                    cartClass.status = "In Stock";
                }
                db.Products.Add(cartClass);
                p.quantityOnHand -= 1;



                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Payment_Successfull()
        {
            return View();
        }

    }
}