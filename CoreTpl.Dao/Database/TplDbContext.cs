﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreTpl.Dao.Database
{

	public class TplDbContext : DbContext
	{
		public TplDbContext(DbContextOptions<TplDbContext> options) : base(options)
		{

		}

		/// <summary>角色</summary>
		public DbSet<RoleInfo> RoleInfo { get; set; }
		
		/// <summary>使用者</summary>
		public DbSet<UserInfo> UserInfo { get; set; }
		
		/// <summary>使用者角色</summary>
		public DbSet<UserRole> UserRole { get; set; }
		
		/// <summary>使用者喜好設定</summary>
		public DbSet<UserPreference> UserPreference { get; set; }
		
		/// <summary>使用者登入記錄</summary>
		public DbSet<UserSignInRecord> UserSignInRecord { get; set; }



		/*-----------------------------------------------------*/

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseNpgsql("Host=localhost;Database=CoreTpl;Username=postgres;Password=p@ssw0rd");
			}
		}



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//base.OnModelCreating(modelBuilder);
			

			modelBuilder.Entity<UserRole>().HasKey(t => new { t.UserId, t.RoleId });
			modelBuilder.Entity<UserPreference>().HasKey(t => new { t.UserId, t.Name });

			modelBuilder.Entity<UserRole>()
				.HasOne(p => p.RoleInfo)
				.WithMany(b => b.UserRole)
				.HasForeignKey(p => p.RoleId);

			modelBuilder.Entity<UserRole>()
				.HasOne(p => p.UserInfo)
				.WithMany(b => b.UserRole)
				.HasForeignKey(p => p.UserId);


			foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
			{
				foreach (IMutableProperty prop in entity.GetProperties())
				{
					var attr = prop.PropertyInfo.GetCustomAttribute<IndexAttribute>();
					if (attr == null) { continue; }

					IMutableIndex index = entity.AddIndex(prop);
					index.IsUnique = attr.IsUnique;
				}
			}
		}



	}




	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexAttribute : Attribute
	{
		public bool IsUnique { get; set; }
	}



	/*===============================================================*/





    /// <summary>角色</summary>
	[Table(nameof(RoleInfo))]
	public class RoleInfo
	{

        /// <summary>角色Id</summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoleId { get; set; }

        /// <summary>角色名稱</summary>
		public string RoleName { get; set; }

        /// <summary>允許權限</summary>
		public string AllowActList { get; set; }

        /// <summary>備註</summary>
		public string RemarkText { get; set; }

        /// <summary>使用狀態</summary>
		public string UseStatus { get; set; }

        /// <summary>建立人</summary>
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		public DateTimeOffset ModifyDate { get; set; }

		/// <summary>使用者角色</summary>
		public virtual List<UserRole> UserRole { get; set; } = new List<UserRole>();

	}



    /// <summary>使用者</summary>
	[Table(nameof(UserInfo))]
	public class UserInfo 
	{

        /// <summary>使用者Id</summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

        /// <summary>帳號</summary>
		[Index, MaxLength(256), Required]
		public string Account { get; set; }

		/// <summary>姓名</summary>
		[Index, MaxLength(256), Required]
		public string UserName { get; set; }

        /// <summary>E-Mail</summary>
		[MaxLength(256)]
		public string Email { get; set; }

        /// <summary>密碼</summary>
		[MaxLength(256)]
		public string Password { get; set; }

		/// <summary>允許權限</summary>
		[MaxLength(1024)]
		public string AllowActList { get; set; }

        /// <summary>拒絕權限</summary>
		[MaxLength(1024)]
		public string DenyActList { get; set; }

		/// <summary>使用狀態</summary>
		[MaxLength(32), Required]
		public string UseStatus { get; set; }

        /// <summary>部門</summary>
		[MaxLength(64)]
		public string Department { get; set; }

        /// <summary>分機號碼</summary>
		[MaxLength(16)]
		public string ExtensionNum { get; set; }

        /// <summary>職稱</summary>
		[MaxLength(256)]
		public string UserTitle { get; set; }

        /// <summary>備註</summary>
		[MaxLength(2048)]
		public string RemarkText { get; set; }

        /// <summary>建立人</summary>
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		public DateTimeOffset ModifyDate { get; set; }

		/// <summary>使用者角色</summary>
		public virtual List<UserRole> UserRole { get; set; } = new List<UserRole>();

	}



	/// <summary>使用者角色</summary>
	[Table(nameof(UserRole))]
	public class UserRole
	{

        /// <summary>使用者Id</summary>
		[Key, Index]
		public int UserId { get; set; }

        /// <summary>角色Id</summary>
		[Key, Index]
		public int RoleId { get; set; }

        /// <summary>建立人</summary>
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		public DateTimeOffset ModifyDate { get; set; }

        /// <summary>使用者</summary>
		public virtual UserInfo UserInfo { get; set; }

		/// <summary>角色</summary>
		public virtual RoleInfo RoleInfo { get; set; }

	}



	/// <summary>使用者喜好設定</summary>
	[Table(nameof(UserPreference))]
	public class UserPreference 
	{
        /// <summary>使用者Id</summary>
		[Key, Index]
		public int UserId { get; set; }

		/// <summary>設定名稱</summary>
		[Key, Index, MaxLength(256)]
		public string Name { get; set; }

		/// <summary>設定值</summary>
		public string Value { get; set; }

        /// <summary>建立日期</summary>
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改日期</summary>
		public DateTimeOffset ModifyDate { get; set; }

	}



	/// <summary>使用者登入記錄</summary>
	[Table(nameof(UserSignInRecord))]
	public class UserSignInRecord 
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        /// <summary>帳號</summary>
		public string Account { get; set; }

        /// <summary>登入 IP</summary>
		[Index]
		public string SignInIp { get; set; }

		/// <summary>登入類型</summary>
		public string SignInType { get; set; }

        /// <summary>狀態代碼</summary>
		[Index]
		public string StatusCode { get; set; }

		/// <summary>狀態訊息</summary>
		public string StatusMsg { get; set; }

        /// <summary>建立日期</summary>
		public DateTimeOffset CreateDate { get; set; }

	}


}
