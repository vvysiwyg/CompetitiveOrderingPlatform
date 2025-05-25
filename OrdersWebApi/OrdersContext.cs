using Microsoft.EntityFrameworkCore;
using OrdersShared.POCOs;

public partial class OrdersContext : DbContext
{
    public OrdersContext()
    {
    }

    public OrdersContext(DbContextOptions<OrdersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Delivery> Deliveries { get; set; }
    public virtual DbSet<Emp> Emps { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetailProduct> OrderDetailProducts { get; set; }
    public virtual DbSet<OrderDetailService> OrderDetailServices { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<Courier> Couriers { get; set; }
    public virtual DbSet<CourierHistory> CourierHistories { get; set; }
    public virtual DbSet<CourierOrder> CourierOrders { get; set; }
    public virtual DbSet<OrderIssuePoint> OrderIssuePoints { get; set; }

    public override void Dispose()
    {
        base.Dispose();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

        modelBuilder.HasSequence<int>("address_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("customer_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("delivery_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("emp_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("order_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("product_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("service_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("courier_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("courier_history_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.HasSequence<int>("order_issue_point_id_seq", schema: "public")
                .StartsAt(1)
                .HasMax(2147483647)
                .IncrementsBy(1);

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("address");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('address_id_seq'::regclass)");

            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("city");

            entity.Property(e => e.StreetName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("street_name");

            entity.Property(e => e.BuildingNum)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("building_num");

            entity.Property(e => e.ApartmentNum)
                .HasMaxLength(5)
                .HasColumnName("apartment_num");

            entity.Property(e => e.EntranceNum)
                .HasMaxLength(2)
                .HasColumnName("entrance_num");

            entity.Property(e => e.IntercomeCode)
                .HasMaxLength(4)
                .HasColumnName("intercome_code");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customer");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('customer_id_seq'::regclass)");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("first_name");

            entity.Property(e => e.MiddleName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("middle_name");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("last_name");

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(11)
                .HasColumnName("phone");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

            entity.Property(e => e.CustomerType)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("customer_type");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("delivery");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('delivery_id_seq'::regclass)");

            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("date")
                .HasColumnName("date");

            entity.Property(e => e.Commentary)
                .HasColumnType("text")
                .HasColumnName("commentary");

            entity.Property(e => e.CallInHour)
                .IsRequired()
                .HasColumnName("call_in_hour");

            entity.Property(e => e.AddressId)
                .HasColumnName("address_id");

            entity.HasOne(e => e.Address)
                .WithMany(a => a.Deliveries)
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("delivery_address_id_fk");
        });
        
        modelBuilder.Entity<Emp>(entity =>
        {
            entity.ToTable("emp");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('emp_id_seq'::regclass)");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("first_name");

            entity.Property(e => e.MiddleName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("middle_name");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("last_name");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

            entity.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("position");

            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");

            entity.Property(e => e.MobilePhone)
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnName("mobile_phone");

            entity.Property(e => e.Auth0Id)
                .HasMaxLength(255)
                .HasColumnName("auth0_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("order");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('order_id_seq'::regclass)");

            entity.Property(e => e.Number)
                .IsRequired()
                .HasMaxLength(12)
                .HasColumnName("number");

            entity.Property(e => e.Sum)
                .IsRequired()
                .HasMaxLength(7)
                .HasColumnName("sum");

            entity.Property(e => e.CreationDate)
                .IsRequired()
                .HasColumnName("creation_date");

            entity.Property(e => e.PaymentType)
                .IsRequired()
                .HasMaxLength(13)
                .HasColumnName("payment_type");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(12)
                .HasDefaultValue("Не назначен")
                .HasColumnName("status");

            entity.Property(e => e.DeliveryId)
                .HasColumnName("delivery_id");

            entity.Property(e => e.CustomerId)
                .HasColumnName("customer_id");

            entity.HasOne(e => e.Delivery)
                .WithMany(d => d.Orders)
                .HasForeignKey(e => e.DeliveryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_delivery_id_fk");

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_customer_id_fk");
        });

        modelBuilder.Entity<OrderDetailProduct>(entity =>
        {
            entity.ToTable("order_detail_product");

            entity
                .HasKey(keys => new { keys.OrderId, keys.ProductId });

            entity.Property(e => e.Qty)
                .HasMaxLength(3)
                .HasColumnName("qty");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id");

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");

            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderDetailProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_detail_product_product_id_fk");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderDetailProducts)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_detail_product_order_id_fk");
        });
        
        modelBuilder.Entity<OrderDetailService>(entity =>
        {
            entity.ToTable("order_detail_service");

            entity.HasKey(keys => new { keys.OrderId, keys.ServiceId });

            entity.Property(e => e.Qty)
                .HasMaxLength(3)
                .HasColumnName("qty");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id");

            entity.Property(e => e.ServiceId)
                .HasColumnName("service_id");

            entity.HasOne(e => e.Service)
                .WithMany(s => s.OrderDetailServices)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_detail_service_service_id_fk");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderDetailServices)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_detail_service_order_id_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('product_id_seq'::regclass)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.Price)
                .HasMaxLength(7)
                .HasColumnName("price");

            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("code");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("service");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('service_id_seq'::regclass)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.Price)
                .HasMaxLength(7)
                .HasColumnName("price");

            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
        }); 

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.ToTable("courier");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('courier_id_seq'::regclass)");

            entity.Property(e => e.NumberOfSuccessOrders)
                .HasColumnName("number_of_success_orders");

            entity.Property(e => e.NumberOfFailedOrders)
                .HasColumnName("number_of_failed_orders");

            entity.Property(e => e.EmpId)
                .HasColumnName("emp_id");

            entity.Property(e => e.Profile)
                .HasMaxLength(30)
                .HasColumnName("profile");

            entity.HasIndex(e => e.EmpId, "unique_index_emp_id_courier")
                .IsUnique();

            entity.HasOne(e => e.Emp)
                .WithMany(a => a.Couriers)
                .HasForeignKey(e => e.EmpId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("courier_emp_id_fk");

            entity.Ignore(e => e.ProfileType);
        });

        modelBuilder.Entity<CourierHistory>(entity =>
        {
            entity.ToTable("courier_history");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('courier_history_id_seq'::regclass)");

            entity.Property(e => e.NumberOfSuccessOrders)
                .HasColumnName("number_of_success_orders");

            entity.Property(e => e.NumberOfFailedOrders)
                .HasColumnName("number_of_failed_orders");

            entity.Property(e => e.Income)
                .HasPrecision(6, 2)
                .HasColumnName("income");

            entity.Property(e => e.MeasurementDate)
                .HasColumnType("date")
                .HasColumnName("measurement_date");

            entity.Property(e => e.CourierId)
                .HasColumnName("courier_id");

            entity.HasOne(e => e.Courier)
                .WithMany(a => a.CourierHistories)
                .HasForeignKey(e => e.CourierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("courier_history_courier_id_fk");
        });

        modelBuilder.Entity<CourierOrder>(entity =>
        {
            entity.ToTable("courier_order");

            entity.HasKey(keys => new { keys.OrderId, keys.CourierId });

            entity.Property(e => e.Cost)
                .HasPrecision(4, 2)
                .HasColumnName("cost");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id");

            entity.Property(e => e.CourierId)
                .HasColumnName("courier_id");

            entity.HasOne(e => e.Courier)
                .WithMany(s => s.CourierOrders)
                .HasForeignKey(e => e.CourierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("courier_order_courier_id_fk");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.CourierOrders)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("courier_order_order_id_fk");
        });

        modelBuilder.Entity<OrderIssuePoint>(entity =>
        {
            entity.ToTable("order_issue_point");

            entity
                .Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("nextval('order_issue_point_id_seq'::regclass)");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.Longitude)
                .HasPrecision(7, 4)
                .HasColumnName("longitude");

            entity.Property(e => e.Latitude)
                .HasPrecision(7, 4)
                .HasColumnName("latitude");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
