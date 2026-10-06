# Documentación técnica — SDVE

Aplicación de escritorio en **C# / Windows Forms (.NET 10)**. Sin base de datos ni paquetes externos: los votos se guardan en un archivo CSV.

## Arquitectura por módulos

| Módulo | Rama | Archivos | Responsabilidad |
|---|---|---|---|
| Convocatorias | `feature/convocatorias` | `Convocatoria.cs`, `Candidato.cs`, `FrmConvocatorias.cs` | Elegir convocatorias activas y candidatos |
| Votación | `feature/votacion` | `Alumno.cs`, `Voto.cs`, `VotacionService.cs`, `FrmVotacion.cs` | Capturar y validar el voto, evitar duplicados, guardar |
| Resultados | `feature/resultados` | `Resultado.cs`, `Estadisticas.cs`, `ResultadoService.cs` | Conteo, agrupación, participación, abstencionismo |
| Reportes | `feature/reportes` | `FrmResultados.cs`, `GraficaBarras.cs`, `ExportacionService.cs` | Pantalla de resultados, gráficas, exportación |

## Flujo de datos

```
FrmVotacion --(Voto)--> VotacionService --> votos.csv
                              |
                              v  ObtenerVotos()
                        ResultadoService  --> ResultadoConvocatoria / ResultadoGrupo / Estadisticas
                              |
                              v
                        FrmResultados --> GraficaBarras
                              |
                              v
                     ExportacionService --> .csv / .json / .xml
```

**Regla de integración:** `FrmVotacion` y `FrmResultados` deben recibir **la misma instancia** de `VotacionService`, porque la lista de votos se mantiene en memoria. Si cada pantalla crea la suya, la de resultados no verá los votos nuevos hasta reiniciar el programa.

## Clases compartidas

**`Voto`** — un voto en una convocatoria: `AlumnoId`, `Grupo`, `Carrera`, `CentroUniversitario`, `Convocatoria`, `Candidato`, `EsWriteIn`, `FechaHora`. Guarda los datos del alumno para poder agrupar sin consultar otra clase.

**`VotacionService`**
- `RegistrarVotos(Alumno, IEnumerable<Voto>, out string error)`
- `ConvocatoriasYaVotadas(alumnoId, convocatorias)`
- `ObtenerVotos()`

**`ResultadoService`**
- `ContarVotos(filtro)`, `TotalVotos(filtro)`
- `AgruparPor(DimensionAgrupacion, filtro)`
- `ParticipacionGeneral(padron, filtro)`, `ParticipacionPorConvocatoria(padron, filtro)`
- `ObtenerGrupos()`, `ObtenerCarreras()`, `ObtenerCentros()`

**`ExportacionService`** — `ExportarCsv`, `ExportarJson`, `ExportarXml`; todos reciben una ruta y un `ReporteResultados`.

## Formato de `votos.csv`

```
AlumnoId,Grupo,Carrera,Centro,Convocatoria,Candidato,EsWriteIn,FechaHora
2021001,1A,Sistemas,CUCEI,Sociedad de Alumnos,Candidato A1,0,2026-10-05T09:00:45.0000000
```

Una fila por voto (un alumno que vota en 3 convocatorias genera 3 filas). `EsWriteIn` es `1` para candidato no registrado. Los campos con comas o comillas van entre comillas.

## Cálculos

- **Votos absolutos:** número de votos de un candidato en una convocatoria.
- **Porcentaje:** votos del candidato / votos de esa convocatoria × 100 (2 decimales).
- **Participación:** alumnos distintos que votaron / padrón × 100.
- **Abstencionismo:** 100 − participación (y `padrón − votantes` en alumnos).
- El **padrón** (alumnos que podían votar) no se guarda en ningún archivo: se indica en la pantalla de resultados. Con filtros, debe ser el padrón de ese subconjunto.
- Los nombres de candidato se comparan sin distinguir mayúsculas ("juan perez" = "Juan Perez").

## Exportación

- **CSV:** columnas `Convocatoria, Candidato, Tipo, Votos, Porcentaje, TotalVotosConvocatoria, Grupo, Carrera, Centro`. UTF-8 con BOM (Excel muestra bien los acentos). Los textos que empiezan con `= + - @` llevan apóstrofo al inicio para que Excel no los ejecute como fórmula. El separador decimal del porcentaje es el punto.
- **JSON / XML:** incluyen además filtro aplicado, fecha de generación y estadísticas de participación (si se indicó el padrón).

## Gráficas

`GraficaBarras` es un control propio dibujado con GDI+ (sin paquetes externos). Se usa dos veces: votos por candidato (una convocatoria a la vez) y participación/abstencionismo (escala fija 0–100 %).

## Compilar y publicar

Ver `INSTALACION.md`.

## Pruebas manuales sugeridas

1. Copiar `DatosPrueba/votos_demo.csv` como `votos.csv` junto al ejecutable.
2. Resultados sin filtro: 95 votos en total.
3. Elegir una carrera: tabla y gráficas cambian; la suma de votos coincide.
4. Padrón = 60: participación y abstencionismo suman 100 %.
5. Exportar CSV y abrirlo en Excel; exportar JSON y XML.
6. Votar dos veces con el mismo ID: el sistema debe rechazarlo.
