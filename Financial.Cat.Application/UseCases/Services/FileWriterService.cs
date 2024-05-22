using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Interfaces.Services;
using Financial.Cat.Domain.Models.Business.Reports;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.UseCases.Services
{
	public class FileWriterService : IFileWriterService
	{
		public FileModel WriteFile<TRowModel>(IReadOnlyList<TRowModel> rows, FileFormat format, IList<string>? customHeaders = null, string? fileName = "File")
		{
			if (customHeaders != null)
			{
				var props = typeof(TRowModel).GetProperties().Where(x => x.CanRead).ToArray();
				if (props.Length != customHeaders.Count && !typeof(IEnumerable<string>).IsAssignableFrom(typeof(TRowModel)))
					throw new InvalidOperationException($"customHeaders length {customHeaders.Count} does not match readable properties {props.Length}");
			}

			var ret = new MemoryStream();

			var resultModel = new FileModel { Name = fileName, Content = ret };

			switch (format)
			{
				case FileFormat.Csv:
					WriteCsv(ret, rows, customHeaders);
					resultModel.ContentType = "text/csv";
					break;

				case FileFormat.Xlsx:
					WriteXlsx(ret, rows, customHeaders);
					resultModel.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
					break;

				default:
					throw new NotSupportedException($"{format} is not supported.");
			}

			ret.Position = 0;


			return resultModel;
		}


		private static void WriteCsv<TRowModel>(Stream ret, IReadOnlyList<TRowModel> rows, IList<string>? customHeaders = null)
		{
			/*using StreamWriter sw = new(ret, new UTF8Encoding(true), leaveOpen: true);
			using var cw = new CsvWriter(sw, CultureInfo.InvariantCulture, leaveOpen: true);

			if (customHeaders != null)
				customHeaders.ForEach(x => cw.WriteField(x));
			else
				cw.WriteHeader<TRowModel>();

			cw.NextRecord();

			if (typeof(IEnumerable<string>).IsAssignableFrom(typeof(TRowModel)))
			{
				foreach (var i in rows)
				{
					foreach (var j in (IEnumerable<string>)i!)
					{
						cw.WriteField(j);
					}
					cw.NextRecord();
				}
				return;
			}

			foreach (var i in rows)
			{
				cw.WriteRecord(i);
				cw.NextRecord();
			}*/
		}

		private static void WriteXlsx<TRowModel>(Stream ret, IReadOnlyList<TRowModel> rows, IList<string>? customHeaders = null)
		{
			var props = typeof(TRowModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var workbook = new XSSFWorkbook();
			var sheet = workbook.CreateSheet();
			var colsLength = new List<int>();

			{
				var headerRow = sheet.CreateRow(0);

				IEnumerable<(string Header, int CellIndex)> headers;
				if (customHeaders != null)
					headers = customHeaders.Select((header, index) => (header, index));
				else
					headers = props.Select((p, index) => (p.Name, index));

				foreach (var x in headers)
				{
					headerRow.CreateCell(x.CellIndex).SetCellValue(x.Header);
					FillCols(colsLength, x.CellIndex, x.Header.Length);
				}
			}

			if (typeof(IEnumerable<string>).IsAssignableFrom(typeof(TRowModel)))
			{
				for (var i = 1; i <= rows.Count; i++)
				{
					var row = sheet.CreateRow(i);
					var obj = rows[i - 1]!;
					var values = ((IEnumerable<string>)obj).Select((x, index) => (Value: x, Index: index));
					foreach (var x in values)
					{
						row.CreateCell(x.Index).SetCellValue(x.Value);
						FillCols(colsLength, x.Index, x.Value.Length);
					}
				}
			}
			else
			{
				var propsWithGetter = props.Select<PropertyInfo, (PropertyInfo Property, Func<TRowModel, object> GetFunc)>(x => (x, (y) => x.GetValue(y)!));
				for (var i = 1; i <= rows.Count; i++)
				{
					var row = sheet.CreateRow(i);
					var obj = rows[i - 1];
					var values = propsWithGetter.Select((x, index) => (Value: x.GetFunc(obj)?.ToString() ?? string.Empty, Index: index));
					foreach (var x in values)
					{
						row.CreateCell(x.Index).SetCellValue(x.Value);
						FillCols(colsLength, x.Index, x.Value.Length);
					}
				}
			}

			for (int i = 0; i < colsLength.Count; i++)
			{
				sheet.SetColumnWidth(i, colsLength[i] * 2 * /*256*/ 200);
			}

			sheet.SetAutoFilter(new CellRangeAddress(0, rows.Count, 0, colsLength.Count - 1));
			workbook.Write(ret, true);
		}

		private static void FillCols(IList<int> cols, int index, int length)
		{
			if (cols.Count == index)
			{
				cols.Add(length);
			}
			else
			{
				var colLength = cols[index];
				if (colLength < length)
				{
					cols[index] = length;
				}
			}
		}

	}
}
