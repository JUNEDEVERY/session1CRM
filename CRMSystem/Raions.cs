
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace CRMSystem
{

using System;
    using System.Collections.Generic;
    
public partial class Raions
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Raions()
    {

        this.ResidentialAddress = new HashSet<ResidentialAddress>();

    }


    public int RaionID { get; set; }

    public string RaionName { get; set; }

    public double Square { get; set; }

    public int Population { get; set; }

    public int CountMetroStations { get; set; }

    public int BuildingTypeID { get; set; }



    public virtual BuildingTypes BuildingTypes { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ResidentialAddress> ResidentialAddress { get; set; }

}

}
