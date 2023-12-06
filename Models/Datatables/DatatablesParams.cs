namespace aes.Models.Datatables
{
    public class DtParams
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string SearchValue { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }
    }
}
