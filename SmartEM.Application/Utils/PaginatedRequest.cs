namespace SmartEM.Application.Utils
{
    public class PaginatedRequest
    {
        public int? ForwardLastKey { get; set; }
        public int? BackwardLastKey { get; set; }
        public int? PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsBackward { get; set; }

        /// <summary>
        /// Determines if the request can use the last key for pagination.
        /// </summary>
        /// <returns></returns>
        public bool UseLastKey()
        {
            if (ForwardLastKey.GetValueOrDefault() <= 0 && BackwardLastKey.GetValueOrDefault() <= 0) return false;
            if (IsBackward && BackwardLastKey.GetValueOrDefault() <= 0) return false;
            if (!IsBackward && ForwardLastKey.GetValueOrDefault() <= 0) return false;
            return true;
        }
    }
}
