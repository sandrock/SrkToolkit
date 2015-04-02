
namespace SrkToolkit.WcfServiceRef.Common {

    /// <summary>
    /// Delegate for a service response not containing data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AsyncResponseHandler(object sender, AsyncResponseArgs e);

    /// <summary>
    /// Delegate for a service response containing data.
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AsyncResponseHandler<T>(object sender, AsyncResponseArgs<T> e);
    
}
