//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Thesis.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Counterparty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Counterparty()
        {
            this.ApplicationUser = new HashSet<ApplicationUser>();
            this.UserOrder = new HashSet<UserOrder>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string ContactPerson { get; set; }
        public Nullable<int> AddressId { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserOrder> UserOrder { get; set; }
    }
}
