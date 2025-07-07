# Age of Empires P2 - RTS Discord Bot

## Qué desarrollamos

Desarrollamos un juego de estrategia en tiempo real inspirado en Age of Empires que se ejecuta completamente a través de Discord. Los jugadores pueden crear partidas, elegir civilizaciones, construir edificios, entrenar unidades y enfrentarse en combate usando comandos slash.

Implementamos todas las historias de usuario requeridas: configuración de partidas 2v2, 3 civilizaciones con bonificaciones únicas, gestión de 4 tipos de recursos, sistema de construcción, combate con ventajas tipo piedra-papel-tijera, límites de población y condición de victoria por destrucción del centro cívico.

El objetivo principal era aplicar los principios SOLID y GRASP en un proyecto real y funcional.

## Organización del equipo

- **Rodrigo**: Lógica central del juego - unidades, combate, recursos
- **Jesus**: Bot de Discord - comandos, matchmaking, integración  
- **Natalia**: Testing, QA, tarjetas CRC y diagramas UML

Trabajamos inicialmente con reuniones frecuentes, evolucionando hacia desarrollo independiente con coordinación para integración.

## Principales desafíos

**Concurrencia**: Al integrar las partes surgieron conflictos por múltiples partidas simultáneas. Solucionamos usando estructuras thread-safe como ConcurrentDictionary.

**Discord.NET**: Los comandos tardan una hora en propagarse globalmente, complicando las pruebas. Además, ciertos caracteres Unicode causaban errores de compilación.

**Testing**: Natalia identificó casos edge complejos que no habíamos anticipado, mejorando significativamente la estabilidad del sistema.

## Aplicación de principios SOLID

Antes de codear, Natalia nos guió en la creación de tarjetas CRC y diagramas UML, lo que previno problemas posteriores.

**SRP**: Cada clase tiene responsabilidad única. Aldeano solo maneja lógica de aldeanos, no interfaz o persistencia.

**OCP**: Al agregar unidades especiales (Samurai, Legionario, Berserker) no modificamos código existente.

**LSP**: Cualquier código que opere con Unidad funciona con sus subclases sin conocer detalles específicos.

**ISP**: Usamos interfaces específicas (IAtacable, IRecolector, IConstructor) en lugar de una monolítica.

**DIP**: Los métodos dependen de abstracciones (interfaces) no de implementaciones concretas.

## Aplicación de principios GRASP

**Expert**: Cada clase maneja su propia información. Unidad conoce su posición, Inventario conoce recursos disponibles.

**Controller**: JuegoFacade coordina operaciones complejas, evitando comunicación caótica entre objetos.

**Creator**: Cuartel crea unidades, Mapa genera recursos, siguiendo la lógica del dominio.

## Aprendizajes principales

- Los principios SOLID son herramientas prácticas que facilitan extensión y mantenimiento
- La arquitectura inicial previene horas de refactoring posterior  
- El testing sistemático revela casos que no se consideran durante desarrollo normal
- El desarrollo en equipo requiere coordinación y consideración de integración

## Resultado

Sistema completamente funcional que cumple todos los requisitos obligatorios. Superamos expectativas con integración Discord completa, matchmaking automático y múltiples partidas simultáneas.

No implementamos el bonus de guardar partidas (era opcional).

## Reflexión final

Primer proyecto importante aplicando principios de ingeniería de software en contexto real. Confirmamos que implementamos todos los elementos obligatorios de las historias de usuario originales.

Los principios de diseño demostraron ser herramientas prácticas que realmente facilitan el desarrollo cuando el proyecto aumenta en complejidad.

El proyecto demuestra aplicación exitosa de SOLID y GRASP, cumple completamente con requisitos académicos, y resulta en un producto funcional.

El código está disponible en el repositorio. Solo requiere configurar un bot de Discord para jugar.
