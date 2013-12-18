namespace Phytel.Framework.SQL.Cache
{
	public interface ICacheConfiguration
	{
		bool IsSwitchable { get; }
		bool IsInstrumented { get; }
	}
}
