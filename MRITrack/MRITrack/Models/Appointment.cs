namespace MRITrack.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Appointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointment()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public int UserId { get; set; }

        public int DoctorId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual User User { get; set; }
    }
}
