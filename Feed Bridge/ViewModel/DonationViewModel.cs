namespace Feed_Bridge.ViewModel
{
    public class DonationViewModel
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public DateOnly ExpirDate { get; set; }
        public decimal Quantity { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Description { get; set; }
    }
}
