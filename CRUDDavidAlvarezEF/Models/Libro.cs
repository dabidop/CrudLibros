using System;
using System.Collections.Generic;

namespace CRUDDavidAlvarezEF.Models;

public partial class Libro
{
    public string Isbn { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    public string NombreAutor { get; set; }

    public DateTime? Publicacion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? CodigoCategoria { get; set; }

    public int? NitEditorial { get; set; }

    public virtual Categoria CodigoCategoriaNavigation { get; set; }

    public virtual Editoriale NitEditorialNavigation { get; set; }
}
