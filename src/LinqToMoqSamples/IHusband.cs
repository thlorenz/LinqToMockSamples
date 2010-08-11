namespace LinqToMoqSamples
{
    public interface IHusband
    {
        string Name { get; }
        int Age { get; set; }
        IWife Wife { get; }
    }
}