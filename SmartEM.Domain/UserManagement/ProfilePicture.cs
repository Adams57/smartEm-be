namespace SmartEM.Domain.UserManagement;

public class ProfilePicture
{
    protected ProfilePicture() { }
    public ProfilePicture(string fileName, string contenttype)
    {
        FileName = fileName;
        ContentType = contenttype;
    }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}