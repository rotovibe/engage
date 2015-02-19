using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace Phytel.API.AppDomain.NG.Search.LuceneStrategy
{
    public class LuceneIndexWriter
    {
        private static IndexWriter writer;

        private LuceneIndexWriter()
        {
        }

        public static IndexWriter Writer(FSDirectory directory, Analyzer analyzer)
        {
            try
            {
                writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED){ WriteLockTimeout = 20 };
            }
            catch (LockObtainFailedException ex)
            {
                FSDirectory indexFSDir = FSDirectory.Open(directory.Directory, new SimpleFSLockFactory(directory.Directory));
                IndexWriter.Unlock(indexFSDir);
                writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED) { WriteLockTimeout = 20 };
            }
            return writer;
        }
    }
}
