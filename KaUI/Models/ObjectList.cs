using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaUI.Models
{
    public class ObjectList<T>
    {
        private List<T> newList;

        public List<T> NewList
        {
            get { return newList; }
            set { newList = value; }
        }
    }
}
