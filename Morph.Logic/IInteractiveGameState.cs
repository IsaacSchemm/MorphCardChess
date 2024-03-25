namespace Morph.Logic
{
    public interface IInteractiveGameState<T> where T : IComparable<T>, IEquatable<T>
    {
        public T UnderlyingState { get; }
        public IReadOnlyList<IButton<T>> TopCards { get; }
        public IReadOnlyList<IButton<T>> BottomCards { get; }
        public IReadOnlyList<IReadOnlyList<IButton<T>>> Rows { get; }
    }

    public interface IButton<T>
    {
        public string Label { get; }
        public bool Enabled { get; }
        public T NextState { get; }
    }
}