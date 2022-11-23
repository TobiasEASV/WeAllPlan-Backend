namespace Application.DTO;

public class LoggedInUserDto
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string HashId { get; set; }
}