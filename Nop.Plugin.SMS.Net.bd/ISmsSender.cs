namespace Nop.Plugin.SMS.Alpha
{
    public interface ISmsSender
    {
        SmsType SendSmsAsync(string number, string message, string baseUrl, string api_key, string sender_id = null);
        SmsType FindById(long id);
    }
}
