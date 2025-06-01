namespace Library;

public class Mapa
{
    public int Ancho { get; private set; }
    public int Alto { get; private set; }
    public CasillaMapa[,] Casillas { get; private set; }
    
    // Símbolos para visualización ASCII
    private readonly Dictionary<string, char> simbolos = new()
    {
        ["vacio"] = '.',
        ["madera"] = 'T',
        ["alimento"] = 'F',
        ["oro"] = 'G',
        ["piedra"] = 'S',
        ["centro_civico"] = 'C',
        ["casa"] = 'H',
        ["cuartel"] = 'B',
        ["aldeano"] = 'A',
        ["infanteria"] = 'I',
        ["arquero"] = 'R',
        ["caballeria"] = 'K'
    };

    public Mapa(int ancho, int alto)
    {
        Ancho = ancho;
        Alto = alto;
        Casillas = new CasillaMapa[ancho, alto];
        InicializarMapa();
        GenerarRecursosAleatorios();
    }

    private void InicializarMapa()
    {
        for (int x = 0; x < Ancho; x++)
        {
            for (int y = 0; y < Alto; y++)
            {
                Casillas[x, y] = new CasillaMapa(new Posicion(x, y));
            }
        }
    }

    private void GenerarRecursosAleatorios()
    {
        Random random = new Random();
        int totalCasillas = Ancho * Alto;
        
        // Generar recursos según porcentajes
        int cantidadMadera = (int)(totalCasillas * 0.15); // 15% madera
        int cantidadAlimento = (int)(totalCasillas * 0.10); // 10% alimento
        int cantidadOro = (int)(totalCasillas * 0.05); // 5% oro
        int cantidadPiedra = (int)(totalCasillas * 0.08); // 8% piedra

        ColocarRecursosAleatorios(TipoRecurso.Madera, cantidadMadera, random);
        ColocarRecursosAleatorios(TipoRecurso.Alimento, cantidadAlimento, random);
        ColocarRecursosAleatorios(TipoRecurso.Oro, cantidadOro, random);
        ColocarRecursosAleatorios(TipoRecurso.Piedra, cantidadPiedra, random);
    }

    private void ColocarRecursosAleatorios(TipoRecurso tipo, int cantidad, Random random)
    {
        int colocados = 0;
        while (colocados < cantidad)
        {
            int x = random.Next(Ancho);
            int y = random.Next(Alto);
            
            if (Casillas[x, y].EstaVacia())
            {
                int cantidadRecurso = tipo switch
                {
                    TipoRecurso.Madera => random.Next(300, 501),   // 300-500
                    TipoRecurso.Alimento => random.Next(200, 401), // 200-400
                    TipoRecurso.Oro => random.Next(400, 601),      // 400-600
                    TipoRecurso.Piedra => random.Next(350, 551),   // 350-550
                    _ => 100
                };
                
                Casillas[x, y].Recurso = new Recurso(tipo, cantidadRecurso);
                colocados++;
            }
        }
    }

    public bool EsPosicionValida(Posicion pos)
    {
        return pos.X >= 0 && pos.X < Ancho && pos.Y >= 0 && pos.Y < Alto;
    }

    public bool PuedeColocar(Posicion pos)
    {
        return EsPosicionValida(pos) && Casillas[pos.X, pos.Y].EstaVacia();
    }

    public void ColocarUnidad(Unidad unidad, Posicion pos)
    {
        if (PuedeColocar(pos))
        {
            Casillas[pos.X, pos.Y].Unidad = unidad;
            unidad.Posicion = pos;
        }
    }

    public void ColocarEdificio(Edificio edificio, Posicion pos)
    {
        if (PuedeColocar(pos))
        {
            Casillas[pos.X, pos.Y].Edificio = edificio;
            edificio.Posicion = pos;
        }
    }

    public void MoverUnidad(Posicion origen, Posicion destino)
    {
        if (EsPosicionValida(origen) && PuedeColocar(destino))
        {
            var unidad = Casillas[origen.X, origen.Y].Unidad;
            if (unidad != null)
            {
                Casillas[origen.X, origen.Y].Unidad = null;
                ColocarUnidad(unidad, destino);
            }
        }
    }

    public void MostrarArea(Posicion centro, int radio = 10)
    {
        Console.WriteLine($"\n=== Mapa (Centro: {centro}, Radio: {radio}) ===");
        
        int inicioX = Math.Max(0, centro.X - radio);
        int finX = Math.Min(Ancho - 1, centro.X + radio);
        int inicioY = Math.Max(0, centro.Y - radio);
        int finY = Math.Min(Alto - 1, centro.Y + radio);

        // Mostrar coordenadas X
        Console.Write("   ");
        for (int x = inicioX; x <= finX; x++)
        {
            Console.Write($"{x % 10}");
        }
        Console.WriteLine();

        for (int y = inicioY; y <= finY; y++)
        {
            Console.Write($"{y:D2} ");
            for (int x = inicioX; x <= finX; x++)
            {
                char simbolo = ObtenerSimbolo(new Posicion(x, y));
                Console.Write(simbolo);
            }
            Console.WriteLine();
        }
    }

    private char ObtenerSimbolo(Posicion pos)
    {
        var casilla = Casillas[pos.X, pos.Y];
        
        if (casilla.Unidad != null)
        {
            return casilla.Unidad.Tipo switch
            {
                TipoUnidad.Aldeano => 'A',
                TipoUnidad.Infanteria => 'I',
                TipoUnidad.Arquero => 'R',
                TipoUnidad.Caballeria => 'K',
                _ => 'U'
            };
        }
        
        if (casilla.Edificio != null)
        {
            return casilla.Edificio.Tipo switch
            {
                TipoEdificio.CentroCivico => 'C',
                TipoEdificio.Casa => 'H',
                TipoEdificio.Cuartel => 'B',
                _ => 'E'
            };
        }
        
        if (casilla.Recurso != null && !casilla.Recurso.EstaAgotado)
        {
            return casilla.Recurso.Tipo switch
            {
                TipoRecurso.Madera => 'T',
                TipoRecurso.Alimento => 'F',
                TipoRecurso.Oro => 'G',
                TipoRecurso.Piedra => 'S',
                _ => 'R'
            };
        }
        
        return '.';
    }

    public List<Posicion> EncontrarRecursosCercanos(Posicion pos, TipoRecurso tipo, int radio = 10)
    {
        var recursos = new List<Posicion>();
        
        for (int x = Math.Max(0, pos.X - radio); x <= Math.Min(Ancho - 1, pos.X + radio); x++)
        {
            for (int y = Math.Max(0, pos.Y - radio); y <= Math.Min(Alto - 1, pos.Y + radio); y++)
            {
                var casilla = Casillas[x, y];
                if (casilla.Recurso?.Tipo == tipo && !casilla.Recurso.EstaAgotado)
                {
                    recursos.Add(new Posicion(x, y));
                }
            }
        }
        
        return recursos.OrderBy(r => pos.DistanciaA(r)).ToList();
    }
}

// Clase auxiliar para representar cada casilla del mapa
public class CasillaMapa
{
    public Posicion Posicion { get; private set; }
    public Unidad? Unidad { get; set; }
    public Edificio? Edificio { get; set; }
    public Recurso? Recurso { get; set; }

    public CasillaMapa(Posicion posicion)
    {
        Posicion = posicion;
    }

    public bool EstaVacia()
    {
        return Unidad == null && Edificio == null && (Recurso == null || Recurso.EstaAgotado);
    }

    public bool TieneRecurso(TipoRecurso tipo)
    {
        return Recurso?.Tipo == tipo && !Recurso.EstaAgotado;
    }
}