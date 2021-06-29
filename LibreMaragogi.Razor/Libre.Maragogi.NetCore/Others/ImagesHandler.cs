using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibreMaragogi.Others
{
    public class ImagesHandler
    {
        public string path;

        public ImagesHandler(string path)
        {
            this.path = path;

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void DeletingFiles()
        {
            var dir = new DirectoryInfo(path);
            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }
        }

        public List<string> GettingFiles()
        {
            return Directory.GetFiles(path).ToList();
        }

        public async Task<bool> SaveImage(List<IFormFile> files)
        {
            DeletingFiles();

            for (int i = 0; i < files.Count; i++)
            {
                string filename = Guid.NewGuid().ToString();

                //verificando o arquivo
                if (files[i].FileName.ToLower().Contains(".jpg") || files[i].FileName.ToLower().Contains(".png"))
                {
                    filename += ".jpg";
                }
                else
                {
                    return false;
                }

                string fullPath = path + filename;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await files[i].CopyToAsync(stream);
                }
            }

            return true;
        }
    }
}
