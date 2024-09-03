namespace _0_framework.Infrastructure;

public static class Roles
{
    public const string Administrator = "1";
    public const string SystemUser = "2";
    public const string Operator = "3";
    public const string ColleagueUser = "10002";
        
    public static string GetRoleBy(long id)
    {
        switch (id)
        {
            case 1:
                return "مدیرسیستم";
            case 3:
                return "اپراتور";
            default:
                return "";
        }
    }
}