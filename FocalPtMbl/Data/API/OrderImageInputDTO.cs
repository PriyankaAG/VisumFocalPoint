namespace FocalPoint.Data.API
{
    public class OrderImageInputDTO
    {
        public OrderImageDetail FPImage { get; set; }
    }

    public class OrderImageDetail
    {
        public int OrderNo { get; set; }
        public int? OrderDtlNo { get; set; }
        public int OrderImageGroup { get; set; }
        public string OrderImageName { get; set; }
        public string OrderImageExt { get; set; }
        public string OrderImageDscr { get; set; }
        public string Image { get; set; }
        public string OrderImageEmpID { get; set; }
    }
}
