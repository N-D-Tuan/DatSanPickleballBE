namespace DatSanPickleballBE.ModelDto
{
    public class SanPhamDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public string Badge { get; set; }

        public List<string> Images { get; set; } = new();
        public List<string> Features { get; set; } = new();

        public double? Rating { get; set; }
        public int Reviews { get; set; }
    }
}
