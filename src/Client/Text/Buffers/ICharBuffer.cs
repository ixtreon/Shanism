namespace Shanism.Client.Models.Text
{
    interface ICharBuffer<T>
    {
        T this[int i] { get; }

        int Length { get; }

        void Delete(int start, int count);

        void Insert(int pos, char c);

        void Reset();

        void Reset(string text);
    }

}
