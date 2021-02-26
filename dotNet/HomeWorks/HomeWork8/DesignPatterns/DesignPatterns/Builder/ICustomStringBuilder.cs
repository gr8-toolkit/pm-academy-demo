namespace DesignPatterns.Builder
{
    public interface ICustomStringBuilder
    {
        ICustomStringBuilder Append(string str);

        ICustomStringBuilder Append(char ch);

        ICustomStringBuilder AppendLine();

        ICustomStringBuilder AppendLine(string str);

        ICustomStringBuilder AppendLine(char ch);

        string Build();
    }
}
