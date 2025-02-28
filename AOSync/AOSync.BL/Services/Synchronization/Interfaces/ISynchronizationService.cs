namespace AOSync.BL.Services.Synchronization.Interfaces;

public interface ISynchronizationService
{
    /// <summary>
    /// Sets changes to the remote server.
    /// </summary>
    Task SetChanges();

    /// <summary>
    /// Retrieves changes from the remote server starting from the specified last transaction ID.
    /// </summary>
    /// <param name="lastTranId">The last transaction ID to start fetching changes from.</param>
    Task GetChanges(string lastTranId);

    /// <summary>
    /// Retrieves the last transaction ID from the local database or the remote server.
    /// </summary>
    /// <returns>The last transaction ID.</returns>
    Task<string> GetLastTransactionId();

    /// <summary>
    /// Retrieves initial changes from the remote server starting from the specified last transaction ID.
    /// </summary>
    /// <param name="lastTranId">The last transaction ID to start fetching initial changes from.</param>
    Task GetInitialChanges(string lastTranId);

    /// <summary>
    /// Stores the transaction ID in the local database.
    /// </summary>
    /// <param name="tranId">The transaction ID to store.</param>
    /// <returns>True if the transaction ID was successfully stored; otherwise, false.</returns>
    Task<bool> StoreTransactionIdAsync(string tranId);
}