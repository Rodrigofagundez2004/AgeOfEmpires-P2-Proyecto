namespace Library
{
    public enum Iconos
    { //recursos
        Oro = '¤',
        Alimento = '♫',
        Piedra = '♦',
        Madera = '♣',
        // === UNIDADES BÁSICAS ===
        Aldeano = 'A',
        Infanteria = 'I',
        Caballeria = 'C',
        Arquero = 'R',

        // === UNIDADES ESPECIALES ===
        Samurai = 'S',
        Legionario = 'L',
        Berserker = 'B',

        // === EDIFICIOS PRINCIPALES ===
        CentroCivico = 'H',  // o usar 'H' si no soporta unicode
        Cuartel = 'Q',
        Casa = 'h',

        // === EDIFICIOS DE ALMACENAMIENTO ===
        Molino = 'M',
        Granja = 'G',
        AlmacenOro = '$',
        AlmacenPiedra = '#',
        AlmacenMadera = 'W',

        // === RECURSOS NATURALES ===
        MinaOro = 'g',       // gold mine (minúscula para diferenciar)
        MinaPiedra = 's',    // stone mine
        Bosque = 'T',        // Tree/forest

        // === ALDEANOS EN ACCIÓN ===
        AldeanoTalando = 'a',
        AldeanoMinandoOro = 'o',
        AldeanoMinandoPiedra = 'p',
        AldeanoRecolectando = 'r',   // recolectando alimento
        AldeanoAtacando = 'x',

        // === ALDEANOS CONSTRUYENDO ===
        AldeanoConstruyeCuartel = '1',
        AldeanoConstruyeCasa = '2',
        AldeanoConstruyeAlmacenOro = '3',
        AldeanoConstruyeAlmacenPiedra = '4',
        AldeanoConstruyeAlmacenMadera = '5',
        AldeanoConstruyeGranja = '6',
        AldeanoConstruyeMolino = '7',

        // === UNIDADES ATACANDO A OTRAS UNIDADES ===
        // Infantería atacando
        InfanteriaVsInfanteria = 'i',
        InfanteriaVsArquero = 'j',
        InfanteriaVsCaballeria = 'k',
        InfanteriaVsAldeano = 'n',
        InfanteriaVsSamurai = '¡',
        InfanteriaVsLegionario = '¿',
        InfanteriaVsBerserker = '¢',

        // Arquero atacando
        ArqueroVsInfanteria = 'r',
        ArqueroVsArquero = 't',
        ArqueroVsCaballeria = 'u',
        ArqueroVsAldeano = 'v',
        ArqueroVsSamurai = '£',
        ArqueroVsLegionario = '¤',
        ArqueroVsBerserker = '¥',

        // Caballería atacando
        CaballeriaVsInfanteria = 'c',
        CaballeriaVsArquero = 'd',
        CaballeriaVsCaballeria = 'e',
        CaballeriaVsAldeano = 'f',
        CaballeriaVsSamurai = '¦',
        CaballeriaVsLegionario = '§',
        CaballeriaVsBerserker = '¨',

        // Samurai atacando
        SamuraiVsInfanteria = 's',
        SamuraiVsArquero = 'm',
        SamuraiVsCaballeria = 'w',
        SamuraiVsAldeano = 'z',
        SamuraiVsLegionario = 'ª',
        SamuraiVsBerserker = '«',

        // Legionario atacando
        LegionarioVsInfanteria = 'l',
        LegionarioVsArquero = 'q',
        LegionarioVsCaballeria = 'Ñ',
        LegionarioVsAldeano = 'ñ',
        LegionarioVsSamurai = '¬',
        LegionarioVsBerserker = '¯',

        // Berserker atacando
        BerserkerVsInfanteria = 'b',
        BerserkerVsArquero = 'Ø',
        BerserkerVsCaballeria = 'ø',
        BerserkerVsAldeano = 'æ',
        BerserkerVsSamurai = '°',
        BerserkerVsLegionario = '±',

        // === UNIDADES ATACANDO EDIFICIOS ===
        // Atacando Centro Cívico (objetivo principal)
        InfanteriaVsCentroCivico = 'α',
        ArqueroVsCentroCivico = 'β',
        CaballeriaVsCentroCivico = 'γ',
        SamuraiVsCentroCivico = 'σ',
        LegionarioVsCentroCivico = 'λ',
        BerserkerVsCentroCivico = 'φ',

        // Atacando Cuartel
        InfanteriaVsCuartel = 'δ',
        ArqueroVsCuartel = 'ε',
        CaballeriaVsCuartel = 'ζ',
        SamuraiVsCuartel = 'τ',
        LegionarioVsCuartel = 'μ',
        BerserkerVsCuartel = 'χ',

        // Atacando Casa
        InfanteriaVsCasa = 'η',
        ArqueroVsCasa = 'θ',
        CaballeriaVsCasa = 'ι',
        SamuraiVsCasa = 'κ',
        LegionarioVsCasa = 'ν',
        BerserkerVsCasa = 'ψ',

        // Atacando Molino
        InfanteriaVsMolino = 'π',
        ArqueroVsMolino = 'ρ',
        CaballeriaVsMolino = 'ς',
        SamuraiVsMolino = 'υ',
        LegionarioVsMolino = 'ξ',
        BerserkerVsMolino = 'ω',

        // Atacando Granja
        InfanteriaVsGranja = 'Α',
        ArqueroVsGranja = 'Β',
        CaballeriaVsGranja = 'Γ',
        SamuraiVsGranja = 'Δ',
        LegionarioVsGranja = 'Ε',
        BerserkerVsGranja = 'Ζ',

        // Atacando AlmacenOro
        InfanteriaVsAlmacenOro = 'Η',
        ArqueroVsAlmacenOro = 'Θ',
        CaballeriaVsAlmacenOro = 'Ι',
        SamuraiVsAlmacenOro = 'Κ',
        LegionarioVsAlmacenOro = 'Λ',
        BerserkerVsAlmacenOro = 'Μ',

        // Atacando AlmacenPiedra
        InfanteriaVsAlmacenPiedra = 'Ν',
        ArqueroVsAlmacenPiedra = 'Ξ',
        CaballeriaVsAlmacenPiedra = 'Ο',
        SamuraiVsAlmacenPiedra = 'Π',
        LegionarioVsAlmacenPiedra = 'Ρ',
        BerserkerVsAlmacenPiedra = 'Σ',


        // Atacando AlmacenMadera
        InfanteriaVsAlmacenMadera = 'Τ',
        ArqueroVsAlmacenMadera = 'Υ',
        CaballeriaVsAlmacenMadera = 'Φ',
        SamuraiVsAlmacenMadera = 'Χ',
        LegionarioVsAlmacenMadera = 'Ψ',
        BerserkerVsAlmacenMadera = 'Ω',
        // === ALDEANO ATACANDO A UNIDADES ===
        AldeanoVsInfanteria = '¿',
        AldeanoVsCaballeria = '¡',
        AldeanoVsArquero = '¬',
        AldeanoVsSamurai = '¼',
        AldeanoVsLegionario = '½',
        AldeanoVsBerserker = '¾',

        // === ALDEANO ATACANDO EDIFICIOS ===
        AldeanoVsCentroCivico = '¹',
        AldeanoVsCuartel = '²',
        AldeanoVsCasa = '³',
        AldeanoVsMolino = '¤',
        AldeanoVsGranja = '¥',
        AldeanoVsAlmacenOro = '©',
        AldeanoVsAlmacenPiedra = '®',
        AldeanoVsAlmacenMadera = '¶',
        Vacio = '_'
    }
}