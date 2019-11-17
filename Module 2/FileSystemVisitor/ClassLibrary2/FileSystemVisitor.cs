using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor
{
    public class FileSystemVisitor: IEnumerable<string>
    {
        private Func<string, bool> _searchPattern;
        private string _startPosition;

        public FileSystemVisitor(string startPosition)
        {
            this._startPosition = startPosition;
            _searchPattern = null;
        }

        public FileSystemVisitor(string startPosition, Func<string, bool> searchPattern)
        {
            this._startPosition = startPosition;
            _searchPattern = searchPattern;
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
            List<string> result = new List<string>();

            OnStart?.Invoke(this, null);

            var allFiles = Directory.GetFileSystemEntries(_startPosition, "*.*", SearchOption.AllDirectories);

            foreach (var item in allFiles)
            {
                bool isDir = Directory.Exists(item);
                var arg = new CharachteristicsOfItems();
                result.Add(item);
                if (isDir)
                {
                    OnDirectoryFinded?.Invoke(this, item);                    
                    if (_searchPattern != null)
                    {
                        if (_searchPattern(item))
                        {
                            OnFilteredDirectoryFinded?.Invoke(item, arg);
                            if (arg.cancelSearch) return result;
                        }
                    }
                }
                else
                {
                    OnFileFinded?.Invoke(this, item);
                    if (_searchPattern != null)
                    {
                        if (_searchPattern(item))
                        {
                            OnFilteredFileFinded?.Invoke(item, arg);
                            if (arg.cancelSearch) return result;
                        }
                    }
                }
            }
            OnFinish?.Invoke(this, null);
            return result;
        }        
    }
}
