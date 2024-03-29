using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;
[Table("customer", Schema = "dbo")]
public class CustomerA
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("customer_name")]
    public string CustomerName { get; set; }

    [Column("mobile_no")]
    public string MobileNumber { get; set; }

    [Column("email")]
    public string Email { get; set; }

}