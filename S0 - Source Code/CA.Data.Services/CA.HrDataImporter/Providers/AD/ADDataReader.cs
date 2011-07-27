namespace CA.HrDataImporter.Providers.AD
{
    using System;
    using System.DirectoryServices;
    using Extensions;

    public class ADDataReader: IADDataReader, IDisposable
    {
        private readonly DirectorySearcher searcher;

        public ADDataReader(string ldapString, string[] propertiesToLoad)
        {
            ldapString.AssertNotNull("ldapString");

            var directoryEntry = new DirectoryEntry(ldapString) {AuthenticationType = AuthenticationTypes.Secure};

            this.searcher = new DirectorySearcher(directoryEntry);

            if (propertiesToLoad != null)
            {
                this.searcher.PropertiesToLoad.AddRange(propertiesToLoad);
            }

            this.searcher.Filter = "(&(objectClass=user))";
        }

        public SearchResultCollection Read()
        {
            return this.searcher.FindAll();
        }

        public void Dispose()
        {
            this.searcher.Dispose();
        }
    }
}