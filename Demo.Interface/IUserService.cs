namespace Demo.Interface
{
    public interface IUserService
    {
        Task<bool> Login(string account, string password);
        Task<bool> SignUp(string account, string password);

    }
}

