using System;
using System.IO;
using DarkMultiPlayerServer;

namespace DMPPlayerLogger
{
    public class PlayerLogger : DMPPlugin
    {
        private object logLock = new object();
        private string playerFolderPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Players");

        public PlayerLogger()
        {
            if (!Directory.Exists(playerFolderPath))
            {
                Directory.CreateDirectory(playerFolderPath);
            }
        }

        public override void OnClientAuthenticated(ClientObject client)
        {
            LogPlayer(client.playerName, client.ipAddress.ToString());
        }

        public void LogPlayer(string playerName, string playerIP)
        {
            lock (logLock)
            {
                string playerFile = Path.Combine(playerFolderPath, playerName + ".txt");
                bool addIP = true;

                if (File.Exists(playerFile))
                {
                    using (StreamReader sr = new StreamReader(playerFile))
                    {
                        string currentLine = null;
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            if (currentLine == playerIP)
                            {
                                addIP = false;
                            }
                        }
                    }
                }
                if (addIP)
                {
                    //Append to end of the file
                    using (StreamWriter sw = new StreamWriter(playerFile, true))
                    {
                        sw.WriteLine(playerIP);
                    }
                }
            }
        }
    }
}

