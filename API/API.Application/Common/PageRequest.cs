namespace API.Application.Common;

public sealed record PagedRequest(
    int Page,
    int Size
)
{
    public int Skip => Math.Max(0, (Page <= 1 ? 0 : (Page - 1)) * Math.Max(1, Size));
    public int Take => Math.Clamp(Size, 1, 100);
}
