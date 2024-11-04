namespace KhangNghi_BE.Data.ViewModels;

public class ApiReponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}