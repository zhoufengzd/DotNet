
namespace Zen.Utilities.Algorithm
{
    using IDisposable = System.IDisposable;

    /// DataProvider interface.
    public interface IDataProvider<DataType>
    {
        DataType Fetch();
    }

    /// DataStream Provider interface.
    public interface IDataStreamProvider<DataType> : IDataProvider<DataType>, IDisposable
    {
        bool EndOfData { get; }
        void Close();
    }

    /// DataConsumer 
    public interface IDataConsumer<DataType>
    {
        void Feed(DataType data);
    }

    /// DataStreamConsumer interface is a simple "data sink" interface, following 
    ///   the similar pattern used by standard Python modules such as xmllib and sgmllib.
    /// Other examples include the GZIP consumer.
    public interface IDataStreamConsumer<DataType> : IDataConsumer<DataType>, IDisposable
    {
        /// <summary>
        /// To allow some batch behavior, like flush stream buffer, 
        /// when certain line count (e.g., every 80 lines) reached
        /// </summary>
        void Flush();
        void Close();
    }

    #region TextStream
    //using ITextStreamProvider = Zen.Utilities.Algorithm.IDataStreamProvider<string>;
    //using ITextStreamConsumer = Zen.Utilities.Algorithm.IDataStreamConsumer<string>;
    #endregion
}
