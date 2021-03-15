using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Interfaces
{
    public interface ISettings<out T>
    {
        /// <summary>
        ///     Gets the current settings instance
        /// </summary>
        /// <value>
        ///     The current settings.
        /// </value>
        T Current { get; }

        /// <summary>
        ///     Saves the settings to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        void Save(string target = "");

        /// <summary>
        ///     Loads the settings from the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        void Load(string location = "");
    }
}
