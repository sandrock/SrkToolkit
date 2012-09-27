
namespace SrkToolkit.Services.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Writes <see cref="Trace"/> events in a log file.
    /// </summary>
    public class FileTraceListener : TraceListener
    {
        /// <summary>
        /// Permits to disable the listener if an exception occurs.
        /// </summary>
        private bool enabled = true;

        private string directory;

        /// <summary>
        /// This is the filename to use.
        /// </summary>
        private string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";

        /// <summary>
        /// This is the computer path to save the file to.
        /// </summary>
        private string filePath;

        /// <summary>
        /// A filestream that stays opened with specific locks.
        /// </summary>
        private FileStream fstream;

        /// <summary>
        /// Prevents concurrent access to <see cref="fstream"/>. 
        /// </summary>
        private object fileLock = new object();

        /// <summary>
        /// The number of events to wait for before flushing the <see cref="fstream"/>. 
        /// </summary>
        private int flushDelay = 10;

        private int flushIndex = 0;

        /// <summary>
        /// Gets (and creates) the file path.
        /// </summary>
        private string FilePath
        {
            get { return this.filePath ?? (this.filePath = CreateFilePath()); }
        }

        /// <summary>
        /// Gets (and creates) the file stream.
        /// </summary>
        private FileStream FileStream
        {
            get { return this.fstream ?? (this.fstream = CreateFileStream()); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTraceListener"/> class.
        /// </summary>
        public FileTraceListener()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTraceListener"/> class.
        /// </summary>
        /// <param name="logDirectory">The logging directory.</param>
        public FileTraceListener(string logDirectory)
        {
            this.directory = logDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTraceListener"/> class.
        /// </summary>
        /// <param name="logDirectory">The log directory.</param>
        /// <param name="fileName">The file name (must be unique for each instance).</param>
        public FileTraceListener(string logDirectory, string fileName)
        {
            this.directory = logDirectory;
            this.fileName = fileName;
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            // this is not required
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            // this is not required
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        ///   </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            lock (this.fileLock)
            {
                if (this.enabled)
                {
                    try
                    {
                        message = message.Replace(Environment.NewLine, Environment.NewLine + "            ");
                        string str = string.Format("{0,-11} {1}", eventType, message) + Environment.NewLine;
                        byte[] data = Encoding.UTF8.GetBytes(str);
                        this.FileStream.Write(data, 0, data.Length);

                        if (eventType < TraceEventType.Information || (++this.flushIndex % this.flushDelay == 0))
                            this.FileStream.Flush();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        this.enabled = false;
                    }
                    catch (IOException)
                    {
                        this.enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
        public override void Flush()
        {
            if (this.fstream != null)
                this.fstream.Flush();

            base.Flush();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this.fileLock)
                {
                    this.enabled = false;

                    if (this.fstream != null)
                    {
                        try
                        {
                            this.fstream.Close();
                            this.fstream = null;
                        }
                        catch (IOException)
                        {
                            // hide this exception to prevent bad behavior
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

        private FileStream CreateFileStream()
        {
            if (!enabled)
                return null;

            if (FilePath == null)
            {
                this.enabled = false;
                return null;
            }

            return new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
        }

        private string CreateFilePath()
        {
            var dir = this.directory ?? "Logs-" + Environment.MachineName;
            string path = null;
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                path = Path.Combine(dir, fileName);
            }
            catch (IOException)
            {
                // hide this exception to prevent bad behavior
            }
            catch (UnauthorizedAccessException)
            {
                // hide this exception to prevent bad behavior
            }

            return path;
        }
    }
}
