namespace KhangNghi_BE.Data.ViewModels;

public class Token
{
    public string TokenId { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}