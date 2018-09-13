using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common;
using Models.Entities;

namespace Game2048EF
{
    public partial class GameDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly ILogger _logger;
        private readonly ConfigHelper _configHelper;
        public GameDbContext(DbContextOptions<GameDbContext> options, ILogger<GameDbContext> logger, ConfigHelper configHelper) : base(options)
        {

            _logger = logger;
            _configHelper = configHelper;
        }

        public GameDbContext()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfos { get; set; }
        public virtual DbSet<SignWeekReward> SignWeekRewards { get; set; }
        public virtual DbSet<SignInHistory> SignInHistories { get; set; }
        public virtual DbSet<Mall> Malls { get; set; }

        public virtual DbSet<UserGood> UserGoods { get; set; }

        public virtual DbSet<GoodsUseHistory> GoodsUseHistories { get; set; }
        public virtual DbSet<BuyGoodHistory> BuyGoodHistories { get; set; }
        public virtual DbSet<UserPayHistory> UserPayHistories { get; set; }

        public virtual DbSet<UserScore> UserScores { get; set; }

        public virtual DbSet<GameGate> GameGates { get; set; }

        public virtual DbSet<InviteHistory> InviteHistories { get; set; }

        public virtual DbSet<InviteReward> InviteRewards { get; set; }
        public virtual DbSet<Poster> Posters { get; set; }
        public virtual DbSet<PosterGallery> PosterGalleries { get; set; }
        public virtual DbSet<PosterReward> PosterRewards { get; set; }


        public override int SaveChanges()
        {

            try
            {
                return base.SaveChanges();

            }
            catch (Exception e)
            {

                _logger.LogError(e, e.Message);

                throw new Exception("数据保存错误");
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                return await base.SaveChangesAsync(cancellationToken);

            }
            catch (Exception e)
            {
                
                    _logger.LogError(e, e.Message);
                
                throw new Exception("数据保存错误");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_configHelper.Config.Connection);
            }
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 

        }
    }
}
