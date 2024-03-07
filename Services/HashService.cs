using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using OLEToolBoxWebAPIPruebas.Classes;

namespace OLEToolBoxWebAPIPruebas.Services
    {
    public class HashService
        {
        // Método para generar el salt
        public ResultadoHash Hash(string textoPlano)
            {
            // Generamos el salt aleatorio
            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
                {
                random.GetBytes(salt); // Genera un array aleatorio de bytes
                }

            // Llamamos al método ResultadoHash y retornamos el hash con el salt
            return Hash(textoPlano, salt);
            }


        public ResultadoHash Hash(string textoPlano, byte[] salt)
            {
            //Pbkdf2 es un algoritmo de encriptación
            var claveDerivada = KeyDerivation.Pbkdf2(password: textoPlano,
                salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(claveDerivada);

            return new ResultadoHash()
                {
                Hash = hash,
                Salt = salt
                };
            }
        }
    }

