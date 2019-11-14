using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        private Func<string, bool> search_Pattern;

        public FileSystemVisitor()
        {
            search_Pattern = null;
        }

        public FileSystemVisitor(Func<string, bool> searchPattern)
        {
            search_Pattern = searchPattern;
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] fileList={};
            foreach (var item in fileList)
                yield return item;
        }

        public event EventHandler OnStart;
        public event EventHandler OnFinish;
        public event EventHandler OnFileFinded;
        public event EventHandler OnDirectoryFinded;
        public event EventHandler OnFilteredFileFinded;
        public event EventHandler OnFilteredDirectoryFinded;

        public string FindAFile(string fileName) => fileName;

        public string FindADirectory(string dirName) => dirName;

        public string FindAFilteredFile(string fileName) => fileName;

        public string FindAFilteredDirectory(string dirName) => dirName;

        public void Searching (string startPosition)
        {
            OnStart?.Invoke(this, null);

            var allFiles = Directory.GetFileSystemEntries (startPosition, "*.*", SearchOption.AllDirectories);

            foreach (var item in allFiles)
            {
                bool isDir = Directory.Exists(item);

                if (isDir)
                {
                    OnDirectoryFinded?.Invoke(this, null);
                    if (search_Pattern(item)) OnFilteredDirectoryFinded?.Invoke(this, null);
                }
                else
                {
                    OnFileFinded?.Invoke(this, null);
                    if (search_Pattern(item)) OnFilteredFileFinded?.Invoke(this, null);
                }
            }
            OnFinish?.Invoke(this, null);
        }
    }
}
