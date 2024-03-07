using System;
namespace OLEToolBoxWebAPIPruebas.Classes
    {
    public class ResultadoHash
        {
        //Representa el HASH de una password encriptada.
        public string Hash { get; set; }

        //El algoritmo SALT que se usó para erncriptar.
        public byte[] Salt { get; set; }
        }
    }

