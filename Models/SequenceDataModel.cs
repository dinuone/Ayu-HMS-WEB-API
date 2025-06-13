using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("sequence_data")]
    public class SequenceDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Column("title", TypeName = "varchar(128)")]
        public string Title { get; set; }

        [Column("sequence", TypeName = "varchar(128)")]
        public string Sequence { get; set; }

        [Column("start")]
        public long Start { get; set; }

        [Column("end")]
        public long End { get; set; }

        [Column("current")]
        public long Current { get; set; }

        [Column("increment")]
        public long Increment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime created_at { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        public object Clone()
        {
            var model = (SequenceDataModel)MemberwiseClone();
            return model;
        }
    }
}
