using Manager.DataAccess.Repository;
using Manager.Model.Services.Abstraction;
using System.Threading.Tasks;

namespace Manager.DataAccess.Services
{
    public class SinhnhatdailyService : ISinhnhatdailyService
    {
        private readonly SinhnhatdailyRepository _repository;

        public SinhnhatdailyService(SinhnhatdailyRepository repository)
        {
            _repository = repository;
        }

        public async Task CheckBirthdayAsync()
        {
           
            await _repository.SendBirthdayEmailsAsync();
        }

    }
}
