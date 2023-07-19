namespace MRITrack.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        public short Star { get; set; }

        public int AppointmentId { get; set; }

        [Required]
        public string DoctorId { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}
