namespace SuperMarket.Domain.DTO
{
    //public class ApiResponseModel<T, TFilter> : ApiResponseModel<T>
    //{
    //    public TFilter Filter { get; }

    //    public ApiResponseModel(T? data, TFilter filter, int? total = null, bool success = true, DateTime? lastUpdatedTimeUtc = null)
    //        : base(data, total, success)
    //    {
    //        Filter = filter;
    //    }
    //}

    public class ApiResponseModel<T>
    {
        public T? Data { get; }

        public bool Success { get; }

        public int? Total { get; }

        public string? ErrorMessage { get; set; }

        public List<string> Errors { get; set; }

        public ApiResponseModel(T? data, int? total = null, bool success = true)
        {
            Success = success;
            Data = data;
            Total = total;
        }

        public ApiResponseModel()
        {
        }
    }


}
