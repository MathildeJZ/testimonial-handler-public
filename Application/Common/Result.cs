namespace Testimonial.Application.Common;

public class Result<T>
{
    public bool Succeeded { get; init; }
    public T? Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();

    public static Result<T> Success(T data) =>
        new() { Succeeded = true, Data = data };

    public static Result<T> Failure(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}