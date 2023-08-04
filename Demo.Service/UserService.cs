using Demo.Interface;
using Entities.Models;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;

namespace Demo.Service
{
    public class UserService : IUserService
    {
        private readonly ITrackableRepository<UserDetail> userRepo;
        private readonly IUnitOfWork unitOfWork;

        public UserService(ITrackableRepository<UserDetail> userRepo, IUnitOfWork unitOfWork)
        {
            this.userRepo = userRepo;
            this.unitOfWork = unitOfWork;

        }

        public Task<bool> Login(string account, string password)
        {
            return Task.Run(() => (this.userRepo.Queryable().Where(x => x.Account == account && x.Password == password).FirstOrDefault() != null));
        }

        public Task<bool> SignUp(string account, string password)
        {
            if (this.userRepo.Queryable().Where(x => x.Account == account).FirstOrDefault() == null)
            {
                this.userRepo.Insert(new UserDetail() { Name = account, Account = account, Password = password });
                this.unitOfWork.SaveChangesAsync();
                return Task.Run(() => (true));
            }
            return Task.Run(() => (false));
        }
    }
}

