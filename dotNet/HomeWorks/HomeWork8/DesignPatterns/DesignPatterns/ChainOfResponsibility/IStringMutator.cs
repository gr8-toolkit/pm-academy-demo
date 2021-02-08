namespace DesignPatterns.ChainOfResponsibility
{
    public interface IStringMutator
    {
        IStringMutator SetNext(IStringMutator next);

        string Mutate(string str);
    }
}
