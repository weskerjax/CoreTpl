using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using CoreTpl.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Orion.API.Extensions;
using Orion.API.Models;

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


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			
			addIndexFromAttribute(modelBuilder);

			modelBuilder.Entity<RoleInfo>()
				.Property(t => t.RoleId)
				.HasIdentityOptions(startValue: 2);

			modelBuilder.Entity<UserInfo>()
				.Property(t => t.UserId)
				.HasIdentityOptions(startValue: 12);


			/* UserRole */
			modelBuilder.Entity<UserRole>()
				.HasKey(t => new { t.UserId, t.RoleId });

			modelBuilder.Entity<UserRole>()
				.HasOne(p => p.RoleInfo)
				.WithMany(b => b.UserRole)
				.HasForeignKey(p => p.RoleId);

			modelBuilder.Entity<UserRole>()
				.HasOne(p => p.UserInfo)
				.WithMany(b => b.UserRole)
				.HasForeignKey(p => p.UserId);

			modelBuilder.Entity<UserPreference>()
				.HasKey(t => new { t.UserId, t.Name });

			/* 配置 Npgsql 的 TableInfo */
			modelBuilder.TableInfoUseNpgsql(this);


			/* 設定初始資料 */
			seedData(modelBuilder);
		}




		private void addIndexFromAttribute(ModelBuilder modelBuilder)
		{
			foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
			{
				foreach (IMutableProperty prop in entity.GetProperties())
				{
					var attr = prop.PropertyInfo.GetCustomAttribute<IndexAttribute>();
					if (attr == null) { continue; }

					IMutableIndex index = entity.FindIndex(prop);
					if (index == null) { index = entity.AddIndex(prop); }
					
					index.IsUnique = attr.IsUnique;
				}
			}
		}


		/// <summary>設定初始資料</summary>
		private void seedData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RoleInfo>().HasData(
				new RoleInfo
				{
					RoleId = 1,
					RoleName = "管理者",
					AllowActList = string.Join(',', ACT.RoleSetting, ACT.UserSetting),
					UseStatus = UseStatus.Enable.ToString(),
					CreateBy = 1,
					CreateDate = DateTimeOffset.Now,
					ModifyBy = 1,
					ModifyDate = DateTimeOffset.Now,
				}
			);


			modelBuilder.Entity<UserInfo>().HasData(
				new UserInfo
				{
					UserId = 1,
					Account = "system",
					UserName = "系統",
					UserType = UserType.System.ToString(),
					UseStatus = UseStatus.Disable.ToString(),
					CreateBy = 1,
					CreateDate = DateTimeOffset.Now,
					ModifyBy = 1,
					ModifyDate = DateTimeOffset.Now,
				},
				new UserInfo
				{
					UserId = 11,
					Account = "admin",
					UserName = "Admin",
					UserType = UserType.Manager.ToString(),
					UseStatus = UseStatus.Enable.ToString(),
					Password = "YP50QG5/NT7ZefNQ8vu2ouhpCl+n0bDDKYPR2LP5X2c=", /* Admin1234	*/
					CreateBy = 1,
					CreateDate = DateTimeOffset.Now,
					ModifyBy = 1,
					ModifyDate = DateTimeOffset.Now,
				}
			);


			modelBuilder.Entity<UserRole>().HasData(
				new UserRole
				{
					UserId = 11,
					RoleId = 1,
					CreateBy = 1,
					CreateDate = DateTimeOffset.Now,
					ModifyBy = 1,
					ModifyDate = DateTimeOffset.Now,
				}
			);
		}



	}




	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IndexAttribute : Attribute
	{
		public bool IsUnique { get; set; }
	}



	/*===============================================================*/





    /// <summary>角色</summary>
	[Table(nameof(RoleInfo)), Description("角色")]
	public class RoleInfo
	{

        /// <summary>角色Id</summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Description("角色Id")]
		public int RoleId { get; set; }

        /// <summary>角色名稱</summary>
		[MaxLength(256), Description("角色名稱")]
		public string RoleName { get; set; }

        /// <summary>允許權限</summary>
		[Description("允許權限")]
		public string AllowActList { get; set; }

        /// <summary>備註</summary>
		[MaxLength(1024), Description("備註")]
		public string RemarkText { get; set; }

        /// <summary>使用狀態</summary>
		[MaxLength(32), Description("使用狀態")]
		public string UseStatus { get; set; }

        /// <summary>建立人</summary>
		[Description("建立人")]
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		[Description("建立日期")]
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		[Description("修改人")]
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		[Description("修改日期")]
		public DateTimeOffset ModifyDate { get; set; }

		/// <summary>使用者角色</summary>
		[Description("使用者角色")]
		public virtual List<UserRole> UserRole { get; set; } = new List<UserRole>();

	}



    /// <summary>使用者</summary>
	[Table(nameof(UserInfo)), Description("使用者")]
	public class UserInfo
	{

        /// <summary>使用者Id</summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Description("使用者Id")]
		public int UserId { get; set; }

        /// <summary>帳號</summary>
		[Index, MaxLength(256), Required, Description("帳號")]
		public string Account { get; set; }

		/// <summary>姓名</summary>
		[Index, MaxLength(256), Required, Description("姓名")]
		public string UserName { get; set; }
		
		/// <summary>使用者類型</summary>
		[MaxLength(32), Required, Description("使用者類型")]
		public string UserType { get; set; }
		

		/// <summary>E-Mail</summary>
		[MaxLength(256), Description("E-Mail")]
		public string Email { get; set; }

        /// <summary>密碼</summary>
		[MaxLength(256), Description("密碼")]
		public string Password { get; set; }

		/// <summary>允許權限</summary>
		[MaxLength(1024), Description("允許權限")]
		public string AllowActList { get; set; }

        /// <summary>拒絕權限</summary>
		[MaxLength(1024), Description("拒絕權限")]
		public string DenyActList { get; set; }

		/// <summary>使用狀態</summary>
		[MaxLength(32), Required, Description("使用狀態")]
		public string UseStatus { get; set; }

        /// <summary>部門</summary>
		[MaxLength(64), Description("部門")]
		public string Department { get; set; }

        /// <summary>分機號碼</summary>
		[MaxLength(16), Description("分機號碼")]
		public string ExtensionNum { get; set; }

        /// <summary>職稱</summary>
		[MaxLength(256), Description("職稱")]
		public string UserTitle { get; set; }

        /// <summary>備註</summary>
		[MaxLength(2048), Description("備註")]
		public string RemarkText { get; set; }

        /// <summary>建立人</summary>
		[Description("建立人")]
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		[Description("建立日期")]
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		[Description("修改人")]
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		[Description("修改日期")]
		public DateTimeOffset ModifyDate { get; set; }

		/// <summary>使用者角色</summary>
		[Description("使用者角色")]
		public virtual List<UserRole> UserRole { get; set; } = new List<UserRole>();

	}



	/// <summary>使用者角色</summary>
	[Table(nameof(UserRole)), Description("使用者角色")]
	public class UserRole
	{
        /// <summary>使用者Id</summary>
		[Key, Index, Description("使用者Id")]
		public int UserId { get; set; }

        /// <summary>角色Id</summary>
		[Key, Index, Description("角色Id")]
		public int RoleId { get; set; }

        /// <summary>建立人</summary>
		[Description("建立人")]
		public int CreateBy { get; set; }

        /// <summary>建立日期</summary>
		[Description("建立日期")]
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改人</summary>
		[Description("修改人")]
		public int ModifyBy { get; set; }

        /// <summary>修改日期</summary>
		[Description("修改日期")]
		public DateTimeOffset ModifyDate { get; set; }

        /// <summary>使用者</summary>
		[Description("使用者")]
		public virtual UserInfo UserInfo { get; set; }

		/// <summary>角色</summary>
		[Description("角色")]
		public virtual RoleInfo RoleInfo { get; set; }

	}



	/// <summary>使用者喜好設定</summary>
	[Table(nameof(UserPreference)), Description("使用者喜好設定")]
	public class UserPreference
	{
        /// <summary>使用者Id</summary>
		[Key, Index, Description("使用者Id")]
		public int UserId { get; set; }

		/// <summary>設定名稱</summary>
		[Key, Index, MaxLength(256), Description("設定名稱")]
		public string Name { get; set; }

		/// <summary>設定值</summary>
		[Description("設定值")]
		public string Value { get; set; }

        /// <summary>建立日期</summary>
		[Description("建立日期")]
		public DateTimeOffset CreateDate { get; set; }

        /// <summary>修改日期</summary>
		[Description("修改日期")]
		public DateTimeOffset ModifyDate { get; set; }

	}



	/// <summary>使用者登入記錄</summary>
	[Table(nameof(UserSignInRecord)), Description("使用者登入記錄")]
	public class UserSignInRecord
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Description("Id")]
		public int Id { get; set; }

		/// <summary>帳號</summary>
		[MaxLength(256), Description("帳號")]
		public string Account { get; set; }

		/// <summary>登入 IP</summary>
		[Index, MaxLength(48), Description("登入 IP")]
		public string SignInIp { get; set; }

		/// <summary>登入類型</summary>
		[MaxLength(32), Description("登入類型")]
		public string SignInType { get; set; }

        /// <summary>狀態代碼</summary>
		[Index, MaxLength(32), Description("狀態代碼")]
		public string StatusCode { get; set; }

		/// <summary>狀態訊息</summary>
		[MaxLength(256), Description("狀態訊息")]
		public string StatusMsg { get; set; }

        /// <summary>建立日期</summary>
		[Description("建立日期")]
		public DateTimeOffset CreateDate { get; set; }

	}
	 



}
