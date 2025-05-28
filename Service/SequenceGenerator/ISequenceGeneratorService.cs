namespace Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator
{
    public interface ISequenceGeneratorService
    {

        Task<string> GenerateRegistrationNumberAsync();
        Task<string> GenerateChitNumberAsync();
    }
}
