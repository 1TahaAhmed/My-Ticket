using System;
using System.Collections.Generic;
using System.Text;

namespace TicketBooking.Application.Common.Models
{
    public class Result<T> : Result
    {
        private readonly T? _value;
        private Result(T? value, bool isSuccess, Error error) 
            : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");
                return _value!;
            }
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null!);
        public static new Result<T> Failure(Error error) => new Result<T>(default, false, error);
    
        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(Error error) => Failure(error);
        
    }
}
