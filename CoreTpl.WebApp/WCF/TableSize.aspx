<%@ Page Language="C#" EnableSessionState="False" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Configuration" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Table Size Info</title>
	<style>
		table {
			border-collapse: collapse;
			border-spacing: 0;
		}
		th, td {
			border: 1px solid #ddd;
			padding: 5px 10px;
			font-size: 14px;
		}
		tr:nth-of-type(2n+1) {
			background-color: #f9f9f9;
		}
		td:nth-of-type(n+2) {
			text-align:right;
		}
	</style>
</head>
<body>
	<h2>Table Size Info</h2>
	<table>
	<%
		string cntString = ConfigurationManager.ConnectionStrings["STK"].ConnectionString;
		var table = new DataTable();

		using (var conn = new SqlConnection(cntString))
		{
			new SqlDataAdapter(@"
				CREATE TABLE #T (
					[Name] VARCHAR(200),
					[Rows] VARCHAR(20),
					[Total] VARCHAR(20),
					[Data] VARCHAR(20),
					[Index] VARCHAR(20),
					[Unused] VARCHAR(20)
				);
				EXEC sp_MSforeachtable 'INSERT INTO #T EXEC sp_spaceused [?]';
				SELECT * FROM #T
				UNION ALL
				SELECT 
					'總計' AS [Name],
					CONCAT(SUM(CAST([Rows] AS INT)),'') AS [Rows],
					CONCAT(SUM(CAST(REPLACE([Total],'KB','') AS INT)),' KB') AS [Total],
					CONCAT(SUM(CAST(REPLACE([Data],'KB','') AS INT)),' KB') AS [Data],
					CONCAT(SUM(CAST(REPLACE([Index],'KB','') AS INT)),' KB') AS [Index],
					CONCAT(SUM(CAST(REPLACE([Unused],'KB','') AS INT)),' KB') AS [Unused]
				FROM #T
				ORDER BY 1
			", conn).Fill(table);
		}

		
		Func<string, string> comma = size => Regex.Replace(size, @"(?<=\d)\d{3}", ",$0", RegexOptions.RightToLeft);

		Action<Func<DataColumn, string>> writeRow = getCell =>
		{
			Response.Write("<tr>");
			foreach (DataColumn item in table.Columns) { Response.Write(getCell(item)); }
			Response.Write("</tr>\n");
		};

		writeRow(col => ("<th>" + col.ColumnName + "</th>")); 
		foreach (DataRow row in table.Rows) { writeRow(col => comma("<td>" + row[col] + "</td>")); } 
	%>
	</table>
</body>
</html>


