using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class UserConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("culture")]
        public string Culture
        {
            get { return ((string)base["culture"]); }
        }

        [ConfigurationProperty("defaultFolder")]
        public string defaultFolder
        {
            get { return ((string)base["defaultFolder"]); }
        }

        [ConfigurationProperty("rulesForFiles")]
        public RulesCollection RulesForFiles
        {
            get { return ((RulesCollection)base["rulesForFiles"]); }
        }

        [ConfigurationProperty("observFolders")]
        public FoldersCollection FolderItems
        {
            get { return ((FoldersCollection)(base["observFolders"])); }
        }
    }

    [ConfigurationCollection(typeof(FolderElement))]
    public class FoldersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)(element)).Name;
        }
    }

    [ConfigurationCollection(typeof(RuleElement))]
    public class RulesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)(element)).Rule;
        }
    }

    public class FolderElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
        }
    }

    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("nameFile", DefaultValue = "", IsKey = true)]
        public string Rule
        {
            get { return ((string)(base["nameFile"])); }
        }

        [ConfigurationProperty("destination", DefaultValue = "", IsKey = false)]
        public string Destination
        {
            get { return ((string)(base["destination"])); }
        }

        [ConfigurationProperty("addNumber", DefaultValue = "false", IsKey = false)]
        public bool AddNumber
        {
            get { return ((bool)(base["addNumber"])); }
        }

        [ConfigurationProperty("addDate", DefaultValue = "false", IsKey = false)]
        public bool AddDate
        {
            get { return ((bool)(base["addDate"])); }
        }
    }
}
