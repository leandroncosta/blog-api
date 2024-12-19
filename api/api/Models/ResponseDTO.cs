
using System;
using System.Collections.Generic;


namespace api.Models
{

    public class ResponseDto<T>
    {
        public bool Success { get; private set; }
        public int Status { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Message { get; private set; }

        public T Data { get; private set; }
        public object Error { get; private set; }

   
        private ResponseDto(bool success, int status, DateTime timestamp, string message, T data, object error)
        {
            Success = success;
            Status = status;
            Timestamp = timestamp;
            Message = message;
            Data = data;
            Error = error;
        }

     
        public class Builder
        {
            public bool Success { get; private set; } = true;
            public int Status { get; private set; }
            public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
            public string Message { get; private set; } = null;
            public object Data { get; private set; }
            public object Error { get; private set; } = null;

            public Builder SetSuccess(bool success)
            {
                Success = success;
                return this;
            }

            public Builder SetStatus(int status)
            {
                Status = status;
                return this;
            }

            public Builder SetTimestamp(DateTime timestamp)
            {
                Timestamp = timestamp;
                return this;
            }

            public Builder SetMessage(string message)
            {
                Message = message;
                return this;
            }

            public Builder SetData(object data)
            {
                Data = data;
                return this;
            }

            public Builder SetError(object error)
            {
                Error = error;
                return this;
            }

            public ResponseDto<T> Build()
            {
                return new ResponseDto<T>((bool)Success, Status, Timestamp, Message, (T)Data, Error);
            }
        }
    }
}




