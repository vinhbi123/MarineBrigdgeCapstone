using System.Text.Json;

namespace ShipCapstone.Domain.Models.Common;

public class ApiResponse<T>
{
    public int Status { get; set; }
    public string? Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class ApiResponse : ApiResponse<object>
{
    public ApiResponse() { }
}