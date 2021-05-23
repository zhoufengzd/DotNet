using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Retrieve the list of files or directories by the search condition.
    /// More powerful than the default 'Directory.GetFiles / GetDirectories' implementation 
    ///   since it supports regular expressions.
    /// </summary>
    public sealed class FileDirLister
    {
        public static List<string> GetFiles(FileDirSearchOpt fso)
        {
            if (!Directory.Exists(fso.QueryDirectory))
                return null;

            Stack<string> directoryStack = new Stack<string>();
            directoryStack.Push(fso.QueryDirectory);

            List<string> fileList = new List<string>();
            SearchOpt searchOpt = fso.SearchOptions;
            Regex regexInFilter = new Regex(searchOpt.IncludeFilter);
            Regex regexOutFilter = string.IsNullOrEmpty(searchOpt.ExcludeFilter) ? null : new Regex(searchOpt.ExcludeFilter);

            while (directoryStack.Count > 0)
            {
                string currentDir = directoryStack.Pop();
                try
                {
                    string[] fileListTmp = Directory.GetFiles(currentDir, "*.*");
                    foreach (string filePath in fileListTmp)
                    {
                        string fileName = Path.GetFileName(filePath);
                        if (regexInFilter.Match(fileName).Success)
                        {
                            if (regexOutFilter == null || !regexOutFilter.IsMatch(fileName))
                                fileList.Add(filePath);
                        }
                    }

                    if (!fso.Recursive)
                        break;

                    // depth-first: list sub directories
                    foreach (string subDirectory in Directory.GetDirectories(currentDir))
                        directoryStack.Push(subDirectory);

                }
                catch(Exception)
                {
                    throw;
                }
            }

            return fileList;
        }

        public static List<string> GetDirectories(FileDirSearchOpt fso)
        {
            if (!Directory.Exists(fso.QueryDirectory))
                return null;

            Stack<string> directoryStack = new Stack<string>();
            directoryStack.Push(fso.QueryDirectory);

            List<string> directoryList = new List<string>();
            SearchOpt searchOpt = fso.SearchOptions;
            Regex regexInFilter = new Regex(searchOpt.IncludeFilter);
            Regex regexOutFilter = string.IsNullOrEmpty(searchOpt.ExcludeFilter) ? null : new Regex(searchOpt.ExcludeFilter);

            while (directoryStack.Count > 0)
            {
                string currentDir = directoryStack.Pop();
                try
                {
                    string[] subDirectories = Directory.GetDirectories(currentDir);
                    foreach (string dirPath in subDirectories)
                    {
                        string dirName = Path.GetFileName(dirPath);
                        if (regexInFilter.Match(dirName).Success)
                        {
                            if (regexOutFilter == null || !regexOutFilter.IsMatch(dirName))
                                directoryList.Add(dirPath);
                        }

                        if (fso.Recursive)
                            directoryStack.Push(dirPath);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return directoryList;
        }

    }
}