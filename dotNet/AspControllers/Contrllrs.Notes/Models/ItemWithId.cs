namespace Contrllrs.Notes.Models
{
    public class ItemWithId<T> where T: class
    {
        public int Id { get; set; }
        public T Item { get; set; }
    }
}
