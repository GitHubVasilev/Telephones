namespace Telephones.API.Infrastructure
{
    public abstract class WrapperResult
    {
        public string? Message { get; set; }
        public Exception? ExceptionObject { get; set; }
        public bool IsSuccess
        {
            get => ExceptionObject is null;
        }

        public static WrapperResult Build<T>(T result, string? message = null, Exception? error = null) 
        {
            return new WrapperResult<T>
                {
                    Result = result,
                    Message = message,
                    ExceptionObject = error,
                };
        }

        public static WrapperResult<T> Build<T>()
        {
            return new WrapperResult<T>
            {
                Result = default(T),
            };
        }
    }

    public class WrapperResult<T> : WrapperResult 
    { 
        public T? Result { get; set; }
    }
}
