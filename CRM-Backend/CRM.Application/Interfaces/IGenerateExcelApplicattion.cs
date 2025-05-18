namespace CRM.Application.Interfaces
{
    public interface IGenerateExcelApplicattion
    {
        byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumnName, string PropertyName)> columns);

    }
}
