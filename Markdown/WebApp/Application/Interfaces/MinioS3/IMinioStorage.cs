namespace Application.Interfaces.Services;

public interface IMinioStorage
{
    public Task<Result> UploadFile(string fileName, byte[] fileBytes);
    public Task<Result> DeleteFile(string fileName);
    public Task<Result<byte[]>> DownloadFile(string fileName);
}