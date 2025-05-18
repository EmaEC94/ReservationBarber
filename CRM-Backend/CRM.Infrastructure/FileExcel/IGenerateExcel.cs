using CRM.Utilities.Static;

namespace CRM.Infrastructure.FileExcel
{
    public interface IGenerateExcel
    {
        MemoryStream GenerateToExcel<T>(IEnumerable<T> data, List<TableColumn> columns);

    }
}
