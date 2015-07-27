using System;
using System.Runtime.Serialization;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    [Serializable]
    public enum FilterArgumentType
    {        
        PleaseSelect = 0,
        IsEqualTo = 1,
        IsNotEqualTo = 2,
        Contains = 3,
        DoesNotContain = 4,
        StartsWith = 5,
        EndsWith = 6,
        IsNotEmpty = 7,
		IsEmpty = 8,
        IsNull = 9,
        IsNotNull = 10,
        Between = 11,
		NotBetween = 12,
        GreaterThan = 13,
		NotGreaterThan = 14,
        GreaterThanOrEqualTo = 15,
		NotGreaterThanOrEqualTo = 16,
        LessThan = 17,
		NotLessThan = 18,
        LessThanOrEqualTo = 19,
		NotLessThanOrEqualTo = 20,
        True = 21,
        False = 22,
        IsToday = 23,
		IsNotToday = 24,
        IsTomorrow = 25,
		IsNotTomorrow = 26,
        InOneWeek = 27,
		NotInOneWeek = 28,
        InTwoWeeks = 29,
		NotInTwoWeeks = 30,
        InFourWeeks = 31,
		NotInFourWeeks = 32,
        Ascending = 33,
        Descending = 34
	}

    public enum FilterBooleanType
    {
        AND = 0,
        OR = 1,
        XOR = 2,
        NOT = 3
    }
}
