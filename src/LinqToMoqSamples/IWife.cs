namespace LinqToMoqSamples
{
    public interface IWife
    {
        string Name { get; }
        int Age { get; set; }
        IHusband Husband { get; }
    }
}