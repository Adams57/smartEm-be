namespace SmartEM.Application.Utils;

public class PaginatedResponse<T> where T : class
{
    public PaginatedResponse(
        int pageSize,
        List<T> entities,
        int totalCount,
        int pageNumber = 1,
        int forwardLastKey = default,
        int backwardLastKey = default)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Entities = entities;
        ForwardLastKey = forwardLastKey;
        BackwardLastKey = backwardLastKey;
    }

    public PaginatedResponse(
        int pageSize,
        int forwardLastKey,
        int backwardLastKey,
        List<T> entities,
        int totalCount,
        int pageNumber = 1)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Entities = entities;
        ForwardLastKey = forwardLastKey;
        BackwardLastKey = backwardLastKey;
    }

    public int ForwardLastKey { get; }
    public int BackwardLastKey { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int PageNumber { get; }
    public int TotalCount { get; }
    public List<T> Entities { get; }
}
