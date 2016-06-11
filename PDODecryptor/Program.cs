/*
* Copyright © 2016 Silv3r <silv3r@openmailbox.org>
* This work is free. You can redistribute it and/or modify it under the
* terms of the Do What The Fuck You Want To Public License, Version 2,
* as published by Sam Hocevar. See the LICENSE file for more details.
*/

using System.IO;
using System.Text;

namespace PDODecryptor
{
    class Program
    {
        static string TexturePassword = "fghOP[]F$%*gfd£DD22dDF2";
        static string MapPassword = "dfDE-232'-OPju;-=4dd889&£2%";
        static string Separator = "[STARTOFDATA]";

        static byte[] EncryptBuffer(byte[] buffer, string password)
        {
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)(buffer[i] ^ (byte)password[i % password.Length]);
            }
            return buffer;
        }

        static byte[] RemoveGarbage(byte[] buffer)
        {
            string content = Encoding.Default.GetString(buffer);
            int index = content.IndexOf(Separator);
            if (index != -1)
            {
                content = content.Substring(index + Separator.Length);
            }
            return Encoding.Default.GetBytes(content);
        }
        
        static void Main(string[] args)
        {
            foreach (string file in args)
            {
                string extension = Path.GetExtension(file).ToUpperInvariant();
                if (extension != ".MAP" && extension != ".PNG") continue;
                
                string password = extension == ".MAP" ? MapPassword : TexturePassword;
                string outputFile = Path.ChangeExtension(file, ".out" + Path.GetExtension(file));

                byte[] buffer = File.ReadAllBytes(file);

                buffer = EncryptBuffer(buffer, password);
                buffer = RemoveGarbage(buffer);

                File.WriteAllBytes(outputFile, buffer);
            }
        }
    }
}
