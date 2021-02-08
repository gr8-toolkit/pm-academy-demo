namespace DesignPatterns.ChainOfResponsibility
{
    public class InvertMutator : IStringMutator
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