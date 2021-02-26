namespace DesignPatterns.Builder
{
    public class CustomStringBuilder : ICustomStringBuilder
    {
        public CustomStringBuilder()
        {
        }

        public CustomStringBuilder(string text)
        {
        }

        public ICustomStringBuilder Append(string str)
        {
            throw new System.NotImplementedException();
        }

        public ICustomStringBuilder Append(char ch)
        {
            throw new System.NotImplementedException();
        }

        public ICustomStringBuilder AppendLine()
        {
            throw new System.NotImplementedException();
        }

        public ICustomStringBuilder AppendLine(string str)
        {
            throw new System.NotImplementedException();
        }

        public ICustomStringBuilder AppendLine(char ch)
        {
            throw new System.NotImplementedException();
        }

        public string Build()
        {
            throw new System.NotImplementedException();
        }
    }
}