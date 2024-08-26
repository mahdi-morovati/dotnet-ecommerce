namespace _0_framework.Application;

public interface IAuthHelper
{
    void SignOut();
    bool IsAuthenticated();
    void Signin(AuthViewModel account);
    string CurrentAccountRole();
    AuthViewModel CurrentAccountInfo();
    List<int> GetPermissions();
    long CurrentAccountId();
    string CurrentAccountMobile();
}