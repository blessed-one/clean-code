namespace API;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    
    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string message) => new() { IsSuccess = false, Message = message };
}

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(string message) => new() { IsSuccess = false, Message = message };
}