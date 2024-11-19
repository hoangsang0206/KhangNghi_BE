using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Data.Models;

public partial class KhangNghiContext : DbContext
{
    public KhangNghiContext()
    {
    }

    public KhangNghiContext(DbContextOptions<KhangNghiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMember> ChatMembers { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractCategory> ContractCategories { get; set; }

    public virtual DbSet<ContractDetail> ContractDetails { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<FunctionAuthorization> FunctionAuthorizations { get; set; }

    public virtual DbSet<FunctionCategory> FunctionCategories { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<JobAssignment> JobAssignments { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }

    public virtual DbSet<ProductsInWarehouse> ProductsInWarehouses { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionUsage> PromotionUsages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCatalog> ServiceCatalogs { get; set; }

    public virtual DbSet<ServiceImage> ServiceImages { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<StockEntry> StockEntries { get; set; }

    public virtual DbSet<StockEntryDetail> StockEntryDetails { get; set; }

    public virtual DbSet<StockExit> StockExits { get; set; }

    public virtual DbSet<StockExitDetail> StockExitDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AORUS-Laptop;Database=KhangNghi;User Id=sang;Password=123456;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFBB5983F92");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.Ward).HasMaxLength(100);
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Chats__A9FBE7C675545334");

            entity.Property(e => e.ChatId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChatName).HasMaxLength(100);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsGroupChat).HasDefaultValue(false);
        });

        modelBuilder.Entity<ChatMember>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.UserId }).HasName("PK__ChatMemb__78836B02587D7A98");

            entity.Property(e => e.ChatId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JoinedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMembe__ChatI__4A8310C6");

            entity.HasOne(d => d.User).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMembe__UserI__4B7734FF");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D346980C2246F");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CategoryId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SignedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Contracts__Categ__02084FDA");

            entity.HasOne(d => d.Customer).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contracts__Custo__02FC7413");
        });

        modelBuilder.Entity<ContractCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Contract__19093A0BCA532175");

            entity.Property(e => e.CategoryId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<ContractDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC076329C188");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContractD__Contr__0E04126B");

            entity.HasOne(d => d.Product).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ContractD__Produ__0EF836A4");

            entity.HasOne(d => d.Service).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ContractD__Servi__0FEC5ADD");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D89741C7AA");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName).HasMaxLength(150);
            entity.Property(e => e.CusTypeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FaxNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MemberSince)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PositionInCompany).HasMaxLength(150);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__Addre__72C60C4A");

            entity.HasOne(d => d.CusType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CusTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__CusTy__73BA3083");
        });

        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.CusTypeId).HasName("PK__Customer__F3E59F7237B43D5C");

            entity.Property(e => e.CusTypeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CusTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED80F814D5");

            entity.Property(e => e.DepartmentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(150);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11232DB593");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EducationalLevel).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HireDate).HasColumnType("datetime");
            entity.Property(e => e.Major).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PositionId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Address).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Addre__7B5B524B");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Depar__7A672E12");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Posit__7C4F7684");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Employee__60BB9A7960FF9A50");

            entity.Property(e => e.PositionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PositionName).HasMaxLength(100);
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.HasKey(e => e.FuncId).HasName("PK__Function__834DE213BAA48D9C");

            entity.Property(e => e.FuncId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CateId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FuncName).HasMaxLength(100);

            entity.HasOne(d => d.Cate).WithMany(p => p.Functions)
                .HasForeignKey(d => d.CateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Functions__CateI__67DE6983");
        });

        modelBuilder.Entity<FunctionAuthorization>(entity =>
        {
            entity.HasKey(e => e.AuthId).HasName("PK__Function__12C15DD342B8E6B9");

            entity.Property(e => e.FunctionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsAuthorized).HasDefaultValue(false);

            entity.HasOne(d => d.Function).WithMany(p => p.FunctionAuthorizations)
                .HasForeignKey(d => d.FunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunctionA__Funct__41EDCAC5");

            entity.HasOne(d => d.Group).WithMany(p => p.FunctionAuthorizations)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunctionA__Group__42E1EEFE");
        });

        modelBuilder.Entity<FunctionCategory>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Function__27638D14A8C8686D");

            entity.Property(e => e.CateId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CateName).HasMaxLength(100);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB59B49F69A");

            entity.HasIndex(e => e.ContractId, "UQ__Invoices__C90D3468CB7F1951").IsUnique();

            entity.Property(e => e.InvoiceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.PaidDate).HasColumnType("datetime");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalAmout).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Contract).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Contra__7EC1CEDB");

            entity.HasOne(d => d.Employee).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Invoices__Employ__7DCDAAA2");
        });

        modelBuilder.Entity<JobAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__JobAssig__32499E7741C940D0");

            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.JobName).HasMaxLength(200);
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.WorkAddressId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Contract).WithMany(p => p.JobAssignments)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__JobAssign__Contr__2BFE89A6");

            entity.HasOne(d => d.WorkAddress).WithMany(p => p.JobAssignments)
                .HasForeignKey(d => d.WorkAddressId)
                .HasConstraintName("FK__JobAssign__WorkA__2CF2ADDF");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CDD9F57E2");

            entity.Property(e => e.ChatId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsReceiverRead).HasDefaultValue(false);
            entity.Property(e => e.SenderId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__ChatId__503BEA1C");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__51300E55");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD6D0F71DA");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CalculationUnit).HasMaxLength(30);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.Origin).HasMaxLength(100);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductName).HasMaxLength(150);

            entity.HasMany(d => d.Catalogs).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<ProductCatalog>().WithMany()
                        .HasForeignKey("CatalogId")
                        .HasConstraintName("FK__ProductCa__Catal__59904A2C"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__ProductCa__Produ__589C25F3"),
                    j =>
                    {
                        j.HasKey("ProductId", "CatalogId").HasName("PK__ProductC__2829D57BEB9B31FB");
                        j.ToTable("ProductCategories");
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("CatalogId")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<ProductCatalog>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__ProductC__C2513B682C3FCD4B");

            entity.Property(e => e.CatalogId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatalogName).HasMaxLength(100);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07898C1BBF");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__4F7CD00D");
        });

        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC078531990D");

            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpecName).HasMaxLength(100);
            entity.Property(e => e.SpecValue).HasMaxLength(255);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSpecifications)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSp__Produ__52593CB8");
        });

        modelBuilder.Entity<ProductsInWarehouse>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseId, e.ProductId }).HasName("PK__Products__ED48639501ED6A99");

            entity.ToTable("ProductsInWarehouse");

            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsInWarehouses)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductsI__Produ__6383C8BA");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ProductsInWarehouses)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductsI__Wareh__6477ECF3");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42FCF2C5E0870");

            entity.Property(e => e.PromotionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MaxDiscountAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PromotionName).HasMaxLength(150);
            entity.Property(e => e.PromotionType).HasMaxLength(100);

            entity.HasMany(d => d.Products).WithMany(p => p.Promotions)
                .UsingEntity<Dictionary<string, object>>(
                    "PromotionProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__Promotion__Produ__55BFB948"),
                    l => l.HasOne<Promotion>().WithMany()
                        .HasForeignKey("PromotionId")
                        .HasConstraintName("FK__Promotion__Promo__54CB950F"),
                    j =>
                    {
                        j.HasKey("PromotionId", "ProductId").HasName("PK__Promotio__9984E3A38D5F4D06");
                        j.ToTable("PromotionProducts");
                        j.IndexerProperty<string>("PromotionId")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(50)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<PromotionUsage>(entity =>
        {
            entity.HasKey(e => e.UsageId).HasName("PK__Promotio__29B197202BF46448");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PromotionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.PromotionUsages)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Custo__056ECC6A");

            entity.HasOne(d => d.Invoice).WithMany(p => p.PromotionUsages)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Promotion__Invoi__047AA831");

            entity.HasOne(d => d.Promotion).WithMany(p => p.PromotionUsages)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Promo__038683F8");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.JwtId }).HasName("PK__RefreshT__24A725C6552FD809");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JwtId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__1B9317B3");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AF3FCF81A");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A7B94FFCF");

            entity.Property(e => e.ServiceId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CalculationUnit).HasMaxLength(30);
            entity.Property(e => e.CatalogId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);

            entity.HasOne(d => d.Catalog).WithMany(p => p.Services)
                .HasForeignKey(d => d.CatalogId)
                .HasConstraintName("FK__Services__Catalo__5AEE82B9");
        });

        modelBuilder.Entity<ServiceCatalog>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__ServiceC__C2513B6804013E8A");

            entity.Property(e => e.CatalogId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CatalogName).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceI__3214EC0707080644");

            entity.Property(e => e.ServiceId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceImages)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceIm__Servi__2EA5EC27");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__Shifts__C0A8388168E5E964");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftName).HasMaxLength(100);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<StockEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__StockEnt__F57BD2F7D56609AA");

            entity.Property(e => e.EntryId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmout).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Supplier).WithMany(p => p.StockEntries)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockEntr__Suppl__17036CC0");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockEntries)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockEntr__Wareh__17F790F9");
        });

        modelBuilder.Entity<StockEntryDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.EntryId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Entry).WithMany()
                .HasForeignKey(d => d.EntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockEntr__Entry__1BC821DD");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockEntr__Produ__1AD3FDA4");
        });

        modelBuilder.Entity<StockExit>(entity =>
        {
            entity.HasKey(e => e.ExitId).HasName("PK__StockExi__26D64EB89F627177");

            entity.Property(e => e.ExitId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContractId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExitDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.TotalAmout).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Contract).WithMany(p => p.StockExits)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__StockExit__Contr__2180FB33");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockExits)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockExit__Wareh__208CD6FA");
        });

        modelBuilder.Entity<StockExitDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ExitId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Exit).WithMany()
                .HasForeignKey(d => d.ExitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockExit__ExitI__25518C17");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockExit__Produ__245D67DE");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B41EAB6449");

            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(150);

            entity.HasOne(d => d.Address).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Suppliers__Addre__68487DD7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0790C433");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E493D675E7").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");
            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Users)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Users__CustomerI__395884C4");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Users__EmployeeI__3864608B");

            entity.HasOne(d => d.Group).WithMany(p => p.Users)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Users__GroupId__3E1D39E1");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__37703C52");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__UserGrou__149AF36A4ADD971B");

            entity.Property(e => e.GroupId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupName).HasMaxLength(100);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9A8D5FFB2");

            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarehouseName).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Addre__60A75C0F");
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.SheduleId).HasName("PK__WorkSche__6052D82EDE09D67B");

            entity.Property(e => e.Note).HasMaxLength(200);

            entity.HasOne(d => d.Assignment).WithMany(p => p.WorkSchedules)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("FK__WorkSched__Assig__30C33EC3");

            entity.HasOne(d => d.Shift).WithMany(p => p.WorkSchedules)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkSched__Shift__2FCF1A8A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
