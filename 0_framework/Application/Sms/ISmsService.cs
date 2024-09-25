namespace _0_framework.Application.Sms
{
    public interface ISmsService
    {
        void Send(string number, string message);
    }
}