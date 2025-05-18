namespace CRM.Utilities.Static
{
    public class ExcelColumnNames
    {
        public static List<TableColumn> GetColumns(IEnumerable<(string ColumnName, string PropertyName)> columnsProperties)
        {
            var columns = new List<TableColumn>();
            foreach (var (ColumnName, PropertyName) in columnsProperties)
            {
                var column = new TableColumn()
                {
                    Label = ColumnName,
                    PropertyName = PropertyName
                };
                columns.Add(column);
            }
            return columns;
        }

        #region GetColumnsClients
        public static List<(string ColumnName, string PropertyName)> GetColumnsClients()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
                ("NOMBRE","Name"),
                ("EMAIL","Email"),
                ("TIPO DE DOCUMENTO","DocumentType"),
                ("N° DE DOCUMENTO","DocumentNumber"),
                ("DIRECIÓN","Address"),
                ("TELEFONO","Phone"),
                ("FECHA DE CREACIÓN","AuditCreateDate"),
                ("ESTADO","StateClient")
            };
            return columnsProperties;
        }
        #endregion

        #region GetColumnsUsers
        public static List<(string ColumnName, string PropertyName)> GetColumnsUsers()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
                ("NOMBRE Usuario","UserName"),
                ("EMAIL","Email"),
                ("FECHA DE CREACIÓN","AuditCreateDate"),
                ("ESTADO","StateClient")
            };
            return columnsProperties;
        }

        #endregion

    }
}
