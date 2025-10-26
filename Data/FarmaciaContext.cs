using System;
using System.Collections.Generic;
using Farmacia.UI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.UI.Data;

public partial class FarmaciaContext : DbContext
{
    public FarmaciaContext()
    {
    }

    public FarmaciaContext(DbContextOptions<FarmaciaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AltaArchivo> AltaArchivos { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetSystem> AspNetSystems { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<EstatusInventario> EstatusInventarios { get; set; }

    public virtual DbSet<GetAltasPendiente> GetAltasPendientes { get; set; }

    public virtual DbSet<ImportAlta> ImportAltas { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<InventarioDetalle> InventarioDetalles { get; set; }

    public virtual DbSet<Lote> Lotes { get; set; }

    public virtual DbSet<RequerimientoCatalogoIi> RequerimientoCatalogoIis { get; set; }

    public virtual DbSet<RequerimientoCatalogoIiarchivo> RequerimientoCatalogoIiarchivos { get; set; }

    public virtual DbSet<RequerimientoCatalogoIiestatus> RequerimientoCatalogoIiestatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=11.57.20.130;Initial Catalog=Farmacia;Persist Security Info=True;User ID=sa;Password=Administrador2;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AltaArchivo>(entity =>
        {
            entity.ToTable("AltaArchivo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AltaContable).HasMaxLength(50);
            entity.Property(e => e.Archivo).HasMaxLength(500);
            entity.Property(e => e.RutaArchivo).HasMaxLength(4000);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetSystem>(entity =>
        {
            entity.ToTable("AspNetSystem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(500);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.ToTable("Configuracion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Codigo).HasMaxLength(500);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Modulo).HasMaxLength(50);
            entity.Property(e => e.ValorDate).HasColumnType("datetime");
            entity.Property(e => e.ValorDecimal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ValorString).HasMaxLength(500);
        });

        modelBuilder.Entity<EstatusInventario>(entity =>
        {
            entity.ToTable("EstatusInventario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<GetAltasPendiente>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Cantidad).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.CantidadAutorizada).HasMaxLength(50);
            entity.Property(e => e.DescripcionArticulo).HasMaxLength(2000);
            entity.Property(e => e.Dif).HasMaxLength(2000);
            entity.Property(e => e.Esp).HasMaxLength(2000);
            entity.Property(e => e.FechaHoraAlta).HasMaxLength(2000);
            entity.Property(e => e.Gen).HasMaxLength(2000);
            entity.Property(e => e.Gpo).HasMaxLength(2000);
            entity.Property(e => e.NumeroAltaContable).HasMaxLength(50);
            entity.Property(e => e.NumeroProveedor).HasMaxLength(2000);
            entity.Property(e => e.RazonSocial).HasMaxLength(2000);
            entity.Property(e => e.Recepcion).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.RfcProveedor).HasMaxLength(2000);
            entity.Property(e => e.Var).HasMaxLength(2000);
        });

        modelBuilder.Entity<ImportAlta>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CantidadAutorizada).HasMaxLength(2000);
            entity.Property(e => e.CantidadConteo).HasMaxLength(2000);
            entity.Property(e => e.CantidadPresentacion).HasMaxLength(2000);
            entity.Property(e => e.Cargoa).HasMaxLength(2000);
            entity.Property(e => e.ClasPtalOrigen).HasMaxLength(2000);
            entity.Property(e => e.ClasPtalUnidadEntrega).HasMaxLength(2000);
            entity.Property(e => e.Creditoa).HasMaxLength(2000);
            entity.Property(e => e.DescripcionArticulo).HasMaxLength(2000);
            entity.Property(e => e.DescripcionMovimiento).HasMaxLength(2000);
            entity.Property(e => e.Dif).HasMaxLength(2000);
            entity.Property(e => e.Enviado).HasMaxLength(2000);
            entity.Property(e => e.Esp).HasMaxLength(2000);
            entity.Property(e => e.FechaAlta).HasColumnType("datetime");
            entity.Property(e => e.FechaEnvioPrei).HasMaxLength(2000);
            entity.Property(e => e.FechaHoraAlta).HasMaxLength(2000);
            entity.Property(e => e.FechaHoraEntrega).HasMaxLength(2000);
            entity.Property(e => e.FechaHoraRecepcion).HasMaxLength(2000);
            entity.Property(e => e.FechaRegistro).HasMaxLength(2000);
            entity.Property(e => e.Gen).HasMaxLength(2000);
            entity.Property(e => e.Gpo).HasMaxLength(2000);
            entity.Property(e => e.ImporteAltaConIva).HasMaxLength(2000);
            entity.Property(e => e.ImporteArticuloSinIva).HasMaxLength(2000);
            entity.Property(e => e.Iva).HasMaxLength(2000);
            entity.Property(e => e.LineaArticulo).HasMaxLength(2000);
            entity.Property(e => e.NombreOoad).HasMaxLength(2000);
            entity.Property(e => e.NombreUnidadEntrega).HasMaxLength(2000);
            entity.Property(e => e.NumeroAltaContable).HasMaxLength(50);
            entity.Property(e => e.NumeroDeDocumento).HasMaxLength(2000);
            entity.Property(e => e.NumeroDeReposicion).HasMaxLength(2000);
            entity.Property(e => e.NumeroLicitacion).HasMaxLength(2000);
            entity.Property(e => e.NumeroProveedor).HasMaxLength(2000);
            entity.Property(e => e.PartidaPresupuestal).HasMaxLength(2000);
            entity.Property(e => e.PrecioCatalogoArticulos).HasMaxLength(2000);
            entity.Property(e => e.PrecioCompra).HasMaxLength(2000);
            entity.Property(e => e.RazonSocial).HasMaxLength(2000);
            entity.Property(e => e.RfcProveedor).HasMaxLength(2000);
            entity.Property(e => e.TipoError).HasMaxLength(2000);
            entity.Property(e => e.TipoPresentacion).HasMaxLength(2000);
            entity.Property(e => e.TipoReporte).HasMaxLength(2000);
            entity.Property(e => e.UnidadPresentacion).HasMaxLength(2000);
            entity.Property(e => e.Var).HasMaxLength(2000);
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.ToTable("Inventario");

            entity.Property(e => e.InventarioId).ValueGeneratedNever();
            entity.Property(e => e.ControlDeCaducidades).HasMaxLength(500);
            entity.Property(e => e.CveAdsc).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaTermino).HasColumnType("datetime");
            entity.Property(e => e.Generado).HasMaxLength(400);
            entity.Property(e => e.GeneradoPuesto).HasMaxLength(500);
            entity.Property(e => e.Responsable).HasMaxLength(500);
            entity.Property(e => e.ResponsableConteo).HasMaxLength(500);
            entity.Property(e => e.ResponsableControPuesto).HasMaxLength(500);
            entity.Property(e => e.ResponsablePuesto).HasMaxLength(500);

            entity.HasOne(d => d.EstatusInventarioNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.EstatusInventario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_EstatusInventario");
        });

        modelBuilder.Entity<InventarioDetalle>(entity =>
        {
            entity.HasKey(e => new { e.InventarioId, e.LoteId }).HasName("PK_LoteDetalle");

            entity.ToTable("InventarioDetalle");

            entity.Property(e => e.ClaveProveedor).HasMaxLength(50);
            entity.Property(e => e.Consumo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Conteo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.NombreProveedor).HasMaxLength(200);
            entity.Property(e => e.Teorico).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Unidad).HasMaxLength(200);

            entity.HasOne(d => d.Inventario).WithMany(p => p.InventarioDetalles)
                .HasForeignKey(d => d.InventarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoteDetalle_Inventario");

            entity.HasOne(d => d.Lote).WithMany(p => p.InventarioDetalles)
                .HasForeignKey(d => d.LoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoteDetalle_Lote");
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.ToTable("Lote");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Caducidad).HasColumnType("datetime");
            entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Dif).HasMaxLength(10);
            entity.Property(e => e.Disponible).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Esp).HasMaxLength(10);
            entity.Property(e => e.Gen).HasMaxLength(10);
            entity.Property(e => e.Gpo).HasMaxLength(10);
            entity.Property(e => e.Lote1)
                .HasMaxLength(50)
                .HasColumnName("Lote");
            entity.Property(e => e.NumeroAltaContable).HasMaxLength(50);
            entity.Property(e => e.Var).HasMaxLength(10);
        });

        modelBuilder.Entity<RequerimientoCatalogoIi>(entity =>
        {
            entity.ToTable("RequerimientoCatalogoII");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Agregado).HasMaxLength(50);
            entity.Property(e => e.ClaveMedicamento).HasMaxLength(50);
            entity.Property(e => e.Diagnostico).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEvaluacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");
            entity.Property(e => e.Folio).HasMaxLength(50);
            entity.Property(e => e.NombrePaciente).HasMaxLength(50);
            entity.Property(e => e.Nss)
                .HasMaxLength(50)
                .HasColumnName("NSS");
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.Ooad)
                .HasMaxLength(50)
                .HasColumnName("OOAD");
            entity.Property(e => e.RequerimientoMensual).HasMaxLength(500);

            entity.HasOne(d => d.Estatus).WithMany(p => p.RequerimientoCatalogoIis)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequerimientoCatalogoII_RequerimientoCatalogoIIEstatus");
        });

        modelBuilder.Entity<RequerimientoCatalogoIiarchivo>(entity =>
        {
            entity.ToTable("RequerimientoCatalogoIIArchivo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Archivo).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Ruta).HasMaxLength(500);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Requerimiento).WithMany(p => p.RequerimientoCatalogoIiarchivos)
                .HasForeignKey(d => d.RequerimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequerimientoCatalogoIIArchivo_RequerimientoCatalogoII");
        });

        modelBuilder.Entity<RequerimientoCatalogoIiestatus>(entity =>
        {
            entity.ToTable("RequerimientoCatalogoIIEstatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
