﻿// <auto-generated />
using System;
using Darjeeling.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Darjeeling.Migrations
{
    [DbContext(typeof(Repositories.DataContext))]
    [Migration("20250128065049_store_freecompany_lodestone_id")]
    partial class store_freecompany_lodestone_id
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Darjeeling.Models.Entities.DiscordNameHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscordNickName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DiscordUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FcGuildMemberId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FcGuildMemberId");

                    b.ToTable("DiscordNameHistories");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscordUserUId")
                        .HasColumnType("text");

                    b.Property<int>("FCGuildServerId")
                        .HasColumnType("integer");

                    b.Property<string>("LodestoneId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FCGuildServerId");

                    b.ToTable("FCMembers");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscordGuildUid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FCGuildServerId")
                        .HasColumnType("integer");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FCGuildServerId")
                        .IsUnique();

                    b.ToTable("FCGuildRoles");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminChannelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AdminRoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscordGuildName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DiscordGuildUid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FreeCompanyId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FreeCompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FCGuildServers");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.LodestoneNameHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FCMemberId")
                        .HasColumnType("integer");

                    b.Property<int>("FcGuildMemberId")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FcGuildMemberId");

                    b.ToTable("LodestoneNameHistories");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.DiscordNameHistory", b =>
                {
                    b.HasOne("Darjeeling.Models.Entities.FCGuildMember", "FcGuildMember")
                        .WithMany("DiscordNameHistories")
                        .HasForeignKey("FcGuildMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FcGuildMember");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildMember", b =>
                {
                    b.HasOne("Darjeeling.Models.Entities.FCGuildServer", "FCGuildServer")
                        .WithMany("FCMembers")
                        .HasForeignKey("FCGuildServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FCGuildServer");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildRole", b =>
                {
                    b.HasOne("Darjeeling.Models.Entities.FCGuildServer", "FCGuildServer")
                        .WithOne("FCAdminRole")
                        .HasForeignKey("Darjeeling.Models.Entities.FCGuildRole", "FCGuildServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FCGuildServer");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.LodestoneNameHistory", b =>
                {
                    b.HasOne("Darjeeling.Models.Entities.FCGuildMember", "FcGuildMember")
                        .WithMany("LodestoneNameHistories")
                        .HasForeignKey("FcGuildMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FcGuildMember");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildMember", b =>
                {
                    b.Navigation("DiscordNameHistories");

                    b.Navigation("LodestoneNameHistories");
                });

            modelBuilder.Entity("Darjeeling.Models.Entities.FCGuildServer", b =>
                {
                    b.Navigation("FCAdminRole")
                        .IsRequired();

                    b.Navigation("FCMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
