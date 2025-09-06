using Feed_Bridge.Models.Entities;

namespace Feed_Bridge.IServices
{
    public interface IParteerService
    {
        Task Create(Partener partener);
    }
}
