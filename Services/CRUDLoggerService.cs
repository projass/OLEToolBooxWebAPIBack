using System;
using OLEToolBoxWebAPIPruebas.Models;
using Microsoft.EntityFrameworkCore;

namespace OLEToolBoxWebAPIPruebas.Services
    {
    public class CRUDLoggerService : IHostedService
        {
        private readonly IServiceProvider serviceProvider;
        private readonly IWebHostEnvironment _env;
        private readonly string nombreArchivo = "Archivo.txt";
        private readonly TimeSpan intervalo = TimeSpan.FromDays(1); // Intervalo de 1 día
        private Timer timer;

        public CRUDLoggerService(IServiceProvider serviceProvider, IWebHostEnvironment env)
            {
            this.serviceProvider = serviceProvider;
            this._env = env;
            }

        public Task StartAsync(CancellationToken cancellationToken)
            {
            var ahora = DateTime.Now;
            var tiempoHastaPrimeraEjecucion = GetTiempoHastaPrimeraEjecucion(ahora);

            //timer = new Timer(EscribirDatos, null, tiempoHastaPrimeraEjecucion, intervalo);
            Escribir("Proceso iniciado");

            timer = new Timer(EscribirDatos, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));


            return Task.CompletedTask;
            }

        public Task StopAsync(CancellationToken cancellationToken)
            {
            throw new NotImplementedException();
            }

        private TimeSpan GetTiempoHastaPrimeraEjecucion(DateTime ahora)
            {
            var manana = ahora.AddDays(1).Date; // Obtiene la fecha de mañana a medianoche
            var tiempoHastaManana = manana - ahora; // Calcula el tiempo hasta mañana
            return tiempoHastaManana;
            }

        private async void EscribirDatos(object state)
            {
            using (var scope = serviceProvider.CreateScope())
                {
                var context = scope.ServiceProvider.GetRequiredService<PruebasoletoolboxContext>();

                // Obtén las operaciones del día actual
                var operacionesDelDia = await ObtenerOperacionesDelDia(context);



                foreach (var operacion in operacionesDelDia)
                    {
                    var datos = $"Id: {operacion.Id}\n Fecha: {operacion.FechaAccion.ToString("ddMMyyyy")}\n" +
                        $" Operación: {operacion.Operacion}\n Controller: {operacion.Controller}\n Dirección-IP-Petición: {operacion.Ip}";

                    Escribir($"Nueva operación:\n {datos}\n Fin del registro.\n");
                    Escribir($"{operacionesDelDia.Count} operaciones almacenadas en la base de datos");
                    }
                }
            }

        private async Task<List<Operacione>> ObtenerOperacionesDelDia(PruebasoletoolboxContext context)
            {
            var hoy = DateTime.Now.Date;
            return await context.Operaciones
                .Where(op => op.FechaAccion.Date == hoy)
                .ToListAsync();
            }


        private void Escribir(string mensaje)
            {
            var ruta = $@"{_env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
                {
                writer.WriteLine(mensaje);
                }
            }
        }
    }

