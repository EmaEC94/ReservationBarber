using CRM.Application.Interfaces;
using CRM.Infrastructure.FileExcel;
using CRM.Utilities.Static;

namespace CRM.Application.Services
{
    public class GenerateExcelApplication : IGenerateExcelApplicattion
    {
        private readonly IGenerateExcel _generateExcel;

        public GenerateExcelApplication(IGenerateExcel generateExcel)
        {
            _generateExcel = generateExcel;
        }

        public byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns)
        {
            var excelColumns = ExcelColumnNames.GetColumns(columns);
            var memoryStreamExcel = _generateExcel.GenerateToExcel(data, excelColumns);
            var fileBytes = memoryStreamExcel.ToArray();
            return fileBytes;
        }
    }
}
