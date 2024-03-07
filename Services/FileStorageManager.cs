namespace OLEToolBoxWebAPIPruebas.Services
    {

    /**
     * 
     * Clase tipo Servicio para el almacenamiento de archivos en el servidor local.
     * 
     * Contiene métodos para almacenar Modificar y Borrar archivos
     * 
     * */

    public class FileStorageManager
        {

        //Variable privada IWebHostEnvironment
        // Para poder localizar wwwroot incluyendo el acceso a las variables de entorno de la aplicación.
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor; // Para conocer la configuración del servidor para construir la url de la imagen

        //Constructor con inyección de servicios.

        public FileStorageManager(IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
            {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
            }

        /**
         * 
         * Método DELETE FILE
         * 
         * Para borrar un archivo guardado en el servidor.
         * 
         * */

        public Task DeleteFile(string path, string folder)
            {
            if (path != null)
                {
                var nombreArchivo = Path.GetFileName(path);
                string fileFolder = Path.Combine(env.WebRootPath, folder, nombreArchivo);

                if (File.Exists(fileFolder))
                    {
                    File.Delete(fileFolder);
                    }
                }

            return Task.FromResult(0);
            }

        /**
         * 
         * Método CHANGE FILE
         * 
         * Para editar los datos un archivo guardado en el servidor.
         * 
         * */


        public async Task<string> ChangeFile(byte[] content, string extension, string folder, string path,
            string contentType)
            {
            await DeleteFile(path, folder);
            return await SaveFile(content, extension, folder, contentType);
            }

        /**
         * 
         * Método SAVE FILE
         * 
         * Para guardar los datos un archivo en el servidor.
         * 
         * */

        public async Task<string> SaveFile(byte[] content, string extension, string folder,
            string contentType)
            {
            // Creamos un nombre aleatorio con la extensión
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            // La ruta será wwwroot/carpeta (en este caso imagenes)
            string fldrStr = Path.Combine(env.WebRootPath, folder);

            // Si no existe la carpeta la creamos
            if (!Directory.Exists(fldrStr))
                {
                Directory.CreateDirectory(fldrStr);
                }

            // La ruta donde dejaremos el archivo será la concatenación de la ruta de la carpeta y el nombre del archivo
            string path = Path.Combine(fldrStr, nombreArchivo);
            // Guardamos el archivo
            await File.WriteAllBytesAsync(path, content);

            // La url de la ímagen será http o https://dominio/carpeta/nombreimagen
            var actualUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(actualUrl, folder, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
            }
        }
    }
