using System.Data.Entity;
using Lautech_Bank.Models.MyModels;

namespace Lautech_Bank.Models
{
    public class Lautech_BankContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        

        public Lautech_BankContext() : base("name=Lautech_BankContext")
        {
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Lautech_Bank.Models.Lautech_BankContext>());
        }
        
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Userdetail> Userdetails { get; set; }
    }
}
