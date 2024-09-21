﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectPet.Infrastructure;

#nullable disable

namespace ProjectPet.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectPet.Domain.Models.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("address");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("breed");

                    b.Property<string>("Coat")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("coat");

                    b.Property<DateOnly>("CreatedOn")
                        .HasColumnType("date")
                        .HasColumnName("created_on");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.Property<string>("Health")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("health");

                    b.Property<float>("Height")
                        .HasColumnType("real")
                        .HasColumnName("height");

                    b.Property<bool>("IsSterilized")
                        .HasColumnType("boolean")
                        .HasColumnName("is_sterilized");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("phone_number");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("species");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<float>("Weight")
                        .HasColumnType("real")
                        .HasColumnName("weight");

                    b.Property<Guid?>("pet_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("pet_id")
                        .HasDatabaseName("ix_pets_pet_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("ProjectPet.Domain.Models.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("full_name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("phone_number");

                    b.Property<int>("YOExperience")
                        .HasColumnType("integer")
                        .HasColumnName("yo_experience");

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("ProjectPet.Domain.Models.Pet", b =>
                {
                    b.HasOne("ProjectPet.Domain.Models.Volunteer", null)
                        .WithMany("OwnedPets")
                        .HasForeignKey("pet_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("fk_pets_volunteers_pet_id");

                    b.OwnsOne("ProjectPet.Domain.Models.PaymentMethodsList", "PaymentMethods", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId")
                                .HasName("pk_pets");

                            b1.ToTable("pets");

                            b1.ToJson("payment_methods");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_pet_id");

                            b1.OwnsMany("ProjectPet.Domain.Models.PaymentInfo", "Data", b2 =>
                                {
                                    b2.Property<Guid>("PaymentMethodsListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Instructions")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("character varying(300)");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)");

                                    b2.HasKey("PaymentMethodsListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("payment_methods");

                                    b2.WithOwner()
                                        .HasForeignKey("PaymentMethodsListPetId")
                                        .HasConstraintName("fk_pets_pets_payment_methods_list_pet_id");
                                });

                            b1.Navigation("Data");
                        });

                    b.OwnsOne("ProjectPet.Domain.Models.PhotoList", "Photos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("photos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("ProjectPet.Domain.Models.PetPhoto", "Data", b2 =>
                                {
                                    b2.Property<Guid>("PhotoListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool>("IsPrimary")
                                        .HasColumnType("boolean");

                                    b2.Property<string>("StoragePath")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)");

                                    b2.HasKey("PhotoListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("photos");

                                    b2.WithOwner()
                                        .HasForeignKey("PhotoListPetId")
                                        .HasConstraintName("fk_pets_pets_photo_list_pet_id");
                                });

                            b1.Navigation("Data");
                        });

                    b.Navigation("PaymentMethods");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("ProjectPet.Domain.Models.Volunteer", b =>
                {
                    b.OwnsOne("ProjectPet.Domain.Models.SocialNetworkList", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("social_networks");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("ProjectPet.Domain.Models.SocialNetwork", "Data", b2 =>
                                {
                                    b2.Property<Guid>("SocialNetworkListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Link")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("character varying(300)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)");

                                    b2.HasKey("SocialNetworkListVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("social_networks");

                                    b2.WithOwner()
                                        .HasForeignKey("SocialNetworkListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_social_network_list_volunteer_id");
                                });

                            b1.Navigation("Data");
                        });

                    b.OwnsOne("ProjectPet.Domain.Models.PaymentMethodsList", "PaymentMethods", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("payment_methods");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("ProjectPet.Domain.Models.PaymentInfo", "Data", b2 =>
                                {
                                    b2.Property<Guid>("PaymentMethodsListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Instructions")
                                        .IsRequired()
                                        .HasMaxLength(300)
                                        .HasColumnType("character varying(300)");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)");

                                    b2.HasKey("PaymentMethodsListVolunteerId", "Id");

                                    b2.ToTable("volunteers");

                                    b2.ToJson("payment_methods");

                                    b2.WithOwner()
                                        .HasForeignKey("PaymentMethodsListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_payment_methods_list_volunteer_id");
                                });

                            b1.Navigation("Data");
                        });

                    b.Navigation("PaymentMethods");

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("ProjectPet.Domain.Models.Volunteer", b =>
                {
                    b.Navigation("OwnedPets");
                });
#pragma warning restore 612, 618
        }
    }
}