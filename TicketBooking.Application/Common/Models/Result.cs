using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Application.Common.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } = null!;

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException();

            if (!isSuccess && error == null)
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error!;
        }

        public static Result Success() => new Result(true, null!);
        public static Result Failure(Error error) => new Result(false, error);

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
    }
}
