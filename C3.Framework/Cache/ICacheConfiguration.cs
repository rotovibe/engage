namespace C3.Framework.Cache
{
	public interface ICacheConfiguration
	{
		bool IsSwitchable { get; }
		bool IsInstrumented { get; }
	}
}
