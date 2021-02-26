namespace DesignPatterns.ChainOfResponsibility
{
    public class RemoveNumbersMutator : IStringMutator
    {
        public IStringMutator SetNext(IStringMutator next)
        {
            throw new System.NotImplementedException();
        }

        public string Mutate(string str)
        {
            throw new System.NotImplementedException();
        }
    }
}