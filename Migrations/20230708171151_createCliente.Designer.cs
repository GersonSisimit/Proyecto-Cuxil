﻿// <auto-generated />
using System;
using AgroservicioCuxil.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AgroservicioCuxil.Migrations
{
    [DbContext(typeof(AgroservicioContext))]
    [Migration("20230708171151_createCliente")]
    partial class createCliente
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AgroservicioCuxil.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdTipoCliente")
                        .IsUnicode(false)
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("IdCliente")
                        .HasName("PK__Cliente__D5946642B600C305");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("AgroservicioCuxil.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("ID_USUARIO");

                    b.Property<string>("Apellidos")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("APELLIDOS");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("NOMBRE");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("PASS");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("SALT");

                    b.Property<string>("Tipo")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("TIPO")
                        .HasDefaultValueSql("('USUARIO')");

                    b.HasKey("IdUsuario")
                        .HasName("PK__USUARIOS__91136B90D711E405");

                    b.ToTable("USUARIOS");
                });
#pragma warning restore 612, 618
        }
    }
}
