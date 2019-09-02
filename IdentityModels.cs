using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using GateBoys.Models;
namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }
        
        public System.Data.Entity.DbSet<GateBoys.Models.InventoryProduct> Products { get; set; }

        

        public System.Data.Entity.DbSet<GateBoys.Models.Category_> Category_ { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.OrderDetail> OrderDetail { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.orderConfirmation> orderConfirmations { get; set; }


        public System.Data.Entity.DbSet<GateBoys.Models.Order> Orders { get; set; }


        public System.Data.Entity.DbSet<GateBoys.Models.DeliveryOption> DeliveryOptions { get; set; }
        public System.Data.Entity.DbSet<GateBoys.Models.OrderItem> OrderItems { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.Item> Items { get; set; }       

        public System.Data.Entity.DbSet<GateBoys.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.Delivery> Deliveries { get; set; }
        public System.Data.Entity.DbSet<GateBoys.Models.DeliveryAddress> DeliveryAddresses { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.OrderVM> OrderVMs { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.OrdersVM> OrdersVMs { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.OrderTrack> OrderTracks { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.PurchaseOrder> PurchaseOrders { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.OrderRowDetails> OrderRowdetail { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.Shipment> Shipments { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.AdminOrder> AdminOrders { get; set; }

       public System.Data.Entity.DbSet<GateBoys.Models.OrderSupply> OrderSupplies { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.TrackOrder> TrackOrders { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.addInfo> addInfoes { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.brand> brands { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.driver> drivers { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.parcelHandler> parcelHandlers { get; set; }

        public System.Data.Entity.DbSet<GateBoys.Models.stockmanager> stockmanagers { get; set; }
    }
}