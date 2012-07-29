using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Jumbleblocks.Core.Configuration
{
    /// <summary>
    /// Wraps configuration manager
    /// </summary>    
    public class ConfigurationManagerWrapper : IConfigurationReader
    {
        /// <summary>
        ///  Gets the AppSettingsSection data for the current application's default configuration.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Could not retrieve a NameValueCollection object with the application settings data.</exception>
        public NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        /// <summary>
        /// Gets the ConnectionStringsSection data for the current application's default configuration.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Could not retrieve a ConnectionStringSettingsCollection object.</exception>
        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        /// <summary>
        /// Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// <typeparam name="T">ConfigurationSection</typeparam>
        /// <param name="sectionName">The configuration section path and name.</param>
        /// <returns>Specified ConfigurationSection object cast as T or null if does not exist</returns>
        /// <exception cref="ConfigurationErrorsException">A configuration file could not be loaded.</exception>
        /// <exception cref="InvalidCastException">Could not cast ConfigurationSection returned to type T</exception>
        public T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            object configurationSection = ConfigurationManager.GetSection(sectionName);

            if (configurationSection == null)
                return configurationSection as T;

            if (configurationSection is T)
                return (T)configurationSection;
            else
                throw new InvalidCastException(String.Format("Configuration Section with name '{0}' can not be cast to the type '{1}'"));
        }
    }
}
