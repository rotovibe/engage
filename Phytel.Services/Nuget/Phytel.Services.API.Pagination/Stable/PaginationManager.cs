namespace Phytel.Services.API.Pagination
{
    public class PaginationManager : IPaginationManager
    {
        private const int DEFAULT_MAX = 2000;

        private const int DEFAULT = 100;

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

                if (take != null)
                {
                    if (take.Value >= 0 && take.Value <= DEFAULT_MAX)
                    {
                        normalizedTake = take;
                    }
                    else
                    {
                        normalizedTake = DEFAULT_MAX;
                    }
                }
                else
                {
                    normalizedTake = DEFAULT;
                }
            }

            return normalizedTake;
        }
    }
}
