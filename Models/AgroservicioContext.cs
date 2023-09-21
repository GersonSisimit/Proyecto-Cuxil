using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AgroservicioCuxil.Models
{
    public partial class AgroservicioContext : DbContext
    {
        public AgroservicioContext()
        {
        }

        public AgroservicioContext(DbContextOptions<AgroservicioContext> options)
            : base(options)
        {
        }
        public DbSet<DetallePresentacionProducto> DetallePresentacionProductos { get; set; }

        public List<DetallePresentacionProducto> ObtenerTodosLosDetallesPresentacionProducto()
        {
            return DetallePresentacionProductos.ToList();
        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Marca> Marca { get; set; }
        public virtual DbSet<TipoProducto> TipoProducto { get; set; }
        public virtual DbSet<SubtipoProducto> SubtipoProducto { get; set; }
        public virtual DbSet<DetalleProducto> DetalleProducto { get; set; }
        public virtual DbSet<PresentacionProducto> PresentacionProducto { get; set; }
        public virtual DbSet<DetallePresentacionProducto> DetallePresentacionProducto { get; set; }


        //linea de codiufo
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //                optionsBuilder.UseSqlServer("server= localhost; database= Agroservicio; integrated security = true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PK__Cliente__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
                entity.Property(e => e.IdTipoCliente).IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Producto__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
                entity.Property(e => e.IdMarca).IsUnicode(false);
                entity.Property(e => e.IdDetalleProducto).IsUnicode(false);
                entity.Property(e => e.RutaImagen).IsUnicode(false);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Marca__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__TipoProducto__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<SubtipoProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__SubtipoProducto__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<DetalleProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__RelacionTipoSubtipoProducto__D5946642B600C305");
                entity.Property(e => e.IdTipoProducto).IsUnicode(false);
                entity.Property(e => e.IdSubtipoProducto).IsUnicode(false);
            });

            modelBuilder.Entity<PresentacionProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__PresentacionProducto__D5946642B600C305");
                entity.Property(e => e.IdProducto).IsUnicode(false);
                entity.Property(e => e.IdDetallePresentacion).IsUnicode(false);
                entity.Property(e => e.Existencia).IsUnicode(false);
                entity.Property(e => e.Precio).IsUnicode(false);
                entity.Property(e => e.RutaImagen).IsUnicode(false);
            });

            modelBuilder.Entity<DetallePresentacionProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__DetallePresentacionProducto__D5946642B600C305");
                entity.Property(e => e.Nombre).IsUnicode(false);
                entity.Property(e => e.Tipo).IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__USUARIOS__91136B90D711E405");

                entity.ToTable("USUARIOS");

                entity.Property(e => e.IdUsuario)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_USUARIO");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .HasColumnName("APELLIDOS");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("PASS");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SALT");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(15)
                    .HasColumnName("TIPO")
                    .HasDefaultValueSql("('USUARIO')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
