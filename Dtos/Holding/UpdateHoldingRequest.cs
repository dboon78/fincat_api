using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Holding
{
    public class UpdateHoldingRequest
    {
        public int HoldingId { get; set; }
        [Range(0,int.MaxValue)]
        public float BookCost { get; set; }=0;
        [Range(0,int.MaxValue)]
        public float Units {get; set; }=0;
        
    }
}