using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APExittest.Bussiness.Model
{
    public class Ordertable
    {        
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductModel _product { get; set; }
        public string UserEmail { get; set; }
        public int OrderQuantity { get; set; }
        public bool isOrderConfirmed { get; set; }

        public DateTime timeStamp { get; set; }

        public Ordertable()
        {
            timeStamp = DateTime.Now;
        }

    }
}
