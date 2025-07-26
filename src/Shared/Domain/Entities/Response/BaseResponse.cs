namespace Shared.Domain.Entities.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Result { get; set; }
        public string? ErrorMessage => !IsSuccess ? Message : null;

        public BaseResponse()
        {
        }

        public BaseResponse(bool isSuccess, string message, object? result = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Result = result;
        }

        public static BaseResponse Success(string message = "Transaction completed successfully.")
        {
            return new BaseResponse(true, message);
        }

        public static BaseResponse Failure(string errorMessage)
        {
            return new BaseResponse(false, errorMessage);
        }
    }
}
