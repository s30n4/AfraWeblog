namespace AW.Application.DtoDefault.Contracts
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// Skip count (beginning of the page).
        /// </summary>
        int SkipCount { get; set; }

        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}