using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace Jumbleblocks.Core.Configuration
{
    /// <summary>
    /// interface for reading configuration file
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        ///  Gets the AppSettingsSection data for the current application's default configuration.
        /// </summary>
        NameValueCollection AppSettings { get; }

        /// <summary>
        /// Gets the ConnectionStringsSection data for the current application's default configuration.
        /// </summary>
        ConnectionStringSettingsCollection ConnectionStrings { get; }

        /// <summary>
        /// Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// <typeparam name="T">ConfigurationSection</typeparam>
        /// <param name="sectionName">The configuration section path and name.</param>
        /// <returns>Specified ConfigurationSection object cast as T or null if does not exist</returns>
        T GetSection<T>(string sectionName) where T : ConfigurationSection;
    }
}
