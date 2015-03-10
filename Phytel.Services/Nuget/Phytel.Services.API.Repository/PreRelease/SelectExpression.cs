namespace Phytel.Services.API.Repository
{
    public struct SelectExpression
    {
        public int ExpressionOrder { get; set; }

        public string FieldName { get; set; }

        public int GroupID { get; set; }

        public SelectExpressionGroupType NextExpressionType { get; set; }

        public SelectExpressionType Type { get; set; }

        public object Value { get; set; }
    }
}