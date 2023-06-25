namespace DDRC.WebApi.Models
{
    public interface ICacheModel
    {
        int Index { get; }

        string GetKey();
        string Serialize();
        TModel Deserialize<TModel>(string? serialized);
    }
}
