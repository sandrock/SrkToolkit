using SrkToolkit.WildServiceRef.Clients;

namespace SrkToolkit.WildServiceRef {
    /// <summary>
    /// Base client factory.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <typeparam name="TDefault">The type of the default.</typeparam>
    public class BaseClientFactory<TInterface, TDefault> where TDefault : BaseHttpClient {

        //public virtual TInterface CreateDefaultClient() {
        //    Activator.CreateInstance(typeof(TDefault), 
        //}

    }
}
