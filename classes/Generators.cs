namespace OLEToolBoxWebAPIPruebas.classes
    {
    public static class Generators
        {

        public static string GenerateIDMainCompany()
            {
            // Hemos cambiado company po maincompany
            var id = "MainCompany";
            Random rnd = new Random();

            //OPeramos con los datos
            var numStr = rnd.Next(1000, 10000).ToString();
            id += numStr;
            id += Generators.GenerateChars();
            return id;

            }

        public static string GenerateIDSystemData()
            {

            var id = "OLEToolBox";
            Random rnd = new Random();

            //Operamos con los datos
            var numStr = rnd.Next(1000, 10000).ToString();
            id += numStr;
            id += Generators.GenerateChars();
            return id;

            }

        public static string GenerateIDCompanies()

            {
            var id = "Company";
            // Creamos una instancia de la clase Random
            Random random = new Random();


            //Operamos con los datos
            var numStr = random.Next(1000, 10000).ToString();
            id += numStr;
            id += Generators.GenerateChars();
            return id;

            }

        public static string GenerateChar()
            {

            // Creamos una instancia de la clase Random
            Random random = new Random();

            // Generamos dos letras mayúsculas aleatorias
            char character1 = (char)random.Next('A', 'Z' + 1);
            string character = character1.ToString();
            // Mostramos las letras aleatorias generadas
            return character;

            }

        public static string GenerateChars()
            {

            // Creamos una instancia de la clase Random
            Random random = new Random();

            // Generamos dos letras mayúsculas aleatorias
            char character1 = (char)random.Next('A', 'Z' + 1);
            char character2 = (char)random.Next('A', 'Z' + 1);

            string characters = character1.ToString() + character2.ToString();

            // Mostramos las letras aleatorias generadas
            return characters;

            }

        }
    }
