using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ViewModels
{
    public class DataChunkViewModel<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}
