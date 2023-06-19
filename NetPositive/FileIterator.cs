using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace NetPositive
{
    class FileIterator : IEnumerable<string>
    {
        string[] basePaths;
        string[] resolvedBasePaths;
        bool recursive;
        bool allowPathEscape;
        public FileIterator(string[] basePaths, bool recursive, bool allowPathEscape)
        {
            this.basePaths = basePaths;
            this.recursive = recursive;
            this.allowPathEscape = allowPathEscape;
            this.resolvedBasePaths = new string[this.basePaths.Length];
            for (int i = 0; i < this.resolvedBasePaths.Length; i++)
            {
                this.resolvedBasePaths[i] = NativeMethods.GetFinalPathName(this.basePaths[i]);
            }
        }

        private void _EnqueueChildren(string path, Queue<string> queue, HashSet<string> done)
        {
            try
            {
                string[] subDirs = Directory.GetDirectories(path);
                for (int i = 0; i < subDirs.Length; i++)
                {
                    string newPath = subDirs[i];
                    string newAbsPath = Path.GetFullPath(subDirs[i]);
                    string newResolvedPath = NativeMethods.GetFinalPathName(newAbsPath);
                    if (!done.Contains(newPath) && !done.Contains(newAbsPath) && !done.Contains(newResolvedPath))
                    {
                        //Only enqueue if we allow not being in a sub path of a base path or if it is in such a sub path
                        if (allowPathEscape || isBelowBasePath(newResolvedPath))
                            queue.Enqueue(newPath);
                        done.Add(newPath); //Add to done list here, so we don't ever need to slowly do linear search through the queue
                        done.Add(newAbsPath);
                        done.Add(newResolvedPath);
                    }
                }
            }
            catch (Win32Exception)
            {
                //Ignore Win32Exception, we probably errored due to access when resolving path
            }
            catch (UnauthorizedAccessException uae)
            {
                //Ignore access violations, we don't need to spam the command line with errors we can't fix anyway
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Couldn't iterate directories in {0}. {1}", path, e));
            }
            try
            {
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    string newPath = files[i];
                    string newAbsPath = Path.GetFullPath(files[i]);
                    string newResolvedPath = NativeMethods.GetFinalPathName(newAbsPath);
                    if (!done.Contains(newPath) && !done.Contains(newAbsPath) && !done.Contains(newResolvedPath))
                    {
                        //Only enqueue if we allow not being in a sub path of a base path or if it is in such a sub path
                        if (allowPathEscape || isBelowBasePath(newResolvedPath))
                            queue.Enqueue(newPath);
                        done.Add(newPath);
                        done.Add(newAbsPath);
                        done.Add(newResolvedPath);
                    }
                }
            }
            catch (Win32Exception)
            {
                //Ignore Win32Exception, we probably errored due to access when resolving path
            }
            catch (UnauthorizedAccessException uae)
            {
                //Ignore access violations, we don't need to spam the command line with errors we can't fix anyway
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Couldn't iterate files in {0}. {1}", path, e));
            }
        }

        private bool isBelowBasePath(string resolvedPath)
        {
            for (int i = 0; i < resolvedBasePaths.Length; i++)
            {
                if (resolvedPath.StartsWith(resolvedBasePaths[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool MatchesFilter(string path)
        {
            var ext = Path.GetExtension(path);
            return ext == ".exe" || ext == ".dll";
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            Queue<string> pendingPaths = new Queue<string>();
            HashSet<string> traversedPaths = new HashSet<string>();
            foreach (var p in this.basePaths)
                pendingPaths.Enqueue(p);
            while (pendingPaths.Count > 0)
            {
                string current = pendingPaths.Dequeue();

                if (this.recursive && Directory.Exists(current))
                {
                    this._EnqueueChildren(current, pendingPaths, traversedPaths);
                }

                if (File.Exists(current) && this.MatchesFilter(current))
                {
                    yield return current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<string>).GetEnumerator();
        }
    }
}
