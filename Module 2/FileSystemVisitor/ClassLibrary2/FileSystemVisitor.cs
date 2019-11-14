using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor
{
    public class FileSystemVisitor: IEnumerable<string>
    {
        private Func<string, bool> search_Pattern;
        private string startPosition;

        public FileSystemVisitor(string startPosition)
        {
            this.startPosition = startPosition;
            search_Pattern = null;
        }

        public FileSystemVisitor(string startPosition, Func<string, bool> searchPattern)
        {
            this.startPosition = startPosition;
            search_Pattern = searchPattern;
        }

        public class CharachteristicsOfItems: EventArgs
        {
            public bool cancelSearch;
            public bool excludeItem;
        }        

        private IEnumerable<string> fileList
        { get
            {
                return Searching();
            }
        }        

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var file in fileList)
            {
                yield return file;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event EventHandler OnStart;
        public event EventHandler OnFinish;
        public event EventHandler<string> OnFileFinded;
        public event EventHandler<string> OnDirectoryFinded;
        public event Action<string, CharachteristicsOfItems> OnFilteredFileFinded;
        public event Action<string, CharachteristicsOfItems> OnFilteredDirectoryFinded;

        public string FindAFile(string fileName) => fileName;

        public string FindADirectory(string dirName) => dirName;

        public string FindAFilteredFile(string fileName) => fileName;

        public string FindAFilteredDirectory(string dirName) => dirName;

        public IEnumerable<string> Searching()
        {
            OnStart?.Invoke(this, null);

            var allFiles = Directory.GetFileSystemEntries(startPosition, "*.*", SearchOption.AllDirectories);

            foreach (var item in allFiles)
            {
                bool isDir = Directory.Exists(item);
                var arg = new CharachteristicsOfItems();
                if (isDir)
                {
                    OnDirectoryFinded?.Invoke(this, item);
                    if (search_Pattern(item))
                    {
                        OnFilteredDirectoryFinded?.Invoke(item, arg);
                        if (arg.cancelSearch) return allFiles;
                    }
                }
                else
                {
                    OnFileFinded?.Invoke(this, item);
                    if (search_Pattern(item))
                    {
                        OnFilteredFileFinded?.Invoke(item, arg);
                        if (arg.cancelSearch) return allFiles;
                    }
                }
            }
            OnFinish?.Invoke(this, null);
            return allFiles;
        }        
    }
}
