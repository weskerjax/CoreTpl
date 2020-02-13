using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orion.API.Models;

namespace Orion.API.Extensions
{
    public static class TableInfoExtensions
    {

		/// <summary>Table 資訊</summary>
		public static List<TableInfo> GetTableInfo(this DbContext context)
		{
			return context.Set<TableInfo>().ToList();
		}


		/// <summary>設定 Npgsql Table 資訊</summary>
		public static EntityTypeBuilder<TableInfo> NpgsqlTableInfo(this ModelBuilder modelBuilder, DbContext context) 
        {
			return modelBuilder.Entity<TableInfo>()
				.HasNoKey()
				.ToQuery(() => context.Set<TableInfo>().FromSqlRaw(@"
					SELECT
						*,
						(TotalBytes - IndexBytes - ToastBytes) AS TableBytes
					FROM (
						SELECT
							t.table_schema AS Schema, 
							t.table_name AS Name,
							s.n_live_tup AS TotalRows,
							pg_total_relation_size(c.oid) AS TotalBytes,
							pg_indexes_size(c.oid) AS IndexBytes,
							COALESCE(pg_total_relation_size(c.reltoastrelid),0) AS ToastBytes
						FROM information_schema.tables t
						INNER JOIN pg_class c ON t.table_name = c.relname
						INNER JOIN pg_stat_user_tables s ON t.table_name = s.relname
						WHERE t.table_schema = 'public'
					) a
					ORDER BY Name ASC
				"));
		}


	}
}
