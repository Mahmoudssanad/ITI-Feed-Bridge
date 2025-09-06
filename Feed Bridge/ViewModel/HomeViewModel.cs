using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Feed_Bridge.Models.Entities.Review> Reviews { get; set; }
        public IEnumerable<Feed_Bridge.Models.Entities.Partener> Partners { get; set; }
    }
}
