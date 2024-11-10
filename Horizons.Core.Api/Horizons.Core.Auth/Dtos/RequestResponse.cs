using Horizons.Core.Auth.Enums;

namespace Horizons.Core.Auth.Dtos
{
    public class RequestResponse
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public ResponseTypeEnum ResponseType { get; set; }
        public string Message { get; set; } = "";
    }
}
