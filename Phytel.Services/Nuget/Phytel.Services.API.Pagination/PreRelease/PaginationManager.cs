namespace Phytel.Services.API.Pagination
{
    public class PaginationManager : IPaginationManager
    {
        public int GetNormalizeSkip(object requestDto)
        {
            int normalizedSkip = 0;

            var request = requestDto as IPaginationRequest;

            if (request != null)
            {
                var skip = request.Skip;
                normalizedSkip = skip <= 0 ? 0 : skip;
            }

            return normalizedSkip;
        }

        public int? GetNormalizeTake(object requestDto)
        {
            int? normalizedTake = null;

            var request = requestDto as IPaginationRequest;

            if (request != null)
            {
                var take = request.Take;

                if (take != null && take.Value > 0)
                {
                    normalizedTake = take;
                }
            }

            return normalizedTake;
        }
    }
}
