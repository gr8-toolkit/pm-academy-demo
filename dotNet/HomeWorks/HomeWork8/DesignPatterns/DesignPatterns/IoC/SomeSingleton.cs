namespace DesignPatterns.IoC
{
    public class SomeSingleton
    {
        private int _counter = 0;

        public SomeSingleton()
        {
            _counter++;
        }

        public int Counter => _counter;
    }
}
