using System;

namespace Common
{
    public class ExecResult
    {
        public ExecStatus Status { get; set; }
        public string Message { get; set; }

        public static ExecResult NotFound(string message)
        {
            return new ExecResult { Status = ExecStatus.NotFound, Message = message };
        }

        public static ExecResult Failure(string message)
        {
            return new ExecResult { Status = ExecStatus.Failure, Message = message };
        }

        public static ExecResult Success(string message)
        {
            return new ExecResult { Status = ExecStatus.Success, Message = message };
        }
    }

    public class ExecResult<T> : ExecResult
    {
        public T Data { get; set; }
    }
}
