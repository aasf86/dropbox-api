using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApiDropBoxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MDO 
            using (var dbx = new DropboxClient("<token>"))
            {
                var full = dbx.Users.GetCurrentAccountAsync();
                full.Wait();
                
                Console.WriteLine("{0} - {1}", full.Result.Name.DisplayName, full.Result.Email);

                var list = dbx.Files.ListFolderAsync("");

                // show folders then files
                foreach (var item in list.Result.Entries.Where(i => i.IsFolder))
                {
                    Console.WriteLine("D  {0}/", item.Name);
                }

                foreach (var item in list.Result.Entries.Where(i => i.IsFile))
                {
                    Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
                }

                var response = dbx.Files.DownloadAsync("/backup/02.02.2018_17.45.zip");
                response.Wait();
                var bytes = response.Result.GetContentAsByteArrayAsync();
                bytes.Wait();
                var arquivo = bytes.Result;

                File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "02.02.2018_17.45.zip"), arquivo);

            }

            Console.ReadKey();
        }
    }
}
