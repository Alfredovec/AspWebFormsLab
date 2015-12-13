using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.CompilerServices;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL.Model;
using GameStore.Models.Entities;

namespace GameStore.GameStoreDAL
{
    class GameStoreContext : DbContext
    {
        static GameStoreContext()
        {
            Database.SetInitializer(new GameStoreInitializer());
        }

        public GameStoreContext()
            : base("GameStoreContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasMany(t => t.Quotes)
                .WithOptional(t => t.Quote);
            modelBuilder.Entity<Comment>().HasMany(t => t.Children)
                .WithOptional(t => t.Parent);
            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Game> Games { get; set; }

        public virtual IDbSet<Genre> Genres { get; set; }

        public virtual IDbSet<PlatformType> PlatformTypes { get; set; }

        public virtual IDbSet<Publisher> Publishers { get; set; }

        public virtual IDbSet<Payment> Payments { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<OrderDetail> OrderDetails { get; set; }

        public virtual IDbSet<GameViewCounter> GameViewCounters { get; set; }

        public virtual IDbSet<DeletedModel> DeletedModels { get; set; }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }

        public virtual IDbSet<ManagerProfile> ManagerProfiles { get; set; }

        public virtual void SetState(object entity, EntityState state)
        {
            Entry(entity).State = state;
        }
    }
}