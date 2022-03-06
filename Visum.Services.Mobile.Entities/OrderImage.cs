
namespace Visum.Services.Mobile.Entities
{
    public class OrderImage
    {
        public int OrderNo { get; set; }
        public int OrderDtlNo { get; set; }
        public int OrderImageID { get; set; }
        public byte OrderImageGroup { get; set; }
        public string OrderImageName { get; set; }
        public string OrderImageExt { get; set; }
        public string OrderImageDscr { get; set; }
        public string Image { get; set; }
        public string OrderImageEmpID { get; set; }
    }
}
