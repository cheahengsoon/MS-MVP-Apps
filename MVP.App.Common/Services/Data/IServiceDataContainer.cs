﻿namespace MVP.App.Services.Data
{
    using System;
    using System.Threading.Tasks;

    public interface IServiceDataContainer
    {
        /// <summary>
        /// Gets a value indicating whether the container has loaded.
        /// </summary>
        bool Loaded { get; }

        /// <summary>
        /// Gets the time required between checks for updates from the service.
        /// </summary>
        TimeSpan TimeBetweenUpdates { get; }

        /// <summary>
        /// Gets or sets the date the service was last checked for data.
        /// </summary>
        DateTime LastDateChecked { get; set; }

        /// <summary>
        /// Gets a value indicating whether the container requires updating.
        /// </summary>
        bool RequiresUpdate { get; }

        /// <summary>
        /// Updates the local cached data asynchronously.
        /// </summary>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        Task UpdateAsync();

        /// <summary>
        /// Updates the local cached data asynchronously with the option to force update.
        /// </summary>
        /// <param name="forceUpdate">
        /// A value indicating whether to force the update.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        Task UpdateAsync(bool forceUpdate);

        /// <summary>
        /// Loads the local cached data asynchronously.
        /// </summary>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        Task LoadAsync();

        /// <summary>
        /// Saves the local cached data asynchronously.
        /// </summary>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        Task SaveAsync();
    }
}