namespace MikrocopUsers.Api.DTOs;

public sealed class CollectionResponse<T>
{
    public List<T> Data { get; init; }
}
