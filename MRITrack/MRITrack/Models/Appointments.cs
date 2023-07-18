namespace MRITrack.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Appointments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointments()
        {
            Comments = new HashSet<Comments>();
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        public int UserId { get; set; }

        public int DoctorId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comments> Comments { get; set; }

        public virtual Doctors Doctors { get; set; }

        public virtual Users Users { get; set; }
    }
}
