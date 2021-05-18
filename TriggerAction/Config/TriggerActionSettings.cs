/*
 * https://gist.github.com/danielgreen/5772822
 * Code to create a custom configuration section with child collections
 */
using System.Configuration;

namespace TriggerAction.Config
{
    public class TriggerActionSettings : ConfigurationSection
    {
        private static TriggerActionSettings _settings = ConfigurationManager.GetSection("TriggerActionSettings") as TriggerActionSettings;

        public static TriggerActionSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        [ConfigurationProperty("defaultEnvironment", IsRequired = true)]
        public string DefaultEnvironment
        {
            get { return (string)this["defaultEnvironment"]; }
            set { this["defaultEnvironment"] = value; }
        }

        [ConfigurationProperty("Environments", IsRequired = true)]
        [ConfigurationCollection(typeof(EnvironmentSettings), AddItemName = "Environment")]
        public EnvironmentSettings Environments
        {
            get { return (EnvironmentSettings)this["Environments"]; }
            set { this["Environments"] = value; }
        }
    }

    public class EnvironmentSettings : BaseConfigurationElementCollection<string, EnvironmentSetting>
    {
        protected override string GetElementKey(EnvironmentSetting element)
        {
            return element.Name;
        }
    }

    public class EnvironmentSetting : ConfigurationElement
    {
        [ConfigurationProperty("Accounts", IsRequired = true)]
        [ConfigurationCollection(typeof(AccountSettings), AddItemName = "Account")]
        public AccountSettings Accounts
        {
            get { return (AccountSettings)this["Accounts"]; }
            set { this["Accounts"] = value; }
        }

        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("webServiceURL", IsRequired = true)]
        public string WebServiceURL
        {
            get { return (string)this["webServiceURL"]; }
            set { this["webServiceURL"] = value; }
        }
    }

    public class AccountSettings : BaseConfigurationElementCollection<string, AccountSetting>
    {
        protected override string GetElementKey(AccountSetting element)
        {
            return element.Username;
        }
    }

    public class AccountSetting : ConfigurationElement
    {
        [ConfigurationProperty("username", IsKey=true, IsRequired=true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("serviceName", IsRequired = true)]
        public string ServiceName
        {
            get { return (string)this["serviceName"]; }
            set { this["serviceName"] = value; }
        }

        [ConfigurationProperty("serviceId", IsRequired = true)]
        public string ServiceID
        {
            get { return (string)this["serviceId"]; }
            set { this["serviceId"] = value; }
        }
    }
}
