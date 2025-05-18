namespace CRM.Application.Commons.Bases.Request
{
    public class BasePaginationRequest
    {
        public int NumPage { get; set; } = 1; //Numeros de paginas que quiero paginar.
        public int NumRecordsPage { get; set; } = 10;

        private readonly int NumMaxRecordPage = 50;
        public string Order { get; set; } = "asc";
        public string? Sort { get; set; } = null; //Campo a ordenar

        public int Records //Los registros que voy a mostrar
        {
            get => NumRecordsPage;
            set
            {
                NumRecordsPage = value > NumMaxRecordPage ? NumMaxRecordPage : value; //Si el valor de estye mismo es mayor a l numero maxiomo de resgistros por pagian muestre l valor maximo
            }
        }
    }
}
