namespace ArmChair.Linq.Pre.Handlers
{

    /// <summary>
    /// processes part of the LinqQuery
    /// </summary>
    public interface ISubPatternHandler
    {
        /// <summary>
        /// if the sub pattern is recongnised and can be handled by this class
        /// </summary>
        bool CanHandle(ProcessingLinqContext ctx);

        /// <summary>
        /// as it can handle the current statement it will then update the document
        /// </summary>
        void Update(ProcessingLinqContext ctx);

        /// <summary>
        /// indicates that we have a index query. and no more can be accomplished by the index, the rest
        /// will need to be handled in proc
        /// </summary>
        bool IndexQueryCompleted(ProcessingLinqContext ctx);
    }
}