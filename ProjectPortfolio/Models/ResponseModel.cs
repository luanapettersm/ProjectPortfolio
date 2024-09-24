﻿using System.Data;

namespace ProjectPortfolio.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}
