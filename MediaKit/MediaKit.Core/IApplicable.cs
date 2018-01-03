namespace MediaKit.Core
{
    public interface IApplicable
    {
        bool IsApplied { get; }
        void Apply();
    }
}