namespace Phytel.Services.API.Repository
{
    public enum SelectExpressionGroupType
    {
        AND,
        OR,
        EQ
    }

    public enum SelectExpressionType
    {
        EQ,
        NOTEQ,
        LIKE,
        NOTLIKE,
        STARTWITH,
        IN
    }
}